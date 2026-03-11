using Domain.Enums;
using Domain.Exceptions;

namespace Application.DTOs.Categories
{
    public class CreateCategoryDTO
    {
        public string Name { get; set; } = string.Empty;
        public string Icon { get; set; } = string.Empty;
        public string Color { get; set; } = string.Empty;

        public void Validate()
        {
            if (string.IsNullOrWhiteSpace(Name))
            {
                throw new BusinessRuleException("Category name is required.", ErrorCode.ValidationError);
            }

            if (string.IsNullOrWhiteSpace(Icon))
            {
                throw new BusinessRuleException("Category icon is required.", ErrorCode.ValidationError);
            }

            if (string.IsNullOrWhiteSpace(Color))
            {
                throw new BusinessRuleException("Category color is required.", ErrorCode.ValidationError);
            }
        }
    }
}
