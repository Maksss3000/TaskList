using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TaskList.Models
{
    public class Status
    {
        public long Id { get; set; }

        public string CurrentStatus { get; set; }

        //Tables RelationShip.
        //One Status may belong to many different Assignments.
        public IEnumerable<Assignment> Assignments { get; set; }
    }
}
