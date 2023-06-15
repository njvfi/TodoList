using System.ComponentModel.DataAnnotations;

namespace TodoList.DAL.Entities
{
    public class User
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Обов'язкове поле")]
        public string Email { get; set; }
        
        [Required(ErrorMessage ="Обов'язкове поле")]
        public string Name { get; set; }
        
        [Required(ErrorMessage = "Обов'язкове поле")]
        public string Password { get; set; }
        public List<Goal> Goals { get; set; } = new();
    }
}
