using NUnit.Framework;
using TestStack.BDDfy;

namespace Toolbelt.Upserter.Tests
{
    [TestFixture]
    [Story(
         AsA = "Dev",
         IWant = "I want to test when there is no comparer",
         SoThat = "So that I know that Upserter won't crash")]
    public class NoComparer
    {
        private Upserter<int> _upserter;
        private UpsertResult _result;

        public void GivenAHappyUpserter()
        {
            _upserter = new Upserter<int>(x => x,
                                          null,
                                          ints => ints.Length,
                                          ints => ints.Length,
                                          ints => ints.Length);
        }

        public void WhenUpserterRunsForTwoSimpleIntsArrays()
        {
            _result = _upserter.Run(new[] {1, 2, 3, 4, 5}, new[] {5, 2, 9, 10});
        }

        public void ThenNoRowUpdated()
        {
            Assert.AreEqual(2, _result.RowsUpdated);
        }

        [Test]
        public void Execute()
        {
            this.BDDfy();
        }
    }
}