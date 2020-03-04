using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ToDoList.Models
{
    public class SingleTask
    {
        public int SingleTaskId { get; set; }
        public string Label { get; set; }
        public int TaskToDoId { get; set; }
        public TaskToDo TaskToDo { get; set; }
        public bool IsComplete { get; set; }
    }
}
