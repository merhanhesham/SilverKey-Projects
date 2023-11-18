using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using EdgeDB;
using System.ComponentModel.DataAnnotations;

namespace ContactDatabase.Pages;

public class IndexModel : PageModel
{
    private readonly EdgeDBClient _client;
    public IndexModel(EdgeDBClient client)
    {
        _client = client;
    }
    [BindProperty]
    public Contact NewContact { get; set; } = new();
    public async Task<IActionResult> OnPost()
    {
        var query = @"INSERT Contact {
        first_name := <str>$first_name,
        last_name := <str>$last_name,
        email := <str>$email,
        title := <str>$title,
        description := <str>$description,
        birth_date := <str>$birth_date,
        marital_status := <bool>$marital_status
    }";

        var parameters = new Dictionary<string, object?>
    {
        {"first_name", NewContact.FirstName},
        {"last_name", NewContact.LastName},
        {"email", NewContact.Email},
        {"title", NewContact.Title},
        {"description", NewContact.Description},
        {"birth_date", NewContact.BirthDate},
        {"marital_status", NewContact.MaritalStatus}
    };

        await _client.ExecuteAsync(query, parameters);

        return RedirectToPage("/ContactsList");
    }
}

public class Contact
{
    [Required(ErrorMessage = "Please enter a first name.")]
    public string FirstName { get; set; }

    [Required(ErrorMessage = "Please enter a last name.")]
    public string LastName { get; set; }

    [Required(ErrorMessage = "Please enter an email address.")]
    [EmailAddress(ErrorMessage = "Please enter a valid email address.")]
    public string Email { get; set; }

    [Required(ErrorMessage = "Please enter title.")]
    public string Title { get; set; }

    [Required(ErrorMessage = "Please enter a description.")]
    public string Description { get; set; }

    [Required(ErrorMessage = "Please enter a birth date.")]
    [DataType(DataType.Date)]
    public string BirthDate { get; set; }

    [Required(ErrorMessage = "Please specify a marital status.")]
    public bool MaritalStatus { get; set; }
    public Contact() { }
    public Contact(string firstName, string lastName, string email, string title, string description, string birthDate, bool maritalStatus)
    {

        FirstName = firstName;
        LastName = lastName;
        Email = email;
        Title = title;
        Description = description;
        BirthDate = birthDate;
        MaritalStatus = maritalStatus;
    }
}
