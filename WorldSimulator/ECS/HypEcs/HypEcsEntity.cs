using HypEcs;

using System;
using System.Collections.Generic;

using WorldSimulator.ECS.AbstractECS;

namespace WorldSimulator.ECS.HypEcs;
internal class HypEcsEntity : IEntity
{
    private readonly Entity entity;
    private readonly World world;
    private readonly HypEcsFactory factory;

    private readonly List<Action> componentActions = new();

    public HypEcsEntity(Entity entity, World world, HypEcsFactory factory)
    {
        this.entity = entity;
        this.world = world;
        this.factory = factory;

        factory.AddEntity(this);
    }

    public void AddComponent<TComponent>(TComponent component)
        where TComponent : struct 
        => componentActions.Add(() => world.AddComponent(entity, component));

    public void Destroy()
    {
        factory.RemoveEntity(this);
        world.Despawn(entity);
    }

    public ref TComponent GetComponent<TComponent>()
        where TComponent : struct 
        => ref world.GetComponent<TComponent>(entity);

    public bool HasComponent<TComponent>()
        where TComponent : struct 
        => world.HasComponent<TComponent>(entity);

    public bool IsDestroyed()
        => !world.IsAlive(entity);

    public void RemoveComponent<TComponent>()
        where TComponent : struct 
        => componentActions.Add(() => world.RemoveComponent<TComponent>(entity));

    public void Update()
    {
        foreach (var action in componentActions)
        {
            action.Invoke();
        }

        componentActions.Clear();
    }
}
