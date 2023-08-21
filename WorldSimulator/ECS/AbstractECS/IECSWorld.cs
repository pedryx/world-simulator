namespace WorldSimulator.ECS.AbstractECS;
/// <summary>
/// An interface for wrappers around an ECS world.
/// </summary>
public interface IECSWorld
{
    /// <summary>
    /// Update the state of the ECS world.
    /// </summary>
    void Update();
}