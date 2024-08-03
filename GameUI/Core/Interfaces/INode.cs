﻿namespace GameUI.Core.Interfaces;

public interface INode
{
    public Tile Tile { get; set; }
    public ISprite? Sprite { get; set; }
    public IShader? Shader { get; set; }
    public NodeTree NodeTree { get; }
    public NodeCanva Canvas { get; }
    public bool Running { get; set; }
    public Task Draw();
    public void OnSelected(object? sender, TouchEventArgs e);
}