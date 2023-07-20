
using DefaultEcs;

using WorldSimulator.ECS.AbstractECS;

namespace WorldSimulator.ECS.DefaultEcs;
/// <summary>
/// https://github.com/Doraku/DefaultEcs
/// </summary>
internal class DefaultEcsFactory : IECSFactory
{
    public void Initialize() { }

    public IEntityBuilder CreateEntityBuilder(IECSWorld world)
    {
        DefaultEcsEntity prototype = new(((BasicECSWorld<World>)world)
            .World.CreateEntity());
        return new CloneEntityBuilder(prototype, (world, prototype) =>
        {
            return new DefaultEcsEntity
            (
                ((DefaultEcsEntity)prototype)
                    .Entity.CopyTo(((BasicECSWorld<World>)world).World)
            );
        });
    }

    public IECSWorldBuilder CreateWorldBuilder()
        => new BasicECSWorldBuilder<World>(new World());
}
