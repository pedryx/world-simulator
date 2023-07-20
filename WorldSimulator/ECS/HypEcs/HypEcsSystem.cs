using HypEcs;

using WorldSimulator.ECS.AbstractECS;

namespace WorldSimulator.ECS.HypEcs;
internal class HypEcsSystem<TComponent1> : IECSSystem
    where TComponent1 : struct
{
    private readonly EntityProcessor<TComponent1> processor;

    private Query<TComponent1> query;

    public HypEcsSystem(EntityProcessor<TComponent1> processor)
    {
        this.processor = processor;
    }

    public void Initialize(IECSWorld wrapper)
        => query = ((BasicECSWorld<World>)wrapper).World.Query<TComponent1>().Build();

    public void Update(float deltaTime)
    {
        processor.PreUpdate(deltaTime);
        query.Run((count, components1) =>
        {
            for (int i = 0; i < count; i++)
            {
                processor.Process(ref components1[i], deltaTime);
            }
        });
        processor.PostUpdate(deltaTime);
    }
}

internal class HypEcsSystem<TComponent1, TComponent2> : IECSSystem
    where TComponent1 : struct
    where TComponent2 : struct
{
    private readonly EntityProcessor<TComponent1, TComponent2> processor;

    private Query<TComponent1, TComponent2> query;

    public HypEcsSystem(EntityProcessor<TComponent1, TComponent2> processor)
    {
        this.processor = processor;
    }

    public void Initialize(IECSWorld wrapper)
        => query = ((BasicECSWorld<World>)wrapper).World.Query<TComponent1, TComponent2>().Build();

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

internal class HypEcsSystem<TComponent1, TComponent2, TComponent3> : IECSSystem
    where TComponent1 : struct
    where TComponent2 : struct
    where TComponent3 : struct
{
    private readonly EntityProcessor<TComponent1, TComponent2, TComponent3> processor;

    private Query<TComponent1, TComponent2, TComponent3> query;

    public HypEcsSystem(EntityProcessor<TComponent1, TComponent2, TComponent3> processor)
    {
        this.processor = processor;
    }

    public void Initialize(IECSWorld wrapper)
        => query = ((BasicECSWorld<World>)wrapper).World.Query
        <
            TComponent1,
            TComponent2,
            TComponent3
        >().Build();

    public void Update(float deltaTime)
    {
        processor.PreUpdate(deltaTime);
        query.Run((count, components1, components2, components3) =>
        {
            for (int i = 0; i < count; i++)
            {
                processor.Process
                (
                    ref components1[i],
                    ref components2[i],
                    ref components3[i],
                    deltaTime
                );
            }
        });
        processor.PostUpdate(deltaTime);
    }
}

internal class HypEcsSystem<TComponent1, TComponent2, TComponent3, TComponent4> : IECSSystem
    where TComponent1 : struct
    where TComponent2 : struct
    where TComponent3 : struct
    where TComponent4 : struct
{
    private readonly EntityProcessor<TComponent1, TComponent2, TComponent3, TComponent4> processor;

    private Query<TComponent1, TComponent2, TComponent3, TComponent4> query;

    public HypEcsSystem(EntityProcessor<TComponent1, TComponent2, TComponent3, TComponent4> processor)
    {
        this.processor = processor;
    }

    public void Initialize(IECSWorld wrapper)
        => query = ((BasicECSWorld<World>)wrapper).World.Query
        <
            TComponent1,
            TComponent2,
            TComponent3,
            TComponent4
        >().Build();

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