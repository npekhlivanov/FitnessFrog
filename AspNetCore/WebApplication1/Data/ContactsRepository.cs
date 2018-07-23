using MovieStore.Models;
using System.Collections.Generic;

namespace MovieStore.Data
{
    public class ContactsRepository
    {
        public static IList<Contact> GetContacts()
        {
            return new List<Contact>
            {
                new Contact { Id = 1, Name = "Иван Петров", Email = "i.petrov@abv.bg", Phone = "0888123456"},
                new Contact { Id = 2, Name = "Стефан Петров", Email = "s.petrov@abv.bg", Phone = "0888123457"},
                new Contact { Id = 3, Name = "Георги Петров", Email = "g.petrov@abv.bg", Phone = "0888123458"},
                new Contact { Id = 4, Name = "Петър Иванов", Email = "i.ivanov@abv.bg", Phone = "0888123456"},
                new Contact { Id = 5, Name = "Стефан Иванов", Email = "s.ivanov@abv.bg", Phone = "0888123457"},
                new Contact { Id = 6, Name = "Георги Иванов", Email = "g.ivanov@abv.bg", Phone = "0888123458"}
            };
        }
    }
}
