﻿using Microsoft.Xna.Framework;

using System;
using System.Collections.Generic;

namespace WorldSimulator.UI;
/// <summary>
/// Represent user interface element.
/// </summary>
internal abstract class UIElement
{
    private readonly List<UIElement> childs = new();

    /// <summary>
    /// Offset from parent element or position of element if element is directly in UI layer.
    /// </summary>
    public Vector2 Offset;

    public Game Game { get; private set; }
    public GameState GameState { get; private set; }
    /// <summary>
    /// UI layer to which this element balong.
    /// </summary>
    public UILayer UILayer { get; private set; }

    public abstract Rectangle Bounds { get; }

    protected virtual void Initialize() { }

    public void Initialize(UILayer owner)
    {
        Game = owner.Game;
        GameState = owner.GameState;
        UILayer = owner;

        Initialize();
    }

    protected void AddChild(UIElement element)
    {
        element.Initialize(UILayer);
        childs.Add(element);
    }

    protected void RemoveChild(UIElement element)
        => childs.Remove(element);

    public virtual void Update(Vector2 position, float deltaTime)
    {
        foreach (var child in childs)
        {
            child.Update(position + child.Offset, deltaTime);
        }
    }

    public virtual void Draw(Vector2 position, float deltaTime)
    {
        foreach (var child in childs)
        {
            child.Draw(position + child.Offset, deltaTime);
        }
    }
}