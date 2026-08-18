using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using ARKBreedingStats.Library;
using ARKBreedingStats.species;

namespace ARKBreedingStats.BreedingPlanning
{
    /// <summary>
    /// Lets the user pick any two tamed creatures directly (no ranking/filters) and immediately
    /// see the best-possible offspring stats and the level-probability distribution for that pair.
    /// A quick "what if I breed these two" check, as opposed to the Breeding Plan tab's full
    /// library-wide pair recommendation engine.
    /// </summary>
    public partial class BreedingSimulator : UserControl
    {
        private List<Creature> _allCreatures = new List<Creature>();

        public BreedingSimulator()
        {
            InitializeComponent();
            cbMother.SelectedIndexChanged += CbMother_SelectedIndexChanged;
            cbFather.SelectedIndexChanged += CbFather_SelectedIndexChanged;
        }

        /// <summary>
        /// Refreshes the pool of candidate creatures the dropdowns are populated from. Call whenever
        /// the tab is shown, since the library may have changed since the last time.
        /// </summary>
        public void SetCreatures(IEnumerable<Creature> creatures)
        {
            _allCreatures = creatures?
                .Where(c => !c.flags.HasFlag(CreatureFlags.Placeholder) && !c.flags.HasFlag(CreatureFlags.Divider))
                .ToList() ?? new List<Creature>();
            RefreshMotherList();
        }

        private void RefreshMotherList()
        {
            var selected = GetSelected(cbMother);
            cbMother.Items.Clear();
            cbMother.Items.AddRange(_allCreatures
                .Where(c => c.sex == Sex.Female)
                .OrderBy(c => c.SpeciesName).ThenBy(c => c.name)
                .Select(c => (object)new CreatureOption(c))
                .ToArray());

            var toReselect = cbMother.Items.Cast<CreatureOption>().FirstOrDefault(o => o.Creature == selected);
            if (!toReselect.Equals(default(CreatureOption)))
                cbMother.SelectedItem = toReselect;

            RefreshFatherList();
        }

        private void RefreshFatherList()
        {
            var mother = GetSelected(cbMother);
            var selected = GetSelected(cbFather);
            cbFather.Items.Clear();

            var candidates = _allCreatures.Where(c => c.sex == Sex.Male);
            if (mother != null)
                candidates = candidates.Where(c => c.Species == mother.Species);

            cbFather.Items.AddRange(candidates
                .OrderBy(c => c.SpeciesName).ThenBy(c => c.name)
                .Select(c => (object)new CreatureOption(c))
                .ToArray());

            var toReselect = cbFather.Items.Cast<CreatureOption>().FirstOrDefault(o => o.Creature == selected);
            if (!toReselect.Equals(default(CreatureOption)))
                cbFather.SelectedItem = toReselect;

            Recalculate();
        }

        private void CbMother_SelectedIndexChanged(object sender, EventArgs e) => RefreshFatherList();

        private void CbFather_SelectedIndexChanged(object sender, EventArgs e) => Recalculate();

        private static Creature GetSelected(ComboBox cb) => cb.SelectedItem is CreatureOption o ? o.Creature : null;

        private void Recalculate()
        {
            var mother = GetSelected(cbMother);
            var father = GetSelected(cbFather);

            if (mother == null || father == null || mother.Species != father.Species)
            {
                pedigreeCreatureMother.Clear();
                pedigreeCreatureFather.Clear();
                pedigreeCreatureBest.Clear();
                offspringPossibilities1.Clear();
                lbMutationProbability.Text = string.Empty;
                lbHint.Visible = true;
                return;
            }

            lbHint.Visible = false;
            var species = mother.Species;
            int? levelStep = Library.CreatureCollection.CurrentCreatureCollection?.getWildLevelStep();

            var best = new Creature(species, Loc.S("BestPossible"), levelsWild: new int[Stats.StatsCount],
                levelsMutated: new int[Stats.StatsCount], isBred: true, levelStep: levelStep)
            {
                Mother = mother,
                Father = father,
                mutationsMaternal = mother.Mutations,
                mutationsPaternal = father.Mutations
            };

            for (var s = 0; s < Stats.StatsCount; s++)
            {
                if (s == Stats.Torpidity || !species.UsesStat(s)) continue;
                // Assumes a higher wild level is always the desired outcome for every stat, i.e. the
                // Breeding Plan tab's default stat weighting - this tool doesn't expose weighting.
                best.levelsWild[s] = Math.Max(mother.levelsWild[s], father.levelsWild[s]);
                best.levelsMutated[s] = Math.Max(mother.levelsMutated?[s] ?? 0, father.levelsMutated?[s] ?? 0);
                best.valuesBreeding[s] = StatValueCalculation.CalculateValue(species, s, best.levelsWild[s], best.levelsMutated[s], 0, true, 1, 0);
            }
            best.levelsWild[Stats.Torpidity] = best.levelsWild.Sum() + best.levelsMutated.Sum();
            best.RecalculateCreatureValues(levelStep);

            pedigreeCreatureMother.Creature = mother;
            pedigreeCreatureFather.Creature = father;
            pedigreeCreatureBest.Creature = best;

            offspringPossibilities1.Calculate(species, mother, father);
            lbMutationProbability.Text = $"{Loc.S("ProbabilityForOneMutation")}: {BreedingScore.MutationProbability(mother, father):P}";
        }

        /// <summary>
        /// Wraps a Creature for display in the mother/father dropdowns - shows name, level and
        /// species so creatures with a blank (never renamed) name are still distinguishable.
        /// </summary>
        private readonly struct CreatureOption
        {
            public readonly Creature Creature;
            public CreatureOption(Creature creature) => Creature = creature;

            public override string ToString() =>
                (string.IsNullOrEmpty(Creature.name) ? "(unnamed)" : Creature.name)
                + $" - Lvl {Creature.Level} - {Creature.SpeciesName}";
        }
    }
}
