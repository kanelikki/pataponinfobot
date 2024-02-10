using DiscordBot.Database;
using DiscordBot.Database.DataTypes;

namespace DiscordBot.SlashCommands.AutoCompleters
{
    internal class ThemeAutoCompleteHandler : BigChoicesAutoCompleteHandler<ThemeInfo>
    {
        public ThemeAutoCompleteHandler(IDB db) : base(db)
        {
        }

        protected override int _resultAmount => 7;

        protected override bool CompareForAutocompletion(string choice, string input) =>
            choice.Contains(input, StringComparison.InvariantCultureIgnoreCase);

        protected override string GetBigChoicesName(ThemeInfo input) => input.Name;
    }
}
