using WorldSimulator.Components;
using WorldSimulator.ECS.AbstractECS;

namespace WorldSimulator.Systems;
internal readonly struct DamageSystem : IEntityProcessor<DamageDealer, Owner>
{
    public void Process(ref DamageDealer damageDealer, ref Owner owner, float deltaTime)
    {
        if (damageDealer.Target != null && !damageDealer.Target.IsDestroyed())
        {
            ref Health targetHealth = ref damageDealer.Target.GetComponent<Health>();

            targetHealth.Amount -= damageDealer.DamagePerSecond * deltaTime;
            targetHealth.DamageSource = owner.Entity;
        }
    }
}
