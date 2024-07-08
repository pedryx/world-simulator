using System.Runtime.CompilerServices;

using WorldSimulator.Components;
using WorldSimulator.Components.Villages;
using WorldSimulator.ECS.AbstractECS;
using WorldSimulator.ManagedDataManagers;

namespace WorldSimulator.Systems.Villaages;
internal readonly struct ResourceProcessingSystem : IEntityProcessor<Inventory, ResourceProcessor>
{
    private readonly ManagedDataManager<ItemCollection?> itemCollectionManager;

    public ResourceProcessingSystem(Game game)
    {
        itemCollectionManager = game.GetManagedDataManager<ItemCollection?>();
    }

    [MethodImpl(Game.EntityProcessorInline)]
    public void Process(ref Inventory inventory, ref ResourceProcessor processor, float deltaTime)
    {
        ItemCollection inventoryItems = itemCollectionManager[inventory.ItemCollectionID].Value;

        ItemType inputItem = ItemType.Get(processor.InputItemID);
        ItemType outputItem = ItemType.Get(processor.OutputItemID);

        if (processor.Processing)
        {
            processor.Elapsed += deltaTime;
            if (processor.Elapsed >= processor.ProcessingTime)
            {
                processor.Elapsed = 0.0f;
                processor.Processing = false;
                inventoryItems.Add(outputItem, processor.OutputQuantity);
            }
        }
        else if (inventoryItems.GetQuantity(inputItem) >= processor.InputQuantity)
        {
            processor.Processing = true;
            inventoryItems.Remove(inputItem, processor.InputQuantity);
        }

        itemCollectionManager[inventory.ItemCollectionID] = inventoryItems;
    }
}
