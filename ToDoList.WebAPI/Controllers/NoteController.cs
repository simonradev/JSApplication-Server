﻿namespace ToDoList.WebAPI.Controllers
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
        private IActionResult SafelyUpdateNotes(Note note, 
                                                Action<INotesRepository, Note> repoAction,
                                                Func<Note, IActionResult> successResult)
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
            IEnumerable<Note> allNotes = this.notesRepository.GetAll();
            return Ok(allNotes);
        }

        /// <summary>
        /// URL: /api/note/{id:int}
        /// </summary>
        /// <returns>Successful: The wanted note / Unsuccessful: 404 Not Found</returns>
        [HttpGet("{id}")]
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
        ///     "id": 1,
        ///     "content": "some content"
        /// }
        /// </summary>
        /// <returns>Information if the creation was successful or not.</returns>
        [HttpPost]
        public IActionResult Create([FromBody] Note note)
        {
            IActionResult creationResult = this.SafelyUpdateNotes(note, 
                                                                  (r, n) => r.Add(n.Id.Value, n),
                                                                  (n) => CreatedAtAction(nameof(Create), n));
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
        /// <returns>Information if the update was successful or not.</returns>
        [HttpPut]
        public IActionResult Edit([FromBody] Note note)
        {
            IActionResult editResult = this.SafelyUpdateNotes(note,
                                                              (r, n) => r.Edit(n.Id.Value, n),
                                                              (n) => Ok(n));
            return editResult;
        }

        /// <summary>
        /// URL: /api/note/{id:int}
        /// </summary>
        /// <returns>Successful: count of deleted rows / Unsuccessful: 404 Not Found</returns>
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
    }
}