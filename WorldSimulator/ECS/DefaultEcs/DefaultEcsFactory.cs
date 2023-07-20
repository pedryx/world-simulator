
using DefaultEcs;

using WorldSimulator.ECS.AbstractECS;

namespace WorldSimulator.ECS.DefaultEcs;
/// <summary>
/// https://github.com/Doraku/DefaultEcs
/// </summary>
internal class DefaultEcsFactory : IECSFactory
{
    private readonly World prototypesWorld = new();

    public void Initialize() { }

    public IEntityBuilder CreateEntityBuilder(IECSWorld world)
    {
        return new CloneEntityBuilder
        (
            world,
            new DefaultEcsEntity(prototypesWorld.CreateEntity()),
            (world, prototype) =>
            {
                return new DefaultEcsEntity
                (
                    ((DefaultEcsEntity)prototype)
                        .Entity.CopyTo(((BasicECSWorld<World>)world).World)
                );
            }
        );
    }

    public IECSWorldBuilder CreateWorldBuilder()
        => new DefaultECSWorldBuilder();
}
