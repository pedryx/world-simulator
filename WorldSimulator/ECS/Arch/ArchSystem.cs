using Arch.Core;

using WorldSimulator.ECS.AbstractECS;

namespace WorldSimulator.ECS.Arch;
internal class ArchSystem<TComponent1> : IECSSystem
{
    private readonly EntityProcessor<TComponent1> processor;

    private World world;
    private QueryDescription query;

    public ArchSystem(EntityProcessor<TComponent1> processor)
    {
        this.processor = processor;
    }

    public void Initialize(IECSWorld wrapper)
    {
        world = ((BasicECSWorld<World>)wrapper).World;
        query = new QueryDescription().WithAll<TComponent1>();
    }

    public void Update(float deltaTime)
    {
        processor.PreUpdate(deltaTime);
        world.Query(in query, (ref TComponent1 component1) =>
        {
            processor.Process(ref component1, deltaTime);
        });

        processor.PostUpdate(deltaTime);
    }
}

internal class ArchSystem<TComponent1, TComponent2> : IECSSystem
{
    private readonly EntityProcessor<TComponent1, TComponent2> processor;

    private World world;
    private QueryDescription query;

    public ArchSystem(EntityProcessor<TComponent1, TComponent2> processor)
    {
        this.processor = processor;
    }

    public void Initialize(IECSWorld wrapper)
    {
        world = ((BasicECSWorld<World>)wrapper).World;
        query = new QueryDescription().WithAll<TComponent1, TComponent2>();
    }

    public void Update(float deltaTime)
    {
        processor.PreUpdate(deltaTime);
        world.Query(in query, (ref TComponent1 component1, ref TComponent2 component2) =>
        {
            processor.Process(ref component1, ref component2, deltaTime);
        });
        processor.PostUpdate(deltaTime);
    }
}

internal class ArchSystem<TComponent1, TComponent2, TComponent3> : IECSSystem
{
    private readonly EntityProcessor<TComponent1, TComponent2, TComponent3> processor;

    private World world;
    private QueryDescription query;

    public ArchSystem(EntityProcessor<TComponent1, TComponent2, TComponent3> processor)
    {
        this.processor = processor;
    }

    public void Initialize(IECSWorld wrapper)
    {
        world = ((BasicECSWorld<World>)wrapper).World;
        query = new QueryDescription().WithAll<TComponent1, TComponent2, TComponent3>();
    }

    public void Update(float deltaTime)
    {
        processor.PreUpdate(deltaTime);
        world.Query
        (
            in query,
            (
                ref TComponent1 component1,
                ref TComponent2 component2,
                ref TComponent3 component3
            ) =>
            {
                processor.Process(ref component1, ref component2, ref component3, deltaTime);
            }
        );
        processor.PostUpdate(deltaTime);
    }
}

internal class ArchSystem<TComponent1, TComponent2, TComponent3, TComponent4> : IECSSystem
{
    private readonly EntityProcessor<TComponent1, TComponent2, TComponent3, TComponent4>
        processor;

    private World world;
    private QueryDescription query;

    public ArchSystem
    (
        EntityProcessor<TComponent1, TComponent2, TComponent3, TComponent4> processor
    )
    {
        this.processor = processor;
    }

    public void Initialize(IECSWorld wrapper)
    {
        world = ((BasicECSWorld<World>)wrapper).World;
        query = new QueryDescription().WithAll
        <
            TComponent1,
            TComponent2,
            TComponent3,
            TComponent4
        >();
    }

    public void Update(float deltaTime)
    {
        processor.PreUpdate(deltaTime);
        world.Query
        (
            in query,
            (
                ref TComponent1 component1,
                ref TComponent2 component2,
                ref TComponent3 component3,
                ref TComponent4 component4
            ) =>
            {
                processor.Process
                (
                    ref component1,
                    ref component2,
                    ref component3,
                    ref component4,
                    deltaTime
                );
            }
        );
        processor.PostUpdate(deltaTime);
    }
}