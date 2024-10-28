namespace ToDoApp.Models
{
    public interface IToDoRepository
    {
        
        IEnumerable<ToDo> GetAll(ToDoFilter filter);
        ToDo GetSingle(Guid id);
        ToDo Add(ToDo entity);
        void Delete(Guid id);
        ToDo Update(ToDo entity);
    }
}
