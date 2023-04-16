using System.ComponentModel.DataAnnotations;

namespace TodoList.Models
{
    public class Goal
    {
        public int Id { get; set; }

        [Required(ErrorMessage ="Обязательное поле")]
        public string? Name { get; set; }
        public string? Description { get; set; }
        public Boolean Status { get; set; }
        [DataType(DataType.DateTime)]
        public DateTime Time { get; set; }
        public int? TypeId { get; set; }
        public Type? Type { get; set; }
    }
}
