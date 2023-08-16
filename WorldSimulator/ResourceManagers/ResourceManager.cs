using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace WorldSimulator.ResourceManagers;
internal interface IResourceManager
{
    public void LoadAll();
}

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
        this.fileExtension = fileExtension;
        contentFolder = Path.Combine(ContentRootFolder, contentSubFolder);
    }

    /// <summary>
    /// Load all resources in content folder.
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
    /// Get name of resource based on its file.
    /// </summary>
    public virtual string GetName(string file)
        => file.Split('/', '\\').Last().Split('.').First();

    /// <summary>
    /// Load resource from file.
    /// </summary>
    public abstract TResource Load(string file, string name);

    public TResource this[string name] => resources[name];
}
