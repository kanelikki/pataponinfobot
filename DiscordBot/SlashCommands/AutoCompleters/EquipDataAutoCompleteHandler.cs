using DiscordBot.Database;
using DiscordBot.Database.DataTypes;

namespace DiscordBot.SlashCommands.AutoCompleters
{
    internal class EquipDataAutoCompleteHandler : BigChoicesAutoCompleteHandler<BSInfo>
    {
        //some of them are regional, some of them are... just aliases :D
        private Dictionary<string, string> _aliases = new()
        {
            { "Club", "Hammer" },
            { "Horn", "Tuba" },
            { "Sceptre" , "Scepter"},
            { "Boots" , "Shoes"},
            { "Blunderbuss" , "Howitzer" }
        };
       public EquipDataAutoCompleteHandler(IDB db) : base(db)
        {
        }
        protected override int _resultAmount => 6;
        protected override bool CompareForAutocompletion(string choice, string input)
        {
            if (_aliases.TryGetValue(choice, out string alias)
                && alias.Contains(input, StringComparison.InvariantCultureIgnoreCase))
            {
                return true;
            }
            return choice.Contains(input, StringComparison.InvariantCultureIgnoreCase);
        }
        protected override string GetDisplayName(string value)
        {
            if (_aliases.TryGetValue(value, out string alias))
            {
                return $"{value} ({alias})";
            }
            return value;
        }

        protected override string GetBigChoicesName(BSInfo input) => input.Equipment;
    }
}
