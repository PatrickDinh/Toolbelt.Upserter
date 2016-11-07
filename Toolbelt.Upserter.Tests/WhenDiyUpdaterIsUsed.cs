using System.Linq;
using NUnit.Framework;
using TestStack.BDDfy;

namespace Toolbelt.Upserter.Tests
{
    [TestFixture]
    [Story(
         AsA = "Dev",
         IWant = "I want to test the scenario that a DIY updater is used")]
    public class WhenDiyUpdaterIsUsed
    {
        private Upserter<EntityWithIntId> _upserter;
        private UpdateRequest<EntityWithIntId>[] _updateRequests;
        private EntityWithIntId[] _existingEntities;
        private EntityWithIntId[] _newEntities;

        public void GivenAnUpdater()
        {
            _upserter = new Upserter<EntityWithIntId>(x => x.GetId(), null,
                                                      updateRequest =>
                                                      {
                                                          _updateRequests = updateRequest;
                                                          return 0;
                                                      }
                                                      , null);
        }

        public void AndGivenExistingEntities()
        {
            _existingEntities = new[]
                                {
                                    new EntityWithIntId(1, "One"),
                                    new EntityWithIntId(2, "Two"),
                                    new EntityWithIntId(3, "Three"),
                                    new EntityWithIntId(4, "Four"),
                                    new EntityWithIntId(5, "Five"),
                                    new EntityWithIntId(6, "Six"),
                                };
        }

        public void AndGivenNewEntities()
        {
            _newEntities = new[]
                                {
                                    new EntityWithIntId(6, "SixNew"),
                                    new EntityWithIntId(5, "FiveNew"),
                                    new EntityWithIntId(2, "TwoNew"),
                                };
        }

        public void WhenUpserterRuns()
        {
            _upserter.Run(_existingEntities, _newEntities);
        }

        public void ThenUpdateRequestsLengthIs3()
        {
            Assert.AreEqual(3, _updateRequests.Length);
        }

        public void AndThen6IsInTheRequests()
        {
            Assert.IsTrue(_updateRequests.Any(r => r.NewEntity.Id == 6 && r.OldEntity.Id == 6));
        }

        public void AndThen6HasANewName()
        {
            var two = _updateRequests.First(r => r.NewEntity.Id == 6 && r.OldEntity.Id == 6);
            Assert.AreEqual("Six", two.OldEntity.Name);
            Assert.AreEqual("SixNew", two.NewEntity.Name);
        }

        [Test]
        public void Execute()
        {
            this.BDDfy();
        }
    }
}