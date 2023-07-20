using DefaultEcs;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using WorldSimulator.ECS.AbstractECS;

namespace WorldSimulator.ECS.DefaultEcs;
internal class DefaultEcsEntity : IEntity
{
    public Entity Entity { get; private set; }

    public DefaultEcsEntity(Entity entity)
    {
        Entity = entity;
    }

    public void AddComponent<TComponent>(TComponent component)
    {
        throw new NotImplementedException();
    }

    public void Destroy()
    {
        throw new NotImplementedException();
    }

    public ref TComponent GetComponent<TComponent>()
    {
        throw new NotImplementedException();
    }

    public bool HasComponent<TComponent>()
    {
        throw new NotImplementedException();
    }

    public void RemoveComponent<TComponent>()
    {
        throw new NotImplementedException();
    }
}
