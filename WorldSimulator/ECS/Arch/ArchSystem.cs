using Arch.Core;

using WorldSimulator.ECS.AbstractECS;

namespace WorldSimulator.ECS.Arch;
internal class ArchSystem<TEntityProcessor, TComponent> : IECSSystem
    where TEntityProcessor : struct, IEntityProcessor<TComponent>
    where TComponent : struct
{
    private readonly TEntityProcessor processor;

    private World world;
    private QueryDescription query;

    public ArchSystem(TEntityProcessor processor)
    {
        this.processor = processor;
    }

    public void Initialize(IECSWorld wrapper)
    {
        world = ((BasicECSWorld<World>)wrapper).World;
        query = new QueryDescription().WithAll<TComponent>();
    }

    public void Update(float deltaTime)
    {
        processor.PreUpdate(deltaTime);
        world.Query(in query, (ref TComponent component) =>
        {
            processor.Process(ref component, deltaTime);
        });

        processor.PostUpdate(deltaTime);
    }
}

internal class ArchSystem<TEntityProcessor, TComponent1, TComponent2> : IECSSystem
    where TEntityProcessor : struct, IEntityProcessor<TComponent1, TComponent2>
    where TComponent1 : struct
    where TComponent2 : struct
{
    private readonly TEntityProcessor processor;

    private World world;
    private QueryDescription query;

    public ArchSystem(TEntityProcessor processor)
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

internal class ArchSystem<TEntityProcessor, TComponent1, TComponent2, TComponent3> : IECSSystem
    where TEntityProcessor : struct, IEntityProcessor<TComponent1, TComponent2, TComponent3>
    where TComponent1 : struct
    where TComponent2 : struct
    where TComponent3 : struct
{
    private readonly TEntityProcessor processor;

    private World world;
    private QueryDescription query;

    public ArchSystem(TEntityProcessor processor)
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
        world.Query(in query, (ref TComponent1 component1, ref TComponent2 component2, ref TComponent3 component3) =>
        {
            processor.Process(ref component1, ref component2, ref component3, deltaTime);
        });
        processor.PostUpdate(deltaTime);
    }
}

internal class ArchSystem<TEntityProcessor, TComponent1, TComponent2, TComponent3, TComponent4> : IECSSystem
    where TEntityProcessor : struct, IEntityProcessor<TComponent1, TComponent2, TComponent3, TComponent4>
    where TComponent1 : struct
    where TComponent2 : struct
    where TComponent3 : struct
    where TComponent4 : struct
{
    private readonly TEntityProcessor processor;

    private World world;
    private QueryDescription query;

    public ArchSystem(TEntityProcessor processor)
    {
        this.processor = processor;
    }

    public void Initialize(IECSWorld wrapper)
    {
        world = ((BasicECSWorld<World>)wrapper).World;
        query = new QueryDescription().WithAll<TComponent1, TComponent2, TComponent3, TComponent4>();
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
                processor.Process(ref component1, ref component2, ref component3, ref component4, deltaTime);
            }
        );
        processor.PostUpdate(deltaTime);
    }
}