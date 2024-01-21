using DiscordBot.Database.DataTypes;
namespace DiscordBot.Database
{
    /// <summary>
    /// Represents the whole database (actually, it's not real db, but collection of csv data).
    /// </summary>
    /// <remarks>Each file is considered as table.</remarks>
    public class DB : IDB
    {
        private const string _dataPath = "data/";
        private readonly TsvParser _tsvParser;
        private IDbLogger? _dbLogger;
        private readonly Dictionary<string, object> _data = //object is dictionary lol fuck it we ball
            new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase);
        /// <summary>
        /// The optional comparers for some tables. Default is case insensitive. Reserved for future extensibility.
        /// </summary>
        private readonly Dictionary<string, StringComparer> _specialComparers = new Dictionary<string, StringComparer>();
        /// <summary>
        /// Starts the database.
        /// </summary>
        /// <remarks>You can use with <see cref="Microsoft.Extensions.DependencyInjection.ServiceCollection.AddSingleton"/>.</remarks>
        public DB(IDbLogger? logger = null)
        {
            _tsvParser = new TsvParser();
            _dbLogger = logger;
            Init();
        }
        /// <summary>
        /// Set the logger (optional).
        /// </summary>
        /// <param name="logger">Path to log.</param>
        public void SetLogger(IDbLogger logger)
        {
            _dbLogger = logger;
        }
        /// <summary>
        /// Loads all available tsv data. All "tables" will be defined here.
        /// </summary>
        private void Init()
        {
            InitEach<CsGrindInfo>("CS");
            InitEach<PveEnemyInfo>("PVE");
        }
        /// <summary>
        /// Deserializes data from TSV file.
        /// </summary>
        /// <typeparam name="T">The type to deserialize.</typeparam>
        /// <param name="name">The database name, SAME AS FILE NAME WITHOUT EXTENSION (the file MUST be ".tsv"), it works also as a "key"</param>
        private void InitEach<T>(string name) where T : IInfo
        {
            try
            {
                var file = Path.Combine(_dataPath, name + ".tsv");
                var parsed = _tsvParser.Parse<T>(file,
                    _specialComparers.ContainsKey(name) ? _specialComparers[name]:StringComparer.OrdinalIgnoreCase //default
                );
                if (parsed != null) _data.Add(name, parsed);
            }
            catch(Exception ex)
            {
                if (_dbLogger != null) _dbLogger.LogDBMessage(ex.Message);
            }
        }
        /// <summary>
        /// Get one tsv data from database.
        /// </summary>
        /// <typeparam name="T">One data type from the table (not the whole table type, whole table is dictionary)</typeparam>
        /// <param name="tableName">the name of the table (aka KEY for the TSV file).</param>
        /// <param name="result">the table with the key. <c>null</c> if the data doesn't exist.</param>
        /// <returns><c>true</c> if the table was found and it's valid, otherwise <c>false</c>.</returns>
        public bool TryGetTable<T>(string tableName, out Dictionary<string, T>? result) where T:IInfo
        {
            if (!_data.TryGetValue(tableName, out var data))
            {
                result = null;
                return false;
            }
            result = data as Dictionary<string, T>;
            return result != null;
        }
    }
}
