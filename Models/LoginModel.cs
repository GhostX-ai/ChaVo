using System.ComponentModel.DataAnnotations;

namespace ChaVoV1.Models
{
    public class LoginModel
    {
        [Required(ErrorMessage = "Login can not be empty!")]
        [DataType(DataType.Text)]
        public string Login { get; set; }

        [Required(ErrorMessage = "Password can not be empty!")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}