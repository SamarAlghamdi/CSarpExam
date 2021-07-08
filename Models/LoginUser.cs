using System.ComponentModel.DataAnnotations;

namespace CSharpExam.Models
{
    public class LoginUser
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email Address")]
        public string LoggedEmail{ get; set;}

        [Required]
        [DataType(DataType.Password)]
        public string LoggedPassword{ get; set;}
    }
}