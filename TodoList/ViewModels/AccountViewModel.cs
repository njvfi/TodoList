using System.ComponentModel.DataAnnotations;

namespace TodoList.ViewModels
{
    public class AccountModel
    {
        [Required(ErrorMessage = "Не вказано Ім'я")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Не вказаний пароль")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}