using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TaskList.Models
{
   public interface IUsersRepository
    {
        IEnumerable<User> GetUsersAndThemAssignments();

        User GetUser(long id);

        UserAssignment GetUserAsignment(long userId, long assignmentId);
         

        long  GetUserId(string nickName);

        IEnumerable<UserAssignment> GetUserAssignments(long userId);
    }
}
