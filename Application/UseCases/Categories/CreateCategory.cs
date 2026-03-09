using Application.DTOs.Categories;
using Application.Interfaces.Repositories;
using Application.Interfaces.UseCases.Categories;
using AutoMapper;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UseCases.Categories
{
    public class CreateCategory : ICreateCategory
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMapper _mapper;
        public CreateCategory(ICategoryRepository categoryRepository, IMapper mapper)
        {
            _categoryRepository = categoryRepository;
            _mapper = mapper;
        }

        public async Task<CategoryDTO> ExecuteAsync(CreateCategoryDTO createCategoryDTO, Guid userId)
        {
            createCategoryDTO.Validate();
            Category category = new Category(userId, createCategoryDTO.Name, createCategoryDTO.Icon, createCategoryDTO.Color);
            Category newCategory = await _categoryRepository.CreateAsync(category);
            CategoryDTO categoryDTO = _mapper.Map<CategoryDTO>(newCategory);
            return categoryDTO;
        }
    }
}
