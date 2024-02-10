using DiscordBot.Database.DataTypes;

namespace DiscordBot.SlashCommands
{
    /// <summary>
    /// For parameter only, since Discord doesn't support flag parameter well
    /// </summary>
    public enum MissionTypeParam
    {
        NoDLC = MissionType.NoDLC,
        Easy = MissionType.Easy,
        All = MissionType.All
    }
}
