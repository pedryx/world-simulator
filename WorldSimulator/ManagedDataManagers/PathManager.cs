using Microsoft.Xna.Framework;

using System;

namespace WorldSimulator.ManagedDataManagers;

internal class PathManager : ManagedDataManager<Vector2[]>
{
    public PathManager() : base(true) { }

    protected override Vector2[] CreateDataInstance()
        => Array.Empty<Vector2>();
    
    protected override Vector2[] CreateEmpty() 
        => null;
}
