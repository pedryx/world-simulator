using WorldSimulator.ECS.AbstractECS;

namespace WorldSimulator.ManagedDataManagers
{
    internal class EntityArrayManager : ManagedDataManager<IEntity[]>
    {
        public EntityArrayManager() : base(true) { }

        protected override IEntity[] CreateDataInstance()
            => null;

        protected override IEntity[] CreateEmpty()
            => null;
    }
}
