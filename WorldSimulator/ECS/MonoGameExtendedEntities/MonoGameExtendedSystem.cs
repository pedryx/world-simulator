using Microsoft.Xna.Framework;

using MonoGame.Extended.Entities;
using MonoGame.Extended.Entities.Systems;

using WorldSimulator.ECS.AbstractECS;

namespace WorldSimulator.ECS.MonoGameExtendedEntities;
internal class MonoGameExtendedSystem<TEntityProcessor, TComponent1> : EntityUpdateSystem, IECSSystem 
    where TEntityProcessor : IEntityProcessor<TComponent1>
    where TComponent1 : struct
{
    private readonly TEntityProcessor processor;

    private ComponentMapper<ComponentWrapper<TComponent1>> componentMapper1;

    public MonoGameExtendedSystem(TEntityProcessor processor)
        : base(Aspect.All(typeof(ComponentWrapper<TComponent1>)))
    {
        this.processor = processor;
    }

    public void Initialize(IECSWorld world) 
    {
        ((MonoGameExtendedWorld)world).AddSystem(this);
    }

    public override void Initialize(IComponentMapperService mapperService)
    {
        componentMapper1 = mapperService.GetMapper<ComponentWrapper<TComponent1>>();
    }

    public void Update(float deltaTime)
    {
        processor.PreUpdate(deltaTime);

        foreach (var entity in ActiveEntities)
        {
            var component1 = componentMapper1.Get(entity);

            processor.Process(ref component1.Component, deltaTime);
        }

        processor.PostUpdate(deltaTime);
    }

    public override void Update(GameTime gameTime) { }
}

internal class MonoGameExtendedSystem<TEntityProcessor, TComponent1, TComponent2> : EntityUpdateSystem, IECSSystem
    where TEntityProcessor : IEntityProcessor<TComponent1, TComponent2>
    where TComponent1 : struct
    where TComponent2 : struct
{
    private readonly TEntityProcessor processor;

    private ComponentMapper<ComponentWrapper<TComponent1>> componentMapper1;
    private ComponentMapper<ComponentWrapper<TComponent2>> componentMapper2;

    public MonoGameExtendedSystem(TEntityProcessor processor)
        : base(Aspect.All(typeof(ComponentWrapper<TComponent1>), typeof(ComponentWrapper<TComponent2>)))
    {
        this.processor = processor;
    }

    public void Initialize(IECSWorld world)
    {
        ((MonoGameExtendedWorld)world).AddSystem(this);
    }

    public override void Initialize(IComponentMapperService mapperService)
    {
        componentMapper1 = mapperService.GetMapper<ComponentWrapper<TComponent1>>();
        componentMapper2 = mapperService.GetMapper<ComponentWrapper<TComponent2>>();
    }

    public void Update(float deltaTime)
    {
        processor.PreUpdate(deltaTime);

        foreach (var entity in ActiveEntities)
        {
            var component1 = componentMapper1.Get(entity);
            var component2 = componentMapper2.Get(entity);

            processor.Process(ref component1.Component, ref component2.Component, deltaTime);
        }

        processor.PostUpdate(deltaTime);
    }

    public override void Update(GameTime gameTime) { }
}

internal class MonoGameExtendedSystem<TEntityProcessor, TComponent1, TComponent2, TComponent3> 
    : EntityUpdateSystem, IECSSystem
    where TEntityProcessor : IEntityProcessor<TComponent1, TComponent2, TComponent3>
    where TComponent1 : struct
    where TComponent2 : struct
    where TComponent3 : struct
{
    private readonly TEntityProcessor processor;

    private ComponentMapper<ComponentWrapper<TComponent1>> componentMapper1;
    private ComponentMapper<ComponentWrapper<TComponent2>> componentMapper2;
    private ComponentMapper<ComponentWrapper<TComponent3>> componentMapper3;

    public MonoGameExtendedSystem(TEntityProcessor processor)
        : base(Aspect.All
        (
            typeof(ComponentWrapper<TComponent1>), 
            typeof(ComponentWrapper<TComponent2>),
            typeof(ComponentWrapper<TComponent3>)
        ))
    {
        this.processor = processor;
    }

    public void Initialize(IECSWorld world)
    {
        ((MonoGameExtendedWorld)world).AddSystem(this);
    }

    public override void Initialize(IComponentMapperService mapperService)
    {
        componentMapper1 = mapperService.GetMapper<ComponentWrapper<TComponent1>>();
        componentMapper2 = mapperService.GetMapper<ComponentWrapper<TComponent2>>();
        componentMapper3 = mapperService.GetMapper<ComponentWrapper<TComponent3>>();
    }

    public void Update(float deltaTime)
    {
        processor.PreUpdate(deltaTime);

        foreach (var entity in ActiveEntities)
        {
            var component1 = componentMapper1.Get(entity);
            var component2 = componentMapper2.Get(entity);
            var component3 = componentMapper3.Get(entity);

            processor.Process(ref component1.Component, ref component2.Component, ref component3.Component, deltaTime);
        }

        processor.PostUpdate(deltaTime);
    }

    public override void Update(GameTime gameTime) { }
}

internal class MonoGameExtendedSystem<TEntityProcessor, TComponent1, TComponent2, TComponent3, TComponent4>
    : EntityUpdateSystem, IECSSystem
    where TEntityProcessor : IEntityProcessor<TComponent1, TComponent2, TComponent3, TComponent4>
    where TComponent1 : struct
    where TComponent2 : struct
    where TComponent3 : struct
    where TComponent4 : struct
{
    private readonly TEntityProcessor processor;

    private ComponentMapper<ComponentWrapper<TComponent1>> componentMapper1;
    private ComponentMapper<ComponentWrapper<TComponent2>> componentMapper2;
    private ComponentMapper<ComponentWrapper<TComponent3>> componentMapper3;
    private ComponentMapper<ComponentWrapper<TComponent4>> componentMapper4;

    public MonoGameExtendedSystem(TEntityProcessor processor)
        : base(Aspect.All
        (
            typeof(ComponentWrapper<TComponent1>),
            typeof(ComponentWrapper<TComponent2>),
            typeof(ComponentWrapper<TComponent3>),
            typeof(ComponentWrapper<TComponent4>)
        ))
    {
        this.processor = processor;
    }

    public void Initialize(IECSWorld world)
    {
        ((MonoGameExtendedWorld)world).AddSystem(this);
    }

    public override void Initialize(IComponentMapperService mapperService)
    {
        componentMapper1 = mapperService.GetMapper<ComponentWrapper<TComponent1>>();
        componentMapper2 = mapperService.GetMapper<ComponentWrapper<TComponent2>>();
        componentMapper3 = mapperService.GetMapper<ComponentWrapper<TComponent3>>();
        componentMapper4 = mapperService.GetMapper<ComponentWrapper<TComponent4>>();
    }

    public void Update(float deltaTime)
    {
        processor.PreUpdate(deltaTime);

        foreach (var entity in ActiveEntities)
        {
            var component1 = componentMapper1.Get(entity);
            var component2 = componentMapper2.Get(entity);
            var component3 = componentMapper3.Get(entity);
            var component4 = componentMapper4.Get(entity);

            processor.Process
            (
                ref component1.Component, 
                ref component2.Component, 
                ref component3.Component, 
                ref component4.Component,
                deltaTime
            );
        }

        processor.PostUpdate(deltaTime);
    }

    public override void Update(GameTime gameTime) { }
}