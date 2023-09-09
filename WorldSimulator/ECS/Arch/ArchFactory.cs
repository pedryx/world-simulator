using Arch.Core;

using WorldSimulator.ECS.AbstractECS;

namespace WorldSimulator.ECS.Arch;
/// <summary>
/// https://github.com/genaray/Arch
/// </summary>
public class ArchFactory : ECSFactory
{
    public ArchFactory() 
        : base(typeof(ArchSystem<,>), typeof(ArchSystem<,,>), typeof(ArchSystem<,,,>), typeof(ArchSystem<,,,,>)) { }

    public override IEntity CreateEntity(IECSWorld worldWrapper)
    {
        World world = ((BasicECSWorld<World>)worldWrapper).World;

        // World.Create throws exception when creating entity without component.
        IEntity entity = new ArchEntity(world.Create<Empty>(), world);
        entity.RemoveComponent<Empty>();

        return entity;
    }

    public override IECSWorld CreateWorld()
       => new BasicECSWorld<World>(World.Create());

    private struct Empty { }
}
