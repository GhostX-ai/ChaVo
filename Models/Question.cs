using System;
using System.ComponentModel.DataAnnotations;

namespace ChaVoV1.Models
{
    public class Question
    {
        public int Id { get; set; }

        [Required(ErrorMessage="Title can not be empty!")]
        [DataType(DataType.Text)]
        public string QuestionTitle { get; set; }

        [Required(ErrorMessage = "Question can not be empty!")]
        [DataType(DataType.MultilineText)]
        public string QuestionText { get; set; }

        [Required(ErrorMessage = "Category can not be empty!")]
        public Category Category { get; set; }
        public DateTime PubDate { get; set; }
    }
}