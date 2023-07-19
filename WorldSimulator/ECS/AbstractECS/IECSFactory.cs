namespace WorldSimulator.ECS.AbstractECS
{
    public interface IECSFactory
    {
        public EntityIterator<TComponent> CreateIterator<TComponent>();
        public EntityIterator<TComponent1, TComponent2> CreateIterator
            <TComponent1, TComponent2>();
    }
}
