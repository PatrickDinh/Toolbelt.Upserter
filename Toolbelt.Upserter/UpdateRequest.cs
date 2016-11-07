namespace Toolbelt.Upserter
{
    public class UpdateRequest<T>
    {
        public UpdateRequest(T oldEntity, T newEntity)
        {
            OldEntity = oldEntity;
            NewEntity = newEntity;
        }

        public T OldEntity { get; set; }
        public T NewEntity { get; set; }
    }
}