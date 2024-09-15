namespace SFMLGame.core;

public sealed class RegionSchema
{
    public string Name { get; set; } = string.Empty;
    public ESurface ESurface { get; set; } = ESurface.Soil;
    public List<List<NodeSchema>> Nodes { get; set; } = [];
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}

public sealed class NodeSchema
{
    public byte Row { get; set; } = 0;
    public byte Column { get; set; } = 0;
    public bool Discovered { get; set; } = false;
    public BodySchema? Body { get; set; } = null;
    public List<ItemSchema> Items { get; set; } = [];
}

public sealed class BodySchema
{
    public ESprite Sprite { get; set; }
    public EBody Type { get; set; } = EBody.Static;
}

public sealed class ItemSchema
{
    public ESprite Sprite { get; set; }
}
