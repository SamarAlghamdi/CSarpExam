using System.ComponentModel.DataAnnotations;

namespace CSharpExam.Models
{
    public class Association
    {
        [Key]
        public int AssociationId { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        public int HobbyId { get; set; }
        public Hobby Hobby { get; set; }
        public string Proficiency { get; set; }

    }
}