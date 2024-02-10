namespace DiscordBot.Database.DataTypes
{
    //[!] DO NOT USE FLAG ATTRIBUTE HERE [!] : Otherwise Discord parameter won't work well
    public enum MissionType
    {
        None = 0,
        Field = 1,
        VS = 2,
        Dungeon = 4,
        MultiDungeon = 8,
        DLC = 16,
        NoDLC = Field | VS | Dungeon | MultiDungeon,
        Easy = Field | VS | Dungeon,
        All = Field | VS | Dungeon | MultiDungeon | DLC
    }
}