using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TodoList.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using TodoList.ViewModels;
using Microsoft.AspNetCore.Authorization;

namespace TodoList.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        GoalContext db;


        #region Controller
        public HomeController(ILogger<HomeController> logger, GoalContext context)
        {
            _logger = logger;
            db = context;

            // добавим начальные данные для тестирования
            if (!db.Types.Any())
            {
                TodoList.Models.Type not_defined = new TodoList.Models.Type { Name = "Без категорії" };
                TodoList.Models.Type home = new TodoList.Models.Type { Name = "Роботи по дому" };
                TodoList.Models.Type sport = new TodoList.Models.Type { Name = "Спорт" };
                TodoList.Models.Type work = new TodoList.Models.Type { Name = "Робота" };

                Models.Goal goal1 = new Models.Goal { Name = "Ознайомитися з платформою", Description = "Ознайомитися з платформою, навчитися користуватися всіма можливостями", Type = not_defined, Status = false };

                TodoList.Models.User tom = new TodoList.Models.User { Name = "joydip", Id = 0, Password = "joydip123"};
                tom.Goals.Add(goal1);

                db.Types.AddRange(not_defined, home, home, work);
                db.Goals.AddRange(goal1);
                db.Users.AddRange(tom);
                db.SaveChanges();
            }
        }
        #endregion

        [AllowAnonymous]
        public ActionResult Index(int? type, string? name, SortState sortOrder = SortState.NameAsc)
        {
            return View();
        }

        public ActionResult TaskList(int? type, string? name, SortState sortOrder = SortState.NameAsc)
        {
            IQueryable<Models.Goal> goal = db.Goals.Include<Models.Goal, Models.Type>(p => p.Type);

            if (type != null && type != 0)
            {
                goal = goal.Where(p => p.TypeId == type);
            }
            if (!string.IsNullOrEmpty(name))
            {
                goal = goal.Where(p => p.Name!.Contains(name));
            }

            goal = sortOrder switch
            {
                SortState.NameDesc => goal.OrderByDescending(s => s.Name),
                SortState.PriceAsc => goal.OrderBy(s => s.Time),
                SortState.PriceDesc => goal.OrderByDescending(s => s.Time),
                SortState.TypeAsc => goal.OrderBy(s => s.Type!.Name),
                SortState.TypeDesc => goal.OrderByDescending(s => s.Type!.Name),
                _ => goal.OrderBy(s => s.Name),
            };

            List<TodoList.Models.Type> types = db.Types.ToList();
            // устанавливаем начальный элемент, который позволит выбрать всех
            types.Insert(0, new TodoList.Models.Type { Name = "Все", Id = 0 });

            GoalListViewModel viewModel = new GoalListViewModel
            {
                Goals = goal.ToList(),
                Types = new SelectList(types, "Id", "Name", type),
                Name = name,
                SortViewModel = new SortViewModel(sortOrder)
            };

            return View(viewModel);
        }


        public ActionResult Filtration(int? type, string? name)
        {
            IQueryable<Models.Goal> goals = db.Goals.Include<Models.Goal, Models.Type>(p => p.Type);
            if (type != null && type != 0)
            {
                goals = goals.Where(p => p.TypeId == type);
            }
            if (!string.IsNullOrEmpty(name))
            {
                goals = goals.Where(p => p.Name!.Contains(name));
            }

            List<TodoList.Models.Type> types = db.Types.ToList();
            // устанавливаем начальный элемент, который позволит выбрать всех
            types.Insert(0, new TodoList.Models.Type { Name = "Все", Id = 0 });

            GoalListViewModel viewModel = new GoalListViewModel
            {
                Goals = goals.ToList(),
                Types = new SelectList(types, "Id", "Name", type),
                Name = name
            };

            return View(viewModel);
        }

    }
}