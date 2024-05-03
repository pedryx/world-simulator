using MonoGame.Extended.Entities;
using MonoGame.Extended.Entities.Systems;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;

using WorldSimulator.ECS.AbstractECS;

namespace WorldSimulator.ECS.MonoGameExtendedEntities;
internal class MonoGameExtendedWorld : IECSWorld
{
    private WorldBuilder builder = new();
    private Dictionary<MonoGameExtendedEntity, List<object>> scheduledComponents = new();

    public bool IsBuilded { get; private set; } = false;
    public World World { get; private set; }

    public void AddSystem(ISystem system)
    {
        Debug.Assert(!IsBuilded);

        builder.AddSystem(system);
    }

    public void ScheduleComponentAdd<TComponent>(MonoGameExtendedEntity entity, TComponent component)
        where TComponent : struct
    {
        if (!scheduledComponents.ContainsKey(entity))
            scheduledComponents.Add(entity, new List<object>());

        scheduledComponents[entity].Add(component);
    }

    public void ScheduleDestroy(MonoGameExtendedEntity entity)
    {
        scheduledComponents.Remove(entity);
    }

    public ComponentWrapper<TComponent> GetScheduledComponent<TComponent>(MonoGameExtendedEntity entity)
        where TComponent : struct
    {
        if (!scheduledComponents.TryGetValue(entity, out List<object> components))
            return null;

        var query = components.Where(component => component.GetType() == typeof(TComponent));

        if (!query.Any())
            return null;

        object component = query.First();
        Type wrapperType = typeof(ComponentWrapper<>).MakeGenericType(typeof(TComponent));

        return (ComponentWrapper<TComponent>)Activator.CreateInstance(wrapperType, component);
    }

    public void ScheduleComponentRemove<TComponent>(MonoGameExtendedEntity entity)
        where TComponent : struct
    {
        if (!scheduledComponents.TryGetValue(entity, out List<object> components))
            return;
        components.RemoveAll(component => component.GetType() == typeof(TComponent));
    }

    public void Update()
    {
        if (IsBuilded)
        {
            World.Update(null);
            return;
        }

        World = builder.Build();

        MethodInfo addComponentMethod = typeof(MonoGameExtendedEntity)
            .GetMethod("AddComponent", BindingFlags.Instance | BindingFlags.Public);

        foreach (var pair in scheduledComponents)
        {
            pair.Key.Build(World.CreateEntity());

            foreach (var component in scheduledComponents[pair.Key])
            {
                addComponentMethod.MakeGenericMethod(component.GetType()).Invoke(pair.Key, new object[] { component });
            }
        }

        builder = null;
        scheduledComponents = null;
        IsBuilded = true;
    }
}
