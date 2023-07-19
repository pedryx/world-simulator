namespace WorldSimulator.ECS.AbstractECS
{
    public ref struct ComponentTuple<TComponent>
    {
        public ref TComponent Component;

        public ComponentTuple(ref TComponent component)
        {
            Component = ref component;
        }
    }

    public ref struct ComponentTuple<TComponent1, TComponent2>
    {
        public ref TComponent1 Component1;
        public ref TComponent2 Component2;

        public ComponentTuple(ref TComponent1 component1, ref TComponent2 component2)
        {
            Component1 = ref component1;
            Component2 = ref component2;
        }
    }
}
