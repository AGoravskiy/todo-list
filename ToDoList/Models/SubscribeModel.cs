using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ToDoList.Models
{
    public class SubscribeModel
    {
        public ApplicationUser User { get; set; }
        public bool IsSub { get; set; }
    }
}
