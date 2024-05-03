using Leopotam.EcsLite;

using WorldSimulator.ECS.AbstractECS;

namespace WorldSimulator.ECS.LeoEcsLite;
internal class LeoEcsLiteSystem<TEntityProcessor, TComponent> : IECSSystem
    where TEntityProcessor : IEntityProcessor<TComponent>
    where TComponent : struct
{
    private readonly TEntityProcessor processor;

    public LeoEcsLiteSystem(TEntityProcessor processor)
    {
        this.processor = processor;
    }

    private EcsFilter filter;
    private EcsPool<TComponent> components;

    public void Initialize(IECSWorld wrapper)
    {
        EcsWorld world = ((BasicECSWorld<EcsWorld>)wrapper).World;

        filter = world.Filter<TComponent>().End();
        components = world.GetPool<TComponent>();
    }

    public void Update(float deltaTime)
    {
        processor.PreUpdate(deltaTime);

        foreach (var entity in filter)
        {
            ref TComponent component = ref components.Get(entity);

            processor.Process(ref component, deltaTime);
        }

        processor.PostUpdate(deltaTime);
    }
}

internal class LeoEcsLiteSystem<TEntityProcessor, TComponent1, TComponent2> : IECSSystem
    where TEntityProcessor : IEntityProcessor<TComponent1, TComponent2>
    where TComponent1 : struct
    where TComponent2 : struct
{
    private readonly TEntityProcessor processor;

    public LeoEcsLiteSystem(TEntityProcessor processor)
    {
        this.processor = processor;
    }

    private EcsFilter filter;
    private EcsPool<TComponent1> components1;
    private EcsPool<TComponent2> components2;

    public void Initialize(IECSWorld wrapper)
    {
        EcsWorld world = ((BasicECSWorld<EcsWorld>)wrapper).World;

        filter = world.Filter<TComponent1>().Inc<TComponent2>().End();
        components1 = world.GetPool<TComponent1>();
        components2 = world.GetPool<TComponent2>();
    }

    public void Update(float deltaTime)
    {
        processor.PreUpdate(deltaTime);

        foreach (var entity in filter)
        {
            ref TComponent1 component1 = ref components1.Get(entity);
            ref TComponent2 component2 = ref components2.Get(entity);

            processor.Process(ref component1, ref component2, deltaTime);
        }

        processor.PostUpdate(deltaTime);
    }
}

internal class LeoEcsLiteSystem<TEntityProcessor, TComponent1, TComponent2, TComponent3> : IECSSystem
    where TEntityProcessor : IEntityProcessor<TComponent1, TComponent2, TComponent3>
    where TComponent1 : struct
    where TComponent2 : struct
    where TComponent3 : struct
{
    private readonly TEntityProcessor processor;

    public LeoEcsLiteSystem(TEntityProcessor processor)
    {
        this.processor = processor;
    }

    private EcsFilter filter;
    private EcsPool<TComponent1> components1;
    private EcsPool<TComponent2> components2;
    private EcsPool<TComponent3> components3;

    public void Initialize(IECSWorld wrapper)
    {
        EcsWorld world = ((BasicECSWorld<EcsWorld>)wrapper).World;

        filter = world.Filter<TComponent1>().Inc<TComponent2>().Inc<TComponent3>().End();
        components1 = world.GetPool<TComponent1>();
        components2 = world.GetPool<TComponent2>();
        components3 = world.GetPool<TComponent3>();
    }

    public void Update(float deltaTime)
    {
        processor.PreUpdate(deltaTime);

        foreach (var entity in filter)
        {
            ref TComponent1 component1 = ref components1.Get(entity);
            ref TComponent2 component2 = ref components2.Get(entity);
            ref TComponent3 component3 = ref components3.Get(entity);

            processor.Process(ref component1, ref component2, ref component3, deltaTime);
        }

        processor.PostUpdate(deltaTime);
    }
}

internal class LeoEcsLiteSystem<TEntityProcessor, TComponent1, TComponent2, TComponent3, TComponent4> : IECSSystem
    where TEntityProcessor : IEntityProcessor<TComponent1, TComponent2, TComponent3, TComponent4>
    where TComponent1 : struct
    where TComponent2 : struct
    where TComponent3 : struct
    where TComponent4 : struct
{
    private readonly TEntityProcessor processor;

    public LeoEcsLiteSystem(TEntityProcessor processor)
    {
        this.processor = processor;
    }

    private EcsFilter filter;
    private EcsPool<TComponent1> components1;
    private EcsPool<TComponent2> components2;
    private EcsPool<TComponent3> components3;
    private EcsPool<TComponent4> components4;

    public void Initialize(IECSWorld wrapper)
    {
        EcsWorld world = ((BasicECSWorld<EcsWorld>)wrapper).World;

        filter = world.Filter<TComponent1>().Inc<TComponent2>().Inc<TComponent3>().Inc<TComponent4>().End();
        components1 = world.GetPool<TComponent1>();
        components2 = world.GetPool<TComponent2>();
        components3 = world.GetPool<TComponent3>();
        components4 = world.GetPool<TComponent4>();
    }

    public void Update(float deltaTime)
    {
        processor.PreUpdate(deltaTime);

        foreach (var entity in filter)
        {
            ref TComponent1 component1 = ref components1.Get(entity);
            ref TComponent2 component2 = ref components2.Get(entity);
            ref TComponent3 component3 = ref components3.Get(entity);
            ref TComponent4 component4 = ref components4.Get(entity);

            processor.Process(ref component1, ref component2, ref component3, ref component4, deltaTime);
        }

        processor.PostUpdate(deltaTime);
    }
}