using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using ToDoList.Models;
using ToDoList.Data;

namespace ToDoList.Controllers
{
    public class PersonalController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _context;

        public PersonalController(UserManager<ApplicationUser> userManager, ApplicationDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            ViewBag.Subskribers = _context.UserSubscribers.Where(x => x.Subscriber.Id == user.Id).Count();
            ViewBag.Subscriptions = _context.UserSubscribers.Where(x => x.Subscribioner.Id == user.Id).Count();
            ViewBag.ToDo = _context
                .TasksToDo
                .Where(x => x.ApplicationUserId == user.Id 
                && x.FinishTime.Value.Date == DateTime.Now.Date 
                && !x.IsArchive)
                .Count();
            ViewBag.AllTask = _context
                .TasksToDo
                .Where(x => x.ApplicationUserId == user.Id 
                && x.FinishTime != DateTime.Now 
                && !x.IsArchive)
                .Count();
            ViewBag.Archiv = _context
                .TasksToDo
                .Where(x => x.ApplicationUserId == user.Id 
                && x.FinishTime == DateTime.Now 
                && x.IsArchive)
                .Count();

            return View(user);
        }

        //public async Task<IActionResult> ShowSubscribers()
        //{
        //    //var subscribers 
        //    return View();
        //}

        public async Task<IActionResult> Subscribe([FromRoute]string id)
        {
            var userId = (await _userManager.GetUserAsync(HttpContext.User)).Id;
            var sabscriber = _userManager.Users.Where(u => u.Id == userId).FirstOrDefault();
            var subscribioner = _userManager.Users.Where(u => u.Id == id).FirstOrDefault();

            _context.UserSubscribers.Add(new UserSubscriber
            {
                Subscriber = sabscriber,
                Subscribioner = subscribioner
            });

            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Unsubscribe([FromRoute]string id)
        {
            var userId = (await _userManager.GetUserAsync(HttpContext.User)).Id;
            var subToRemove = _context.UserSubscribers.Where(x => x.Subscriber.Id == userId &&
                x.Subscribioner.Id == id).FirstOrDefault();
            _context.UserSubscribers.Remove(subToRemove);

            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> ShowAllSubscribers()
        {
            var userId = (await _userManager.GetUserAsync(HttpContext.User)).Id;
            var subscribersId = _context
                .UserSubscribers
                .Where(x => x.Subscriber.Id == userId)
                .Select(x => x.Subscribioner)
                .Select(y => y.Id)
                .ToList();

            var allUser = _context.Users
                .Where(x => subscribersId.Contains(x.Id))
                .ToList();

            return View(allUser);
        }

        public async Task<IActionResult> ShowAllSubscribtions()
        {
            var userId = (await _userManager.GetUserAsync(HttpContext.User)).Id;
            var subscribtionsId = _context
                .UserSubscribers
                .Where(x => x.Subscribioner.Id == userId)
                .Select(x => x.Subscriber)
                .Select(y => y.Id)
                .ToList();

            var allUser = _context.Users
                .Where(x => subscribtionsId.Contains(x.Id))
                .ToList();

            return View(allUser);
        }
    }
}