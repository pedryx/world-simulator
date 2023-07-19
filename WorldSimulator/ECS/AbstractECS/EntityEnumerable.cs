namespace WorldSimulator.ECS.AbstractECS
{
    public abstract class EntityIterator<TComponent>
    {
        public virtual void Reset() { }
        public abstract ref ComponentTuple<TComponent> GetCurrent();
        public abstract bool MoveNext();
    }

    public abstract class EntityIterator<TComponent1, TComponent2>
    {
        public virtual void Reset() { }
        public abstract ref ComponentTuple<TComponent1, TComponent2> GetCurrent();
        public abstract bool MoveNext();
    }
}
