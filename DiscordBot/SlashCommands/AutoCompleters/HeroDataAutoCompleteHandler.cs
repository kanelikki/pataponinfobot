using DiscordBot.Database;
using DiscordBot.Database.DataTypes;

namespace DiscordBot.SlashCommands.AutoCompleters
{
    internal class HeroDataAutoCompleteHandler : BigChoicesAutoCompleteHandler<HeroInfo>
    {
        public HeroDataAutoCompleteHandler(IDB db) : base(db)
        {
        }

        protected override int _resultAmount => 10;

        protected override string _tableName => "HERO";

        protected override bool CompareForAutocompletion(string choice, string input) =>
            choice.ToLower().StartsWith(input.ToLower());

        protected override string GetBigChoicesName(HeroInfo input) => input.ClassName;
    }
}
