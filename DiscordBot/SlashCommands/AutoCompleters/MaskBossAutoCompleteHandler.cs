using DiscordBot.Database;
using DiscordBot.Database.DataTypes;

namespace DiscordBot.SlashCommands.AutoCompleters
{
    internal class MaskBossAutoCompleteHandler : BigChoicesAutoCompleteHandler<MaskInfo>
    {
        public MaskBossAutoCompleteHandler(IDB db) : base(db)
        {
        }

        protected override int _resultAmount => 8;

        protected override bool CompareForAutocompletion(string choice, string input) =>
            choice.Contains(input, StringComparison.InvariantCultureIgnoreCase);

        protected override string GetBigChoicesName(MaskInfo input) => input.Boss;
    }
}
