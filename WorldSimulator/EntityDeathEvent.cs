using System;

using WorldSimulator.ECS.AbstractECS;

namespace WorldSimulator;
/// <summary>
/// Handler for an event associated with the death of an entity.
/// </summary>
/// <param name="sender">Event sender.</param>
/// <param name="e">Event arguments.</param>
internal delegate void EntityDeathEventHandler(object sender, EntityDeathEventArgs e);

/// <summary>
/// Arguments for events associated with the death of an entity.
/// </summary>
internal class EntityDeathEventArgs : EventArgs
{
    /// <summary>
    /// The entity which has died.
    /// </summary>
    public IEntity Entity { get; private init; }
    /// <summary>
    /// The last entity which dealt damage to the killed entity.
    /// </summary>
    public IEntity DamageSource { get; private init; }

    /// <param name="entity">The entity which has died.</param>
    /// <param name="damageSource">The last entity which dealt damage to the killed entity.</param>
    public EntityDeathEventArgs(IEntity entity, IEntity damageSource)
    {
        Entity = entity;
        DamageSource = damageSource;
    }
}
