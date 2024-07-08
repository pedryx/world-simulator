using RelEcs;

using WorldSimulator.ECS.AbstractECS;

namespace WorldSimulator.ECS.RelEcs;
internal class RelEcsSystem<TEntityProcessor, TComponent1> : IECSSystem
    where TEntityProcessor : IEntityProcessor<TComponent1>
    where TComponent1 : unmanaged
{
    private readonly TEntityProcessor processor;

    Query<ComponentWrapper<TComponent1>> query;

    public RelEcsSystem(TEntityProcessor processor)
    {
        this.processor = processor;
    }

    public void Initialize(IECSWorld wrapper)
    {
        query = ((BasicECSWorld<World>)wrapper).World.Query<ComponentWrapper<TComponent1>>().Build();
    }

    public void Update(float deltaTime)
    {
        processor.PreUpdate(deltaTime);

        foreach (var wrapper in query)
        {
            processor.Process(ref wrapper.Component, deltaTime);
        }

        processor.PostUpdate(deltaTime);
    }
}

internal class RelEcsSystem<TEntityProcessor, TComponent1, TComponent2> : IECSSystem
    where TEntityProcessor : IEntityProcessor<TComponent1, TComponent2>
    where TComponent1 : unmanaged
    where TComponent2 : unmanaged
{
    private readonly TEntityProcessor processor;

    Query<ComponentWrapper<TComponent1>, ComponentWrapper<TComponent2>> query;

    public RelEcsSystem(TEntityProcessor processor)
    {
        this.processor = processor;
    }

    public void Initialize(IECSWorld wrapper)
    {
        query = ((BasicECSWorld<World>)wrapper).World.Query
        <
            ComponentWrapper<TComponent1>,
            ComponentWrapper<TComponent2>
        >().Build();
    }

    public void Update(float deltaTime)
    {
        processor.PreUpdate(deltaTime);

        foreach (var (wrapper1, wrapper2) in query)
        {
            processor.Process(ref wrapper1.Component, ref wrapper2.Component, deltaTime);
        }

        processor.PostUpdate(deltaTime);
    }
}

internal class RelEcsSystem<TEntityProcessor, TComponent1, TComponent2, TComponent3> : IECSSystem
    where TEntityProcessor : IEntityProcessor<TComponent1, TComponent2, TComponent3>
    where TComponent1 : unmanaged
    where TComponent2 : unmanaged
    where TComponent3 : unmanaged
{
    private readonly TEntityProcessor processor;

    Query<ComponentWrapper<TComponent1>, ComponentWrapper<TComponent2>, ComponentWrapper<TComponent3>> query;

    public RelEcsSystem(TEntityProcessor processor)
    {
        this.processor = processor;
    }

    public void Initialize(IECSWorld wrapper)
    {
        query = ((BasicECSWorld<World>)wrapper).World.Query
        <
            ComponentWrapper<TComponent1>,
            ComponentWrapper<TComponent2>,
            ComponentWrapper<TComponent3>
        >().Build();
    }

    public void Update(float deltaTime)
    {
        processor.PreUpdate(deltaTime);

        foreach (var (wrapper1, wrapper2, wrapper3) in query)
        {
            processor.Process(ref wrapper1.Component, ref wrapper2.Component, ref wrapper3.Component, deltaTime);
        }

        processor.PostUpdate(deltaTime);
    }
}

internal class RelEcsSystem<TEntityProcessor, TComponent1, TComponent2, TComponent3, TComponent4> : IECSSystem
    where TEntityProcessor : IEntityProcessor<TComponent1, TComponent2, TComponent3, TComponent4>
    where TComponent1 : unmanaged
    where TComponent2 : unmanaged
    where TComponent3 : unmanaged
    where TComponent4 : unmanaged
{
    private readonly TEntityProcessor processor;

    Query
    <
        ComponentWrapper<TComponent1>,
        ComponentWrapper<TComponent2>,
        ComponentWrapper<TComponent3>,
        ComponentWrapper<TComponent4>
    > query;

    public RelEcsSystem(TEntityProcessor processor)
    {
        this.processor = processor;
    }

    public void Initialize(IECSWorld wrapper)
    {
        query = ((BasicECSWorld<World>)wrapper).World.Query
        <
            ComponentWrapper<TComponent1>,
            ComponentWrapper<TComponent2>,
            ComponentWrapper<TComponent3>,
            ComponentWrapper<TComponent4>
        >().Build();
    }

    public void Update(float deltaTime)
    {
        processor.PreUpdate(deltaTime);

        foreach (var (wrapper1, wrapper2, wrapper3, wrapper4) in query)
        {
            processor.Process
            (
                ref wrapper1.Component,
                ref wrapper2.Component,
                ref wrapper3.Component,
                ref wrapper4.Component,
                deltaTime
            );
        }

        processor.PostUpdate(deltaTime);
    }
}