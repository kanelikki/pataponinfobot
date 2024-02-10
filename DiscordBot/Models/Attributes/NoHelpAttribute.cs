namespace DiscordBot
{
    /// <summary>
    /// Ignores the <see cref="Discord.Interactions.SlashCommandAttribute"/> while *generating help* with <see cref="Extensions.HelpGenerator"/>.
    /// </summary>
    internal class NoHelpAttribute:Attribute
    {
    }
}
