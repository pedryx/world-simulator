using DefaultEcs;

using WorldSimulator.ECS.AbstractECS;

namespace WorldSimulator.ECS.DefaultEcs;
internal class DefaultEcsSystem<TComponent1> : ECSSystem<EntityProcessor<TComponent1>>
{
    private EntitySet set;

    public void Initialize(IECSWorld world)
        => set = ((BasicECSWorld<World>)world).World
            .GetEntities()
            .With<TComponent1>()
            .AsSet();

    public override void Update(float deltaTime)
    {
        Processor.PreUpdate(deltaTime);
        foreach (var entity in set.GetEntities())
        {
            ref var component1 = ref entity.Get<TComponent1>();

            Processor.Process(ref component1, deltaTime);
        }
        Processor.PostProcess(deltaTime);
    }
}

internal class DefaultEcsSystem<TComponent1, TComponent2> 
    : ECSSystem<EntityProcessor<TComponent1, TComponent2>>
{
    private EntitySet set;

    public void Initialize(IECSWorld world)
        => set = ((BasicECSWorld<World>)world).World
            .GetEntities()
            .With<TComponent1>()
            .With<TComponent2>()
            .AsSet();

    public override void Update(float deltaTime)
    {
        Processor.PreUpdate(deltaTime);
        foreach (var entity in set.GetEntities())
        {
            ref var component1 = ref entity.Get<TComponent1>();
            ref var component2 = ref entity.Get<TComponent2>();

            Processor.Process(ref component1, ref component2, deltaTime);
        }
        Processor.PostProcess(deltaTime);
    }
}

internal class DefaultEcsSystem<TComponent1, TComponent2, TComponent3> 
    : ECSSystem<EntityProcessor<TComponent1, TComponent2, TComponent3>>
{
    private EntitySet set;

    public void Initialize(IECSWorld world)
        => set = ((BasicECSWorld<World>)world).World
            .GetEntities()
            .With<TComponent1>()
            .With<TComponent2>()
            .With<TComponent3>()
            .AsSet();

    public override void Update(float deltaTime)
    {
        Processor.PreUpdate(deltaTime);
        foreach (var entity in set.GetEntities())
        {
            ref var component1 = ref entity.Get<TComponent1>();
            ref var component2 = ref entity.Get<TComponent2>();
            ref var component3 = ref entity.Get<TComponent3>();

            Processor.Process(ref component1, ref component2, ref component3, deltaTime);
        }
        Processor.PostProcess(deltaTime);
    }
}

internal class DefaultEcsSystem<TComponent1, TComponent2, TComponent3, TComponent4> 
    : ECSSystem<EntityProcessor<TComponent1, TComponent2, TComponent3, TComponent4>>
{
    private EntitySet set;

    public void Initialize(IECSWorld world)
        => set = ((BasicECSWorld<World>)world).World
            .GetEntities()
            .With<TComponent1>()
            .With<TComponent2>()
            .With<TComponent3>()
            .With<TComponent4>()
            .AsSet();

    public override void Update(float deltaTime)
    {
        Processor.PreUpdate(deltaTime);
        foreach (var entity in set.GetEntities())
        {
            ref var component1 = ref entity.Get<TComponent1>();
            ref var component2 = ref entity.Get<TComponent2>();
            ref var component3 = ref entity.Get<TComponent3>();
            ref var component4 = ref entity.Get<TComponent4>();

            Processor.Process(ref component1, ref component2, ref component3, ref component4, deltaTime);
        }
        Processor.PostProcess(deltaTime);
    }
}