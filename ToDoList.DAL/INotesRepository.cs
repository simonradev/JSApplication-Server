namespace ToDoList.DAL
{
    using System.Collections.Generic;

    using ToDoList.Entities;

    public interface INotesRepository
    {
        void Add(int id, Note noteToAdd);

        void Edit(int id, Note noteToEdit);

        bool Delete(int id);

        IEnumerable<Note> GetAll();

        Note GetById(int id);
    }
}
