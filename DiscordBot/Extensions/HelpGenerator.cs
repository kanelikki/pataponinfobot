using Discord.Interactions;
using DiscordBot.Models;
using System.Collections.ObjectModel;
using System.Reflection;

namespace DiscordBot
{
    internal class HelpGenerator
    {
        internal ReadOnlyDictionary<string, IEnumerable<HelpModel>> GenerateHelp(
            bool generateAllHelp, string defaultLabelNme)
        {
            var moduleTypes = Assembly.GetExecutingAssembly().GetTypes()
                .Where(t =>
                    t.IsAssignableTo(typeof(InteractionModuleBase<SocketInteractionContext>))
                );
            var result = new Dictionary<string, List<HelpModel>>();

            foreach (var moduleType in moduleTypes)
            {
                var helpGroup = moduleType.GetCustomAttribute(typeof(HelpGroupAttribute)) as HelpGroupAttribute;
                if (!generateAllHelp && helpGroup == null) continue;
                var helpGroupName = helpGroup?.Name ?? defaultLabelNme;
                var attributes = GetCommandAttributes(moduleType);
                (string groupName, string groupDesc) = GetGroupName(moduleType);

                List<(string command, string description)> commands = new();
                foreach (var slash in attributes)
                {
                    commands.Add((slash.Name, slash.Description));
                }
                if (commands.Count > 0)
                {
                    AddToHelp(
                        result, helpGroupName,
                        new HelpModel()
                        {
                            Group = groupName,
                            GroupDescription = groupDesc,
                            Commands = commands
                        }
                    );
                }
            }
            return result
                .ToDictionary(kv => kv.Key, kv => kv.Value.AsEnumerable()).AsReadOnly();
        }
        private (string groupName, string groupDesc) GetGroupName(Type moduleType)
        {
            var groupAttribute = moduleType.GetCustomAttribute<GroupAttribute>();
            string groupName = "";
            string groupDesc = "";

            if (groupAttribute != null)
            {
                groupName = groupAttribute.Name;
                groupDesc = groupAttribute.Description;
            }
            return (groupName, groupDesc);
        }
        private IEnumerable<SlashCommandAttribute> GetCommandAttributes(Type moduleType) =>
                moduleType.GetMethods()
                    .Where(t => t.GetCustomAttribute(typeof(NoHelpAttribute)) == null)
                    .Select(m => m.GetCustomAttribute<SlashCommandAttribute>())
                    .Where(m => m != null);
        private void AddToHelp(Dictionary<string, List<HelpModel>> pool, string helpGroupName, HelpModel modelToAdd)
        {
            if (!pool.TryGetValue(helpGroupName, out var commandList))
            {
                pool.Add(helpGroupName, new List<HelpModel>());
            }
            pool[helpGroupName].Add(modelToAdd);
        }
    }
}
