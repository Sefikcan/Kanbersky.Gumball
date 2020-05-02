using AutoMapper;
using Kanbersky.Gumball.Business.Abstract;
using Kanbersky.Gumball.Business.DTO.Request;
using Kanbersky.Gumball.Business.DTO.Response;
using Kanbersky.Gumball.Core.Configs.Abstract;
using Kanbersky.Gumball.Core.Results.Exceptions;
using Kanbersky.Gumball.DAL.Concrete.EntityFramework.GenericRepository;
using Kanbersky.Gumball.Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Kanbersky.Gumball.Business.Concrete
{
    public class ConfigService : IConfigService
    {
        #region fields

        private readonly IGenericRepository<Config> _configRepository;
        private readonly IGenericRepository<Application> _applicationRepository;
        private readonly IMapper _mapper;
        private readonly IConfigFactory _configFactory;

        #endregion

        #region ctor

        public ConfigService(IGenericRepository<Config> configRepository,
            IMapper mapper,
            IConfigFactory configFactory,
            IGenericRepository<Application> applicationRepository)
        {
            _configRepository = configRepository;
            _applicationRepository = applicationRepository;
            _mapper = mapper;
            _configFactory = configFactory;
        }

        #endregion

        #region methods

        public List<ApplicationConfigResponseModel> GetAllConfigsByApplicationId(int applicationId)
        {
            var config = _configRepository.GetQueryable();
            var application = _applicationRepository.GetQueryable();

            var applicationConfigs = (from a in application
                                      join c in config on a.Id equals c.ApplicationId
                                      select new ApplicationConfigResponseModel
                                      {
                                          ApplicationName = a.ApplicationName,
                                          Id = c.Id,
                                          ConfigResponseModel = _mapper.Map<List<ConfigResponseModel>>(a.Configs)
                                      }).Where(x => x.Id == applicationId).ToList();


            var mappedResponse = _mapper.Map<List<ApplicationConfigResponseModel>>(applicationConfigs);
            return mappedResponse;
        }

        public async Task<CreateConfigResponseModel> AddAsync(CreateConfigRequestModel request)
        {
            var isExists = await _configRepository.Get(x => x.Key == request.Key && x.ApplicationId == request.ApplicationId);
            if (isExists != null)
            {
                throw new BadRequestException("Config Insert Fail!");
            }

            var entity = _mapper.Map<Config>(request);
            var response = await _configRepository.Add(entity);
            if (await _configRepository.SaveChangesAsync() > 0)
            {
                var mappedResponse = _mapper.Map<CreateConfigResponseModel>(response);
                await ReloadConfig(request.ApplicationId);
                return mappedResponse;
            }
            else
            {
                throw new BadRequestException("Config Insert Fail!");
            }
        }

        public async Task<UpdateConfigResponseModel> UpdateAsync(int id,UpdateConfigRequestModel request)
        {
            var configEntity = await _configRepository.Get(x => x.Id == id && x.ApplicationId == request.ApplicationId);

            if (configEntity == null)
            {
                throw new NotFoundException("Config Not Found!");
            }

            configEntity.Value = request.Value;
            configEntity.LastModifiedAt = DateTime.Now;
            await _configRepository.Update(configEntity);

            if (await _configRepository.SaveChangesAsync() > 0)
            {
                var mappedResponse = _mapper.Map<UpdateConfigResponseModel>(configEntity);
                await ReloadConfig(request.ApplicationId);
                return mappedResponse;
            }
            else
            {
                throw new BadRequestException("Config Insert Fail!");
            }
        }

        #endregion

        #region helpers

        async Task ReloadConfig(int applicationId)
        {
            var config = _configRepository.GetQueryable();
            var application = _applicationRepository.GetQueryable();

            var applicationConfigs = (from a in application
                                     join c in config on a.Id equals c.ApplicationId
                                     select new
                                     {
                                         a.Configs,
                                         a.ApplicationName,
                                         c.ApplicationId,
                                         c.Id,
                                         c.Key,
                                         c.Value
                                     }).FirstOrDefault(x=>x.ApplicationId == applicationId);
            if (applicationConfigs !=null)
            {
                await _configFactory.PublishAsync(applicationConfigs.ApplicationName, applicationConfigs.Configs.ToDictionary(x => x.Key, x => (object)x.Value));
            }
        }

        #endregion
    }
}
