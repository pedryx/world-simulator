using Microsoft.Xna.Framework;

using System;
using System.Collections.Generic;

namespace WorldSimulator.UI;
/// <summary>
/// Represent user interface element.
/// </summary>
internal abstract class UIElement
{
    private readonly List<UIElement> children = new();

    /// <summary>
    /// The offset from the parent element or position of the element if element is directly in a UI layer.
    /// </summary>
    public Vector2 Offset;

    public Game Game { get; private set; }
    public GameState GameState { get; private set; }
    /// <summary>
    /// The UI layer which owns the element.
    /// </summary>
    public UILayer UILayer { get; private set; }

    /// <summary>
    /// The bounding rectangle of the element.
    /// </summary>
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
        children.Add(element);
    }

    protected void RemoveChild(UIElement element)
        => children.Remove(element);

    public virtual void Update(Vector2 position, float deltaTime)
    {
        foreach (var child in children)
        {
            child.Update(position + child.Offset, deltaTime);
        }
    }

    public virtual void Draw(Vector2 position, float deltaTime)
    {
        foreach (var child in children)
        {
            child.Draw(position + child.Offset, deltaTime);
        }
    }
}
