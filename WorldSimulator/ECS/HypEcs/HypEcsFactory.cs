using HypEcs;

using WorldSimulator.ECS.AbstractECS;

namespace WorldSimulator.ECS.HypEcs;
public class HypEcsFactory : IECSFactory
{
    public void Initialize() { }

    public IEntityBuilder CreateEntityBuilder(IECSWorld world)
    {
        return new HypEcsEntityBuilder(((BasicECSWorld<World>)world).World);
    }

    public IECSWorldBuilder CreateWorldBuilder()
    {
        return new HypEcsWorldBuilder();
    }
}
