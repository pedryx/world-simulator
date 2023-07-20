using Entitas;

namespace WorldSimulator.ECS.Entitas;
internal class ComponentWrapper<TComponent> : IComponent
{
    private static readonly int id = ComponentIDGenerator.NextID();

    public static int ID => id;

    public ComponentWrapper() { }

    public ComponentWrapper(TComponent component)
    {
        Component = component;
    }

    public TComponent Component;
}
