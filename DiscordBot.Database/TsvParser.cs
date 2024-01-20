using CsvHelper;
using DiscordBot.Database.DataTypes;
using System.Globalization;

//[assembly: InternalsVisibleTo("DiscordBot.Tests")]
namespace DiscordBot.Database
{
    public class TsvParser
    {
        private Dictionary<string,T> Parse<T>(string path)where T:IInfo
        {
            CsvHelper.Configuration.CsvConfiguration config =
                new CsvHelper.Configuration.CsvConfiguration(CultureInfo.InvariantCulture);
            config.Delimiter = "\t";
            using (var reader = new StreamReader(path))
            using (var csv = new CsvReader(reader, config))
            {
                return csv.GetRecords<T>().ToDictionary(data => data.GetKey());
            }
        }
        public Dictionary<string, CsGrindInfo> ParseCS()
            => Parse<CsGrindInfo>(FilePaths.CSGrindPath);
    }
}
