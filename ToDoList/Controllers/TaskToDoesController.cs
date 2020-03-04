using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using ToDoList.Data;
using ToDoList.Models;

namespace ToDoList.Controllers
{
    [Authorize]
    public class TaskToDoesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> signInManager;

        public TaskToDoesController(ApplicationDbContext context, UserManager<ApplicationUser> signInManager)
        {
            _context = context;
            this.signInManager = signInManager;
        }

        // GET: TaskToDoes
        [Authorize]
        public async Task<IActionResult> Index(SortState sortOrder = SortState.FinishTimeAsc)
        {
            var user = await signInManager.GetUserAsync(HttpContext.User);
            var tasks = await _context.TasksToDo.Where(task => !task.IsArchive && (DateTime.Now.Date.AddDays(-1)
            < task.FinishTime.Value) && (DateTime.Now.Date.AddDays(1)
            > task.FinishTime.Value) && task.ApplicationUserId == user.Id).ToListAsync();

            AddSingleTasks(tasks);

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
            

            return View(tasks);
        }

        [AllowAnonymous]
        public async Task<IActionResult> ShowAllTasks(SortState sortOrder = SortState.FinishTimeAsc)
        {
            var user = await signInManager.GetUserAsync(HttpContext.User);
            var tasks = await _context.TasksToDo.Where(task => 
            !task.IsArchive && task.ApplicationUserId == user.Id).ToListAsync();

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

        // GET: TaskToDoes/Details/5
        [AllowAnonymous]
        public async Task<IActionResult> Details([FromRoute]int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var taskToDo = await _context.TasksToDo
                .Include(t => t.ApplicationUser)
                .FirstOrDefaultAsync(m => m.TaskToDoId == id);
            taskToDo.SingleTasks = await  _context.SingleTasks.Where(x => x.TaskToDoId == id).ToListAsync();
            if (taskToDo == null)
            {
                return NotFound();
            }

            return View(taskToDo);
        }

        // GET: TaskToDoes/Create
        [Authorize(Roles = "user, admin")]
        public IActionResult Create()
        {
            ViewData["ApplicationUserId"] = new SelectList(_context.Users, "Id", "Id");
            return View();
        }

        // POST: TaskToDoes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("TaskToDoId,Title,Description,StartTime," +
            "FinishTime,ApplicationUserId")] TaskToDo taskToDo)
        {
            var user = await signInManager.GetUserAsync(HttpContext.User);
            if (ModelState.IsValid)
            {
                if (taskToDo.StartTime == null)
                {
                    taskToDo.StartTime = DateTime.Now;
                }
                if (taskToDo.FinishTime == null)
                {
                    taskToDo.FinishTime = DateTime.Now.AddDays(1);
                }
                taskToDo.ApplicationUserId = user.Id;
                _context.Add(taskToDo);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ApplicationUserId"] = new SelectList(_context.Users, "Id", "Id", 
                taskToDo.ApplicationUserId);
            return View(taskToDo);
        }

        // GET: TaskToDoes/Edit/5
        
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var taskToDo = await _context.TasksToDo.FindAsync(id);
            if (taskToDo == null)
            {
                return NotFound();
            }
            ViewData["ApplicationUserId"] = new SelectList(_context.Users, "Id", "Id", taskToDo.ApplicationUserId);
            return View(taskToDo);
        }

        // POST: TaskToDoes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("TaskToDoId,Title,Description,StartTime," +
            "FinishTime,ApplicationUserId")] TaskToDo taskToDo)
        {
            if (id != taskToDo.TaskToDoId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(taskToDo);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TaskToDoExists(taskToDo.TaskToDoId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["ApplicationUserId"] = new SelectList(_context.Users, "Id", "Id", 
                taskToDo.ApplicationUserId);
            return View(taskToDo);
        }

        // GET: TaskToDoes/Delete/5
        public async Task<IActionResult> Delete(int? id)
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

            return View(taskToDo);
        }

        // POST: TaskToDoes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var taskToDo = await _context.TasksToDo.FindAsync(id);
            _context.TasksToDo.Remove(taskToDo);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TaskToDoExists(int id)
        {
            return _context.TasksToDo.Any(e => e.TaskToDoId == id);
        }

        public IActionResult IsCompleteTask(int id)
        {
            var taskToDo = _context.TasksToDo.Find(id);

            taskToDo.IsComplete = !taskToDo.IsComplete;
            _context.SaveChanges();

            return RedirectToAction("Index");
        }

        public IActionResult DeleteAllComplete()
        {
            var CompletedTaskToDo = _context.TasksToDo.Where(m => m.IsComplete == true).ToList();

            _context.TasksToDo.RemoveRange(CompletedTaskToDo);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> SendToArchive(int? id)
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
            _context.SaveChanges();

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> SendToArchiveFromAllTasks(int? id)
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
            _context.SaveChanges();

            return RedirectToAction("ShowAllTask");
        }

        public async Task<IActionResult> OnTaskClick(int? id)
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

            if (taskToDo.SingleTasks.Count != 0)
            {
                return RedirectToAction("Index");
            }

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
