using CsvHelper.Configuration.Attributes;

namespace DiscordBot.Database.DataTypes
{
    public record SetItemInfo : IInfo
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        [Ignore]
        public string[] SetItems { get; private set; }
        private string _itemsRaw;
        [Name("Items")]
        public string ItemsRaw {
            get => _itemsRaw;
            set
            {
                _itemsRaw = value;
                SetItems = value.Split(',');
            }
        }

        public string GetKey() => Id.ToString();
    }
}
