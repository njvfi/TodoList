using Microsoft.AspNetCore.Mvc.Rendering;
using TodoList.Models;

namespace TodoList.ViewModels
{
    public class IndexViewModel
    {
        public IEnumerable<Models.Goal> Products { get; set; } = new List<Models.Goal>();
        public SelectList Types { get; set; } = new SelectList(new List<Models.Type>(), "Id", "Name");
        public string? Name { get; set; }
        public SortViewModel SortViewModel { get; set; } = new SortViewModel(SortState.NameAsc);
    }
}