using System.Text.Json;

namespace DiscordBot
{
    internal class SettingProvider : ISettingProvider
    {
        public Setting Setting { get; private set; }
        const string _settingPath = "./settings.json";
        internal SettingProvider(ILogger logger)
        {
            if (!File.Exists(_settingPath))
            {
                using var writer = File.CreateText(_settingPath);
                var newSetting = GetNewSetting();
                writer.Write(JsonSerializer.Serialize(newSetting));
                Setting = newSetting;
            }
            var file = File.OpenRead(_settingPath);
            var deserialized = JsonSerializer.Deserialize<Setting>(file);
            if (deserialized == null)
            {
                Setting = GetNewSetting();
                logger.Log("Cannot load the setting. Loading default...", Discord.LogSeverity.Error);
            }
            else Setting = deserialized;
        }
        private Setting GetNewSetting() =>
            new Setting()
            {
                ImageUrl = null
            };
    }
}
