namespace ToDoApp.Models
{
    public interface IToDoRepository
    {
       // ToDoListResult GetAll(ToDoFilter filter);
        IEnumerable<ToDo> GetAll();
        ToDo GetSingle(Guid id);
        ToDo Add(ToDo entity);
        void Delete(Guid id);
        ToDo Update(ToDo entity);
    }
}
