using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TodoList.Models;

namespace TodoList.Controllers
{
    [Authorize]
    public class GoalController : Controller
    {
        private readonly ILogger<GoalController> _logger;
        GoalContext db;

        public GoalController(ILogger<GoalController> logger, GoalContext context)
        {
            _logger = logger;
            db = context;
        }

        public async Task<ActionResult> Index(int? id)
        {
            if (id != null)
            {
                Models.Goal? product = await db.Goals.Include<Models.Goal, Models.Type>(pr => pr.Type).FirstOrDefaultAsync(p => p.Id == id);
                if (product != null) return View(product);
            }

            return NotFound();
        }

        #region Creation
        public IActionResult Create()
        {
            List<TodoList.Models.Type> types = db.Types.ToList();

            return View();
        }


        [HttpPost]
        public async Task<IActionResult> Create(Models.Goal goal)
        {
            if (!ModelState.IsValid)
            {
                return View(goal);
            }

            db.Goals.Add(goal);
            await db.SaveChangesAsync();

            return RedirectToAction("TaskList", "Home");
        }
        #endregion


        [HttpPost]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id != null)
            {
                Models.Goal goal = new Models.Goal { Id = id.Value };
                db.Entry(goal).State = EntityState.Deleted;
                await db.SaveChangesAsync();

                return RedirectToAction("TaskList", "Home");
            }

            return NotFound();
        }


        #region Edition
        [Authorize]
        public async Task<IActionResult> Edit(int? id)
        {
            List<TodoList.Models.Type> companies = db.Types.ToList();

            if (id != null)
            {
                Models.Goal? goal = await db.Goals.FirstOrDefaultAsync(p => p.Id == id);
                if (goal != null) return View(goal);
            }

            return NotFound();
        }


        [HttpPost]
        public async Task<IActionResult> Edit(Models.Goal goal)
        {
            if (!ModelState.IsValid)
            {
                return View(goal);
            }

            db.Goals.Update(goal);
            await db.SaveChangesAsync();

            return RedirectToAction("TaskList", "home");
        }
        #endregion

        [HttpPost]
        public async Task<IActionResult> ChangeStatusDone(int? id, bool Status)
        {
            if (id != null)
            {
                Models.Goal? goal = await db.Goals.FirstOrDefaultAsync(p => p.Id == id);
                goal.Status = true;
                db.Goals.Update(goal);
                await db.SaveChangesAsync();

                return RedirectToAction("TaskList", "Home");
            }

            return NotFound();
        }
        [HttpPost]
        public async Task<IActionResult> ChangeStatusUndone(int? id, bool Status)
        {
            if (id != null)
            {
                Models.Goal? goal = await db.Goals.FirstOrDefaultAsync(p => p.Id == id);
                goal.Status = false;
                db.Goals.Update(goal);
                await db.SaveChangesAsync();

                return RedirectToAction("TaskList", "Home");
            }

            return NotFound();
        }
    }
}
