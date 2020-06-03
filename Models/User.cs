using System.ComponentModel.DataAnnotations;

namespace ChaVoV1.Models
{
    public class User
    {
        public string Id { get; set; }
        
        [Required(ErrorMessage = "Firstname can not be empty!")]
        public string FirstName { get; set; }
        
        [Required(ErrorMessage = "Lastname can not be empty!")]
        public string LastName { get; set; }

        public string MiddleName { get; set; }

        [Required(ErrorMessage = "Mail can not be empty!")]
        public string Mail { get; set; }

        [Required(ErrorMessage = "Login can not be empty!")]
        public string Login { get; set; }

        [Required(ErrorMessage = "Password can not be empty!")]
        public string Password { get; set; }
    }
}