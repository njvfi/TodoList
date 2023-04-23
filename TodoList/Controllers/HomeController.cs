using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;
using TodoList.ViewModels;
using Microsoft.AspNetCore.Authorization;
using TodoList.DAL.Repositories;
using TodoList.DAL.Entities;
using Microsoft.Data.SqlClient;

namespace TodoList.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly GoalRepository _goalRepository;


        public HomeController(ILogger<HomeController> logger, GoalRepository goalRepository)
        {
            _logger = logger;
            _goalRepository = goalRepository;
            _goalRepository.InitAsync();
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<ActionResult> Index(int? type, string? name, SortState sortOrder = SortState.NameAsc)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return View();
            }
            else 
            {
				var userid = Convert.ToInt32(Request.Cookies["UserId"]);

				var goal = await _goalRepository.GetGoalsAsync(type, name, userid, sortOrder);

				var types = await _goalRepository.GetTypesAsync();
				// устанавливаем начальный элемент, который позволит выбрать всех
				types.Insert(0, new DAL.Entities.Type { Name = "Все", Id = 0 });

				GoalListViewModel viewModel = new GoalListViewModel
				{
					Goals = goal,
					Types = new SelectList(types, "Id", "Name", type),
					Name = name,
					SortViewModel = new SortViewModel(sortOrder)
				};

				return View(viewModel);
			}
        }

        public async Task<ActionResult> Filtration(int? type, string? name)
        {
            var userid = Convert.ToInt32(Request.Cookies["UserId"]);

            var goal = await _goalRepository.GetGoalsAsync(type, name, userid);

            var types = await _goalRepository.GetTypesAsync();
            // устанавливаем начальный элемент, который позволит выбрать всех
            types.Insert(0, new DAL.Entities.Type { Name = "Все", Id = 0 });

            GoalListViewModel viewModel = new GoalListViewModel
            {
                Goals = goal,
                Types = new SelectList(types, "Id", "Name", type),
                Name = name,
            };

            return View(viewModel);
        }

    }
}