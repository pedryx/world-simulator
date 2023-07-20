using HypEcs;

using WorldSimulator.ECS.AbstractECS;

namespace WorldSimulator.ECS.HypEcs;
internal class HypEcsEntityBuilder : IEntityBuilder
{
    private readonly EntityBuilder builder;
    private readonly World world;

    public HypEcsEntityBuilder(World world)
    {
        this.world = world;
        builder = world.Spawn();
    }

    public void AddComponent<TComponent>(TComponent component) 
        where TComponent : struct
    {
        builder.Add(component);
    }

    public IEntity Build()
    {
        return new HypEcsEntity(builder.Id(), world);
    }
}
