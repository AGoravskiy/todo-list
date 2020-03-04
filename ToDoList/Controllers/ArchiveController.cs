using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ToDoList.Data;
using ToDoList.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace ToDoList.Controllers
{
    public class ArchiveController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> signInManager;

        public ArchiveController(ApplicationDbContext context, UserManager<ApplicationUser> signInManager)
        {
            _context = context;
            this.signInManager = signInManager;
        }

        public async Task<IActionResult> Index(SortState sortOrder = SortState.FinishTimeAsc)
        {
            var user = await signInManager.GetUserAsync(HttpContext.User);
            var tasks = await _context.TasksToDo.Where(task => task.IsArchive && task.ApplicationUserId == user.Id).ToListAsync();
            
            ViewData["TitleSort"] = sortOrder == SortState.TitleAsc ? SortState.TitleDesc : SortState.TitleAsc;
            ViewData["FinishTimeSort"] = sortOrder == SortState.FinishTimeAsc ?
                SortState.FinishTimeDesc : SortState.FinishTimeAsc;
            ViewData["IsCompleteSort"] = sortOrder == SortState.IsCompleteFirst ?
                SortState.IsCompleteSec : SortState.IsCompleteFirst;

            switch (sortOrder)
            {
                case SortState.TitleDesc:
                    tasks = tasks.OrderByDescending(s => s.Title).ToList();
                    break;
                case SortState.TitleAsc:
                    tasks = tasks.OrderBy(s => s.Title).ToList();

                    break;
                case SortState.FinishTimeDesc:
                    tasks = tasks.OrderByDescending(s => s.FinishTime.Value).ToList();
                    break;
                case SortState.IsCompleteFirst:
                    tasks = tasks.OrderBy(s => s.IsComplete).ToList();
                    break;
                case SortState.IsCompleteSec:
                    tasks = tasks.OrderByDescending(s => s.IsComplete).ToList();
                    break;
                default:
                    tasks = tasks.OrderBy(s => s.FinishTime).ToList();
                    break;
            }

            var dateOfTasks = tasks.Select(x => x.FinishTime).ToList();

            var simpleDates = new List<DateTime>();

            foreach (var item in dateOfTasks)
            {
                simpleDates.Add(item.Value.Add(-item.Value.TimeOfDay));
            }

            AddSingleTasks(tasks);

            simpleDates = simpleDates.Distinct().ToList();

            var tasksByDate = new Dictionary<DateTime, List<TaskToDo>>();

            foreach (var item in simpleDates)
            {
                tasksByDate[item] = tasks.Where(x => x.FinishTime.Value.Date == item).ToList();
            }
            return View(tasksByDate);
        }

        public async Task<IActionResult> SendToTasks(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var taskToDo = await _context.TasksToDo
                .Include(t => t.ApplicationUser)
                .FirstOrDefaultAsync(m => m.TaskToDoId == id);

            if (taskToDo == null)
            {
                return NotFound();
            }

            taskToDo.IsArchive = !taskToDo.IsArchive;
            taskToDo.StartTime = DateTime.Now;
            taskToDo.FinishTime = DateTime.Now.AddDays(1);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }

        public void AddSingleTasks(List<TaskToDo> tasks)
        {
            foreach (var task in tasks)
            {
                var taskId = task.TaskToDoId;
                task.SingleTasks = _context.SingleTasks.Where(s => s.TaskToDoId == taskId).ToList();
            }
        }
    }
}