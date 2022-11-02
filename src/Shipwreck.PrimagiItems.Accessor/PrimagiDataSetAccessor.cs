using LibGit2Sharp;

namespace Shipwreck.PrimagiItems;

public sealed class PrimagiDataSetAccessor : IDisposable
{
    private const string URL = "https://github.com/pgrho/primagi-items.git";
    private readonly DirectoryInfo _Directory;

    public PrimagiDataSetAccessor(string directoryPath)
    {
        _Directory = new DirectoryInfo(Path.Combine(directoryPath, "primagi-items"));
    }

    public Task<string> GetItemFileNameAsync(CancellationToken cancellationToken = default)
        => Task.Run(GetItemFileName, cancellationToken);

    public string GetItemFileName()
    {
        if (_Directory.Exists)
        {
            try
            {
                using (var repo = new Repository(_Directory.FullName))
                {
                    if (repo.Network.Remotes.ToList() is var remotes
                        && remotes.Count == 1
                        && remotes[0] is var origin
                        && origin.Url == URL
                        && repo.Branches.FirstOrDefault(e => !e.IsRemote && e.FriendlyName == "master") is Branch master)
                    {
                        repo.Reset(ResetMode.Hard, repo.Head.Tip);
                        master = Commands.Checkout(repo, master);

                        Commands.Pull(repo, new Signature("p", "u", DateTimeOffset.Now), new PullOptions());

                        return Path.Combine(_Directory.FullName, "output", "items.json");
                    }
                }
            }
            catch { }
            DeleteDirectoryRecursive(_Directory.FullName);
        }

        var pd = _Directory.Parent;
        if (!pd.Exists)
        {
            pd.Create();
        }

        Repository.Clone(URL, _Directory.FullName);
        _Directory.Refresh();

        return Path.Combine(_Directory.FullName, "output", "items.json");
    }

    void IDisposable.Dispose()
    {
    }

    internal static void DeleteDirectoryRecursive(string directoryPath)
    {
        var files = Directory.GetFiles(directoryPath, "*", SearchOption.AllDirectories);

        foreach (string file in files)
        {
            File.SetAttributes(file, FileAttributes.Normal);
            File.Delete(file);
        }

        Directory.Delete(directoryPath, true);
    }
}