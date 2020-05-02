using System;
using System.Collections.Generic;
using System.Text;

namespace Kanbersky.Gumball.Business.DTO.Response
{
    public class ConfigResponseModel
    {
        public int Id { get; set; }

        public string Key { get; set; }

        public string Value { get; set; }

        public int ApplicationId { get; set; }
    }
}
