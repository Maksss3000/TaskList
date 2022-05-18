using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TaskList.Models
{
    public class Priority
    {
        public long Id { get; set; }
        public string PriorityStatus { get; set; }

        //Tables RelationShip.
        //One Priority may belong to many different Assignments.
        public IEnumerable<Assignment> Assignments { get; set; }


    }
}
