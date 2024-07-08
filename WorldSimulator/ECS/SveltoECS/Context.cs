using Svelto.ECS;
using Svelto.ECS.Schedulers;

namespace WorldSimulator.ECS.SveltoECS;

internal class Context
{
    private uint lastID = 0;

    public EntitiesSubmissionScheduler Scheduler { get; } = new();
    public EnginesRoot Root { get; }
    public IEntityFactory EntityFactory { get; }
    public IEntityFunctions EntityFunctions { get; }
    public QueryEngine QueryEngine { get; } = new();

    public Context()
    {
        Root = new EnginesRoot(Scheduler);
        EntityFactory = Root.GenerateEntityFactory();
        EntityFunctions = Root.GenerateEntityFunctions();

        AddEngine(QueryEngine);
    }

    public void AddEngine(IEngine engine)
        => Root.AddEngine(engine);

    public uint GenerateEntityID()
        => lastID++;
}
