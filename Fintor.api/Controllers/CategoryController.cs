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
        public CategoryController(ICreateCategory createCategory, IGetAllCategories getAllCategories)
        {
            _createCategory = createCategory;
            _getAllCategories = getAllCategories;
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
    }
}
