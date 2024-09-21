﻿namespace SFMLGame.core;

public sealed class RegionSchema
{
    public string Name { get; set; } = string.Empty;
    public EBiome Biome { get; set; } = EBiome.Forest;
    public List<NodeSchema> Nodes { get; set; } = [];
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}

public sealed class NodeSchema
{
    public byte Row { get; set; } = 0;
    public byte Column { get; set; } = 0;
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
