using DiscordBot.Database.DataTypes;

namespace DiscordBot.SlashCommands.Models
{
    /// <summary>
    /// For only base data for multiplying.
    /// </summary>
    /// <seealso cref="BlacksmithSlashModule"/>
    public record BSBaseRequirementModel
    {
        /// <summary>
        /// Base required Ka-ching amount.
        /// </summary>
        public double Kaching { get; set; }
        private int[] _materialAmounts;
        /// <summary>
        /// Array index must represent the tier. If nothing is required, leave it as zero.
        /// </summary>
        public int[] MaterialAmounts {
            get => _materialAmounts;
            set
            {
                if (value.Length != 5) throw new InvalidOperationException("Material array length must be 5. If material is not required, fill it with zero.");
                _materialAmounts = value;
            }
        }
    }
}
