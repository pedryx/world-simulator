using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace WorldSimulator.AssetManagers;
/// <summary>
/// A base interface for asset managers. An asset manager is responsible for storing content resources and loading
/// them from files.
/// </summary>
internal interface IAssetManager
{
    /// <summary>
    /// Load all resources managed by the resource manager.
    /// </summary>
    public void LoadAll();
}

/// <summary>
/// Base class for an asset manager responsible for managing asset of a specified type.
/// </summary>
/// <typeparam name="TAsset">The type of content asset which will be managed by the asset manager.</typeparam>
internal abstract class AssetManager<TAsset> : IAssetManager
{
    private const string ContentRootFolder = "Content";

    /// <summary>
    /// Contains loaded resources.
    /// </summary>
    private readonly IDictionary<string, TAsset> assets = new Dictionary<string, TAsset>();
    private readonly string fileExtension;
    private readonly string contentFolder;

    public AssetManager(string fileExtension, string contentSubFolder)
    {
        Debug.Assert(!string.IsNullOrEmpty(fileExtension));
        Debug.Assert(!string.IsNullOrEmpty(contentSubFolder));

        this.fileExtension = fileExtension;
        contentFolder = Path.Combine(ContentRootFolder, contentSubFolder);
    }

    /// <summary>
    /// Load all resources in the content folder.
    /// </summary>
    public void LoadAll()
    {
        if (!Directory.Exists(contentFolder))
            return;

        string[] files = Directory.GetFiles
        (
            contentFolder,
            $"*.{fileExtension}",
            SearchOption.AllDirectories
        );

        foreach (var file in files)
        {
            string name = GetName(file);
            TAsset value = Load(file, name);

            assets.Add(name, value);
        }
    }

    /// <summary>
    /// Get the name of the resource based on its file name.
    /// </summary>
    public virtual string GetName(string file)
        => file.Split('/', '\\').Last().Split('.').First();

    /// <summary>
    /// Load a resource from a file.
    /// </summary>
    public abstract TAsset Load(string file, string name);

    public TAsset this[string name] => assets[name];
}
