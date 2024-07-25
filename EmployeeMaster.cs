using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace EmployeeManagementSystem
{
    public class Employee
    {
        public int EmployeeId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Position { get; set; }
        public DateTime DateOfJoining { get; set; }
        public int RoleId { get; set; }
        public Role Role { get; set; }
        public ICollection<Attendance> Attendances { get; set; }
        public ICollection<Payroll> Payrolls { get; set; }
        public ICollection<PerformanceReview> PerformanceReviews { get; set; }
        public ICollection<Task> Tasks { get; set; }
        public ICollection<Notification> Notifications { get; set; }
        public ICollection<Report> Reports { get; set; }
    }

    public class Attendance
    {
        public int AttendanceId { get; set; }
        public DateTime Date { get; set; }
        public bool IsPresent { get; set; }
        public int EmployeeId { get; set; }
        public Employee Employee { get; set; }
    }

    public class Payroll
    {
        public int PayrollId { get; set; }
        public decimal Salary { get; set; }
        public DateTime PaymentDate { get; set; }
        public int EmployeeId { get; set; }
        public Employee Employee { get; set; }
    }

    public class PerformanceReview
    {
        public int PerformanceReviewId { get; set; }
        public DateTime ReviewDate { get; set; }
        public string Comments { get; set; }
        public int EmployeeId { get; set; }
        public Employee Employee { get; set; }
    }

    public class Role
    {
        public int RoleId { get; set; }
        public string RoleName { get; set; }
        public ICollection<Employee> Employees { get; set; }
    }

    public class Task
    {
        public int TaskId { get; set; }
        public string TaskName { get; set; }
        public string Description { get; set; }
        public DateTime DueDate { get; set; }
        public bool IsCompleted { get; set; }
        public int EmployeeId { get; set; }
        public Employee Employee { get; set; }
    }

    public class Notification
    {
        public int NotificationId { get; set; }
        public string Message { get; set; }
        public DateTime Date { get; set; }
        public bool IsRead { get; set; }
        public int EmployeeId { get; set; }
        public Employee Employee { get; set; }
    }

    public class Report
    {
        public int ReportId { get; set; }
        public string ReportName { get; set; }
        public string ReportContent { get; set; }
        public DateTime GeneratedOn { get; set; }
        public int EmployeeId { get; set; }
        public Employee Employee { get; set; }
    }

    public class EmployeeManagementContext : DbContext
    {
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Attendance> Attendances { get; set; }
        public DbSet<Payroll> Payrolls { get; set; }
        public DbSet<PerformanceReview> PerformanceReviews { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Task> Tasks { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<Report> Reports { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("YourConnectionStringHere");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Employee>()
                .HasMany(e => e.Attendances)
                .WithOne(a => a.Employee)
                .HasForeignKey(a => a.EmployeeId);

            modelBuilder.Entity<Employee>()
                .HasMany(e => e.Payrolls)
                .WithOne(p => p.Employee)
                .HasForeignKey(p => p.EmployeeId);

            modelBuilder.Entity<Employee>()
                .HasMany(e => e.PerformanceReviews)
                .WithOne(pr => pr.Employee)
                .HasForeignKey(pr => pr.EmployeeId);

            modelBuilder.Entity<Employee>()
                .HasMany(e => e.Tasks)
                .WithOne(t => t.Employee)
                .HasForeignKey(t => t.EmployeeId);

            modelBuilder.Entity<Employee>()
                .HasMany(e => e.Notifications)
                .WithOne(n => n.Employee)
                .HasForeignKey(n => n.EmployeeId);

            modelBuilder.Entity<Employee>()
                .HasMany(e => e.Reports)
                .WithOne(r => r.Employee)
                .HasForeignKey(r => r.EmployeeId);

            modelBuilder.Entity<Role>()
                .HasMany(r => r.Employees)
                .WithOne(e => e.Role)
                .HasForeignKey(e => e.RoleId);
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            using (var context = new EmployeeManagementContext())
            {
                // Add a new role
                var role = new Role
                {
                    RoleName = "Administrator"
                };
                context.Roles.Add(role);
                context.SaveChanges();

                // Add a new employee
                var employee = new Employee
                {
                    FirstName = "John",
                    LastName = "Doe",
                    Position = "Software Engineer",
                    DateOfJoining = DateTime.Now,
                    RoleId = role.RoleId
                };
                context.Employees.Add(employee);
                context.SaveChanges();

                // Add attendance record
                var attendance = new Attendance
                {
                    Date = DateTime.Today,
                    IsPresent = true,
                    EmployeeId = employee.EmployeeId
                };
                context.Attendances.Add(attendance);
                context.SaveChanges();

                // Add payroll record
                var payroll = new Payroll
                {
                    Salary = 5000,
                    PaymentDate = DateTime.Today,
                    EmployeeId = employee.EmployeeId
                };
                context.Payrolls.Add(payroll);
                context.SaveChanges();

                // Add performance review
                var review = new PerformanceReview
                {
                    ReviewDate = DateTime.Today,
                    Comments = "Excellent performance",
                    EmployeeId = employee.EmployeeId
                };
                context.PerformanceReviews.Add(review);
                context.SaveChanges();

                // Assign a task
                var task = new Task
                {
                    TaskName = "Complete Module 1",
                    Description = "Work on the first module of Project A",
                    DueDate = DateTime.Today.AddDays(7),
                    IsCompleted = false,
                    EmployeeId = employee.EmployeeId
                };
                context.Tasks.Add(task);
                context.SaveChanges();

                // Send a notification
                var notification = new Notification
                {
                    Message = "Your leave request has been approved",
                    Date = DateTime.Today,
                    IsRead = false,
                    EmployeeId = employee.EmployeeId
                };
                context.Notifications.Add(notification);
                context.SaveChanges();

                // Generate a report
                var report = new Report
                {
                    ReportName = "Monthly Attendance Report",
                    ReportContent = "Attendance report content",
                    GeneratedOn = DateTime.Today,
                    EmployeeId = employee.EmployeeId
                };
                context.Reports.Add(report);
                context.SaveChanges();

                // Mark the task as completed
                task.IsCompleted = true;
                context.Tasks.Update(task);
                context.SaveChanges();

                // Mark the notification as read
                notification.IsRead = true;
                context.Notifications.Update(notification);
                context.SaveChanges();

                // Display all employees with their roles
                var employees = context.Employees.Include(e => e.Role);
                foreach (var emp in employees)
                {
                    Console.WriteLine($"{emp.FirstName} {emp.LastName} - {emp.Role.RoleName}");
                }

                // Display tasks of the employee
                var tasks = context.Tasks.Where(t => t.EmployeeId == employee.EmployeeId);
                foreach (var t in tasks)
                {
                    Console.WriteLine($"Task: {t.TaskName}, Completed: {t.IsCompleted}");
                }

                // Display notifications of the employee
                var notifications = context.Notifications.Where(n => n.EmployeeId == employee.EmployeeId);
                foreach (var n in notifications)
                {
                    Console.WriteLine($"Notification: {n.Message}, Read: {n.IsRead}");
                }

                // Display reports generated by the employee
                var reports = context.Reports.Where(r => r.EmployeeId == employee.EmployeeId);
                foreach (var r in reports)
                {
                    Console.WriteLine($"Report: {r.ReportName}, Generated On: {r.GeneratedOn}");
                }
            }
        }
    }
}
