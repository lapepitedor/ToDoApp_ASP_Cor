using System.ComponentModel.DataAnnotations;

namespace ToDoApp.ViewModel
{
    public class PasswordForgottenViewModel
    {
        [Required]
        [EmailAddress]
        public string ? EMail { get; set; }
    }
}
