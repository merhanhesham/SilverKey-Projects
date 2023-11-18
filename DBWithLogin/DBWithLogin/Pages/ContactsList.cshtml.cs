using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using EdgeDB;

namespace DBWithLogin.Pages
{
    public class ContactsListModel : PageModel
    {
        public List<Contact> ContactsList { get; private set; } = new List<Contact>();
        private readonly EdgeDBClient _client;

        public ContactsListModel(EdgeDBClient client)
        {
            _client = client;
        }

        public async Task<IActionResult> OnGet()
        {
            var contacts = await _client.QueryAsync<Contact>("SELECT Contact {*};");

            ContactsList = contacts.Where(contact => contact != null).ToList();

            return Page();
        }
    }
}