namespace WorldSimulator.ECS.RelEcs;
internal class ComponentWrapper<TComponent>
    where TComponent : struct
{
    public TComponent Component;

    public ComponentWrapper(TComponent component)
    {
        Component = component;
    }
}
