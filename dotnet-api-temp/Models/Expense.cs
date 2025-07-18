using System;
using System.ComponentModel.DataAnnotations;

namespace DotnetApi.Models
{
    public class Expense
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public decimal Amount { get; set; }

        public DateTime ExpenseDate { get; set; }

        public string ExpenseCategory { get; set; }

        public string Payment { get; set; }

        public string Comment { get; set; }

        public string Creater { get; set; }
    }
}
