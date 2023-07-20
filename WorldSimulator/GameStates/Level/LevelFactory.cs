namespace WorldSimulator.GameStates.Level;
internal class LevelFactory
{
    private readonly Game game;
    private readonly LevelState gameState;

    public LevelFactory(Game game, LevelState gameState)
    {
        this.game = game;
        this.gameState = gameState;
    }
}
