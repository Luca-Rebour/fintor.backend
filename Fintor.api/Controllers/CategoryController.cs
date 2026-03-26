using Application.DTOs.Categories;
using Application.DTOs.Transactions;
using Application.Interfaces.UseCases.Categories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Fintor.api.Controllers
{
    [ApiController]
    [Route("api/categories")]
    public class CategoryController : Controller
    {
        private readonly ICreateCategory _createCategory;
        private readonly IGetAllCategories _getAllCategories;
        private readonly IDeleteCategory _deleteCategory;
        public CategoryController(ICreateCategory createCategory, IGetAllCategories getAllCategories, IDeleteCategory deleteCategory)
        {
            _createCategory = createCategory;
            _getAllCategories = getAllCategories;
            _deleteCategory = deleteCategory;
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CreateCategory(CreateCategoryDTO createCategoryDTO)
        {
            Guid userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            CategoryDTO categoryDTO = await _createCategory.ExecuteAsync(createCategoryDTO, userId);
            return Ok(categoryDTO);
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetAll()
        {
            Guid userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            List<CategoryDTO> categories = await _getAllCategories.Execute(userId);
            return Ok(categories);
        }

        [HttpDelete("{categoryId:guid}")]
        [Authorize]
        public async Task<IActionResult> DeleteCategory([FromRoute] Guid categoryId)
        {
            Guid userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            await _deleteCategory.ExecuteAsync(categoryId, userId);
            return NoContent();
        }
    }
}
