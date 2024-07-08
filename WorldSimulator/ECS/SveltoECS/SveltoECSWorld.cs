using Svelto.ECS;
using System.Collections.Generic;

using WorldSimulator.ECS.AbstractECS;

namespace WorldSimulator.ECS.SveltoECS;

internal class SveltoECSWorld : IECSWorld
{
    private readonly List<SveltoECSEntity> entitiesToBuild = new();

    public Context Context { get; }

    public SveltoECSWorld(Context context)
    {
        Context = context;
    }

    public void AddEntity(SveltoECSEntity entity)
        => entitiesToBuild.Add(entity);

    public void Update()
    {
        foreach (var entity in entitiesToBuild)
            entity.Build();

        Context.Scheduler.SubmitEntities();

        foreach (var entity in entitiesToBuild)
            entity.SetComponents();

        entitiesToBuild.Clear();
    }
}
