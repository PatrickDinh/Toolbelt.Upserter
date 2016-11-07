namespace Toolbelt.Upserter.Tests
{
    public class EntityWithIntId
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int GetId()
        {
            return Id;
        }

        public EntityWithIntId(int id, string name)
        {
            Id = id;
            Name = name;
        }
    }
}