using WorldSimulator.ECS.AbstractECS;

namespace WorldSimulator.Components;
/// <summary>
/// Tag component, entities which this component will have their <see cref="Sprite.LayerDepth"/> updated based on
/// their <see cref="Transform.Position"/>.
/// </summary>
[Component]
internal struct LayerUpdate { }