using System.Collections.Concurrent;

namespace Task_Management.Models
{
    public class Project
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public ConcurrentDictionary<string, TaskItem> Tasks { get; set; } = new();
    }
}
