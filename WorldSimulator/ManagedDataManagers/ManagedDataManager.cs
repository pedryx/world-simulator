using System.Collections.Generic;
using System.Linq;

namespace WorldSimulator.ManagedDataManagers;

internal interface IManagedDataManager { }

/// <summary>
/// Represent manager for instances of managed data types. Some ECS libs dont allow use of managed data types in
/// components, but in some situations (for example arrays) we need to use them. Used solution is to represent each
/// managed datatype instance that we want to use in components as simple integral id and have manager with all
/// these instances which is responsible for mapping their ids.
/// </summary>
internal abstract class ManagedDataManager<TManagedData> : IManagedDataManager
{
    /// <summary>
    /// Contains all instances of managed data.
    /// </summary>
    private readonly List<TManagedData> data = new();
    /// <summary>
    /// Contains indices of invalid (destroyed) instanes of managed data.
    /// </summary>
    private readonly List<int> freeList = new();
    /// <summary>
    /// Determine if data should be set to null after freeing them up.
    /// </summary>
    private readonly bool shouldNullSet;

    /// <param name="shouldNullSet">Determine if data should be set to null after freeing them up.</param>
    protected ManagedDataManager(bool shouldNullSet)
    {
        this.shouldNullSet = shouldNullSet;
    }

    /// <summary>
    /// Create initial instance of managed data.
    /// </summary>
    protected abstract TManagedData CreateDataInstance();

    /// <summary>
    /// Create empty invalid instance of managed data.
    /// </summary>
    /// <returns></returns>
    protected abstract TManagedData CreateEmpty();

    /// <summary>
    /// Create instance of managed data and map ID to it.
    /// </summary>
    /// <returns>ID of created instance of managed data.</returns>
    public int Create()
    {
        int id;

        if (freeList.Any())
        {
            id = freeList.Last();
            freeList.RemoveAt(freeList.Count - 1);
        }
        else
        {
            id = data.Count;
            data.Add(CreateEmpty());
        }

        data[id] = CreateDataInstance();
        return id;
    }

    /// <summary>
    /// Insert instance into manager. Manager will map ID to it.
    /// </summary>
    /// <returns>Mapped ID of inserted managed data.</returns>
    public int Insert(TManagedData data)
    {
        int id = Create();
        this[id] = data;

        return id;
    }

    /// <summary>
    /// Free instance of managed data with specified ID. After this operation the instance is considered destroyed
    /// and any acess can result into undefined behavior.
    /// </summary>
    /// <param name="id">ID of managed instance to delete.</param>
    public void Free(int id)
    {
        if (id == -1)
            return;

        if (shouldNullSet)
            data[id] = CreateEmpty();
        freeList.Add(id);
    }

    /// <summary>
    /// Get managed instance by its ID.
    /// </summary>
    /// <param name="id">ID of managed instance to get.</param>
    public TManagedData this[int id]
    {
        get => id == -1 ? CreateEmpty() : data[id];
        set => data[id] = value;
    }
}
