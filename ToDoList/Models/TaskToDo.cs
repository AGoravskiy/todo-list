using System;

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ToDoList.Models
{
    public class TaskToDo
    {
        public int TaskToDoId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        [DisplayFormat(DataFormatString = "{0:t}")]
        public DateTime? StartTime { get; set; }
        [DisplayFormat(DataFormatString = "{0:t}")]
        public DateTime? FinishTime { get; set; }
        public string ApplicationUserId { get; set; }
        public ApplicationUser ApplicationUser { get; set; }
        public List<SingleTask> SingleTasks { get; set; }
        public bool IsComplete { get; set; }
        public bool IsArchive { get; set; }

        public TaskToDo()
        {
            SingleTasks = new List<SingleTask>();
        }
    }

}
