using NUnit.Framework;
using TestStack.BDDfy;

namespace Toolbelt.Upserter.Tests
{
    [TestFixture]
    [Story(
         AsA = "Dev",
         IWant = "I want to test when there is no deleter",
         SoThat = "So that I know that Upserter won't crash")]
    public class NoDeleter
    {
        private Upserter<int> _upserter;
        private UpsertResult _result;

        public void GivenAHappyUpserter()
        {
            _upserter = new Upserter<int>(x => x,
                                          (x, y) => x == y,
                                          ints => ints.Length,
                                          ints => ints.Length,
                                          null);
        }

        public void WhenUpserterRunsForTwoSimpleIntsArrays()
        {
            _result = _upserter.Run(new[] {1, 2, 3, 4, 5}, new[] {5, 2, 9, 10});
        }

        public void ThenNoRowDeleted()
        {
            Assert.AreEqual(0, _result.RowsDeleted);
        }

        [Test]
        public void Execute()
        {
            this.BDDfy();
        }
    }
}