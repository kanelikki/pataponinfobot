using DiscordBot.Database.DataTypes;

namespace DiscordBot.SlashCommands.Models
{
    /// <summary>
    /// Contains requirement info. This can be a unit, one level, or multiple level.
    /// </summary>
    /// <seealso cref="BSSlashModule"/>
    public record UpgradeRequirementModel
    {
        /// <summary>
        /// Ka-ching amount required for blacksmithing.
        /// </summary>
        public int Kaching { get; set; }
        /// <summary>
        /// Array index must represent the tier. If nothing is required, leave it as zero.
        /// </summary>
        /// <remarks>Material can be any, and be calculated in somewhere else.</remarks>
        public int[] MaterialAmounts { get; set; }
        /// <summary>
        /// Adds another value to this value. This CHANGES this value.
        /// </summary>
        /// <param name="other">the other bs requirement model</param>
        /// <returns>This model.</returns>
        public UpgradeRequirementModel Add(UpgradeRequirementModel other)
        {
            if (MaterialAmounts.Length != other.MaterialAmounts.Length)
            {
                throw new ArgumentException(
                    $"Incompatible data: this material length {MaterialAmounts.Length}, " +
                    $"target material length {other.MaterialAmounts.Length}");
            }
            Kaching += other.Kaching;
            for (int i = 0; i < MaterialAmounts.Length; i++)
            {
                MaterialAmounts[i] += other.MaterialAmounts[i];
            }
            return this;
        }
        public static UpgradeRequirementModel GetEmpty()
            => new UpgradeRequirementModel() {
                    Kaching = 0, MaterialAmounts = new int[5]
                };
    }
}
