using CsvHelper;
using DiscordBot.Database.DataTypes;
using System.Globalization;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("DiscordBot.Tests")]
namespace DiscordBot.Database
{
    /// <summary>
    /// Deserializes TSV to dictionary.
    /// </summary>
    internal class TsvParser
    {
        /// <summary>
        /// Parse the TSV from path and deserializes it.
        /// </summary>
        /// <typeparam name="T">The data type (which is A LINE of table).</typeparam>
        /// <param name="path">Path of the tsv file.</param>
        /// <param name="comparer">String comparer for the dictionary key searching.</param>
        /// <returns>Dictionary of the TSV data.</returns>
        /// <note>The key is defined in the T(:IInfo) value.</note>
        internal Dictionary<string,T> Parse<T>(string path, StringComparer comparer) where T:IInfo
        {
            CsvHelper.Configuration.CsvConfiguration config =
                new CsvHelper.Configuration.CsvConfiguration(CultureInfo.InvariantCulture);
            config.TrimOptions = CsvHelper.Configuration.TrimOptions.Trim;
            config.Delimiter = "\t";
            using (var reader = new StreamReader(path))
            using (var csv = new CsvReader(reader, config))
            {
                return csv.GetRecords<T>().ToDictionary(data => data.GetKey(), comparer);
            }
        }
    }
}
