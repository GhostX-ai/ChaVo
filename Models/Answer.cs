using System.ComponentModel.DataAnnotations;

namespace ChaVoV1.Models
{
    public class Answer
    {
        public int Id { get; set; }

        [Required(ErrorMessage="Answer can not be empty!")]
        [DataType(DataType.Text)]
        public string AnswerText { get; set; }
        public User Writer { get; set; }
        public Question Question { get; set; }
    }
}