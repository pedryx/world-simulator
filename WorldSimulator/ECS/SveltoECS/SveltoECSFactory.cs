using WorldSimulator.ECS.AbstractECS;

namespace WorldSimulator.ECS.SveltoECS;
/// <summary>
/// https://github.com/sebas77/Svelto.ECS
/// </summary>
public class SveltoECSFactory : ECSFactory
{
    private readonly Context context = new();

    public SveltoECSFactory() 
        : base
        (
            typeof(SveltoECSSystem<,>),
            typeof(SveltoECSSystem<,,>),
            typeof(SveltoECSSystem<,,,>),
            typeof(SveltoECSSystem<,,,,>)
        )
    {
        DescriptorMapper.Init();
    }

    public override IEntity CreateEntity(IECSWorld world)
        => new SveltoECSEntity(context, (SveltoECSWorld)world);

    public override IECSWorld CreateWorld()
        => new SveltoECSWorld(context);
}
