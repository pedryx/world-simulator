using Svelto.ECS;

namespace WorldSimulator.ECS.SveltoECS;

internal struct ComponentWrapper<TComponent> : IEntityComponent
    where TComponent : struct
{
    public TComponent Component;

    public ComponentWrapper(TComponent component)
    {
        Component = component;
    }
}
