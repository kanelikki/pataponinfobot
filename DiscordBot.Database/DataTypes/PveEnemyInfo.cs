using CsvHelper.Configuration.Attributes;

namespace DiscordBot.Database.DataTypes
{
    public record PveEnemyInfo : IInfo
    {
        public string Name { get; set; }
        [Name("Enemy Type")]
        public string  EnemyType { get; set; }
        public string Immunities { get; set; }
        public float SlashTaken { get; set; }
        public float StrikeTaken { get; set; }
        public float StabTaken { get; set; }
        public float CrushTaken { get; set; }
        public float FireTaken { get; set; }
        public float IceTaken { get; set; }
        public float LightningTaken { get; set; }
        public float PoisonTaken { get; set; }
        public float SoundTaken { get; set; }
        public float LightTaken { get; set; }
        public float DarkTaken { get; set; }
        public string GetKey() => Name;
        [Ignore]
        private Dictionary<AttackElement, float> _weaknessPairs;
        [Ignore]
        private  KeyValuePair<AttackElement, float>[] _weaknesses;
        [Ignore]
        private  KeyValuePair<AttackElement, float>[] _strengthes;
        public KeyValuePair<AttackElement, float>[] GetWeaknesses()
        {
            if (_weaknesses == null)
            {
                _weaknesses =
                    GetWeakness().Where(kv => kv.Value > 1).OrderByDescending(kv => kv.Value).ToArray();
            }
            return _weaknesses;
        }
        public KeyValuePair<AttackElement, float>[] GetStrengthes()
        {
            if (_strengthes == null)
            {
                _strengthes =
                    GetWeakness().Where(kv => kv.Value < 1).OrderBy(kv => kv.Value).ToArray();
            }
            return _strengthes;
        }
        private Dictionary<AttackElement,float> GetWeakness()
        {
            if (_weaknessPairs == null)
            {
                _weaknessPairs = new Dictionary<AttackElement, float>
                {
                    { AttackElement.Slash, SlashTaken },
                    { AttackElement.Strike, StrikeTaken },
                    { AttackElement.Stab, StabTaken },
                    { AttackElement.Crush, CrushTaken },
                    { AttackElement.Fire, FireTaken },
                    { AttackElement.Ice, IceTaken },
                    { AttackElement.Lightning, LightningTaken },
                    { AttackElement.Poison, PoisonTaken },
                    { AttackElement.Sound, SoundTaken },
                    { AttackElement.Light, LightTaken },
                    { AttackElement.Darkness, DarkTaken }
                };
            }
            return _weaknessPairs;
        }
    }
}
