﻿using Microsoft.AspNetCore.Mvc.Rendering;

namespace TodoList.Models
{
    public class IndexViewModel
    {
        public IEnumerable<Product> Products { get; set; } = new List<Product>();
        public SelectList Types { get; set; } = new SelectList(new List<Type>(), "Id", "Name");
        public string? Name { get; set; }
        public SortViewModel SortViewModel { get; set; } = new SortViewModel(SortState.NameAsc);
    }
}