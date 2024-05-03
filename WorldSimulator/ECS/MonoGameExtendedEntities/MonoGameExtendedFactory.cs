using MonoGame.Extended.Entities;

using WorldSimulator.ECS.AbstractECS;

namespace WorldSimulator.ECS.MonoGameExtendedEntities;
/// <summary>
/// https://github.com/craftworkgames/MonoGame.Extended
/// </summary>
internal class MonoGameExtendedFactory : ECSFactory
{
    public MonoGameExtendedFactory() 
        : base
        (
            typeof(MonoGameExtendedSystem<,>),
            typeof(MonoGameExtendedSystem<,,>),
            typeof(MonoGameExtendedSystem<,,,>),
            typeof(MonoGameExtendedSystem<,,,,>)
        ) { }

    public override IEntity CreateEntity(IECSWorld wrapper)
    {
        MonoGameExtendedWorld world = (MonoGameExtendedWorld)wrapper;
        Entity entity = null;

        if (world.IsBuilded)
            entity = world.World.CreateEntity();

        return new MonoGameExtendedEntity(entity, world);
    }

    public override IECSWorld CreateWorld()
        => new MonoGameExtendedWorld();
}
