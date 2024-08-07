﻿using WorldSimulator.ECS.AbstractECS;

namespace WorldSimulator.Components.Villages;
/// <summary>
/// Entities with this component will periodically spawn a villager. Only one villager can be spawned at a time.
/// </summary>
[Component]
internal struct VillagerSpawner
{
    /// <summary>
    /// The amount of time, in seconds, that has passed since the last villager was spawned.
    /// </summary>
    public float Elapsed = float.MaxValue;
    /// <summary>
    /// ID of the currently spawned villager.
    /// </summary>
    public int VillagerID = -1;
    /// <summary>
    /// The ID of village where the villager will be spawned.
    /// </summary>
    public int VillageID = -1;
    public VillagerProfession Profession;
    /// <summary>
    /// Determine if villager has been spawned during last game loop iteration.
    /// </summary>
    public bool JustSpawned;

    public VillagerSpawner() { }
}
