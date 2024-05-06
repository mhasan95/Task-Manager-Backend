namespace Task_Manager_Backend.Models
{

    public enum TaskStatus
    {
        Pending,
        Inprogress,
        Completed,
        Archived
    }
    public enum TaskPriority
    {
        Low,
        Medium,
        High
    }
    public class Task
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public TaskPriority Priority { get; set; }

        public DateTime DueDate { get; set; }
        public TaskStatus Status { get; set; }
    }
}
