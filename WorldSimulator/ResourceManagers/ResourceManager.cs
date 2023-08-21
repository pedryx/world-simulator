using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace WorldSimulator.ResourceManagers;
/// <summary>
/// A base interface for resource managers. A resource manager is responsible for storing content resources and loading
/// them from files.
/// </summary>
internal interface IResourceManager
{
    /// <summary>
    /// Load all resources managed by the resource manager.
    /// </summary>
    public void LoadAll();
}

/// <summary>
/// Base class for a resource manager responsible for managing content resources of a specified type.
/// </summary>
/// <typeparam name="TResource">The type of content resource which will be managed by the resource manager.</typeparam>
internal abstract class ResourceManager<TResource> : IResourceManager
{
    private const string ContentRootFolder = "Content";

    /// <summary>
    /// Contains loaded resources.
    /// </summary>
    private readonly IDictionary<string, TResource> resources = new Dictionary<string, TResource>();
    private readonly string fileExtension;
    private readonly string contentFolder;

    public ResourceManager(string fileExtension, string contentSubFolder)
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
            TResource value = Load(file, name);

            resources.Add(name, value);
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
    public abstract TResource Load(string file, string name);

    public TResource this[string name] => resources[name];
}
