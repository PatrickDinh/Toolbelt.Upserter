namespace Toolbelt.Upserter
{
    public class UpsertResult
    {
        public UpsertResult(int rowsAdded, int rowsUpdated, int rowsDeleted)
        {
            RowsAdded = rowsAdded;
            RowsUpdated = rowsUpdated;
            RowsDeleted = rowsDeleted;
        }

        public int RowsAdded { get; private set; }
        public int RowsUpdated { get; private set; }
        public int RowsDeleted { get; private set; }
    }
}