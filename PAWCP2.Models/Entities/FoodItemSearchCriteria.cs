using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PAWCP2.Models
{
    public class FoodItemSearchCriteria
    {
        public int? RoleId { get; set; }
        public string? Category { get; set; }
        public string? Brand { get; set; }
        public decimal? MinPrice { get; set; }
        public decimal? MaxPrice { get; set; }
        public DateOnly? ExpirationDateBefore { get; set; } // Cambiado de DateTime? a DateOnly?
        public bool? IsPerishable { get; set; }
        public int? MaxCalories { get; set; }
        public string? Supplier { get; set; }
        public bool? IsActive { get; set; }
    }
}
