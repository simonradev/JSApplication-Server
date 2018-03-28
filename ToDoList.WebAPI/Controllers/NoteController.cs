namespace ToDoList.WebAPI.Controllers
{
    using System;
    using System.Collections.Generic;
    using Microsoft.AspNetCore.Mvc;

    using ToDoList.DAL;
    using ToDoList.Entities;
    using ToDoList.WebAPI.Constants;
    using ToDoList.WebAPI.Dtos;

    [Route("api/[controller]")]
    public class NoteController : ControllerBase
    {
        private readonly INotesRepository notesRepository;

        public NoteController(INotesRepository notesRepository)
        {
            if (notesRepository == null)
            {
                string exceptionMessage = ExceptionMessage.NullNotesRepository;
                throw new ArgumentNullException(exceptionMessage);
            }

            this.notesRepository = notesRepository;
        }

        /// <summary>
        /// Validate the note and if it is valid perform a repository operation with it.
        /// </summary>
        /// <param name="note">The note to operate on.</param>
        /// <param name="repoAction">The action to perform on the repository.</param>
        /// <param name="successResult">The result to return from the action if it was successful.</param>
        /// <returns>Successful: The specified result action / Not successful: 400 Bad Request</returns>
        private IActionResult SafelyUpdateNotes<T>(T note,
                                                   Action<INotesRepository, T> repoAction,
                                                   Func<T, IActionResult> successResult)
        {
            if (!this.ModelState.IsValid)
            {
                return BadRequest(this.ModelState);
            }

            try
            {
                repoAction(this.notesRepository, note);
            }
            catch (InvalidOperationException ioex)
            {
                return BadRequest(ioex.Message);
            }

            return successResult(note);
        }

        /// <summary>
        /// URL: /api/note
        /// </summary>
        /// <returns>All notes that are present in the repository.</returns>
        [HttpGet]
        public IActionResult Get()
        {
            IEnumerable<Note> allNotes = this.notesRepository.GetAll(n => n.Id);
            return Ok(allNotes);
        }

        /// <summary>
        /// URL: /api/note/{id:int}
        /// </summary>
        /// <returns>Successful: The wanted note / Unsuccessful: 404 Not Found</returns>
        [HttpGet("{id:int}")]
        public IActionResult GetById(int id)
        {
            Note wantedNote = null;
            try
            {
                wantedNote = this.notesRepository.GetById(id);
            }
            catch (InvalidOperationException ioex)
            {
                return NotFound(ioex.Message);
            }

            return Ok(wantedNote);
        }

        /// <summary>
        /// URL: /api/note
        /// Body:
        /// {
        ///     "content": "some content"
        /// }
        /// </summary>
        /// <returns>Successful: The wanted resource / Unsuccessful: 404 Not Found or 400 Bad Request</returns>
        [HttpPost]
        public IActionResult Create([FromBody] NoteCreationDto noteCreationDto)
        {
            Note createdNote = null;
            IActionResult creationResult = this.SafelyUpdateNotes(noteCreationDto,
                                                                  (r, n) =>
                                                                  {
                                                                      createdNote = new Note(n.Content);
                                                                      r.Add(createdNote);
                                                                  },
                                                                  (n) => CreatedAtAction(nameof(Create), createdNote));
            return creationResult;
        }

        /// <summary>
        /// URL: /api/note
        /// Body:
        /// {
        ///     "id": 1,
        ///     "content": "some content"
        /// }
        /// </summary>
        /// <returns>Successful: Edits the resource and returns it / Unsuccessful: 404 Not Found or 400 Bad Request</returns>
        [HttpPut]
        public IActionResult Edit([FromBody] Note note)
        {
            IActionResult editResult = this.SafelyUpdateNotes(note,
                                                              (r, n) => r.Edit(n.Id, n),
                                                              (n) => Ok(n));
            return editResult;
        }

        /// <summary>
        /// URL: /api/note/{id:int}
        /// </summary>
        /// <returns>Successful: count of deleted rows / Unsuccessful: 404 Not Found or 400 Bad Request</returns>
        [HttpDelete("{id?}")]
        public IActionResult Delete(int? id)
        {
            if (!id.HasValue)
            {
                DeletionError deletionError = new DeletionError(id);
                return BadRequest(deletionError);
            }

            bool deletionIsSuccessful = this.notesRepository.Delete(id.Value);

            if (deletionIsSuccessful)
            {
                DeletionInfo deletionInfo = new DeletionInfo(NoteControllerConstant.CountOfDeletedRows);
                return Ok(deletionInfo);
            }
            else
            {
                DeletionError deletionError = new DeletionError(id);
                return NotFound(deletionError);
            }
        }

        /// <summary>
        /// URL: /api/note
        /// Body:
        /// {
        ///     "Ids": [1, 2, 3]
        /// }
        /// </summary>
        /// <returns>Successful: count of deleted rows / Unsuccessful: 404 Not Found or 400 Bad Request</returns>
        [HttpDelete]
        public IActionResult Delete([FromBody] NoteDeletionDto noteDeletionDto)
        {
            if (!this.ModelState.IsValid)
            {
                return BadRequest(this.ModelState);
            }

            int countOfDeletedRows = this.notesRepository.DeleteMany(noteDeletionDto.Ids);

            DeletionInfo deletionInfo = new DeletionInfo(countOfDeletedRows);
            return Ok(deletionInfo);
        }
    }
}
