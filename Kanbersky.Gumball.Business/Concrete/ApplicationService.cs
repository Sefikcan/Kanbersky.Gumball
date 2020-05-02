using AutoMapper;
using Kanbersky.Gumball.Business.Abstract;
using Kanbersky.Gumball.Business.DTO.Request;
using Kanbersky.Gumball.Business.DTO.Response;
using Kanbersky.Gumball.Core.Results.Exceptions;
using Kanbersky.Gumball.DAL.Concrete.EntityFramework.GenericRepository;
using Kanbersky.Gumball.Entities.Concrete;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Kanbersky.Gumball.Business.Concrete
{
    public class ApplicationService : IApplicationService
    {
        private readonly IGenericRepository<Application> _applicationRepository;
        private readonly IMapper _mapper;

        public ApplicationService(IGenericRepository<Application> applicationRepository,
            IMapper mapper)
        {
            _applicationRepository = applicationRepository;
            _mapper = mapper;
        }

        public async Task<List<ApplicationResponseModel>> GetAllApplications()
        {
            var applications = await _applicationRepository.GetList();
            var mappedResponse = _mapper.Map<List<ApplicationResponseModel>>(applications);
            return mappedResponse;
        }

        public async Task<CreateApplicationResponseModel> AddAsync(CreateApplicationRequestModel request)
        {
            var entity = _mapper.Map<Application>(request);
            var response = await _applicationRepository.Add(entity);
            if (await _applicationRepository.SaveChangesAsync() > 0)
            {
                var mappedResponse = _mapper.Map<CreateApplicationResponseModel>(response);
                return mappedResponse;
            }
            else
            {
                throw new BadRequestException("Application Insert Fail!");
            }
        }

        public async Task Remove(int applicationId)
        {
            var applicationIsExists = await _applicationRepository.Get(x => x.Id == applicationId);
            if (applicationIsExists == null)
            {
                throw new NotFoundException("Application Not Found!");
            }

            _applicationRepository.Delete(applicationIsExists);
            if (await _applicationRepository.SaveChangesAsync() == 0)
            {
                throw new BadRequestException("Application Delete Fail!");
            }
        }
    }
}
