using DiscordBot.Database;
using DiscordBot.Database.DataTypes;
using DiscordBot.SlashCommands.AutoCompleters;
using System.Reflection;
using Moq;

namespace DiscordBot.Tests
{
    public class AliasDataCompleteHandlerTest
    {
        private readonly MethodInfo _method;
        private readonly EquipDataAutoCompleteHandler _handler;

        public AliasDataCompleteHandlerTest()
        {
            var data =
                new System.Collections.ObjectModel.ReadOnlyDictionary<string, BSInfo>(
                new Dictionary<string, BSInfo>()
                {
                    { "Blunderbuss",
                        new BSInfo(){
                            Equipment = "Blunderbuss"
                        }
                    },
                    { "Club",
                        new BSInfo(){
                            Equipment = "Club"
                        }
                    }
                });
            var db = new Mock<IDB>();
            db.Setup(d => d.TryGetTable(out data))
                .Callback(new GetTable((out System.Collections.ObjectModel.ReadOnlyDictionary<string, BSInfo> d) => d = data)).Returns(true);
            _handler = new EquipDataAutoCompleteHandler(db.Object);

            //discord auto suggestion testability is pain
            _method = _handler.GetType()
                .GetMethod("CompareForAutocompletion", BindingFlags.NonPublic | BindingFlags.Instance);
        }

        [Theory]
        [InlineData("Club", "mm", true)]
        [InlineData("Blunderbuss", "lu", true)]
        [InlineData("Club", "bu", false)]
        public async Task EquipDataAutoCompleteHandler_FindAlias_Found(string value, string input, bool expected)
        {
            var res = _method.Invoke(_handler, new[] { value, input }) as bool?;
            Assert.Equal(expected, res);
        }
        private delegate void GetTable(out System.Collections.ObjectModel.ReadOnlyDictionary<string, BSInfo> data);
    }
}
