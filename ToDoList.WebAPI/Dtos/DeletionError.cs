namespace ToDoList.WebAPI.Dtos
{
    public class DeletionError
    {
        public DeletionError(int? invalidId)
        {
            this.InvalidId = invalidId;
        }

        public int? InvalidId { get; }
    }
}
