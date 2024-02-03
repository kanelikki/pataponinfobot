using DiscordBot.Database;
using DiscordBot.Database.DataTypes;

namespace DiscordBot.Tests
{
    public class DbTest
    {
        [Fact]
        public void TryGetValidTable_LoadsNotNullTable()
        {
            //yep. making interface for one simple method for one test is too much.
            Assert.True(new DB().TryGetTable<CsGrindInfo>(out var result));
            Assert.NotNull(result);
        }
    }
}
