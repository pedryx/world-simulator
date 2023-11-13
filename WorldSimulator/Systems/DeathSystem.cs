using WorldSimulator.Components;
using WorldSimulator.ECS.AbstractECS;

namespace WorldSimulator.Systems;
/// <summary>
/// Handles deaths of entities.
/// </summary>
internal readonly struct DeathSystem : IEntityProcessor<Health, Owner>
{
    public void Process(ref Health health, ref Owner owner, float deltaTime)
    {
        if (health.Amount < 0.0f)
        {
            if (health.DamageSource != null &&
                !health.DamageSource.IsDestroyed() &&
                health.DamageSource.HasComponent<Inventory>())
            {
                ref Inventory attackerInventory = ref health.DamageSource.GetComponent<Inventory>();

                if (owner.Entity.HasComponent<ItemDrop>())
                {
                    ref ItemDrop itemDrop = ref owner.Entity.GetComponent<ItemDrop>();
                    itemDrop.Items.TransferTo(ref attackerInventory.Items);
                }

                if (owner.Entity.HasComponent<Inventory>())
                {
                    ref Inventory inventory = ref owner.Entity.GetComponent<Inventory>();
                    inventory.Items.TransferTo(ref attackerInventory.Items);
                }
            }

            owner.Entity.Destroy();
        }
    }
}
