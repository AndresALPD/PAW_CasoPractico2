using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PAWCP2.Models
{
    public class FoodItemListViewModel
    {
        public IEnumerable<FoodItem> FoodItems { get; set; }
        public FoodItemSearchCriteria SearchCriteria { get; set; }
    }
}