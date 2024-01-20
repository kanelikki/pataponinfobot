using Discord.Interactions;
using Discord;

namespace DiscordBot.SlashCommands
{
    /// <summary>
    /// This is mandatory since the class is over 25 which is more than max discord suggestion
    /// </summary>
    public class ClassAutoComplete : AutocompleteHandler
    {
        private static string[] _classes = new string[]
        {
            "Yarida", "Taterazay", "Yumiyacha", "Kibadda", "Destrobo", "Piekron", "Wooyari", "Pyokorider", "Cannassault", "Charibasa",
            "Guardira", "Tondenga", "Myamsar", "Bowmunk", "Grenburr", "Alosson", "Wondabarappa", "Jamsch", "Oohoroc", "Pingrek", "Cannogabang",
            "Ravenous", "Sonarchy", "Ragewolf", "Naughtyfins", "Slogturtle", "Covet-hiss", "Buzzcrave"
        };
        private static IEnumerable<AutocompleteResult> _classList;
        private static IEnumerable<AutocompleteResult> GetClassList()
        {
            if (_classList == null)
            {
                var classList = new List<AutocompleteResult>();
                foreach (var cl in _classes)
                {
                    //AutoCompleteResult works when value is ALSO string
                    classList.Add(new AutocompleteResult(cl, cl));
                }
                _classList = classList;
            }
            return _classList;
        }
        public override async Task<AutocompletionResult> GenerateSuggestionsAsync(IInteractionContext context, IAutocompleteInteraction autocompleteInteraction, IParameterInfo parameter, IServiceProvider services)
        {
            return AutocompletionResult.FromSuccess(
                    GetClassList().Where(s =>
                        s.Name.ToLower()
                        .StartsWith(((string)autocompleteInteraction.Data.Current.Value).ToLower()))
                    .Take(10)
                );
        }
        //will move to other class and will add test...
        internal static bool IsValidClassName(string className, out string resultName)
        {
            var result = _classes.SingleOrDefault(name => name.ToLower() == className.ToLower());
            if (result == null)
            {
                resultName = "";
                return false;
            }
            else resultName = result;
            return true;
        }
    }
}
