using api_ecommerce_v1.Models;

namespace api_ecommerce_v1.Services
{
    public interface IContact
    {
        /*
         *     Métodos que debe tener la clase que implemente esta interfaz.
         *     
         *     Nota: Todos los métodos deben ser implementados en la clase que implemente esta interfaz para evitar
         *     algún error de implementación.
        */

        // Método para obtener todos los contactos.
        List<Contact> GetAllContacts();

        // Método para obtener un contacto por su id.
        Contact GetByIdContacts(int id);

        // Método para crear un contacto.
        Contact CreateContacts(Contact contact);

        // Método para actualizar un contacto.
        Contact UpdateContacts(int contactId, Contact updatedContact);

        // Método para eliminar un contacto.
        void DeleteContacts(int id);
    }
}
