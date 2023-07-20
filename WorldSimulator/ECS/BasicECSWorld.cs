using System;

using WorldSimulator.ECS.AbstractECS;

namespace WorldSimulator.ECS;
/// <summary>
/// Simple wrapper around ECS world of specific type.
/// </summary>
/// <typeparam name="TWorld"></typeparam>
public class BasicECSWorld<TWorld> : IECSWorld
{
    private readonly Action<TWorld> update;

    public TWorld World { get; private set; }

    public BasicECSWorld(TWorld world, Action<TWorld> update)
    {
        World = world;
        this.update = update;
    }

    public BasicECSWorld(TWorld world)
        : this(world, _ => { }) { }

    public void Update() 
        => update(World);
}
