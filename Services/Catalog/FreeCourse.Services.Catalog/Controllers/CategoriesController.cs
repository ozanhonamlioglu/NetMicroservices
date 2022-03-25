using FreeCourse.Services.Catalog.Dtos;
using FreeCourse.Services.Catalog.Services;
using FreeCourse.Shared.ControllerBases;
using FreeCourse.Shared.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace FreeCourse.Services.Catalog.Controllers
{
  [Route("api/[controller]/[action]")]
  [ApiController]
  public class CategoriesController : CustomBaseController
  {

    private readonly ICategoryService _categoryService;

    public CategoriesController(ICategoryService categoryService)
    {
      _categoryService = categoryService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
      var response = await _categoryService.GetAll();
      return CreateActionResultInstance(response);
    }

    [Authorize(Policy = "RequireAdmin")]
    [HttpGet]
    public async Task<IActionResult> GetById(string id)
    {
      var response = await _categoryService.GetByIdAsync(id);
      return CreateActionResultInstance(response);
    }

    [HttpPost]
    public async Task<IActionResult> Create(CategoryCreateDto categoryCreateDto)
    {
      var response = await _categoryService.CreateAsync(categoryCreateDto);
      return CreateActionResultInstance(response);
    }
  }
}
