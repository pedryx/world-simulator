namespace WorldSimulator.ManagedDataManagers;

internal class ItemCollectionManager : ManagedDataManager<ItemCollection?>
{
    public ItemCollectionManager() : base(true) { }

    protected override ItemCollection? CreateDataInstance()
        => new ItemCollection();

    protected override ItemCollection? CreateEmpty()
        => null;
}
