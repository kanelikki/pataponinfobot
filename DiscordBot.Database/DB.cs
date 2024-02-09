using CsvHelper;
using DiscordBot.Database.DataTypes;
using System.Collections.ObjectModel;
using System.Reflection;
[assembly: System.Runtime.CompilerServices.InternalsVisibleTo("DiscordBot.Tests")]
namespace DiscordBot.Database
{
    /// <summary>
    /// Represents the whole database (actually, it's not real db, but collection of csv data).
    /// </summary>
    /// <remarks>Each file is considered as table.</remarks>
    public class DB : IDB
    {
        private const string _dataPath = "data/";
        private IDbLogger? _dbLogger;
        private readonly Dictionary<Type, Dictionary<string, IInfo>> _data =
            new Dictionary<Type, Dictionary<string, IInfo>>();
        /// <summary>
        /// Starts the database.
        /// </summary>
        /// <remarks>You can use with <see cref="Microsoft.Extensions.DependencyInjection.ServiceCollection.AddSingleton"/>.</remarks>
        public DB(IDbLogger? logger = null)
        {
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
            var dbTypes = Assembly.GetExecutingAssembly().GetTypes()
                .Where(t => t.IsAssignableTo(typeof(IInfo)) && t != typeof(IInfo));
            foreach (var dbType in dbTypes)
            {
                InitEach(dbType);
            }
        }
        /// <summary>
        /// Deserializes data from TSV file.
        /// </summary>
        /// <typeparam name="T">The type to deserialize.</typeparam>
        /// <param name="name">The database name, SAME AS FILE NAME WITHOUT EXTENSION (the file MUST be ".tsv"), it works also as a "key"</param>
        private void InitEach(Type type)
        {
            try
            {
                var name = type.Name;
                var file = Path.Combine(_dataPath, name + ".tsv");

                var parsed = Parse(file, type);
                if (parsed != null) _data.Add(type, parsed);
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
        public bool TryGetTable<T>(out ReadOnlyDictionary<string, T>? result) where T:IInfo
        {
            if (!_data.TryGetValue(typeof(T), out var data) || data == null)
            {
                result = null;
                return false;
            }
            result = data.ToDictionary(kv => kv.Key, kv => (T)kv.Value).AsReadOnly();
            return true;
        }
        internal Dictionary<string, IInfo> Parse(string path, Type type)
        {
            CsvHelper.Configuration.CsvConfiguration config =
                new CsvHelper.Configuration.CsvConfiguration(System.Globalization.CultureInfo.InvariantCulture);
            config.TrimOptions = CsvHelper.Configuration.TrimOptions.Trim;
            config.Delimiter = "\t";
            using (var reader = new StreamReader(path))
            using (var csv = new CsvReader(reader, config))
            {
                return csv.GetRecords(type)
                    .Select(d => d as IInfo)
                    .ToDictionary(data => data.GetKey(), StringComparer.OrdinalIgnoreCase);
            }
        }
    }
}
