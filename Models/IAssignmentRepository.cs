using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TaskList.Models
{
   public interface IAssignmentRepository
    {
        IEnumerable<Assignment> GetAllTasks();

        IEnumerable<Assignment> GetAssignmentsByStatus(long statusId);

        IEnumerable<Assignment> DeadLineThisWeek(DateTime limit);

        void AddUpdateAssignment(Assignment assignment);

        Assignment GetAssignmentById(long id);

        void RemoveAssignment(Assignment assignment);
    }
}
