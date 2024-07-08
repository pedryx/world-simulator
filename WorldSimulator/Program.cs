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
using WorldSimulator.ECS.SveltoECS;
using WorldSimulator.Level;

SeedGenerator.SetGlobalSeed(0);
ECSFactory factory = new SveltoECSFactory();

Game game = new(factory);
game.SwitchState(new LevelState(true));
game.Run();