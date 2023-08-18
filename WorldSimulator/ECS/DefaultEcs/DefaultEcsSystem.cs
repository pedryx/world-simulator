// Ignore Spelling: Ecs

using DefaultEcs;

using WorldSimulator.ECS.AbstractECS;

namespace WorldSimulator.ECS.DefaultEcs;
internal class DefaultEcsSystem<TEntityProcessor, TComponent> : IECSSystem
    where TEntityProcessor : struct, IEntityProcessor<TComponent>
    where TComponent : struct
{
    private readonly TEntityProcessor processor;

    private EntitySet set;

    public DefaultEcsSystem(TEntityProcessor processor)
    {
        this.processor = processor;
    }

    public void Initialize(IECSWorld world)
        => set = ((BasicECSWorld<World>)world).World
            .GetEntities()
            .With<TComponent>()
            .AsSet();

    public void Update(float deltaTime)
    {
        processor.PreUpdate(deltaTime);
        foreach (var entity in set.GetEntities())
        {
            ref var component = ref entity.Get<TComponent>();

            processor.Process(ref component, deltaTime);
        }
        processor.PostUpdate(deltaTime);
    }
}

internal class DefaultEcsSystem<TEntityProcessor, TComponent1, TComponent2> : IECSSystem
    where TEntityProcessor : struct, IEntityProcessor<TComponent1, TComponent2>
    where TComponent1 : struct
    where TComponent2 : struct
{
    private readonly TEntityProcessor processor;

    private EntitySet set;

    public DefaultEcsSystem(TEntityProcessor processor)
    {
        this.processor = processor;
    }

    public void Initialize(IECSWorld world)
        => set = ((BasicECSWorld<World>)world).World
            .GetEntities()
            .With<TComponent1>()
            .With<TComponent2>()
            .AsSet();

    public void Update(float deltaTime)
    {
        processor.PreUpdate(deltaTime);
        foreach (var entity in set.GetEntities())
        {
            ref var component1 = ref entity.Get<TComponent1>();
            ref var component2 = ref entity.Get<TComponent2>();

            processor.Process(ref component1, ref component2, deltaTime);
        }
        processor.PostUpdate(deltaTime);
    }
}

internal class DefaultEcsSystem<TEntityProcessor, TComponent1, TComponent2, TComponent3> : IECSSystem
    where TEntityProcessor : struct, IEntityProcessor<TComponent1, TComponent2, TComponent3>
    where TComponent1 : struct
    where TComponent2 : struct
    where TComponent3 : struct
{
    private readonly TEntityProcessor processor;

    private EntitySet set;

    public DefaultEcsSystem(TEntityProcessor processor)
    {
        this.processor = processor;
    }

    public void Initialize(IECSWorld world)
        => set = ((BasicECSWorld<World>)world).World
            .GetEntities()
            .With<TComponent1>()
            .With<TComponent2>()
            .With<TComponent3>()
            .AsSet();

    public void Update(float deltaTime)
    {
        processor.PreUpdate(deltaTime);
        foreach (var entity in set.GetEntities())
        {
            ref var component1 = ref entity.Get<TComponent1>();
            ref var component2 = ref entity.Get<TComponent2>();
            ref var component3 = ref entity.Get<TComponent3>();

            processor.Process(ref component1, ref component2, ref component3, deltaTime);
        }
        processor.PostUpdate(deltaTime);
    }
}

internal class DefaultEcsSystem<TEntityProcessor, TComponent1, TComponent2, TComponent3, TComponent4> : IECSSystem
    where TEntityProcessor : struct, IEntityProcessor<TComponent1, TComponent2, TComponent3, TComponent4>
    where TComponent1 : struct
    where TComponent2 : struct
    where TComponent3 : struct
    where TComponent4 : struct
{
    private readonly TEntityProcessor processor;

    private EntitySet set;

    public DefaultEcsSystem(TEntityProcessor processor)
    {
        this.processor = processor;
    }

    public void Initialize(IECSWorld world)
        => set = ((BasicECSWorld<World>)world).World
            .GetEntities()
            .With<TComponent1>()
            .With<TComponent2>()
            .With<TComponent3>()
            .With<TComponent4>()
            .AsSet();

    public void Update(float deltaTime)
    {
        processor.PreUpdate(deltaTime);
        foreach (var entity in set.GetEntities())
        {
            ref var component1 = ref entity.Get<TComponent1>();
            ref var component2 = ref entity.Get<TComponent2>();
            ref var component3 = ref entity.Get<TComponent3>();
            ref var component4 = ref entity.Get<TComponent4>();

            processor.Process
            (
                ref component1,
                ref component2,
                ref component3,
                ref component4,
                deltaTime
            );
        }
        processor.PostUpdate(deltaTime);
    }
}