namespace DiscordBot
{
    /// <summary>
    /// Group the command in <see cref="SlashCommands.HelpSlashModule"/>, as field, by name.
    /// </summary>
    /// <remarks>This is used only for displaying in help command. This doesn't affect to real slash command.</remarks>
    internal class HelpGroupAttribute:Attribute
    {
        internal string Name { get; }
        internal HelpGroupAttribute(string name)
        {
            Name = name;
        }
    }
}
