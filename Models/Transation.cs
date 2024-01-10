using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Expense_Tracker.Models
{
    public class Transation
    {
        [Key]
        public int TransationId { get; set; }
        [Range(1, int.MaxValue, ErrorMessage = "Please select a Division.")]
        public int DivisionId { get; set; }
        public Division? Division { get; set; }

        [Column(TypeName = "nvarchar(75)")]
        public string? note { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Amount should be greater than 0.")]
        public int Amount { get; set; }
        public DateTime Date {  get; set; } = DateTime.Now;

        [NotMapped]
        public string? FormattedAmount
        {
            get
            {
                return ((Division == null || Division.Type == "Expense") ? "- " : "+ ") + Amount.ToString("C0");
            }
        }
        [NotMapped]
        public string? DivisionName
        {
            get
            {
                return Division == null ? "" : Division.Name;
            }
        }

    }
}
