using Kanbersky.Gumball.Core.Configs.Abstract;
using Kanbersky.Gumball.Core.Extensions;
using Kanbersky.Gumball.Core.Models;
using Kanbersky.Gumball.Core.Settings;
using Microsoft.Extensions.Options;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Threading.Tasks;

namespace Kanbersky.Gumball.Core.Configs.Concrete
{
    public class ConfigFactory : IConfigFactory
    {
        private ImmutableDictionary<string, object> _configurations; //Değeri değişmez collection yarattık
        private readonly ConnectionMultiplexer _connection;
        private readonly ConfigSettings _configSettings;

        public ConfigFactory(IOptions<ConfigSettings> configSettings)
        {
            _configSettings = configSettings.Value;

            var options = ConfigurationOptions.Parse(_configSettings.RedisServer);
            options.ClientName = _configSettings.RedisClientName;

            _connection = ConnectionMultiplexer.Connect(options);
            _configurations = new Dictionary<string, object>().ToImmutableDictionary();
        }

        public T GetValue<T>(string key, T defaultValue = default)
        {
            return (T)_configurations[key];
        }

        public async Task InitClientAsync()
        {
            var publisher = _connection.GetSubscriber();
            await publisher.PublishAsync(_configSettings.RedisClientName,new ConfigInitModel 
            {
                RedisClientName = _configSettings.RedisClientName,
                MachineName = Environment.MachineName
            }.SerializeAsJson());
        }

        public async Task LoadAsync()
        {
            var subscriber = _connection.GetSubscriber();
            await subscriber.SubscribeAsync(_configSettings.RedisClientName, (channel, message) =>
            {
                if (message.HasValue)
                {
                    _configurations = message.ToString().DeserializeAs<Dictionary<string, object>>().ToImmutableDictionary();
                }
            });
        }

        public async Task PublishAsync(string applicationName, Dictionary<string, object> configurations)
        {
            var publisher = _connection.GetSubscriber();
            await publisher.PublishAsync(applicationName, configurations.SerializeAsJson());
        }

        public async Task InitServer(Action<ConfigInitModel> action)
        {
            var subscriber = _connection.GetSubscriber();
            await subscriber.SubscribeAsync(_configSettings.RedisClientName, (channel, message) =>
            {
                if (message.HasValue)
                {
                    var initMessage = message.ToString().DeserializeAs<ConfigInitModel>();
                    action.Invoke(initMessage);
                }
            });
        }
    }
}
