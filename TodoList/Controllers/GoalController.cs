using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TodoList.DAL.Entities;
using TodoList.DAL.Repositories;

namespace TodoList.Controllers
{
    [Authorize]
    public class GoalController : Controller
    {
        private readonly ILogger<GoalController> _logger;
        private readonly GoalRepository _goalRepository;

        public GoalController(ILogger<GoalController> logger, GoalRepository goalRepository)
        {
            _logger = logger;
            _goalRepository = goalRepository;
        }

        public async Task<ActionResult> Index(int? id)
        {
            if (id != null)
            {
                var goal = await _goalRepository.GetGoalIncludeAsync(id.Value);
                if (goal != null)
                {
                    return View(goal);
                }
            }

            return NotFound();
        }

        #region Creation
        public IActionResult Create()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> Create(Goal goal)
        {
            var userid = Convert.ToInt32(Request.Cookies["UserId"]);
            goal.UserId = userid;

            if (!ModelState.IsValid)
            {
                return View(goal);
            }

            await _goalRepository.CreateGoalAsync(goal);

            return RedirectToAction("Index", "Home");
        }
        #endregion


        [HttpPost]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id != null)
            {
                await _goalRepository.DeleteGoalAsync(id.Value);

                return RedirectToAction("Index", "Home");
            }

            return NotFound();
        }


        #region Edition
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Edit(int? id)
        {

            if (id != null)
            {
                var goal = await _goalRepository.GetGoalAsync(id.Value);
                if (goal != null)
                {
                    return View(goal);
                }
            }

            return NotFound();
        }


        [HttpPost]
        public async Task<IActionResult> Edit(Goal goal)
        {
            var userid = Convert.ToInt32(Request.Cookies["UserId"]);
            goal.UserId = userid;

            if (!ModelState.IsValid)
            {
                return View(goal);
            }

            await _goalRepository.EditGoalAsync(goal);

            return RedirectToAction("Index", "home");
        }
        #endregion

        [HttpPost]
        public async Task<IActionResult> SwitchStatus(int? id)
        {
            if (id != null)
            {
                await _goalRepository.SwitchGoalStatusAsync(id.Value);

                return RedirectToAction("Index", "Home");
            }

            return NotFound();
        }
    }
}
