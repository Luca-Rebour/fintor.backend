using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Categories
{
    public class CategorySummaryDto
    { 
        public Guid CategoryId { get; set; }
        public string CategoryName { get; set; }
        public decimal Total { get; set; }
        public string CategoryColor { get; set; } = null!;

        public CategorySummaryDto(Guid categoryId, string categoryName, decimal total) { 
            CategoryId = categoryId;
            CategoryName = categoryName;
            Total = total;
        }
    }
}
