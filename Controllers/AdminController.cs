using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskList.Models;

namespace TaskList.Controllers
{
    //[Authorize(Roles = "Admin")]
    [ApiController]
    [Route("api/[controller]")]
    public class AdminController : ControllerBase
    {
        //Our DatabaseContext.
        private TaskListDbContext context;
        private IAssignmentRepository assignmentRepo;
        private IUsersRepository userRepo;

        //By Dependency Injection we getting our Database.
        //And methods to work with data.
        public AdminController(TaskListDbContext ctx,IAssignmentRepository assignR,
                               IUsersRepository userR)
        {
            context = ctx;
            assignmentRepo = assignR;
            userRepo = userR;
        }


        //Method returns all existed tasks.
        [HttpGet("allTasks")]
        public IActionResult GetAllTasks()
        {
            IEnumerable<Assignment> assignments = assignmentRepo.GetAllTasks();
            return AssignmentResult(assignments);
        }

        //Return Task`s with specific status.
        //In View admin can choose the Status,
        //(By status name , for example Pending, In Progress, Done, or Canceled).
        //By choosing status , method will get statusId and return Task`s with this
        //Status(StatusId).
        [HttpGet("{statusId}")]
        public  IActionResult GetTaskByStatus(long statusId)
        {
            IEnumerable<Assignment> assignments = assignmentRepo.
                                                        GetAssignmentsByStatus(statusId);

            return AssignmentResult(assignments);
        }

        //Return all Task`s which deadLine time this Week.
        //(Just Task`s with status In Progress.)
        [HttpGet("deadLineClose")]
        public IActionResult DeadLineThisWeek()
        {
            
            DateTime limitDate = DateTime.Now.AddDays(7);

            IEnumerable<Assignment> assignments = assignmentRepo.DeadLineThisWeek(limitDate);
            return AssignmentResult(assignments);
        }

        //Admin choosing limit Date(Method get it`s value ) .
        //To see User`s that completed Task before or in this Date.
        //Data sorted from most productive users.
        [HttpGet("statistic/{limitDate}")]
        public IActionResult UsersTaskCompletion(DateTime limitDate)
        {
           
            var result = context.UserAssignments.
               Where(u => u.Done == true &&u.Date<=limitDate).GroupBy(l => l.User.Email, l => l,
                               (key, g) => new { Key = key, Count = g.Count() });
            //Return`s Users ( User-Email and Count of Completed Tasks of this user.)
            return Ok(result);
        }
        //Admin Can see All Users.
        //And of what Task(Assignment) every user Responsible.
        [HttpGet("allUsers")]
        public IActionResult SeeUsers()
        {
            IEnumerable<User> users = userRepo.GetUsersAndThemAssignments();
            if (users == null)
            {
                return NotFound();
                //return BadRequest();
            }
            foreach (User user in users)
            {
             //Prevent Loop in Include Data.
             foreach(UserAssignment ua in user.UserAssignments)
                {
                    ua.User = null;
                }   
            }
            
            return Ok(users);
        }

        /*
         * Admin Filling "AddingEditing" Form for Task.(In View)
         * Sending Data. In method will be creation of new Task
         * Or Updating existing one.
         */
        [HttpPost("addUpdateTask")]
        public IActionResult AddUpdateAssignment(Assignment assignment)
        {

            //Updating Existing assignment
            //Or Adding new.
             assignmentRepo.AddUpdateAssignment(assignment);
             return Redirect("/api/admin/allTasks");
            
        }


        //In Form(View) Admin choosing Task and User that will be Responsible for this Task.
        //By entering Submit button admin send taskId and userId to Action.
        
        [HttpPost("addTask")]
        public IActionResult AddTaskToUser(Ids ids)
        {

            long taskId =ids.TaskId;
            long userId = ids.UserId;
            Assignment task = assignmentRepo.GetAssignmentById(taskId);
            User user = userRepo.GetUser(userId);

            //Can`t find Task or User with this Id.
            if (task == null || user == null)
            {
                return NotFound();
                //return BadRequest();
            }
            UserAssignment ua = userRepo.GetUserAsignment(userId, taskId);
            //Trying to add task to user , that already have this task.
            if (ua != null)
            {
                return Redirect("/api/admin/allTasks");
            }
            //Adding to Task user.
            //That mean`s that now user is Responsible for this Task.
            task.Users.Add(user);
            context.SaveChanges();
            return Redirect("/api/admin/allTasks");
        }

        
        //In Form(View) Admin choosing Task to Remove.
        //By submit button he send taskId.
        [HttpPost("deleteTask")]
        public IActionResult RemoveTask(Ids ids)
        {
            long taskId = ids.TaskId;
            Assignment task = assignmentRepo.GetAssignmentById(taskId);
            if (task == null)
            { 
                return BadRequest(); 
            }
            else
            {
                assignmentRepo.RemoveAssignment(task);
            }
            return Redirect("/api/admin/allTasks"); 

        }

        /*
         As Alternative way , we can use HttpDelete.
        [HttpDelete("{taskId}")]
        public void RemoveTask(long taskId)
        {
          
            Assignment task = assignmentRepo.GetAssignmentById(taskId);
           
            if(task!=null)
            {
               assignmentRepo.RemoveAssignment(task);
            }
            

        }

         */


        //Method check`s if argument is null, if it`s null returns NotFound() response.
        //If it`s not , return Ok status Response ,with argument data.
        public IActionResult AssignmentResult(IEnumerable<Assignment> assignments)
        {
            if (assignments == null)
            {
                return NotFound();
                //return BadRequest();
            }


            preventLoopCycle(assignments);

            return Ok(assignments);

        }

        //Preventing  Loop Cycle.
        //When we getting Include data , there will be loop,that we must Prevent.
        public void preventLoopCycle(IEnumerable<Assignment> assignments)
        {

            foreach (Assignment assignment in assignments)
            {

                assignment.Status.Assignments = null;
                assignment.Priority.Assignments = null;
            }
        }

    }
}
