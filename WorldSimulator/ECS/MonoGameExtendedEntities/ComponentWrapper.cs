namespace WorldSimulator.ECS.MonoGameExtendedEntities;
internal class ComponentWrapper<TComponent>
    where TComponent : struct
{
    public TComponent Component;

    public ComponentWrapper(TComponent component)
    {
        Component = component;
    }
}
