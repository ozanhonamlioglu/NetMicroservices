using FreeCourse.Services.Basket.Dtos;
using FreeCourse.Shared.Dtos;
using System.Text.Json;
using System.Threading.Tasks;

namespace FreeCourse.Services.Basket.Services
{
  public class BasketService : IBasketService
  {
    private readonly RedisService _readisService;

    public BasketService(RedisService readisService)
    {
      _readisService = readisService;
    }

    public async Task<Response<bool>> Delete(string userId)
    {
      var status = await _readisService.GetDb().KeyDeleteAsync(userId);
      return status ? Response<bool>.Success(204) : Response<bool>.Fail("Basket not found", 500);
    }

    public async Task<Response<BasketDto>> GetBasket(string userId)
    {
      var existBasket = await _readisService.GetDb().StringGetAsync(userId);

      if (string.IsNullOrEmpty(existBasket))
      {
        return Response<BasketDto>.Fail("Basket not found", 404);
      }

      return Response<BasketDto>.Success(JsonSerializer.Deserialize<BasketDto>(existBasket), 200);
    }

    public async Task<Response<bool>> SaveOrUpdate(BasketDto basketDto, string userId)
    {
      var data = new BasketDto() { DiscountCode = basketDto.DiscountCode, UserId = userId, basketItems = basketDto.basketItems };
      var status = await _readisService.GetDb().StringSetAsync(userId, JsonSerializer.Serialize(data));

      return status ? Response<bool>.Success(204) : Response<bool>.Fail("Basket couldn't update or save", 500);
    }
  }
}
