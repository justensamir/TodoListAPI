using Microsoft.AspNetCore.Identity;

namespace TodosAPI.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        List<Todo> Todos { get; set; }
    }
}
