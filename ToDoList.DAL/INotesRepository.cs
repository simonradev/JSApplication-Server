namespace ToDoList.DAL
{
    using System;
    using System.Collections.Generic;

    using ToDoList.Entities;

    public interface INotesRepository
    {
        void Add(Note noteToAdd);

        void Edit(int id, Note noteToEdit);

        bool Delete(int id);

        int DeleteMany(int[] allIds);

        IEnumerable<Note> GetAll<TKey>(Func<Note, TKey> orderFunc);

        Note GetById(int id);
    }
}
