using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TaskList.Models
{
    //Join Table.
   //Between Users and Assignments.
    public class UserAssignment
    {
        public long UserId { get; set; }

        public long AssignmentId { get; set; }

        public bool Done { get; set; }

        public DateTime Date { get; set; }
        public User User { get; set; }

        public Assignment Assignment { get; set; }
    }
}
