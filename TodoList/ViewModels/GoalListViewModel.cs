using Microsoft.AspNetCore.Mvc.Rendering;
using TodoList.DAL.Entities;

namespace TodoList.ViewModels
{
    public class GoalListViewModel
    {
        public IEnumerable<Goal> Goals { get; set; } = new List<Goal>();
        public SelectList Types { get; set; } = new SelectList(new List<DAL.Entities.Type>(), "Id", "Name");
        public string? Name { get; set; }
        public SortViewModel SortViewModel { get; set; } = new SortViewModel(SortState.NameAsc);
    }
}
