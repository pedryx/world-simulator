using System.Runtime.CompilerServices;

using WorldSimulator.Components;
using WorldSimulator.Components.Villages;
using WorldSimulator.ECS.AbstractECS;

namespace WorldSimulator.Systems.Villaages;
internal readonly struct ResourceProcessingSystem : IEntityProcessor<Inventory, ResourceProcessor>
{
    [MethodImpl(Game.EntityProcessorInline)]
    public void Process(ref Inventory inventory, ref ResourceProcessor processor, float deltaTime)
    {
        if (processor.Processing)
        {
            processor.Elapsed += deltaTime;
            if (processor.Elapsed >= processor.ProcessingTime)
            {
                processor.Elapsed = 0.0f;
                processor.Processing = false;
                inventory.Items.Add(processor.OutputItem, processor.OutputQuantity);
            }
        }
        else if (inventory.Items.GetQuantity(processor.InputItem) >= processor.InputQuantity)
        {
            processor.Processing = true;
            inventory.Items.Remove(processor.InputItem, processor.InputQuantity);
        }
    }
}
