namespace ToDoList.DAL.Concrete
{
    using System.Collections.Generic;
    using ToDoList.Entities;

    public class NotesInMemoryContext : INotesRepository
    {
        private readonly ICollection<Note> notes;
    }
}
