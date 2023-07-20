namespace WorldSimulator.ECS.AbstractECS;
/// <summary>
/// Represent interface for wrapper of ECS world which can be used for managing entities.
/// </summary>
public interface IECSWorld
{
    /// <summary>
    /// Update world's logic.
    /// </summary>
    void Update();
}