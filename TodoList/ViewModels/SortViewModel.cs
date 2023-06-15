using Microsoft.AspNetCore.Mvc.Rendering;
using TodoList.DAL.Entities;

namespace TodoList.ViewModels
{
    public class SortViewModel
    {
        public SortState NameSort { get; set; } 
        public SortState TimeSort { get; set; }   
        public SortState TypeSort { get; set; }   
        public SortState StatusSort { get; set; } 
        public SortState Current { get; set; }     
        public bool Up { get; set; }  

        public SortViewModel(SortState sortOrder)
        {
            
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
