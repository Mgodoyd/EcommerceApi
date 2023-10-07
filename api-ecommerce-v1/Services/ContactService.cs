using api_ecommerce_v1.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;

namespace api_ecommerce_v1.Services
{
    public class ContactService : IContact
    {
        private readonly ApplicationDbContext _context;
        private readonly IDistributedCache _distributedCache;

        /*
         *  Inyectamos el Servicio
         */
        public ContactService(ApplicationDbContext context, IDistributedCache distributedCache)
        {
            _context = context;
            _distributedCache = distributedCache;
        }

        /*
         *  Método para obtener todos los contactos
         */
        public List<Contact> GetAllContacts()
        {
            return _context.Contact.ToList();
        }

        /*
         *  Método para obtener un contacto por id
         */
        public Contact GetByIdContacts(int id)
        {
            return _context.Contact.FirstOrDefault(x => x.Id == id);
        }

        /*
         *  Método para crear un contacto, se inserta el estado como "Abierto"
         */
        public Contact CreateContacts(Contact contact)
        {
            contact.state = "Abierto";
            _context.Contact.Add(contact);
            _context.SaveChanges();
            return contact;
        }

        /*
         * Método para actualizar un contacto
         */
        public Contact UpdateContacts(int contactId, Contact updatedContact)
        {
            var existingContact = _context.Contact.FirstOrDefault(c => c.Id == contactId);

            if (existingContact != null)
            {
                existingContact.message = updatedContact.message;
                existingContact.subject = updatedContact.subject;
                existingContact.state = updatedContact.state;
                _context.SaveChanges();

                return existingContact; 
            }
            else
            {
                return null; 
            }
        }

        /*
         * Método para eliminar un contacto
         */
        public void DeleteContacts(int id)
        {
            var contact = _context.Contact.FirstOrDefault(x => x.Id == id);
            _context.Contact.Remove(contact);

            var cacheKey = $"Contact_{id}";
            _distributedCache.Remove(cacheKey);

            _context.SaveChanges();
        }
    }
}
