using ARKBreedingStats.Pedigree;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ARKBreedingStats.Tests
{
    /// <summary>
    /// Regression tests for a crash where importing a mod-free savegame and opening the Breeding
    /// Plan tab threw "Parameter is not valid" constructing a 0-height Bitmap. Root cause:
    /// PedigreeElementHeight was only ever set by DisplayMutationLevels, which was only reached
    /// for collections that needed a mod-value reload - never for a plain savegame import.
    /// </summary>
    [TestClass]
    public class PedigreeCreationTests
    {
        [TestMethod]
        public void InitializeScaling_Alone_SetsValidPedigreeElementHeight()
        {
            // Act: only InitializeScaling is called here, mirroring a savegame import that never
            // reaches DisplayMutationLevels (i.e. LoadModValuesOfCollection is never invoked).
            // Deliberately does not call DisplayMutationLevels itself, so this only passes if
            // InitializeScaling gives PedigreeElementHeight a valid value on its own - it must not
            // rely on another test (or another static-state mutator elsewhere) having already
            // called DisplayMutationLevels first, since MSTest doesn't guarantee test order.
            PedigreeCreation.InitializeScaling(3f);

            // Assert
            Assert.AreEqual(PedigreeCreature.ControlHeightWoMutations, PedigreeCreation.PedigreeElementHeight,
                "PedigreeElementHeight must default to the no-mutations control height after " +
                "InitializeScaling, otherwise Breeding Plan crashes constructing a 0-height Bitmap " +
                "for the first pairing whenever a collection never reaches DisplayMutationLevels.");
        }

        [TestMethod]
        public void InitializeScaling_ScalesLayoutConstantsProportionally()
        {
            // Act
            PedigreeCreation.InitializeScaling(2f);

            // Assert
            Assert.AreEqual(20, PedigreeCreation.Margin);
            Assert.AreEqual(80, PedigreeCreation.LeftMargin);
            Assert.AreEqual(40, PedigreeCreation.TopMargin);
            Assert.AreEqual(20, PedigreeCreation.ControlDistance);
            Assert.AreEqual(758, PedigreeCreation.PedigreeElementWidth);
        }

        [TestMethod]
        public void DisplayMutationLevels_True_UsesTallerControlHeight()
        {
            // Arrange
            PedigreeCreation.InitializeScaling(1f);

            // Act
            PedigreeCreation.DisplayMutationLevels(false);
            var heightWithout = PedigreeCreation.PedigreeElementHeight;
            PedigreeCreation.DisplayMutationLevels(true);
            var heightWith = PedigreeCreation.PedigreeElementHeight;

            // Assert
            Assert.IsTrue(heightWith > heightWithout,
                "Displaying mutation levels should use a taller pedigree element than not displaying them.");
        }
    }
}
