﻿using WorldSimulator.ECS.AbstractECS;

namespace WorldSimulator.Components;
/// <summary>
/// Adds capability to own items to an entity.
/// </summary>
[Component]
internal struct Inventory
{
    public ItemCollection Items = new();

    public Inventory() { }
}
