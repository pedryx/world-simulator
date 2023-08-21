using Microsoft.Xna.Framework;
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
    /// Contains elements that belong to this layer.
    /// </summary>
    private readonly List<UIElement> elements = new();
    private readonly SpriteBatch spriteBatch;

    private MouseState mouseState;

    public Game Game { get; private set; }
    public GameState GameState { get; private set; }
    /// <summary>
    /// Determine if the mouse is hovering over some element of this UI layer.
    /// </summary>
    public bool MouseHover { get; private set; }

    public UILayer(GameState gameState)
    {
        Game = gameState.Game;
        GameState = gameState;
        spriteBatch = Game.SpriteBatch;
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
        spriteBatch.Begin
        (
            transformMatrix: Matrix.CreateScale(Game.ResolutionScale.X, Game.ResolutionScale.Y, 1.0f)
        );

        foreach (var element in elements)
        {
            element.Draw(element.Offset, deltaTime);
        }

        spriteBatch.End();
    }
}
