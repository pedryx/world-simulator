using Entitas;

using WorldSimulator.ECS.AbstractECS;

namespace WorldSimulator.ECS.Entitas;

internal class EntitasSystem<TComponent> : IECSSystem
    where TComponent : struct
{
    private readonly System system;

    public EntitasSystem(EntityProcessor<TComponent> processor, Context<Entity> context) 
    {
        system = new System(processor, context);
    }

    public EntitasSystem<TComponent> Clone(Context<Entity> context)
        => new(system.Processor, context);

    public void Initialize(IECSWorld world) { }

    public void Update(float deltaTime)
    {
        system.DeltaTime = deltaTime;

        system.Processor.PreUpdate(deltaTime);
        system.Execute();
        system.Processor.PostUpdate(deltaTime);
    }

    private class System : JobSystem<Entity>
    {
        public EntityProcessor<TComponent> Processor { get; private set; }
        public float DeltaTime { get; set; }

        public System(EntityProcessor<TComponent> processor, Context<Entity> context)
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

internal class EntitasSystem<TComponent1, TComponent2> : IECSSystem
    where TComponent1 : struct
    where TComponent2 : struct
{
    private readonly System system;

    public EntitasSystem(EntityProcessor<TComponent1, TComponent2> processor, Context<Entity> context)
    {
        system = new System(processor, context);
    }

    public EntitasSystem<TComponent1, TComponent2> Clone(Context<Entity> context)
        => new(system.Processor, context);

    public void Initialize(IECSWorld world) { }

    public void Update(float deltaTime)
    {
        system.DeltaTime = deltaTime;

        system.Processor.PreUpdate(deltaTime);
        system.Execute();
        system.Processor.PostUpdate(deltaTime);
    }

    private class System : JobSystem<Entity>
    {
        public EntityProcessor<TComponent1, TComponent2> Processor { get; private set; }
        public float DeltaTime { get; set; }

        public System(EntityProcessor<TComponent1, TComponent2> processor, Context<Entity> context)
            : base(context.GetGroup(Matcher<Entity>.AllOf(
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

internal class EntitasSystem<TComponent1, TComponent2, TComponent3> : IECSSystem
    where TComponent1 : struct
    where TComponent2 : struct
    where TComponent3 : struct
{
    private readonly System system;

    public EntitasSystem
    (
        EntityProcessor<TComponent1, TComponent2, TComponent3> processor,
        Context<Entity> context
    )
    {
        system = new System(processor, context);
    }

    public EntitasSystem<TComponent1, TComponent2, TComponent3> Clone(Context<Entity> context)
        => new(system.Processor, context);

    public void Initialize(IECSWorld world) { }

    public void Update(float deltaTime)
    {
        system.DeltaTime = deltaTime;

        system.Processor.PreUpdate(deltaTime);
        system.Execute();
        system.Processor.PostUpdate(deltaTime);
    }

    private class System : JobSystem<Entity>
    {
        public EntityProcessor<TComponent1, TComponent2, TComponent3> Processor { get; private set; }
        public float DeltaTime { get; set; }

        public System
        (
            EntityProcessor<TComponent1, TComponent2, TComponent3> processor,
            Context<Entity> context
        )
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

internal class EntitasSystem<TComponent1, TComponent2, TComponent3, TComponent4> : IECSSystem
    where TComponent1 : struct
    where TComponent2 : struct
    where TComponent3 : struct
    where TComponent4 : struct
{

    private readonly System system;

    public EntitasSystem
    (
        EntityProcessor<TComponent1, TComponent2, TComponent3, TComponent4> processor,
        Context<Entity> context
    )
    {
        system = new System(processor, context);
    }

    public EntitasSystem<TComponent1, TComponent2, TComponent3, TComponent4> Clone
    (
        Context<Entity> context
    )
        => new(system.Processor, context);

    public void Initialize(IECSWorld world) { }

    public void Update(float deltaTime)
    {
        system.DeltaTime = deltaTime;

        system.Processor.PreUpdate(deltaTime);
        system.Execute();
        system.Processor.PostUpdate(deltaTime);
    }

    private class System : JobSystem<Entity>
    {
        public EntityProcessor<TComponent1, TComponent2, TComponent3, TComponent4> Processor
        {
            get; 
            private set; 
        }
        public float DeltaTime { get; set; }

        public System
        (
            EntityProcessor<TComponent1, TComponent2, TComponent3, TComponent4> processor,
            Context<Entity> context
        )
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

            Processor.Process
            (
                ref component1,
                ref component2,
                ref component3,
                ref component4,
                DeltaTime
            );
        }
    }
}