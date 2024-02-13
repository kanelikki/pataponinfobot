using Discord;
using Discord.Interactions;

namespace DiscordBot.SlashCommands
{
    public class ClassTreeSlashModule : InteractionModuleBase<SocketInteractionContext>

    {
        private readonly Uri? _imageUrl;
        public ClassTreeSlashModule(ISettingProvider settingProvider)
        {
            var url = settingProvider?.Setting?.ImageUrl;
            if (url == null) _imageUrl = null;
            else _imageUrl = new Uri(url);
        }
        [SlashCommand("classtree", "Displays class tree.")]
        public async Task GetClassTree(ClassTreeName classTree)
        {
            var name = classTree.ToString();
            if (_imageUrl == null || string.IsNullOrWhiteSpace(name))
            {
                await RespondAsync(GetClassTreeAsText(classTree));
                return;
            }
            var file = $"./ClassTree/{name}.jpg";
            await RespondAsync(embed:new EmbedBuilder().WithTitle($"{name} Class Tree")
                .WithImageUrl(new Uri(_imageUrl, file).ToString()).Build());
        }
        private string GetClassTreeAsText(ClassTreeName classTree) => classTree switch
        {
            ClassTreeName.Spear =>
@"* **Kibadda**: Yarida Level 3
* **Piekron**: Yairda Level 5
* **Wooyari**: Yarida Level 9, Piekron Level 9
* **Cannassault**: Yarida Level 7
* **Pyokorider**: Kibadda Level 8
* **Charibasa**: Yarida Level 12, Cannassault Level 10, Pyokorider Level 10
* **Taterazay, Yumiyacha**: Yarida Level 15",
            ClassTreeName.Shield =>
@"* **Tondenga**: Taterazay Level 3
* **Guardira**: Taterazay Level 7
* **Bowmunk**: Taterazay Level 10, Guardira Level 10
* **Destrobo**: Taterazay Level 5
* **Myamsar**: Tondenga Level 8, Destrobo Level 8
* **Grenburr**: Taterazay Level 12, Tondenga Level 12
* **Yarida, Yumiyacha**: Taterazay Level 15",
            ClassTreeName.Archer =>
@"* **Wondabarappa**: Yumiyacha Level 3
* **Pingrek**: Yumiyacha Level 5
* **Oohoroc**: Yumiyacha Level 8, Pingrek Level 8
* **Jamsch**: Oohoroc Level 10, Wondabarappa Level 10
* **Alosson**: Yumiyacha Level 7
* **Cannogabang**: Yumiyacha Level 12, Alosson Level 12
* **Taterazay, Yarida**: Yumiyacha Level 15",
            _ => throw new ArgumentException("Incorrect class tree!")
        };
    }
}
