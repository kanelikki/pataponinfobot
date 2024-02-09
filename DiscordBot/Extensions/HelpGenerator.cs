using Discord.Interactions;
using DiscordBot.Models;
using System.Reflection;

namespace DiscordBot.Extensions
{
    internal class HelpGenerator
    {
        //maybe add attribute that excludes from generating?
        internal IEnumerable<HelpGeneratorModel> GenerateHelp()
        {
            var execAssembly = Assembly.GetExecutingAssembly();
            var moduleTypes = execAssembly.GetTypes()
                .Where(t => t.IsAssignableTo(typeof(InteractionModuleBase<SocketInteractionContext>)));
            var result = new List<HelpGeneratorModel>();
            foreach (var moduleType in moduleTypes)
            {
                var groupAttribute = moduleType.GetCustomAttribute<GroupAttribute>();
                var attributes = moduleType.GetMethods()
                    .Select(m=>m.GetCustomAttribute<SlashCommandAttribute>())
                    .Where(m => m != null);
                string groupName = "";
                string groupDesc = "";
                List<(string command, string description)> commands = new();
                if (groupAttribute != null)
                {
                    groupName = groupAttribute.Name;
                    groupDesc = groupAttribute.Description;
                }
                foreach (var slash in attributes)
                {
                    commands.Add((slash.Name, slash.Description));
                }
                if (commands.Count > 0)
                {
                    result.Add(
                        new HelpGeneratorModel()
                        {
                            Name = moduleType.Name.Replace("SlashModule",""),
                            Group = groupName,
                            GroupDescription = groupDesc,
                            Commands = commands
                        }
                    );
                }
            }
            return result;
        }
    }
}
