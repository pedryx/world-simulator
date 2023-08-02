using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using System.Collections.Generic;

namespace WorldSimulator.UI;
/// <summary>
/// Represent layer with user interface elements.
/// </summary>
internal class UILayer
{
    /// <summary>
    /// Contains elements which belongs to this layer.
    /// </summary>
    private readonly List<UIElement> elements = new();

    private MouseState mouseState;

    public Game Game { get; private set; }
    public GameState GameState { get; private set; }
    /// <summary>
    /// Determine if mouse is hovering over some element of this UI layer.
    /// </summary>
    public bool MouseHover { get; private set; }

    public UILayer(GameState gameState)
    {
        Game = gameState.Game;
        GameState = gameState;
    }

    public void AddElement(UIElement element)
    {
        element.Initialize(this);
        elements.Add(element);
    }

    public void RemoveElement(UIElement element)
        => elements.Remove(element);

    public void Update(float deltaTime)
    {
        MouseHover = false;
        mouseState = Mouse.GetState();

        foreach (var element in elements)
        {
            element.Update(element.Offset, deltaTime);

            if (element.Bounds.Contains(mouseState.Position))
            {
                MouseHover = true;
            }
        }
    }

    public void Draw(float deltaTime)
    {
        foreach (var element in elements)
        {
            element.Draw(element.Offset, deltaTime);
        }
    }
}
