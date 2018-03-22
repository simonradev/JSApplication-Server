namespace ToDoList.WebAPI.Dtos
{
    public class DeletionInfo
    {
        public DeletionInfo(int deletedRows)
        {
            this.DeletedRows = deletedRows;
        }

        public int DeletedRows { get; }
    }
}
