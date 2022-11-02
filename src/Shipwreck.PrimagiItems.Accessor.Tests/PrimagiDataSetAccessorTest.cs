using System.Runtime.CompilerServices;

namespace Shipwreck.PrimagiItems;

public class PrimagiDataSetAccessorTest
{
    [Fact]
    public void GetItemFileNameTest()
    {
        var dir = new DirectoryInfo(GetRepositoryDirectory());
        var repPath = Path.Combine(dir.FullName, "primagi-items");
        string? jsonPath = null;

        if (dir.Exists)
        {
            PrimagiDataSetAccessor.DeleteDirectoryRecursive(dir.FullName);
        }

        void assert()
        {
            using (var ac = new PrimagiDataSetAccessor(dir.FullName))
            {
                jsonPath = ac.GetItemFileName();
            }

            Assert.True(File.Exists(jsonPath));
        }

        // 新規作成
        assert();

        File.Delete(jsonPath!);

        // リセット
        assert();

        PrimagiDataSetAccessor.DeleteDirectoryRecursive(dir.FullName);
    }

    private static string GetRepositoryDirectory([CallerMemberName] string testName = "", [CallerFilePath] string filePath = "")
    => Path.Combine(Path.GetTempPath(), typeof(PrimagiDataSetAccessorTest).Assembly.GetName().Name!, testName);
}