using Svelto.ECS;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

using WorldSimulator.ECS.AbstractECS;

namespace WorldSimulator.ECS.SveltoECS;

internal class SveltoECSEntity : IEntity
{
    private readonly Context context;
    private readonly Dictionary<Type, object> componentValues = new();

    private EGID egid;
    private IEntityDescriptor entityDescriptor;
    private List<IComponentBuilder> componentBuilders = new();
    private bool isBuild = false;
    private bool isDestroyed = false;
    private Type descriptorType;

    public SveltoECSEntity(Context context, SveltoECSWorld world)
    {
        this.context = context;
        world.AddEntity(this);
    }

    public void AddComponent<TComponent>(TComponent component)
        where TComponent : unmanaged
    {
        if (isBuild)
        {
            throw new InvalidOperationException("Svelto.ECS dont allow adding components after the entity is build.");
        }
        else
        {
            componentValues.Add(component.GetType(), component);
            componentBuilders.Add(new ComponentBuilder<ComponentWrapper<TComponent>>());
        }
    }

    public void Destroy()
    {
        if (isBuild)
        {
            typeof(IEntityFunctions)
                .GetMethod("RemoveEntity", new Type[] { typeof(EGID), typeof(string) })
                .MakeGenericMethod(descriptorType)
                .Invoke(context.EntityFunctions, new object[] { egid, null });
        }
        else
        {
            componentValues.Clear();
            componentBuilders.Clear();
        }

        isDestroyed = true;
    }

    public ref TComponent GetComponent<TComponent>()
        where TComponent : unmanaged
    {
        if (isBuild)
            return ref context.QueryEngine.GetComponent<TComponent>(egid);
        else
            return ref Unsafe.Unbox<TComponent>(componentValues[typeof(TComponent)]);
    }

    public bool HasComponent<TComponent>()
        where TComponent : unmanaged
    {
        if (isBuild)
            return context.QueryEngine.HasComponent<TComponent>(egid);
        else
            return componentValues.ContainsKey(typeof(TComponent));
    }

    public bool IsDestroyed()
        => isDestroyed;

    public void RemoveComponent<TComponent>()
        where TComponent : unmanaged
    {
        if (isBuild)
        {
            throw new InvalidOperationException
            (
                "Svelto.ECS dont allow removing components after the entity is build."
            );
        }
        else
        {
            componentValues.Remove(typeof(TComponent));
        }
    }

    internal void Build()
    {
        if (isBuild)
            throw new InvalidOperationException("Entity has been already builded.");

        descriptorType = DescriptorMapper.GetDescriptorType(componentValues.Keys.ToArray());
        egid = new EGID(context.GenerateEntityID(), DescriptorMapper.GetExclusiveGroup(descriptorType));
        entityDescriptor = (IEntityDescriptor)typeof(DynamicEntityDescriptor<>)
            .MakeGenericType(descriptorType)
            .GetConstructor(new Type[] { typeof(IComponentBuilder[]) })
            .Invoke(new object[] { componentBuilders.ToArray() });
        componentBuilders = null;
        context.EntityFactory.BuildEntity(egid, entityDescriptor);
    }

    internal void SetComponents()
    {
        if (isBuild)
            throw new InvalidOperationException("Entity has been already builded.");

        foreach (var (type, component) in componentValues)
        {
            typeof(QueryEngine)
                .GetMethod("SetComponent")
                .MakeGenericMethod(type)
                .Invoke(context.QueryEngine, new object[] { egid, component });
        }
        
        isBuild = true;
    }
}
