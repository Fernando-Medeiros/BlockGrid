namespace SFMLGame.pipeline.schemas;

public sealed class PlayerSchema
{
    #region Identity
    public string Token { get; set; } = Guid.NewGuid().ToString();
    public string Name { get; set; } = string.Empty;
    #endregion

    #region World and Region Location
    public string WorldToken { get; set; } = string.Empty;
    public string RegionToken { get; set; } = string.Empty;
    public (byte Row, byte Column) RegionPosition { get; set; } = (0, 0);
    #endregion

    #region Build
    public ERace Race { get; set; }
    public EProfession Profession { get; set; }
    public EAlignment Alignment { get; set; }
    #endregion

    #region Primary Stats
    #endregion

    #region Seed Stats
    public int TotalExperience { get; set; }
    #endregion

    #region Skill Stats
    #endregion

    #region Spell Stats
    #endregion

    #region Inventory
    #endregion

    #region Game Stats
    public int TotalVictory { get; set; }
    public int TotalDefeat { get; set; }
    public DateTime StartedOn { get; set; } = DateTime.Now;
    public DateTime UpdatedOn { get; set; } = DateTime.Now;
    #endregion

    public override string ToString()
    {
        return string.Format("{0} - {1} | {2} | {3}", Name, Race, Alignment, Profession);
    }
}