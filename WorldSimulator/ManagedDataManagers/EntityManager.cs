using WorldSimulator.ECS.AbstractECS;

namespace WorldSimulator.ManagedDataManagers;

internal class EntityManager : ManagedDataManager<IEntity>
{
    public EntityManager() 
        : base(true) { }

    protected override IEntity CreateDataInstance()
        => null;

    protected override IEntity CreateEmpty()
        => null;
}
