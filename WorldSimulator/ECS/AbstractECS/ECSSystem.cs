namespace WorldSimulator.ECS.AbstractECS
{
    public abstract class ECSSystem
    {
        protected Game Game { get; private set; }

        public void Initialize(Game game)
        {
            Game = game;
            Initialize();
        }

        protected abstract void Initialize();
        protected virtual void PreUpdate(float deltaTime) { }
        protected virtual void PostUpdate(float deltaTime) { }
        protected abstract void ProcessEntities(float deltaTime);

        public void Update(float deltaTime)
        {
            PreUpdate(deltaTime);
            ProcessEntities(deltaTime);
            PostUpdate(deltaTime);
        }
    }

    public abstract class ECSSystem<TComponent> : ECSSystem
    {
        private EntityIterator<TComponent> iterator;

        protected override void Initialize()
        {
            iterator = Game.ECSFactory.CreateIterator<TComponent>();
        }

        protected override void ProcessEntities(float deltaTime)
        {
            iterator.Reset();
            while (!iterator.MoveNext())
            {
                ref var current = ref iterator.GetCurrent();
                Process(deltaTime, ref current.Component);
            }
        }

        protected abstract void Process(float deltaTime, ref TComponent component);
    }

    public abstract class ECSSystem<TComponent1, TComponent2> : ECSSystem
    {
        private EntityIterator<TComponent1, TComponent2> iterator;

        protected override void Initialize()
        {
            iterator = Game.ECSFactory.CreateIterator<TComponent1, TComponent2>();
        }

        protected override void ProcessEntities(float deltaTime)
        {
            iterator.Reset();
            while (!iterator.MoveNext())
            {
                ref var current = ref iterator.GetCurrent();
                Process(deltaTime, ref current.Component1, ref current.Component2);
            }
        }

        protected abstract void Process(
            float deltaTime,
            ref TComponent1 component1,
            ref TComponent2 component2);
    }

}
