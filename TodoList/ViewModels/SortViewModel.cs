using Microsoft.AspNetCore.Mvc.Rendering;
using TodoList.DAL.Entities;

namespace TodoList.ViewModels
{
    public class SortViewModel
    {
        public SortState NameSort { get; set; } // значение для сортировки по имени
        public SortState TimeSort { get; set; }   // значение для сортировки по времени
        public SortState TypeSort { get; set; }   // значение для сортировки по типу
        public SortState StatusSort { get; set; } //значение для сортировки по статусу
        public SortState Current { get; set; }     // значение свойства, выбранного для сортировки
        public bool Up { get; set; }  // Сортировка по возрастанию или убыванию

        public SortViewModel(SortState sortOrder)
        {
            // значения по умолчанию
            NameSort = SortState.NameAsc;
            TypeSort = SortState.TypeAsc;
            TimeSort = SortState.TimeAsc;
            StatusSort = SortState.StatusAsc;
            Up = true;

            if (sortOrder == SortState.NameDesc
                || sortOrder == SortState.TypeDesc
                || sortOrder == SortState.TimeDesc
                || sortOrder == SortState.StatusDesc)
            {
                Up = false;
            }

            switch (sortOrder)
            {
                case SortState.NameDesc:
                    Current = NameSort = SortState.NameAsc;
                    break;
                case SortState.TypeAsc:
                    Current = TypeSort = SortState.TypeDesc;
                    break;
                case SortState.TypeDesc:
                    Current = TypeSort = SortState.TypeAsc;
                    break;
                case SortState.TimeAsc:
                    Current = TimeSort = SortState.TimeDesc;
                    break;
                case SortState.TimeDesc:
                    Current = TimeSort = SortState.TimeAsc;
                    break;
                case SortState.StatusAsc:
                    Current= StatusSort = SortState.StatusDesc;
                    break;
                case SortState.StatusDesc:
                    Current= StatusSort = SortState.StatusAsc;
                    break;
                default:
                    Current = NameSort = SortState.NameDesc;
                    break;
            }
        }
    }
}
