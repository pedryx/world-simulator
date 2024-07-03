using System;

using WorldSimulator.ECS.AbstractECS;

namespace WorldSimulator.ECS;
/// <summary>
/// A simple wrapper around a specified type of ECS world.
/// </summary>
/// <typeparam name="TWorld">The type of ECS world.</typeparam>
public class BasicECSWorld<TWorld> : IECSWorld
{
    /// <summary>
    /// Callback for the Update method of the ECS world.
    /// </summary>
    private readonly Action<TWorld> updateCallback;

    /// <summary>
    /// Wrapped ECS world instance.
    /// </summary>
    public TWorld World { get; private set; }

    /// <summary>
    /// Create a new wrapper around the ECS world.
    /// </summary>
    /// <param name="world">The instance of the ECS world.</param>
    /// <param name="updateCallback">
    /// The callback for the Update method of the ECS world. Leavne null for no callback.
    /// </param>
    public BasicECSWorld(TWorld world, Action<TWorld> updateCallback = null)
    {
        World = world ?? throw new ArgumentNullException(nameof(world));
        this.updateCallback = updateCallback;
    }

    /// <summary>
    /// Call Update method on the wrapped ECS world instance.
    /// </summary>
    public void Update() 
        => updateCallback?.Invoke(World);
}
