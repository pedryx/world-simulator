// Ignore Spelling: Ecs Hyp

using HypEcs;

using WorldSimulator.ECS.AbstractECS;

namespace WorldSimulator.ECS.HypEcs;
internal class HypEcsSystem<TEntityProcessor, TComponent> : IECSSystem
    where TEntityProcessor : IEntityProcessor<TComponent>
    where TComponent : unmanaged
{
    private readonly TEntityProcessor processor;

    private Query<TComponent> query;

    public HypEcsSystem(TEntityProcessor processor)
    {
        this.processor = processor;
    }

    public void Initialize(IECSWorld wrapper)
    {
        query = ((BasicECSWorld<World>)wrapper).World.Query<TComponent>().Build();
    }

    public void Update(float deltaTime)
    {
        processor.PreUpdate(deltaTime);
        query.Run((count, components) =>
        {
            for (int i = 0; i < count; i++)
            {
                processor.Process(ref components[i], deltaTime);
            }
        });
        processor.PostUpdate(deltaTime);
    }
}

internal class HypEcsSystem<TEntityProcessor, TComponent1, TComponent2> : IECSSystem
    where TEntityProcessor : IEntityProcessor<TComponent1, TComponent2>
    where TComponent1 : unmanaged
    where TComponent2 : unmanaged
{
    private readonly TEntityProcessor processor;

    private Query<TComponent1, TComponent2> query;

    public HypEcsSystem(TEntityProcessor processor)
    {
        this.processor = processor;
    }

    public void Initialize(IECSWorld wrapper)
    {
        query = ((BasicECSWorld<World>)wrapper).World.Query<TComponent1, TComponent2>().Build();
    }

    public void Update(float deltaTime)
    {
        processor.PreUpdate(deltaTime);
        query.Run((count, components1, components2) =>
        {
            for (int i = 0; i < count; i++)
            {
                processor.Process(ref components1[i], ref components2[i], deltaTime);
            }
        });
        processor.PostUpdate(deltaTime);
    }
}

internal class HypEcsSystem<TEntityProcessor, TComponent1, TComponent2, TComponent3> : IECSSystem
    where TEntityProcessor : IEntityProcessor<TComponent1, TComponent2, TComponent3>
    where TComponent1 : unmanaged
    where TComponent2 : unmanaged
    where TComponent3 : unmanaged
{
    private readonly TEntityProcessor processor;

    private Query<TComponent1, TComponent2, TComponent3> query;

    public HypEcsSystem(TEntityProcessor processor)
    {
        this.processor = processor;
    }

    public void Initialize(IECSWorld wrapper)
    {
        query = ((BasicECSWorld<World>)wrapper).World.Query<TComponent1, TComponent2, TComponent3>().Build();
    }

    public void Update(float deltaTime)
    {
        processor.PreUpdate(deltaTime);
        query.Run((count, components1, components2, components3) =>
        {
            for (int i = 0; i < count; i++)
            {
                processor.Process(ref components1[i], ref components2[i], ref components3[i], deltaTime);
            }
        });
        processor.PostUpdate(deltaTime);
    }
}

internal class HypEcsSystem<TEntityProcessor, TComponent1, TComponent2, TComponent3, TComponent4> : IECSSystem
    where TEntityProcessor : IEntityProcessor<TComponent1, TComponent2, TComponent3, TComponent4>
    where TComponent1 : unmanaged
    where TComponent2 : unmanaged
    where TComponent3 : unmanaged
    where TComponent4 : unmanaged
{
    private readonly TEntityProcessor processor;

    private Query<TComponent1, TComponent2, TComponent3, TComponent4> query;

    public HypEcsSystem(TEntityProcessor processor)
    {
        this.processor = processor;
    }

    public void Initialize(IECSWorld wrapper)
    {
        query = ((BasicECSWorld<World>)wrapper)
            .World
            .Query<TComponent1, TComponent2, TComponent3, TComponent4>()
            .Build();
    }

    public void Update(float deltaTime)
    {
        processor.PreUpdate(deltaTime);
        query.Run((count, components1, components2, components3, components4) =>
        {
            for (int i = 0; i < count; i++)
            {
                processor.Process
                (
                    ref components1[i],
                    ref components2[i],
                    ref components3[i],
                    ref components4[i],
                    deltaTime
                );
            }
        });
        processor.PostUpdate(deltaTime);
    }
}