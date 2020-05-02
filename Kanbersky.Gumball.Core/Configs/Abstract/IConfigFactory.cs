using Kanbersky.Gumball.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Kanbersky.Gumball.Core.Configs.Abstract
{
    public interface IConfigFactory
    {
        T GetValue<T>(string key, T defaultValue = default);

        Task InitClientAsync();

        Task LoadAsync();

        Task PublishAsync(string applicationName, Dictionary<string, object> configurations);

        Task InitServer(Action<ConfigInitModel> action);
    }
}
