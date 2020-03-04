using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ToDoList.Data;
using ToDoList.Models;

namespace ToDoList.Controllers
{
    public class SearchController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public SearchController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public IActionResult SearchResult(string searchString)
        {
            List<SubscribeModel> subList = new List<SubscribeModel>(); 
            var searchResult = _context.Users.Where(user => user.UserName.Contains(searchString)).ToList();
            var userId = _userManager.GetUserId(HttpContext.User);
            var allSubscribtions = _context.UserSubscribers
                .Where(x => x.Subscriber.Id == userId)
                .Select(x => x.Subscribioner)
                .ToList();

            foreach (var item in searchResult)
            {
                SubscribeModel newSubscriber = new SubscribeModel();
                newSubscriber.User = item;

                if(allSubscribtions.Contains(item))
                {
                    newSubscriber.IsSub = true;
                }
                else
                {
                    newSubscriber.IsSub = false;
                }

                subList.Add(newSubscriber);
            }
            return View(subList);
        }
    }
}