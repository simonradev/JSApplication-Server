namespace ToDoList.WebAPI.Dtos
{
    using System.ComponentModel.DataAnnotations;

    public class NoteCreationDto
    {
        [Required]
        [MinLength(3), MaxLength(150)]
        public string Content { get; set; }
    }
}
