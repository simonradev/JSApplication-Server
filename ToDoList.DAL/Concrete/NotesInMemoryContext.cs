namespace ToDoList.DAL.Concrete
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using ToDoList.DAL.Constants;
    using ToDoList.Entities;

    public class NotesInMemoryContext : INotesRepository
    {
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

        public void Add(int id, Note noteToAdd)
        {
            if (this.notesContext.ContainsKey(id))
            {
                string exceptionMessage = string.Format(ExceptionMessage.IdOfNoteAlreadyExists, noteToAdd.Id);
                throw new InvalidOperationException(exceptionMessage);
            }

            this.notesContext[id] = noteToAdd;
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
