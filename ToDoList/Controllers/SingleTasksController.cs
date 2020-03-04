using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ToDoList.Data;
using ToDoList.Models;

namespace ToDoList.Controllers
{
    public class SingleTasksController : Controller
    {
        private readonly ApplicationDbContext _context;

        public SingleTasksController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: SingleTasks
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.SingleTasks.Include(s => s.TaskToDo);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: SingleTasks/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var singleTask = await _context.SingleTasks
                .Include(s => s.TaskToDo)
                .FirstOrDefaultAsync(m => m.SingleTaskId == id);
            if (singleTask == null)
            {
                return NotFound();
            }

            return View(singleTask);
        }

        // GET: SingleTasks/Create
        [HttpGet("SingleTasks/Create/{id}")]
        public IActionResult Create([FromRoute] int id)
        {
            ViewBag.TaskToDoId = id;
            return View();
        }

        // POST: SingleTasks/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("SingleTaskId,Label,TaskToDoId")] SingleTask singleTask)
        {
            if (ModelState.IsValid)
            {
                _context.Add(singleTask);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", "TaskToDoes");
            }
            ViewData["TaskToDoId"] = new SelectList(_context.TasksToDo, "TaskToDoId", 
                "TaskToDoId", singleTask.TaskToDoId);
            return View(singleTask);
        }

        // GET: SingleTasks/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var singleTask = await _context.SingleTasks.FindAsync(id);
            if (singleTask == null)
            {
                return NotFound();
            }
            ViewData["TaskToDoId"] = new SelectList(_context.TasksToDo, "TaskToDoId", "TaskToDoId", singleTask.TaskToDoId);
            return View(singleTask);
        }

        // POST: SingleTasks/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("SingleTaskId,Label,TaskToDoId")] SingleTask singleTask)
        {
            if (id != singleTask.SingleTaskId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(singleTask);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SingleTaskExists(singleTask.SingleTaskId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Details", "TaskToDoes", new { id = singleTask.TaskToDoId });
            }
            ViewData["TaskToDoId"] = new SelectList(_context.TasksToDo, "TaskToDoId", 
                "TaskToDoId", singleTask.TaskToDoId);
            return View(singleTask);
        }

        // GET: SingleTasks/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            
            var singleTask = await _context.SingleTasks
                .Include(s => s.TaskToDo)
                .FirstOrDefaultAsync(m => m.SingleTaskId == id);
            _context.SingleTasks.Remove(singleTask);
            await _context.SaveChangesAsync();
            if (singleTask == null)
            {
                return NotFound();
            }

            return RedirectToAction("Details", "TaskToDoes", 
                new { id = singleTask.TaskToDoId });
        }

        // POST: SingleTasks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var singleTask = await _context.SingleTasks.FindAsync(id);
            _context.SingleTasks.Remove(singleTask);
            await _context.SaveChangesAsync();
            return RedirectToAction("Details", "TaskToDoes");
        }

        private bool SingleTaskExists(int id)
        {
            return _context.SingleTasks.Any(e => e.SingleTaskId == id);
        }

        public IActionResult IsComplete(int id)
        {
            var singleTask = _context.SingleTasks.Find(id);

            singleTask.IsComplete = !singleTask.IsComplete;
            _context.SaveChanges();

            return RedirectToAction("Details", "TaskToDoes", new { id = singleTask.TaskToDoId });
        }

        public IActionResult IsCompleteTask(int id)
        {
            var singleTaskToDo = _context.SingleTasks.Find(id);

            singleTaskToDo.IsComplete = !singleTaskToDo.IsComplete;
            _context.SaveChanges();

            return RedirectToAction("Index", "TaskToDoes");
        }
    }
}



