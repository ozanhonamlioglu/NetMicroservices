using FreeCourse.Services.Catalog.Dtos;
using FreeCourse.Services.Catalog.Models;
using FreeCourse.Shared.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FreeCourse.Services.Catalog.Services
{
    public interface ICategoryService
    {
        Task<Response<List<CategoryDto>>> GetAll();
        Task<Response<CategoryDto>> CreateAsync(CategoryCreateDto category);
        Task<Response<CategoryDto>> GetByIdAsync(string id);
    }
}
