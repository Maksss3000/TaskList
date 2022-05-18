using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskList.Models;


namespace TaskList.Controllers
{
    //[Authorize(Roles = "Admin,User")]
    [ApiController]
    [Route("api/[controller]")]
    public class HomeController : ControllerBase
    {
        //Our DatabaseContext.
        private TaskListDbContext context;
        private IUsersRepository userRepo;
        public HomeController(TaskListDbContext ctx,IUsersRepository userR)
        {

            context = ctx;
            userRepo=userR;
            
        }

        
        [HttpGet("allTasks")]
        //Return`s all Tasks of specific user.
        //When user Log-In , he can see all his Taks`s.
        public IActionResult GetAllTasks()
        {
            
            //When user Log-In.
            //Getting his Current Loged-In name.
            //Finding his id in Users Table by his Loged-In(NickName) name.
            string NickName = User.FindFirstValue(ClaimTypes.Name);
            Console.WriteLine($"Nick Name : {NickName}");
            long id = userRepo.GetUserId(NickName);
           

            IEnumerable<UserAssignment> ua = userRepo.GetUserAssignments(id);
            List<Assignment> assignments=new List<Assignment>();
            
            foreach (UserAssignment u in ua)
            {

                assignments.Add( context.Assignments.Include(s => s.Priority).
                    Include(s => s.Status).Where(s => s.Id == u.AssignmentId).FirstOrDefault());

            }
            
            //Return all Task`s of specific user.
            return AssignmentResult(assignments);
        }


        
        [HttpPut("changeTaskStatus")]
        //In Form User Choosing Task(When he finished it.) from his task list.
        //And Changing to Status Done.
        public IActionResult ChangeTaskStatus(Ids ids)
        {
           
            long taskId = ids.TaskId;
            string  NickName = User.FindFirstValue(ClaimTypes.Name);
            long id = userRepo.GetUserId(NickName);
           
           
            /*
             * If we work with CMD/Powersell/PostMan 
             * We can just send id of user.
             * id=ids.UserId
             */
           
            UserAssignment ua = userRepo.GetUserAsignment(id, taskId);
            //If User are Responisble for this task.
            //He can change Task`s Status, when he Finish Task.
            if (ua != null)
            {
               
                ua.Done = true;
                ua.Date = DateTime.Now;
                context.SaveChanges();
                return Ok();
            }
            return NotFound();
            //return BadRequest();
            
        }

        //Method check if argument is null, if it`s null returns NotFound() response.
        //If it`s not , return Ok status Response ,with argument data.
        public IActionResult AssignmentResult(IEnumerable<Assignment> assignments)
        {
            if (assignments == null || !assignments.Any())
            {
               
                return NotFound();
            }
           
            preventLoopCycle(assignments);
            return Ok(assignments);

        }

        

        //Preventing  Loop Cycle.
        public void preventLoopCycle(IEnumerable<Assignment> assignments)
        {
           
            foreach (Assignment assignment in assignments)
            {

                assignment.Status.Assignments = null;
                assignment.Priority.Assignments = null;
                assignment.UserAssignments = null;
              
            }
        }

      

    }
}
