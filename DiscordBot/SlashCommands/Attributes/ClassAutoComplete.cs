using Discord.Interactions;
using Discord;

namespace DiscordBot.SlashCommands
{
    public class ClassAutoComplete : AutocompleteHandler
    {
        private static string[] _classes = new string[]
        {
            "Yarida", "Taterazay", "Yumiyacha", "Kibadda", "Destrobo", "Piekron", "Wooyari", "Pyokorider", "Cannassault", "Charibasa",
            "Guardira", "Tondenga", "Myamsar", "Bowmunk", "Grenburr", "Alosson", "Wondabarappa", "Jamsch", "Oohoroc", "Pingrek", "Cannogabang",
            "Ravenous", "Sonarchy", "Ragewolf", "Naughtyfins", "Slogturtle", "CovetHiss", "Buzzcrave"
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
                    GetClassList().Where(s => s.Name.StartsWith((string)autocompleteInteraction.Data.Current.Value)).Take(10)
                );
        }
        internal static bool IsValidClassName(string className) => Array.IndexOf(_classes, className) > -1;
    }
}
