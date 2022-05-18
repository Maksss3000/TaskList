using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace TaskList.Models
{
    public class EFAssignmentRepository : IAssignmentRepository
    {
        private TaskListDbContext context;
        public EFAssignmentRepository(TaskListDbContext ctx)
        {
            context = ctx;
        }

        public void AddUpdateAssignment(Assignment assignment)
        {
            context.Assignments.Update(assignment);
            context.SaveChanges();
        }

        public IEnumerable<Assignment> DeadLineThisWeek(DateTime limitDate)
        {
            return context.Assignments.Include(s => s.Priority).
                                       Include(s => s.Status).
                                       Where(assig => assig.Status.CurrentStatus == 
                                       "In Progress" && limitDate > assig.DeadLineDate);
        }

        public IEnumerable<Assignment> GetAllTasks()
        {
            return context.Assignments.Include(s => s.Priority).Include(s => s.Status);
        }

        public Assignment GetAssignmentById(long id)
        {
            return context.Assignments.FirstOrDefault(t => t.Id == id);
        }

        public IEnumerable<Assignment> GetAssignmentsByStatus(long statusId)
        {
            return context.Assignments.
                Include(s => s.Priority).Include(s => s.Status).
                Where(s => s.StatusId == statusId);
        }

        public void RemoveAssignment(Assignment assignment)
        {
            context.Assignments.Remove(assignment);
            context.SaveChanges();
        }
    }
}
