using Kanbersky.Gumball.Business.DTO.Request;
using Kanbersky.Gumball.Business.DTO.Response;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Kanbersky.Gumball.Business.Abstract
{
    public interface IApplicationService
    {
        Task<List<ApplicationResponseModel>> GetAllApplications();

        Task<CreateApplicationResponseModel> AddAsync(CreateApplicationRequestModel request);

        Task Remove(int applicationId);
    }
}
