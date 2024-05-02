using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using WorldSimulator.ECS.AbstractECS;

namespace WorldSimulator.ECS.FrifloEngineEcs;
internal class FrifloEngineEcsSystem<TEntityProcessor, TComponent> : IECSSystem
    where TEntityProcessor : struct, IEntityProcessor<TComponent>
    where TComponent : struct
{
    public void Initialize(IECSWorld world)
    {
        throw new NotImplementedException();
    }

    public void Update(float deltaTime)
    {
        throw new NotImplementedException();
    }
}
