﻿using WorldSimulator.ECS.AbstractECS;

namespace WorldSimulator.Components;
/// <summary>
/// Adds behavior of villager to entity.
/// </summary>
[Component]
internal struct VillagerBehavior
{
    public int[] TreeState;
    public IEntity MovementTarget;
}
