using WorldSimulator;
using WorldSimulator.ECS.AbstractECS;
using WorldSimulator.ECS.Arch;
using WorldSimulator.ECS.DefaultEcs;
using WorldSimulator.ECS.Entitas;
using WorldSimulator.ECS.HypEcs;
using WorldSimulator.ECS.LeoECS;
using WorldSimulator.ECS.LeoEcsLite;
using WorldSimulator.ECS.MonoGameExtendedEntities;
using WorldSimulator.ECS.RelEcs;
using WorldSimulator.Level;

const int seed = 0;
ECSFactory factory = new ArchFactory();

Game game = new(factory, seed);
game.SwitchState(new LevelState(true));
game.Run();