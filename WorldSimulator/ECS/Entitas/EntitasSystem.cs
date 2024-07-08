using Entitas;

using WorldSimulator.ECS.AbstractECS;

namespace WorldSimulator.ECS.Entitas;

internal class EntitasSystem<TEntityProcessor, TComponent> : IECSSystem
    where TEntityProcessor : IEntityProcessor<TComponent>
    where TComponent : unmanaged
{
    private readonly TEntityProcessor processor;

    private System system;

    public EntitasSystem(TEntityProcessor processor) 
    {
        this.processor = processor;
    }

    public void Initialize(IECSWorld world) 
    {
        system = new System(processor, ((BasicECSWorld<Context<Entity>>)world).World);
    }

    public void Update(float deltaTime)
    {
        system.DeltaTime = deltaTime;

        system.Processor.PreUpdate(deltaTime);
        system.Execute();
        system.Processor.PostUpdate(deltaTime);
    }

    private class System : JobSystem<Entity>
    {
        public TEntityProcessor Processor { get; private set; }
        public float DeltaTime { get; set; }

        public System(TEntityProcessor processor, Context<Entity> context)
            : base(context.GetGroup(Matcher<Entity>.AllOf(ComponentWrapper<TComponent>.ID)), 1)
        {
            Processor = processor;
        }

        protected override void Execute(Entity entity)
        {
            ref TComponent component = ref ((ComponentWrapper<TComponent>)entity
                .GetComponent(ComponentWrapper<TComponent>.ID)).Component;

            Processor.Process(ref component, DeltaTime);
        }
    }
}

internal class EntitasSystem<TEntityProcessor, TComponent1, TComponent2> : IECSSystem
    where TEntityProcessor : IEntityProcessor<TComponent1, TComponent2>
    where TComponent1 : unmanaged
    where TComponent2 : unmanaged
{
    private readonly TEntityProcessor processor;

    private System system;

    public EntitasSystem(TEntityProcessor processor)
    {
        this.processor = processor;
    }

    public void Initialize(IECSWorld world)
    {
        system = new System(processor, ((BasicECSWorld<Context<Entity>>)world).World);
    }

    public void Update(float deltaTime)
    {
        system.DeltaTime = deltaTime;

        system.Processor.PreUpdate(deltaTime);
        system.Execute();
        system.Processor.PostUpdate(deltaTime);
    }

    private class System : JobSystem<Entity>
    {
        public TEntityProcessor Processor { get; private set; }
        public float DeltaTime { get; set; }

        public System(TEntityProcessor processor, Context<Entity> context)
            : base(context.GetGroup(Matcher<Entity>.AllOf
            (
                ComponentWrapper<TComponent1>.ID,
                ComponentWrapper<TComponent2>.ID
            )), 1)
        {
            Processor = processor;
        }

        protected override void Execute(Entity entity)
        {
            ref TComponent1 component1 = ref ((ComponentWrapper<TComponent1>)entity
                .GetComponent(ComponentWrapper<TComponent1>.ID)).Component;
            ref TComponent2 component2 = ref ((ComponentWrapper<TComponent2>)entity
                .GetComponent(ComponentWrapper<TComponent2>.ID)).Component;

            Processor.Process(ref component1, ref component2, DeltaTime);
        }
    }
}

internal class EntitasSystem<TEntityProcessor, TComponent1, TComponent2, TComponent3> : IECSSystem
    where TEntityProcessor : IEntityProcessor<TComponent1, TComponent2, TComponent3>
    where TComponent1 : unmanaged
    where TComponent2 : unmanaged
    where TComponent3 : unmanaged
{
    private readonly TEntityProcessor processor;

    private System system;

    public EntitasSystem(TEntityProcessor processor)
    {
        this.processor = processor;
    }

    public void Initialize(IECSWorld world)
    {
        system = new System(processor, ((BasicECSWorld<Context<Entity>>)world).World);
    }

    public void Update(float deltaTime)
    {
        system.DeltaTime = deltaTime;

        system.Processor.PreUpdate(deltaTime);
        system.Execute();
        system.Processor.PostUpdate(deltaTime);
    }

    private class System : JobSystem<Entity>
    {
        public TEntityProcessor Processor { get; private set; }
        public float DeltaTime { get; set; }

        public System(TEntityProcessor processor, Context<Entity> context)
            : base(context.GetGroup(Matcher<Entity>.AllOf
            (
                ComponentWrapper<TComponent1>.ID,
                ComponentWrapper<TComponent2>.ID,
                ComponentWrapper<TComponent3>.ID
            )), 1)
        {
            Processor = processor;
        }

        protected override void Execute(Entity entity)
        {
            ref TComponent1 component1 = ref ((ComponentWrapper<TComponent1>)entity
                .GetComponent(ComponentWrapper<TComponent1>.ID)).Component;
            ref TComponent2 component2 = ref ((ComponentWrapper<TComponent2>)entity
                .GetComponent(ComponentWrapper<TComponent2>.ID)).Component;
            ref TComponent3 component3 = ref ((ComponentWrapper<TComponent3>)entity
                .GetComponent(ComponentWrapper<TComponent3>.ID)).Component;

            Processor.Process(ref component1, ref component2, ref component3, DeltaTime);
        }
    }
}

internal class EntitasSystem<TEntityProcessor, TComponent1, TComponent2, TComponent3, TComponent4> : IECSSystem
    where TEntityProcessor : IEntityProcessor<TComponent1, TComponent2, TComponent3, TComponent4>
    where TComponent1 : unmanaged
    where TComponent2 : unmanaged
    where TComponent3 : unmanaged
    where TComponent4 : unmanaged
{
    private readonly TEntityProcessor processor;

    private System system;

    public EntitasSystem(TEntityProcessor processor)
    {
        this.processor = processor;
    }

    public void Initialize(IECSWorld world)
    {
        system = new System(processor, ((BasicECSWorld<Context<Entity>>)world).World);
    }

    public void Update(float deltaTime)
    {
        system.DeltaTime = deltaTime;

        system.Processor.PreUpdate(deltaTime);
        system.Execute();
        system.Processor.PostUpdate(deltaTime);
    }

    private class System : JobSystem<Entity>
    {
        public TEntityProcessor Processor { get; private set; }

        public float DeltaTime { get; set; }

        public System(TEntityProcessor processor, Context<Entity> context)
            : base(context.GetGroup(Matcher<Entity>.AllOf
            (
                ComponentWrapper<TComponent1>.ID,
                ComponentWrapper<TComponent2>.ID,
                ComponentWrapper<TComponent3>.ID,
                ComponentWrapper<TComponent4>.ID
            )), 1)
        {
            Processor = processor;
        }

        protected override void Execute(Entity entity)
        {
            ref TComponent1 component1 = ref ((ComponentWrapper<TComponent1>)entity
                .GetComponent(ComponentWrapper<TComponent1>.ID)).Component;
            ref TComponent2 component2 = ref ((ComponentWrapper<TComponent2>)entity
                .GetComponent(ComponentWrapper<TComponent2>.ID)).Component;
            ref TComponent3 component3 = ref ((ComponentWrapper<TComponent3>)entity
                .GetComponent(ComponentWrapper<TComponent3>.ID)).Component;
            ref TComponent4 component4 = ref ((ComponentWrapper<TComponent4>)entity
                .GetComponent(ComponentWrapper<TComponent4>.ID)).Component;

            Processor.Process(ref component1, ref component2, ref component3, ref component4, DeltaTime);
        }
    }
}