﻿using ToDoApp.Misc;

namespace ToDoApp.Models
{
    public class UserMemoryRepository: IUserRepository
    {
        private PasswordHelper passwordHelper;
        private Dictionary<Guid, User> items = new Dictionary<Guid, User>();

        public UserMemoryRepository(PasswordHelper passwordHelper)
        {
            this.passwordHelper = passwordHelper;
            Add(new User() { EMail = "alexander.stuckenholz@hshl.de", PasswordHash = passwordHelper.ComputeSha256Hash("secret"), IsAdmin = true });
        }

        public IEnumerable<User> GetAll()
        {
            return items.Values;
        }

        public User GetSingle(Guid id)
        {
            if (items.ContainsKey(id))
                return items[id];

            return null;
        }

        public User Add(User entity)
        {
            entity.ID = Guid.NewGuid();
            items.Add(entity.ID, entity);
            return entity;
        }

        public User Update(User entity)
        {
            items[entity.ID] = entity;
            return entity;
        }

        public void Delete(Guid id)
        {
            items.Remove(id);
        }

        public User FindByEmail(string email)
        {
            foreach (var user in items.Values)
                if (user.EMail.ToLower().Equals(email))
                    return user;

            return null;
        }

        public User FindByLogin(string email, string password)
        {
            var user = FindByEmail(email);
            if (user is null)
                return null;

            var passwordHash = passwordHelper.ComputeSha256Hash(password);
            if (user.PasswordHash.Equals(passwordHash))
                return user;

            return null;
        }

        public User FindByPasswordResetToken(string token)
        {
            foreach (var user in items.Values)
                if (user.PasswordResetToken.Equals(token))
                    return user;

            return null;
        }
    }
}
