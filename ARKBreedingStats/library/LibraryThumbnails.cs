using System.Collections.Concurrent;
using System.Drawing;
using System.Windows.Forms;
using ARKBreedingStats.SpeciesImages;

namespace ARKBreedingStats.Library
{
    /// <summary>
    /// Generates and caches small colored creature portraits (using the creature's species and colors from the save)
    /// for use as row thumbnails in the library list view.
    /// </summary>
    internal static class LibraryThumbnails
    {
        private const int ThumbnailSize = 64;

        private static readonly ConcurrentDictionary<string, Bitmap> Cache = new ConcurrentDictionary<string, Bitmap>();
        private static readonly ConcurrentDictionary<string, byte> PendingRequests = new ConcurrentDictionary<string, byte>();

        /// <summary>
        /// Returns a cached thumbnail for the creature's species/colors if already available.
        /// If not, a request for it is started asynchronously, and the passed list view is invalidated once it's ready.
        /// </summary>
        public static Bitmap GetOrRequestThumbnail(Creature creature, ListView listViewToInvalidate)
        {
            if (creature?.Species == null) return null;

            var key = ThumbnailKey(creature);
            if (Cache.TryGetValue(key, out var bmp)) return bmp;
            if (!PendingRequests.TryAdd(key, 0)) return null; // already requested, wait for callback

            CreatureColored.GetColoredCreatureWithCallback((bitmap, neighbourPoseExist) =>
                {
                    PendingRequests.TryRemove(key, out _);
                    if (bitmap == null) return;
                    Cache[key] = bitmap;
                    if (listViewToInvalidate.IsHandleCreated && !listViewToInvalidate.IsDisposed)
                        listViewToInvalidate.Invalidate();
                }, listViewToInvalidate, creature.colors, creature.Species, creature.Species.EnabledColorRegions,
                ThumbnailSize, creatureSex: creature.sex, game: CreatureCollection.CurrentCreatureCollection?.Game);

            return null;
        }

        private static string ThumbnailKey(Creature creature)
        {
            var colors = creature.colors;
            return creature.Species.blueprintPath + "|"
                   + (colors == null ? string.Empty : string.Join(",", colors)) + "|" + creature.sex;
        }

        /// <summary>
        /// Clears all cached thumbnails, e.g. when a different save/collection is loaded.
        /// </summary>
        public static void ClearCache()
        {
            Cache.Clear();
            PendingRequests.Clear();
        }
    }
}
