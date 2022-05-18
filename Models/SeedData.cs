using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace TaskList.Models
{
    public class SeedData
    {
        public static void Seed(TaskListDbContext context)
        {
            //If there is some waiting Migrations.
            //We need to Update our DB.
            if (context.Database.GetPendingMigrations().Any())
            {
                //Using All Waiting Migrations.(Create/Update DB).
                context.Database.Migrate();
            }
            
            //If in our Database we don`t have any Task`s at all.
            if (!context.Assignments.Any())
            {
                //Creating Three types of Priority.
                Priority lowPriority = new Priority
                {
                    PriorityStatus = "Low"
                };

                Priority mediumPriority = new Priority
                {
                    PriorityStatus = "Medium"
                };

                Priority highPriority = new Priority
                {
                    PriorityStatus = "High"
                };


                //Creating Status types.
                //Pending, In Progress, Done, Canceled
                Status pendingStatus = new Status
                {
                    CurrentStatus = "Pending"
                };
                Status inProgressStatus = new Status
                {
                    CurrentStatus = "In Progress"
                };
                Status doneStatus = new Status
                {
                    CurrentStatus = "Done"
                };

                Status canceledStatus = new Status
                {
                    CurrentStatus = "Canceled"
                };

                context.Priorities.AddRange(lowPriority,mediumPriority,highPriority);

                context.Statuses.AddRange(pendingStatus, inProgressStatus, 
                                          doneStatus, canceledStatus);

                context.Assignments.AddRange(new Assignment
                {
                    Title="MVC Task",
                    Description = "Create new MVC Project using C#",
                    CreationDate =  DateTime.Now,
                    DeadLineDate = DateTime.Now.AddDays(7),
                    Status = pendingStatus,
                    Priority = highPriority,
                    
                },
                new Assignment
                {
                    Title="React Task",
                    Description = "Create Frontend using React",
                    CreationDate = DateTime.Now,
                    DeadLineDate = DateTime.Now.AddDays(6),
                    Status = inProgressStatus,
                    Priority = mediumPriority,
                   

                },

                new Assignment
                {
                    Title="Angular Task",
                    Description = "Create Frontend using Angular",
                    CreationDate = DateTime.Now,
                    DeadLineDate = DateTime.Now.AddDays(8),
                    Status = canceledStatus,
                    Priority = lowPriority,
                   
                });


                context.SaveChanges();
            }
            
        }
    }
}
