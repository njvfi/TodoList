namespace TodoList.DAL.Entities
{
    public class Type
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public List<Goal> Goals { get; set; } = new();
    }
}
