using System;
using System.Linq;
using NUnit.Framework;
using TestStack.BDDfy;

namespace Toolbelt.Upserter.Tests
{
    [TestFixture]
    [Story(
         AsA = "Dev",
         IWant = "I want to test with two simple string arrays using GUID as identifier and no update is allowed")]
    public class HappyPathForGuid
    {
        private Upserter<string, Guid> _upserter;
        private string[] _addedEntities;
        private string[] _updatedEntities;
        private string[] _deletedEntities;

        public void GivenAUpserterThatDoesNotWantToUpdate()
        {
            _upserter = new Upserter<string, Guid>(x => new Guid(x), 
                                                   (x, y) => false,
                                                   strings =>
                                                   {
                                                       _addedEntities = strings;
                                                       return strings.Length;
                                                   },
                                                   strings =>
                                                   {
                                                       _updatedEntities = strings;
                                                       return strings.Length;
                                                   },
                                                   strings =>
                                                   {
                                                       _deletedEntities = strings;
                                                       return strings.Length;
                                                   });
        }

        public void WhenUpserterRunsForTwoSimpleIntsArrays()
        {
            _upserter.Run(new[]
                          {
                              "6EE3928A-AFCB-47BF-874B-AC680B6A9599",
                              "78307059-1D02-4C31-AD58-D5DD3C53D815",
                              "77FE55F5-1057-4B91-8509-DB805BBA961B",
                              "25DF1282-4823-432A-9A4C-D389C2FB486D",
                              "A8D59E64-9126-4CE8-8375-86BE46EBAAE3",
                          }, 
                          new[]
                          {
                              "674CB42E-34BF-4F29-9D4B-F9320F444D70",
                              "A8D59E64-9126-4CE8-8375-86BE46EBAAE3",
                              "78307059-1D02-4C31-AD58-D5DD3C53D815",
                          });
        }

        public void ThenAddedEntitesAreTheNewStrings()
        {
            Assert.IsTrue(_addedEntities.Contains("674CB42E-34BF-4F29-9D4B-F9320F444D70"));
        }

        public void ThenUpdatedEntitiesAreTheOverlappingStrings()
        {
            Assert.AreEqual(0, _updatedEntities.Length);
        }

        public void ThenDeletedEntitiesAreTheObsoletedInts()
        {
            Assert.IsTrue(_deletedEntities.Contains("6EE3928A-AFCB-47BF-874B-AC680B6A9599"));
            Assert.IsTrue(_deletedEntities.Contains("77FE55F5-1057-4B91-8509-DB805BBA961B"));
            Assert.IsTrue(_deletedEntities.Contains("25DF1282-4823-432A-9A4C-D389C2FB486D"));
        }

        [Test]
        public void Execute()
        {
            this.BDDfy();
        }
    }
}