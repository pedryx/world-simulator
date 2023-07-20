using WorldSimulator;
using WorldSimulator.ECS.AbstractECS;
using WorldSimulator.ECS.Arch;
using WorldSimulator.ECS.DefaultEcs;
using WorldSimulator.ECS.Entitas;
using WorldSimulator.ECS.HypEcs;
using WorldSimulator.GameStates.Level;

const int seed = 0;
IECSFactory factory = new ArchFactory();

Game game = new(factory, seed);
game.SwitchState(new LevelState());
game.Run();