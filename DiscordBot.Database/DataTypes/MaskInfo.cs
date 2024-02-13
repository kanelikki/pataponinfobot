using CsvHelper.Configuration.Attributes;

namespace DiscordBot.Database.DataTypes
{
    public record MaskInfo : IInfo
    {
        public string Name { get; set; }
        public string Id { get; set; }
        public string Boss { get; set; }
        public float Stamina { get; set; }
        [Name("Movement Speed")]
        public float MovementSpeed { get; set; }
        [Name("Attack Speed")]
        public float AttackSpeed { get; set; }
        [Name("Min Dmg")]
        public float MinDmg { get; set; }
        [Name("Max Dmg")]
        public float MaxDmg { get; set; }
        public float Critical { get; set; }
        public float Knockback { get; set; }
        public float Cnc { get; set; }
        public float Ignite { get; set; }
        public float Sleep { get; set; }
        public float Freeze { get; set; }
        [Name("Critical Resist")]
        public float CriticalResist { get; set; }
        [Name("Knockback Resist")]
        public float KnockbackResist { get; set; }
        [Name("Cnc Resist")]
        public float CncResist { get; set; }
        [Name("Ignite Resist")]
        public float IgniteResist { get; set; }
        [Name("Sleep Resist")]
        public float SleepResist { get; set; }
        [Name("Freeze Resist")]
        public float FreezeResist { get; set; }
        [Name("Melee Normal Dmg Taken")]
        public float MeleeNormalDmgTaken { get; set; }
        [Name("Melee Lance Dmg Taken")]
        public float MeleeLanceDmgTaken { get; set; }
        [Name("Spear Dmg Taken")]
        public float SpearDmgTaken { get; set; }
        [Name("Bow Dmg Taken")]
        public float BowDmgTaken { get; set; }
        [Name("Unknown Dmg Taken")]
        public float UnknownDmgTaken { get; set; }
        [Name("Crush Magic Dmg Taken")]
        public float CrushMagicDmgTaken { get; set; }
        [Name("Fire Dmg Taken")]
        public float FireDmgTaken { get; set; }
        [Name("Ice Dmg Taken")]
        public float IceDmgTaken { get; set; }
        [Name("Lightning Dmg Taken")]
        public float LightningDmgTaken { get; set; }
        public string GetKey() => Name;
    }
}
