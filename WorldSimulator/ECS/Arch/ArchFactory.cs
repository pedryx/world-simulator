using Arch.Core;
using Arch.Core.Extensions;

using WorldSimulator.ECS.AbstractECS;

namespace WorldSimulator.ECS.Arch;
public class ArchFactory : IECSFactory
{
    public void Initialize() { }

    public IEntityBuilder CreateEntityBuilder(IECSWorld world)
        => new OnPlaceBuildEntityBuilder((types, values, world) =>
        {
            Entity entity = ((BasicECSWorld<World>)world).World.Create(types);
            entity.SetRange(values.ToArray());

            return new ArchEntity(entity, ((BasicECSWorld<World>)world).World);
        });

    public IECSWorldBuilder CreateWorldBuilder()
        => new BasicECSWorldBuilder<World>(World.Create());
}
