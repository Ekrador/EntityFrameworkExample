using EntityFrameworkExample.Repositories;

namespace EntityFrameworkExample
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var userRep = new UserRepository();
            var bookRep = new BookRepository();
        }
    }
}