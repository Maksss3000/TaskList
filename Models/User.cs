using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TaskList.Models
{
    public class User
    {
        public long Id { get; set; }

        public string NickName { get; set; }
        public string Name { get; set; }

        public string Email { get; set; }

        //Every User can be Responsible of Many Tasks.
        public  List<Assignment> Assignments { get; set; } = new List<Assignment>();
        public List<UserAssignment> UserAssignments { get; set; } = new List<UserAssignment>();
    }
}
