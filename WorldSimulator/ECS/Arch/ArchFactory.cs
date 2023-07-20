using Arch.Core;
using Arch.Core.Extensions;

using WorldSimulator.ECS.AbstractECS;

namespace WorldSimulator.ECS.Arch;
/// <summary>
/// https://github.com/genaray/Arch
/// </summary>
public class ArchFactory : IECSFactory
{
    public void Initialize() { }

    public IEntityBuilder CreateEntityBuilder()
        => new OnPlaceBuildEntityBuilder((types, values, world) =>
        {
            Entity entity = ((BasicECSWorld<World>)world).World.Create<Empty>();
            entity.AddRange(values.ToArray());
            entity.Remove<Empty>();

            return new ArchEntity(entity, ((BasicECSWorld<World>)world).World);
        });

    public IECSWorldBuilder CreateWorldBuilder()
        => new ArchWorldBuilder();

    private struct Empty { }
}
