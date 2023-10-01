using api_ecommerce_v1.Models;

namespace api_ecommerce_v1.Services
{
    public interface IContact
    {
        List<Contact> GetAllContacts();

        Contact GetByIdContacts(int id);

        Contact CreateContacts(Contact contact);

        Contact UpdateContacts(int contactId, Contact updatedContact);

        void DeleteContacts(int id);
    }
}
