using System.ComponentModel.DataAnnotations;

namespace ChaVo.Models
{
    public class ChangePasswordModel
    {
        [Required]
        [DataType(DataType.Text)]
        public string LastPassword { get; set; }
        [Required]
        public string NewPassword { get; set; }
        [Compare("Password")]
        public string ConfirmNewPassword { get; set; }
    }
}