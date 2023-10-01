using api_ecommerce_v1.Models;
using Microsoft.EntityFrameworkCore;

namespace api_ecommerce_v1.Services
{
    public class ContactService : IContact
    {
        private readonly ApplicationDbContext _context;

        public ContactService(ApplicationDbContext context)
        {
            _context = context;
        }

        public List<Contact> GetAllContacts()
        {
            return _context.Contact.ToList();
        }

        public Contact GetByIdContacts(int id)
        {
            return _context.Contact.FirstOrDefault(x => x.Id == id);
        }

        public Contact CreateContacts(Contact contact)
        {
            contact.state = "Abierto";
            _context.Contact.Add(contact);
            _context.SaveChanges();
            return contact;
        }
        public Contact UpdateContacts(int contactId, Contact updatedContact)
        {
            var existingContact = _context.Contact.FirstOrDefault(c => c.Id == contactId);

            if (existingContact != null)
            {
                // Actualiza las propiedades del contacto existente con los valores del contacto actualizado
                existingContact.message = updatedContact.message;
                existingContact.subject = updatedContact.subject;
                existingContact.state = updatedContact.state;

                // ... y así sucesivamente

                _context.SaveChanges();

                return existingContact; // Devuelve el contacto actualizado
            }
            else
            {
                // Maneja el caso en el que el contacto no se encuentra en la base de datos
                return null; // o lanza una excepción, según tus necesidades
            }
        }


        public void DeleteContacts(int id)
        {
            var contact = _context.Contact.FirstOrDefault(x => x.Id == id);
            _context.Contact.Remove(contact);
            _context.SaveChanges();
        }
    }
}
