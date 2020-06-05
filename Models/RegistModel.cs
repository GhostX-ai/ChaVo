using System.ComponentModel.DataAnnotations;

namespace ChaVoV1.Models
{
    public class RegistModel
    {
        [Required(ErrorMessage = "Firstname can not be empty!")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Lastname can not be empty!")]
        public string LastName { get; set; }

        public string MiddleName { get; set; }

        [Required(ErrorMessage = "Login can not be empty!")]
        public string Login { get; set; }

        [Required(ErrorMessage = "Password can not be empty!")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Compare("Password",ErrorMessage="Passwords are not equal!")]
        public string ConfirmPassoword { get; set; }
    }
}