using Arch.Core;

using WorldSimulator.ECS.AbstractECS;

namespace WorldSimulator.ECS.Arch;
internal class ArchSystem<TComponent1> : ECSSystem<EntityProcessor<TComponent1>>
{
    private World world;
    private QueryDescription query;

    public void Initialize(IECSWorld world)
    {
        this.world = ((BasicECSWorld<World>)world).World;
        query = new QueryDescription().WithAll<TComponent1>();
    }

    public override void Update(float deltaTime)
    {
        Processor.PreProcess(deltaTime);
        world.Query(in query, (ref TComponent1 component1) =>
        {
            Processor.Process(ref component1, deltaTime);
        });

        Processor.PostProcess(deltaTime);
    }
}

internal class ArchSystem<TComponent1, TComponent2> 
    : ECSSystem<EntityProcessor<TComponent1, TComponent2>>
{
    private World world;
    private QueryDescription query;

    public void Initialize(IECSWorld world)
    {
        this.world = ((BasicECSWorld<World>)world).World;
        query = new QueryDescription().WithAll<TComponent1, TComponent2>();
    }

    public override void Update(float deltaTime)
    {
        Processor.PreProcess(deltaTime);
        world.Query(in query, (ref TComponent1 component1, ref TComponent2 component2) =>
        {
            Processor.Process(ref component1, ref component2, deltaTime);
        });
        Processor.PostProcess(deltaTime);
    }
}

internal class ArchSystem<TComponent1, TComponent2, TComponent3> 
    : ECSSystem<EntityProcessor<TComponent1, TComponent2, TComponent3>>
{
    private World world;
    private QueryDescription query;

    public void Initialize(IECSWorld world)
    {
        this.world = ((BasicECSWorld<World>)world).World;
        query = new QueryDescription().WithAll<TComponent1, TComponent2, TComponent3>();
    }

    public override void Update(float deltaTime)
    {
        Processor.PreProcess(deltaTime);
        world.Query
        (
            in query,
            (
                ref TComponent1 component1,
                ref TComponent2 component2,
                ref TComponent3 component3
            ) =>
            {
                Processor.Process(ref component1, ref component2, ref component3, deltaTime);
            }
        );
        Processor.PostProcess(deltaTime);
    }
}

internal class ArchSystem<TComponent1, TComponent2, TComponent3, TComponent4> 
    : ECSSystem<EntityProcessor<TComponent1, TComponent2, TComponent3, TComponent4>>
{
    private World world;
    private QueryDescription query;

    public void Initialize(IECSWorld world)
    {
        this.world = ((BasicECSWorld<World>)world).World;
        query = new QueryDescription().WithAll
        <
            TComponent1,
            TComponent2,
            TComponent3,
            TComponent4
        >();
    }

    public override void Update(float deltaTime)
    {
        Processor.PreProcess(deltaTime);
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
                Processor.Process
                (
                    ref component1,
                    ref component2,
                    ref component3,
                    ref component4,
                    deltaTime
                );
            }
        );
        Processor.PostProcess(deltaTime);
    }
}