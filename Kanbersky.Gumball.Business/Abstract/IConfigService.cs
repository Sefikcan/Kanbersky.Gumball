using Kanbersky.Gumball.Business.DTO.Request;
using Kanbersky.Gumball.Business.DTO.Response;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Kanbersky.Gumball.Business.Abstract
{
    public interface IConfigService
    {
        List<ApplicationConfigResponseModel> GetAllConfigsByApplicationId(int applicationId);

        Task<CreateConfigResponseModel> AddAsync(CreateConfigRequestModel request);

        Task<UpdateConfigResponseModel> UpdateAsync(int id,UpdateConfigRequestModel request);
    }
}
