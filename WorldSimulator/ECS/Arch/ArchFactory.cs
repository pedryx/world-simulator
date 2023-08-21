using Arch.Core;
using Arch.Core.Extensions;

using WorldSimulator.ECS.AbstractECS;

namespace WorldSimulator.ECS.Arch;
/// <summary>
/// https://github.com/genaray/Arch
/// </summary>
public class ArchFactory : ECSFactory
{
    public ArchFactory() 
        : base(typeof(ArchSystem<,>), typeof(ArchSystem<,,>), typeof(ArchSystem<,,,>), typeof(ArchSystem<,,,,>)) { }


    public override IEntityBuilder CreateEntityBuilder(IECSWorld world)
    {
        return new OnPlaceBuildEntityBuilder(world, (types, values, world) =>
        {
            // World.Create throws exception when creating entity without component.
            Entity entity = ((BasicECSWorld<World>)world).World.Create<Empty>();
            entity.AddRange(values.ToArray());
            entity.Remove<Empty>();

            return new ArchEntity(entity, ((BasicECSWorld<World>)world).World);
        });
    }

    public override IECSWorld CreateWorld()
       => new BasicECSWorld<World>(World.Create());

    private struct Empty { }
}
