using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ToDoList.Models
{
    public class UserSubscriber
    {
        public int UserSubscriberId { get; set; }
        public ApplicationUser Subscriber { get; set; }
        public ApplicationUser Subscribioner { get; set; }
    }
}
