﻿using WorldSimulator.ECS.AbstractECS;

namespace WorldSimulator.Components.Villages;
/// <summary>
/// Entities with this component check their inventory to see if it contains a certain quantity of a specific item. If
/// it does, they will craft it into different items.
/// </summary>
[Component]
internal struct ResourceProcessor
{
    public int InputItemID;
    public int InputQuantity;
    public int OutputItemID;
    public int OutputQuantity;
    /// <summary>
    /// How many seconds has elapsed from the time resource processing started.
    /// </summary>
    public float Elapsed;
    /// <summary>
    /// How many seconds it takes to process resources.
    /// </summary>
    public float ProcessingTime;
    /// <summary>
    /// Determine if a resource processing is currently in progress.
    /// </summary>
    public bool Processing;
}
