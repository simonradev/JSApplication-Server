namespace ToDoList.WebAPI.Dtos
{
    using System.ComponentModel.DataAnnotations;

    public class NoteDeletionDto
    {
        [Required]
        public int[] Ids { get; set; }
    }
}
