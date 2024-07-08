using Svelto.ECS;

using System;

using WorldSimulator.ECS.AbstractECS;

namespace WorldSimulator.ECS.SveltoECS;

internal class SveltoECSSystem<TEntityProcessor, TComponent> : IECSSystem, IQueryingEntitiesEngine
    where TEntityProcessor : struct, IEntityProcessor<TComponent>
    where TComponent : unmanaged
{
    private readonly TEntityProcessor processor;
    
    public EntitiesDB entitiesDB { get; set; }

    public SveltoECSSystem(TEntityProcessor processor)
    {
        this.processor = processor;
    }

    public void Ready() { }

    public void Initialize(IECSWorld world)
    {
        SveltoECSWorld sveltoWorld = (SveltoECSWorld)world;

        sveltoWorld.Context.AddEngine(this);
    }

    public void Update(float deltaTime)
    {
        processor.PreUpdate(deltaTime);

        var groups = entitiesDB.FindGroups<ComponentWrapper<TComponent>>();
        var entities = entitiesDB.QueryEntities<ComponentWrapper<TComponent>>(groups);

        foreach (var ((components, count), _) in entities)
        {
            for (int i = 0; i < count; i++)
            {
                processor.Process(ref components[i].Component, deltaTime);
            }
        }

        processor.PostUpdate(deltaTime);
    }
}

internal class SveltoECSSystem<TEntityProcessor, TComponent1, TComponent2> : IECSSystem, IQueryingEntitiesEngine
    where TEntityProcessor : struct, IEntityProcessor<TComponent1, TComponent2>
    where TComponent1 : unmanaged
    where TComponent2 : unmanaged
{
    private readonly TEntityProcessor processor;

    public EntitiesDB entitiesDB { get; set; }

    public SveltoECSSystem(TEntityProcessor processor)
    {
        this.processor = processor;
    }

    public void Ready() { }

    public void Initialize(IECSWorld world)
    {
        SveltoECSWorld sveltoWorld = (SveltoECSWorld)world;

        sveltoWorld.Context.AddEngine(this);
    }

    public void Update(float deltaTime)
    {
        processor.PreUpdate(deltaTime);

        var groups = entitiesDB.FindGroups<ComponentWrapper<TComponent1>, ComponentWrapper<TComponent2>>();
        var entities = entitiesDB.QueryEntities<ComponentWrapper<TComponent1>, ComponentWrapper<TComponent2>>(groups);

        foreach (var ((components1, components2, count), _) in entities)
        {
            for (int i = 0; i < count; i++)
            {
                processor.Process(ref components1[i].Component, ref components2[i].Component, deltaTime);
            }
        }

        processor.PostUpdate(deltaTime);
    }
}

internal class SveltoECSSystem<TEntityProcessor, TComponent1, TComponent2, TComponent3>
    : IECSSystem, IQueryingEntitiesEngine
    where TEntityProcessor : struct, IEntityProcessor<TComponent1, TComponent2, TComponent3>
    where TComponent1 : unmanaged
    where TComponent2 : unmanaged
    where TComponent3 : unmanaged
{
    private readonly TEntityProcessor processor;

    public EntitiesDB entitiesDB { get; set; }

    public SveltoECSSystem(TEntityProcessor processor)
    {
        this.processor = processor;
    }

    public void Ready() { }

    public void Initialize(IECSWorld world)
    {
        SveltoECSWorld sveltoWorld = (SveltoECSWorld)world;

        sveltoWorld.Context.AddEngine(this);
    }

    public void Update(float deltaTime)
    {
        processor.PreUpdate(deltaTime);

        var groups = entitiesDB.FindGroups
        <
            ComponentWrapper<TComponent1>,
            ComponentWrapper<TComponent2>,
            ComponentWrapper<TComponent3>
        >();
        var entities = entitiesDB.QueryEntities
        <
            ComponentWrapper<TComponent1>,
            ComponentWrapper<TComponent2>,
            ComponentWrapper<TComponent3>
        >(groups);

        foreach (var ((components1, components2, components3, count), _) in entities)
        {
            for (int i = 0; i < count; i++)
            {
                processor.Process
                (
                    ref components1[i].Component,
                    ref components2[i].Component,
                    ref components3[i].Component,
                    deltaTime
                );
            }
        }

        processor.PostUpdate(deltaTime);
    }
}

internal class SveltoECSSystem<TEntityProcessor, TComponent1, TComponent2, TComponent3, TComponent4>
    : IECSSystem, IQueryingEntitiesEngine
    where TEntityProcessor : struct, IEntityProcessor<TComponent1, TComponent2, TComponent3, TComponent4>
    where TComponent1 : unmanaged
    where TComponent2 : unmanaged
    where TComponent3 : unmanaged
    where TComponent4 : unmanaged
{
    private readonly TEntityProcessor processor;

    public EntitiesDB entitiesDB { get; set; }

    public SveltoECSSystem(TEntityProcessor processor)
    {
        this.processor = processor;
    }

    public void Ready() { }

    public void Initialize(IECSWorld world)
    {
        SveltoECSWorld sveltoWorld = (SveltoECSWorld)world;

        sveltoWorld.Context.AddEngine(this);
    }

    public void Update(float deltaTime)
    {
        processor.PreUpdate(deltaTime);

        var groups = entitiesDB.FindGroups
        <
            ComponentWrapper<TComponent1>,
            ComponentWrapper<TComponent2>,
            ComponentWrapper<TComponent3>,
            ComponentWrapper<TComponent4>
        >();
        var entities = entitiesDB.QueryEntities
        <
            ComponentWrapper<TComponent1>,
            ComponentWrapper<TComponent2>,
            ComponentWrapper<TComponent3>,
            ComponentWrapper<TComponent4>
        >(groups);

        foreach (var ((components1, components2, components3, components4, count), _) in entities)
        {
            for (int i = 0; i < count; i++)
            {
                processor.Process
                (
                    ref components1[i].Component,
                    ref components2[i].Component,
                    ref components3[i].Component,
                    ref components4[i].Component,
                    deltaTime
                );
            }
        }

        processor.PostUpdate(deltaTime);
    }

}