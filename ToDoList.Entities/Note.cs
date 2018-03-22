namespace ToDoList.Entities
{
    using System.ComponentModel.DataAnnotations;
    
    public class Note
    {
        public Note(int? id, string content)
        {
            this.Id = id;
            this.Content = content;
        }

        [Required]
        public int? Id { get; set; }

        [Required]
        [MinLength(3), MaxLength(150)]
        public string Content { get; set; }
    }
}
