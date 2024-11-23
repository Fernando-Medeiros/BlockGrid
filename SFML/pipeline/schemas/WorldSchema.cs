namespace SFMLGame.pipeline.schemas;

public sealed class WorldSchema : IPrincipalSchema
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

public sealed class RegionMetaSchema : IPrincipalSchema
{
    public string Token { get; set; } = string.Empty;
    public EBiome Biome { get; set; } = EBiome.Forest;
}

public sealed class RegionSchema : IPrincipalSchema
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
    public Body2DSchema? Body2D { get; set; }
    public List<Item2DSchema> Items { get; set; } = [];
    public (byte Row, byte Column) Position { get; set; } = (0, 0);
}

public sealed class Body2DSchema
{
    public EBody Source { get; set; }
    public int Image { get; set; }
}

public sealed class Item2DSchema
{
    public int Image { get; set; }
}