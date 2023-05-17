using EntityFrameworkExample.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityFrameworkExample.Repositories
{
    public class UserRepository
    {
        public User FindById(int id)
        {
            User user;
            using (var db = new AppContext())
            {
                user = db.Users.Where(user => user.Id == id).ToList().FirstOrDefault();
            }
            return user;
        }
        public List<User> FindAll()
        {
            List<User> users;
            using (var db = new AppContext())
            {
                users = db.Users.ToList();
            }
            return users;
        }
        public void AddUser(string name, string email)
        {
            using (var db = new AppContext())
            {
                var user = new User { Name = name, Email = email };
                db.Users.Add(user);
                db.SaveChanges();
            }
        }
        public void RemoveUser(User user)
        {
            using (var db = new AppContext())
            {
                db.Users.Remove(user);
                db.SaveChanges();
            }
        }
        public void UpdateUserName(int id, string newName)
        {
            using (var db = new AppContext())
            {
                var user = db.Users.Where(_ => _.Id == id).FirstOrDefault();
                user.Name = newName;
                db.SaveChanges();
            }
        }
        // Получаем количество книг на руках у пользователя
        public int CountUserBooks(int id)
        {
            int count;
            using (var db = new AppContext())
            {
                count = db.Users.Join(db.Books, u => u.Id, b => b.UserId, (u, b) => new { u.Id, b.Name }).Where(_ => _.Id == id).Count();
            }
            return count;
        }
    }
}
