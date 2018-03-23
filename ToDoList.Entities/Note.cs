namespace ToDoList.Entities
{
    using System.ComponentModel.DataAnnotations;
    
    public class Note
    {
        public Note()
        {
        }

        public Note(string content)
        {
            this.Content = content;
        }

        public Note(int id, string content)
            : this(content)
        {
            this.Id = id;
        }
        
        public int Id { get; set; }

        [Required]
        [MinLength(3), MaxLength(150)]
        public string Content { get; set; }
    }
}
