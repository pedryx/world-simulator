namespace WorldSimulator.ECS.AbstractECS;
/// <summary>
/// Factory for creation of ECS builder classes.
/// </summary>
public interface IECSFactory
{
    void Initialize();
    IEntityBuilder CreateEntityBuilder(IECSWorld world);
    IECSWorldBuilder CreateWorldBuilder();
}
