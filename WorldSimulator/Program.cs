using WorldSimulator;
using WorldSimulator.ECS.AbstractECS;
using WorldSimulator.ECS.DefaultEcs;
using WorldSimulator.GameStates.Level;

const int seed = 0;
IECSFactory factory = new DefaultEcsFactory();

Game game = new(factory, seed);
game.SwitchState(new LevelState());
game.Run();