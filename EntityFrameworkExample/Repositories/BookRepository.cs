using EntityFrameworkExample.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityFrameworkExample.Repositories
{
    public class BookRepository
    {
        public Book FindById(int id)
        {
            Book book;
            using (var db = new AppContext())
            {
                book = db.Books.Where(book => book.Id == id).ToList().FirstOrDefault();
            }
            return book;
        }
        public List<Book> FindAll()
        {
            List<Book> book;
            using (var db = new AppContext())
            {
                book = db.Books.ToList();
            }
            return book;
        }
        public void AddBook(string name, int year)
        {
            using (var db = new AppContext())
            {
                var book = new Book { Name = name, Year = year };
                db.Books.Add(book);
                db.SaveChanges();
            }
        }
        public void AddBook(string name, int year, string author, string genre)
        {
            using (var db = new AppContext())
            {
                var book = new Book { Name = name, Year = year, Author = author, Genre = genre };
                db.Books.Add(book);
                db.SaveChanges();
            }
        }
        public void RemoveBook(Book book)
        {
            using (var db = new AppContext())
            {
                db.Books.Remove(book);
                db.SaveChanges();
            }
        }
        // Добавление/обновление года книги
        public void UpdateBookYear(int id, int newYear)
        {
            using (var db = new AppContext())
            {
                var book = db.Books.Where(_ => _.Id == id).FirstOrDefault();
                book.Year = newYear;
                db.SaveChanges();
            }
        }
        // Пользователь получил книгу
        public void BookTakenByUser(int userId, int bookId)
        {
            using (var db = new AppContext())
            {
                var book = db.Books.Where(b => b.Id == bookId).FirstOrDefault();
                book.UserId = userId;
                db.SaveChanges();
            }
        }
        // Перегрузка для получения нескольких книг сразу
        public void BookTakenByUser(int userId, int[] booksId)
        {
            using (var db = new AppContext())
            {
                foreach (int id in booksId)
                {
                    var book = db.Books.Where(b => b.Id == id).FirstOrDefault();
                    book.UserId = userId;
                }
                db.SaveChanges();
            }
        }
        // Добавление/обновление автора книги
        public void UpdateBookAuthor(int id, string newAuthor)
        {
            using (var db = new AppContext())
            {
                var book = db.Books.Where(_ => _.Id == id).FirstOrDefault();
                book.Author = newAuthor;
                db.SaveChanges();
            }
        }
        // Добавление/обновление жанра книги
        public void UpdateBookGenre(int id, string newGenre)
        {
            using (var db = new AppContext())
            {
                var book = db.Books.Where(_ => _.Id == id).FirstOrDefault();
                book.Genre = newGenre;
                db.SaveChanges();
            }
        }
        // Получаем список книг определенного жанра и вышедших между определенными годами
        public List<Book> GetBookByGenreAndDate(string genre, int fromYear, int toYear)
        {
            List<Book> books;
            using (var db = new AppContext())
            {
                books = db.Books.Where(_ => _.Genre == genre && (_.Year > fromYear && _.Year < toYear)).ToList();
            }
            return books;
        }
        // Получаем количество книг определенного автора в библиотеке
        public int GetBookCountByAuthor(string author)
        {
            int count;
            using (var db = new AppContext())
            {
                count = db.Books.Where(_ => _.Author == author).Count();
            }
            return count;
        }
        // Получаем количество книг определенного жанра в библиотеке
        public int GetBookCountByGenre(string genre)
        {
            int count;
            using (var db = new AppContext())
            {
                count = db.Books.Where(_ => _.Genre == genre).Count();
            }
            return count;
        }
        // Получаем булевый флаг о том, есть ли книга определенного автора и с определенным названием в библиотеке
        public bool IsInLibrary(string author, string name)
        {
            bool result;
            using (var db = new AppContext())
            {
                result = db.Books.Any(_ => _.Author == author && _.Name == name);
            }
            return result;
        }
        // Получаем булевый флаг о том, есть ли определенная книга на руках у пользователя
        public bool IsOnUse(string author, string name)
        {
            bool result;
            using (var db = new AppContext())
            {
                result = db.Users.Join(db.Books, u => u.Id, b => b.UserId, (u, b) => new { b.Name, b.Author }).Any(_ => _.Author == author && _.Name == name);
            }

            return result;
        }
        // Перегрузка метода выше с выходным параметром Id пользователя, у которого конкретная книга
        public bool IsOnUse(string author, string name, out int? userId)
        {
            bool result;
            using (var db = new AppContext())
            {
                var dbset = db.Users.Join(db.Books, u => u.Id, b => b.UserId, (u, b) => new { b.Name, b.Author, u.Id });
                result = dbset.Any(_ => _.Author == author && _.Name == name);
                userId = dbset.Where(u => u.Author == author && u.Name == name).Select(u => u.Id).FirstOrDefault();
            }

            return result;
        }
        // Получение последней вышедшей книги
        public Book GetNewestBook()
        {
            var book = new Book();
            using (var db = new AppContext())
            {
                book = db.Books.OrderByDescending(b => b.Year).FirstOrDefault();
            }
            return book;
        }
        // Если несколько книг с одинаковым последним годом, берем список 
        public List<Book> GetNewestBooks()
        {
            var books = new List<Book>();
            using (var db = new AppContext())
            {
                books = db.Books.Where(b => b.Year == db.Books.Max(b => b.Year)).ToList();
            }
            return books;
        }
        // Получение списка всех книг, отсортированного в алфавитном порядке по названию
        public List<Book> GetBooksAlphabetical()
        {
            List<Book> books = new List<Book>();
            using (var db = new AppContext())
            {
                books = db.Books.OrderBy(b => b.Name).ToList();
            }
            return books;
        }
        // Получение списка всех книг, отсортированного в порядке убывания года их выхода
        public static List<Book> GetBooksByYearDesc()
        {
            List<Book> books = new List<Book>();
            using (var db = new AppContext())
            {
                books = db.Books.OrderByDescending(b => b.Year).ToList();
            }
            return books;
        }
    }
}
