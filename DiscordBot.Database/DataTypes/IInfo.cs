namespace DiscordBot.Database.DataTypes
{
    public interface IInfo
    {
        public abstract static string DBName { get; }
        public string GetKey();
    }
}
