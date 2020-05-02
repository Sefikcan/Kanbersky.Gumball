using System.Collections.Generic;

namespace Kanbersky.Gumball.Business.DTO.Response
{
    public class ApplicationConfigResponseModel
    {
        public ApplicationConfigResponseModel()
        {
            ConfigResponseModel = new List<ConfigResponseModel>();
        }

        public int Id { get; set; }

        public string ApplicationName { get; set; }

        public List<ConfigResponseModel> ConfigResponseModel { get; set; }
    }
}
