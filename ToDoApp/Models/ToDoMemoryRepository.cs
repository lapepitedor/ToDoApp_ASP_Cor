
using System.Collections;

namespace ToDoApp.Models
{
    public class ToDoMemoryRepository : IToDoRepository
    {
        protected Dictionary<Guid, ToDo> items = new Dictionary<Guid, ToDo>();

        public ToDoMemoryRepository()
        {
            Add(new ToDo() { Title = "Learn ASP.net Core" });
            Add(new ToDo() { Title = "Learn italian" });
        }

        public IEnumerable<ToDo> GetAll()
        {
            return items.Values;
        }

        private IEnumerable<ToDo> Filter(IEnumerable<ToDo> input, ToDoFilter filter)
        {
            if (filter is null || filter.FilterExpressions is null || filter.FilterExpressions.Count() == 0)
                return input;

            var result = new List<ToDo>();
            foreach (var item in input)
            {
                bool applies = true;
                foreach (var expr in filter.FilterExpressions)
                    applies = applies && CompliesFilter(item, expr);

                if (applies)
                    result.Add(item);
            }

            return result;
        }

        private IEnumerable<ToDo> Order(IEnumerable<ToDo> input, ToDoFilter filter)
        {
            if (filter is null || string.IsNullOrEmpty(filter.OrderBy) || input.Count() == 0)
                return input;

            var prop = input.First().GetType().GetProperty(filter.OrderBy);
            return input.OrderBy(x => prop.GetValue(x, null));
        }

        private IEnumerable<ToDo> Paginate(IEnumerable<ToDo> input, ToDoFilter filter)
        {
            if (filter is null || filter.StartPage == -1)
                return input;

            return input.Skip((filter.StartPage - 1) * filter.ItemsPerPage).Take(filter.ItemsPerPage);
        }

        public IEnumerable<ToDo> GetAll(ToDoFilter filter)
        {
            var objects = Filter(items.Values, filter);
            objects = Order(objects, filter);
            var result = Paginate(objects, filter);
            return result;
        }

        private bool CompliesFilter(Object obj, FilterExpression expression)
        {
            var prop = obj.GetType().GetProperty(expression.PropertyName);
            dynamic value = prop.GetValue(obj, null);
            dynamic comparerValue = Convert.ChangeType(expression.Value, prop.PropertyType);

            switch (expression.Relation)
            {
                case RelationType.Equal: return value == comparerValue;
                case RelationType.NotEqual: return value != comparerValue;
                case RelationType.Larger: return value > comparerValue;
                case RelationType.LargerOrEqual: return value >= comparerValue;
                case RelationType.Smaller: return value < comparerValue;
                case RelationType.SmallerOrEqual: return value <= comparerValue;
            }

            return false;
        }

        private IEnumerable<object> OrderBy(IEnumerable<object> list, string properytName)
        {
            var prop = list.First().GetType().GetProperty(properytName);
            return list.OrderBy(x => prop?.GetValue(x, null));
        }

        public ToDo GetSingle(Guid id)
        {
            if (items.ContainsKey(id))
                return items[id];

            return null;
        }

        public ToDo Add(ToDo entity)
        {
            entity.ID = Guid.NewGuid();
            items.Add(entity.ID, entity);
            return entity;
        }

        public ToDo Update(ToDo entity)
        {
            items[entity.ID] = entity;
            return entity;
        }

        public void Delete(Guid id)
        {
            items.Remove(id);
        }
    }
}