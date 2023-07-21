using Leopotam.Ecs;

using WorldSimulator.ECS.AbstractECS;

namespace WorldSimulator.ECS.LeoEcs;
internal class LeoEcsSystem<TComponent1> : IECSSystem, IEcsSystem
    where TComponent1 : struct
{
    private readonly EntityProcessor<TComponent1> processor;
    // will get injected by leo ecs
    private readonly EcsFilter<TComponent1> filter = null;

    public LeoEcsSystem(EntityProcessor<TComponent1> processor)
    {
        this.processor = processor;
    }

    public void Initialize(IECSWorld wrapper)
    {
        new EcsSystems(((BasicECSWorld<EcsWorld>)wrapper).World)
            .Add(this)
            .Init();
    }

    public void Update(float deltaTime)
    {
        processor.PreUpdate(deltaTime);
        foreach (var i in filter)
        {
            ref TComponent1 component1 = ref filter.Get1(i);

            processor.Process(ref component1, deltaTime);
        }
        processor.PostUpdate(deltaTime);
    }
}

internal class LeoEcsSystem<TComponent1, TComponent2> : IECSSystem, IEcsSystem
    where TComponent1 : struct
    where TComponent2 : struct
{
    private readonly EntityProcessor<TComponent1, TComponent2> processor;
    // will get injected by leo ecs
    private readonly EcsFilter<TComponent1, TComponent2> filter = null;

    public LeoEcsSystem(EntityProcessor<TComponent1, TComponent2> processor)
    {
        this.processor = processor;
    }

    public void Initialize(IECSWorld wrapper)
    {
        new EcsSystems(((BasicECSWorld<EcsWorld>)wrapper).World)
            .Add(this)
            .Init();
    }

    public void Update(float deltaTime)
    {
        processor.PreUpdate(deltaTime);
        foreach (var i in filter)
        {
            ref TComponent1 component1 = ref filter.Get1(i);
            ref TComponent2 component2 = ref filter.Get2(i);

            processor.Process(ref component1, ref component2, deltaTime);
        }
        processor.PostUpdate(deltaTime);
    }
}

internal class LeoEcsSystem<TComponent1, TComponent2, TComponent3> : IECSSystem, IEcsSystem
    where TComponent1 : struct
    where TComponent2 : struct
    where TComponent3 : struct
{
    private readonly EntityProcessor<TComponent1, TComponent2, TComponent3> processor;
    // will get injected by leo ecs
    private readonly EcsFilter<TComponent1, TComponent2, TComponent3> filter = null;

    public LeoEcsSystem(EntityProcessor<TComponent1, TComponent2, TComponent3> processor)
    {
        this.processor = processor;
    }

    public void Initialize(IECSWorld wrapper)
    {
        new EcsSystems(((BasicECSWorld<EcsWorld>)wrapper).World)
            .Add(this)
            .Init();
    }

    public void Update(float deltaTime)
    {
        processor.PreUpdate(deltaTime);
        foreach (var i in filter)
        {
            ref TComponent1 component1 = ref filter.Get1(i);
            ref TComponent2 component2 = ref filter.Get2(i);
            ref TComponent3 component3 = ref filter.Get3(i);

            processor.Process(ref component1, ref component2, ref component3, deltaTime);
        }
        processor.PostUpdate(deltaTime);
    }
}

internal class LeoEcsSystem<TComponent1, TComponent2, TComponent3, TComponent4> 
    : IECSSystem, IEcsSystem
    where TComponent1 : struct
    where TComponent2 : struct
    where TComponent3 : struct
    where TComponent4 : struct
{
    private readonly EntityProcessor<TComponent1, TComponent2, TComponent3, TComponent4> processor;
    // will get injected by leo ecs
    private readonly EcsFilter<TComponent1, TComponent2, TComponent3, TComponent4> filter = null;

    public LeoEcsSystem(EntityProcessor<TComponent1, TComponent2, TComponent3, TComponent4> processor)
    {
        this.processor = processor;
    }

    public void Initialize(IECSWorld wrapper)
    {
        new EcsSystems(((BasicECSWorld<EcsWorld>)wrapper).World)
            .Add(this)
            .Init();
    }

    public void Update(float deltaTime)
    {
        processor.PreUpdate(deltaTime);
        foreach (var i in filter)
        {
            ref TComponent1 component1 = ref filter.Get1(i);
            ref TComponent2 component2 = ref filter.Get2(i);
            ref TComponent3 component3 = ref filter.Get3(i);
            ref TComponent4 component4 = ref filter.Get4(i);

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