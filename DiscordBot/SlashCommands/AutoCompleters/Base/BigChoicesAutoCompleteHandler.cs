using Discord;
using Discord.Interactions;
using DiscordBot.Database;
using DiscordBot.Database.DataTypes;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("DiscordBot.Tests")]
namespace DiscordBot.SlashCommands.AutoCompleters
{
    /// <summary>
    /// Provides logic for big choices with discord autocompletion. ONLY STRING IS SUPPORTED.
    /// </summary>
    /// <typeparam name="T">Type of one data (a line in table).</typeparam>
    /// <remarks>Discord has 25 choices limit for autocompletion. This helps to filter data.</remarks>
    public abstract class BigChoicesAutoCompleteHandler<T> : AutocompleteHandler where T:IInfo
    {
        /// <summary>
        /// The string data to suggest.
        /// </summary>
        protected readonly IEnumerable<string> _bigChoices;
        /// <summary>
        /// <see cref="AutocompleteResult"/> collection for suggestion.
        /// </summary>
        private readonly IEnumerable<AutocompleteResult> _autocompleteResults;
        /// <summary>
        /// Max result amount to show. Must be 25 or less.
        /// </summary>
        /// <note>This DOES NOT filter or tell you if the number is valid.</note>
        protected abstract int _resultAmount { get; } //max 25
        /// <summary>
        /// Initialises the basic data of this class.
        /// </summary>
        /// <param name="data">The data, Parsed from <see cref="TsvParser"/></param>
        protected IDB _db;
        internal BigChoicesAutoCompleteHandler(IDB db)
        {
            _db = db;
            _bigChoices = InitBigChoices();
            _autocompleteResults = GetAutoCompleteResults();
        }
        /// <summary>
        /// Gets table dictionary from <see cref="DB"/>. This distinguishes what kind of data is.
        /// </summary>
        /// <returns>The table dictionary value.</returns>
        protected System.Collections.ObjectModel.ReadOnlyDictionary<string, T> GetData()
        {
            if (!_db.TryGetTable(out System.Collections.ObjectModel.ReadOnlyDictionary<string, T> result))
            {
                throw new KeyNotFoundException($"Couldn't find data with \"{typeof(T).Name}\" from database");
            }
            return result;
        }
        /// <summary>
        /// Initialises big choice values for the choice list. Called in the constructor.
        /// </summary>
        /// <returns>The strings that will be used for autocomplete option.</returns>
        /// <note>
        /// Note that this is used in constructor. Overwrite it wisely, if you are going to override this.
        /// If it's not special case, override <see cref="GetBigChoicesName(T)"/> instead as possible.
        /// </note>
        protected virtual IEnumerable<string> InitBigChoices() => GetData().Select(x => GetBigChoicesName(x.Value)).Distinct();
        /// <summary>
        /// Used for initialising <see cref="_autocompleteResults"/>. You'll not use this for other purpose.
        /// </summary>
        /// <returns>IEnumerable Collection of <see cref="AutocompleteResult"/> for <see cref="_autocompleteResults"/></returns>
        private IEnumerable<AutocompleteResult> GetAutoCompleteResults()
        {
            var autoCompleteResult = new List<AutocompleteResult>();
            foreach (var choice in _bigChoices)
            {
                //AutoCompleteResult works when value is ALSO string
                autoCompleteResult.Add(new AutocompleteResult(GetDisplayName(choice), choice));
            }
            return autoCompleteResult;
        }
        /// <summary>
        /// Gets displayed name in the choice menu. Override this for different display name.
        /// </summary>
        /// <remarks>This is called when the choice data is initialised.</remarks>
        /// <param name="value">The choice value.</param>
        /// <returns>Name for display.</returns>
        protected virtual string GetDisplayName(string value) => value;
        /// <summary>
        /// Get name for autocompletion that has possibly over 25 amount of value.
        /// </summary>
        /// <remarks>This is value for choice. For displayed name in the choices, use <see cref="GetDisplayName(string)"/>.</remarks>
        /// <param name="input">The one line of the whole data.</param>
        /// <remarks>The return value can be unique per each data, but NOT ALWAYS UNIQUE.</remarks>
        /// <returns>Autocompletion option, as string.</returns>
        protected abstract string GetBigChoicesName(T input);
        /// <summary>
        /// Gets <see cref="AutocompleteResult"/> for autocompleting suggestion, by overriding existing generate suggestion logic.
        /// </summary>
        /// <remarks>Read Discord.Net <see cref="AutocompleteHandler.GenerateSuggestionsAsync"/> for detail.</remarks>
        /// <returns>The autocomplete suggestions.</returns>
        public override async Task<AutocompletionResult> GenerateSuggestionsAsync(IInteractionContext context, IAutocompleteInteraction autocompleteInteraction, IParameterInfo parameter, IServiceProvider services)
        {
            return AutocompletionResult.FromSuccess(
                    _autocompleteResults.Where(s => CompareForAutocompletion((string)s.Value, (string)autocompleteInteraction.Data.Current.Value))
                    .Take(_resultAmount)
                );
        }
        /// <summary>
        /// Compare choice to the value.
        /// </summary>
        /// <remarks>This can be e.g. suggestion based on first letter, middle letter, or other similarity comparison.</remarks>
        /// <param name="choice">The available choice, case sensitive.</param>
        /// <param name="input">The (possibly incomplete or empty) parameter for interaction.</param>
        /// <returns><c>true</c> if the <c>choice</c> is suggested with <c>input</c> value, otherwise <c>false</c>.</returns>
        protected abstract bool CompareForAutocompletion(string choice, string input);
    }
}
