
using System.Collections;

namespace ToDoApp.Models
{
    public class ToDoMemoryRepository : IToDoRepository
    {
        protected Dictionary<Guid, ToDo> items = new Dictionary<Guid, ToDo>();

        private IEnumerable<object> OrderBy(IEnumerable<object> list, string properytName)
        {
            var prop = list.First().GetType().GetProperty(properytName);
            return list.OrderBy(x => prop?.GetValue(x, null));
        }
        public ToDo Add(ToDo entity)
        {
            entity.ID = Guid.NewGuid();
            items.Add(entity.ID, entity);
            return entity;
        }

        public void Delete(Guid id)
        {
            items.Remove(id);
        }

        public IEnumerable<ToDo> GetAll()
        {
            return items.Values;
        }

        public ToDo GetSingle(Guid id)
        {
            if (items.ContainsKey(id))
                return items[id];

            return null;
        }

        public ToDo Update(ToDo entity)
        {
            items[entity.ID] = entity;
            return entity;
        }
    }
}
