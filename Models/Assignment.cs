using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TaskList.Models
{
    //Task Table.
    public class Assignment
    {

        public long Id { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public DateTime CreationDate { get; set; }

        [Required]
        public DateTime DeadLineDate { get; set; }

        //RelationShip Between Tables.
        //Every Task Have one Priority .
        public Priority Priority { get; set; }

        [Required]
        public long PriorityId { get; set; }

        //RelationShip.
        //Every Task Have one Status.
        public Status Status { get; set; }

        [Required]
        public long StatusId { get; set; }

        //Every Task may have many Users(Responsibles of this specific Task)
        public List<User> Users { get; set; } = new List<User>();
        public List<UserAssignment> UserAssignments { get; set; } = new List<UserAssignment>();

       
    }
}
