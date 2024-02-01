using DiscordBot.Database.DataTypes;

namespace DiscordBot.SlashCommands.Models
{
    /// <summary>
    /// Contains requirement info for blacksmith. This can be a unit, one level, or multiple level.
    /// </summary>
    /// <seealso cref="BSSlashModule"/>
    public record BSRequirementModel
    {
        /// <summary>
        /// Ka-ching amount required for blacksmithing.
        /// </summary>
        public int Kaching { get; set; }
        private int[] _materialAmounts;
        /// <summary>
        /// Array index must represent the tier. If nothing is required, leave it as zero.
        /// </summary>
        /// <remarks>Material can be any, and be calculated in somewhere else.</remarks>
        public int[] MaterialAmounts {
            get => _materialAmounts;
            set
            {
                if (value.Length != 5) throw new InvalidOperationException("Material array length must be 5. If material is not required, fill it with zero.");
                _materialAmounts = value;
            }
        }
        /// <summary>
        /// Adds another value to this value. This CHANGES this value.
        /// </summary>
        /// <param name="other">the other bs requirement model</param>
        /// <returns>This model.</returns>
        public BSRequirementModel Add(BSRequirementModel other)
        {
            Kaching += other.Kaching;
            for (int i = 0; i < MaterialAmounts.Length; i++)
            {
                MaterialAmounts[i] += other.MaterialAmounts[i];
            }
            return this;
        }
        public static BSRequirementModel GetEmpty()
            => new BSRequirementModel() {
                    Kaching = 0, MaterialAmounts = new int[5]
                };
    }
}
