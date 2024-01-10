using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Expense_Tracker.Models
{
    public class Division
    {
        [Key]
        public int DivisionId { get; set; }
        [Column(TypeName ="nvarchar(50)")]
        [Required(ErrorMessage = "Name is required.")]
        public string Name { get; set; }
        [Column(TypeName = "nvarchar(50)")]
        public string Type { get; set; } = "Expense";
    }
}
