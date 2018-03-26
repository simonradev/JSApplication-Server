namespace ToDoList.DAL.Concrete
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using ToDoList.DAL.Constants;
    using ToDoList.Entities;

    public class NotesInMemoryContext : INotesRepository
    {
        private static int CurrentId = 1;

        private readonly IDictionary<int, Note> notesContext;

        public NotesInMemoryContext()
            : this(new Dictionary<int, Note>())
        {
        }

        public NotesInMemoryContext(IDictionary<int, Note> notes)
        {
            if (notes == null)
            {
                string exceptionMessage = ExceptionMessage.NullNotesCollection;
                throw new ArgumentNullException(exceptionMessage);
            }

            this.notesContext = notes;
        }

        private void ValidateNoteIdExistence(int id)
        {
            if (!this.notesContext.ContainsKey(id))
            {
                string exceptionMessage = string.Format(ExceptionMessage.IdOfNoteDoesntExist, id);
                throw new InvalidOperationException(exceptionMessage);
            }
        }

        public void Add(Note noteToAdd)
        {
            noteToAdd.Id = CurrentId;
            this.notesContext[CurrentId] = noteToAdd;

            CurrentId++;
        }

        public void Edit(int id, Note noteToEdit)
        {
            this.ValidateNoteIdExistence(id);

            this.notesContext[id] = noteToEdit;
        }

        public bool Delete(int id)
        {
            bool deletionIsSuccessful = this.notesContext.Remove(id);
            return deletionIsSuccessful;
        }

        public int DeleteMany(int[] allIds)
        {
            int countOfDeletedRows = 0;
            foreach (int id in allIds)
            {
                bool isDeleted = this.Delete(id);

                if (isDeleted)
                {
                    countOfDeletedRows++;
                }
            }

            return countOfDeletedRows;
        }

        public IEnumerable<Note> GetAll()
        {
            IEnumerable<Note> selectedNotes = this.notesContext.Select(kvp => kvp.Value);
            return selectedNotes;
        }

        public Note GetById(int id)
        {
            this.ValidateNoteIdExistence(id);

            Note wantedNote = this.notesContext[id];
            return wantedNote;
        }
    }
}
