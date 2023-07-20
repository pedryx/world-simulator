using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using WorldSimulator.ECS.AbstractECS;

namespace WorldSimulator;
public class GameState
{
    public IECSWorld ECSWorld { get; private set; }
}
