using System.Runtime.CompilerServices;

using WorldSimulator.Components;
using WorldSimulator.ECS.AbstractECS;
using WorldSimulator.ManagedDataManagers;

namespace WorldSimulator.Systems;
internal readonly struct DamageSystem : IEntityProcessor<DamageDealer, Owner>
{
    private readonly ManagedDataManager<IEntity> entityManager;

    public DamageSystem(Game game)
    {
        entityManager = game.GetManagedDataManager<IEntity>();
    }

    [MethodImpl(Game.EntityProcessorInline)]
    public void Process(ref DamageDealer damageDealer, ref Owner owner, float deltaTime)
    {
        if (damageDealer.TargetID != -1 && !entityManager[damageDealer.TargetID].IsDestroyed())
        {
            IEntity targetEntity = entityManager[damageDealer.TargetID];
            ref Health targetHealth = ref targetEntity.GetComponent<Health>();

            targetHealth.Amount -= damageDealer.DamagePerSecond * deltaTime;
            targetHealth.DamageSourceID = entityManager.Insert(entityManager[owner.EntityID]);
        }
    }
}
