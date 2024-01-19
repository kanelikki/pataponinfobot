using Discord.Interactions;
using Discord;

namespace DiscordBot.SlashCommands
{
    public class ClassAutoComplete : AutocompleteHandler
    {
        private static IEnumerable<AutocompleteResult> _classList;
        private static IEnumerable<AutocompleteResult> GetClassList()
        {
            if (_classList == null)
            {
                var classList = new List<AutocompleteResult>();
                foreach (PataClass pataClass in Enum.GetValues(typeof(PataClass)))
                {
                    string name = Enum.GetName(pataClass);
                    classList.Add(new AutocompleteResult(name, name));
                }
                _classList = classList;
            }
            return _classList;
        }
        public override async Task<AutocompletionResult> GenerateSuggestionsAsync(IInteractionContext context, IAutocompleteInteraction autocompleteInteraction, IParameterInfo parameter, IServiceProvider services)
        {
            
            return AutocompletionResult.FromSuccess(
                GetClassList().Where(s => s.Name.StartsWith((string)autocompleteInteraction.Data.Current.Value)).Take(25)
                );
        }
    }
}
