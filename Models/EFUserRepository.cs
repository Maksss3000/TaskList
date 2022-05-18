using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace TaskList.Models
{
    public class EFUserRepository : IUsersRepository
    {
        private TaskListDbContext context;
        public EFUserRepository (TaskListDbContext ctx)
        {
            context = ctx;
        }

        public User GetUser(long id)
        {
           return context.Users.FirstOrDefault(u => u.Id == id);
        }

        public UserAssignment GetUserAsignment(long userId, long assignmentId)
        {
           return context.UserAssignments.
                Where(u => u.UserId == userId && u.AssignmentId == assignmentId)
                                                                    .FirstOrDefault();
        }

        
        public IEnumerable<UserAssignment> GetUserAssignments(long userId)
        {
           return context.UserAssignments.Include(s => s.Assignment).
                                                               Where(s => s.UserId == userId);
        }

        public long GetUserId(string nickName)
        {
           return context.Users.Where(u => u.NickName == nickName).
                                            Select(u => u.Id).FirstOrDefault();
        }

        public IEnumerable<User> GetUsersAndThemAssignments()
        {
           return context.Users.Include(a => a.UserAssignments);
        }
    }
}
