namespace SFMLGame.pipeline.schemas;

public sealed class WorldSchema
{
    #region Identity
    public string Name { get; set; } = string.Empty;
    public string Token { get; set; } = Guid.NewGuid().ToString();
    public EWorldSize Size { get; set; } = EWorldSize.Tiny;
    #endregion

    #region Stats
    public List<RegionMetaSchema> Region { get; set; } = [];
    public DateTime UpdatedOn { get; set; } = DateTime.Now;
    #endregion
}

public sealed class RegionMetaSchema
{
    public string Token { get; set; } = string.Empty;
    public EBiome Biome { get; set; } = EBiome.Forest;
}

public sealed class RegionSchema
{
    #region Identity
    public string Name { get; set; } = string.Empty;
    public string Token { get; set; } = Guid.NewGuid().ToString();
    public EBiome Biome { get; set; } = EBiome.Forest;
    #endregion

    #region Stats
    public List<NodeSchema> Nodes { get; set; } = [];
    public DateTime UpdatedOn { get; set; } = DateTime.Now;
    #endregion
}

public sealed class NodeSchema
{
    public (byte Row, byte Column) Position { get; set; } = (0, 0);
}
