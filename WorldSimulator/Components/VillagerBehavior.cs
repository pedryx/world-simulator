﻿using WorldSimulator.ECS.AbstractECS;

namespace WorldSimulator.Components;
/// <summary>
/// Adds behavior of villager to entity.
/// </summary>
[Component]
internal struct VillagerBehavior
{
    /// <summary>
    /// ID of village the villager belongs to.
    /// </summary>
    public int VillageID;
    /// <summary>
    /// Resource which is currently harvested by the villager.
    /// </summary>
    public IEntity HarvestedResource;
    /// <summary>
    /// Time ellapsed from waiting begins. Used by wait villager behavior node.
    /// </summary>
    public float ellapsedWait;
}
