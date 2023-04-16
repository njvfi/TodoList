using Microsoft.AspNetCore.Mvc.Rendering;
using TodoList.Models;


namespace TodoList.ViewModels
{
    public class GoalListViewModel
    {
        public IEnumerable<Models.Goal> Goals { get; set; } = new List<Models.Goal>();
        public SelectList Types { get; set; } = new SelectList(new List<Models.Type>(), "Id", "Name");
        public string? Name { get; set; }
        public SortViewModel SortViewModel { get; set; } = new SortViewModel(SortState.NameAsc);
    }
}
