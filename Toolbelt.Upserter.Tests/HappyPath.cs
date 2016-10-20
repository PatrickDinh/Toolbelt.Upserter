using System.Linq;
using NUnit.Framework;
using TestStack.BDDfy;

namespace Toolbelt.Upserter.Tests
{
    [TestFixture]
    [Story(
         AsA = "Dev",
         IWant = "I want to test with two simple int arrays",
         SoThat = "So that I know that something is happening")]
    public class HappyPath
    {
        private Upserter<int> _upserter;
        private int[] _addedEntities;
        private int[] _updatedEntities;
        private int[] _deletedEntities;

        public void GivenAHappyUpserter()
        {
            _upserter = new Upserter<int>(x => x,
                                          (x, y) => x == y,
                                          ints =>
                                          {
                                              _addedEntities = ints;
                                              return ints.Length;
                                          },
                                          ints =>
                                          {
                                              _updatedEntities = ints;
                                              return ints.Length;
                                          },
                                          ints =>
                                          {
                                              _deletedEntities = ints;
                                              return ints.Length;
                                          });
        }

        public void WhenUpserterRunsForTwoSimpleIntsArrays()
        {
            _upserter.Run(new[] {1, 2, 3, 4, 5}, new[] {5, 2, 9, 10});
        }

        public void ThenAddedEntitesAreTheNewInts()
        {
            Assert.IsTrue(_addedEntities.Contains(9));
            Assert.IsTrue(_addedEntities.Contains(10));
        }

        public void ThenUpdatedEntitiesAreTheOverlappingInts()
        {
            Assert.IsTrue(_updatedEntities.Contains(5));
            Assert.IsTrue(_updatedEntities.Contains(2));
        }

        public void ThenDeletedEntitiesAreTheObsoletedInts()
        {
            Assert.IsTrue(_deletedEntities.Contains(1));
            Assert.IsTrue(_deletedEntities.Contains(3));
            Assert.IsTrue(_deletedEntities.Contains(4));
        }

        [Test]
        public void Execute()
        {
            this.BDDfy();
        }
    }
}