using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using WorldSimulator.ECS.AbstractECS;

namespace WorldSimulator.ECS
{
    public class Game
    {
        public IECSFactory ECSFactory { get; private set; }
    }
}
