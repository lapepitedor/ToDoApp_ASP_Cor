namespace ToDoApp.Models
{
    public class ToDo : Entity
    {
        public string? Title { get; set; }
        public int Completion { get; set; }
        public string? Description { get; set; }
    }
}
