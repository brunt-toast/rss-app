using System.Xml.Linq;

namespace RssApp.Native.Windows.Tests;

[TestClass]
public sealed class InternationalisationTests
{
    private static string RepoRootPath => Path.GetFullPath(Path.Combine("..", "..", "..", "..", ".."));
    private static string StringsFolderPath => Path.GetFullPath(Path.Combine(RepoRootPath, "src", "RssApp.Native.Windows", "Strings"));

    [TestMethod]
    public void FolderForDefaultLanguageExists()
    {
        Assert.IsTrue(GetSupportedLanguages().Contains(GetDefaultLanguage()));
    }

    [TestMethod]
    public void AlternativeLanguages_HaveKeys_MatchingMainLanguage()
    {
        var defaultLangKeys = GetKeys(GetFolderForLanguage(GetDefaultLanguage())).ToList();
        bool success = true;

        foreach (string altLang in GetSupportedLanguages())
        {
            string altLangFolder = GetFolderForLanguage(altLang);
            IEnumerable<string> altLangKeys = GetKeys(altLangFolder);

            foreach (string missingKey in altLangKeys.Where(x => !defaultLangKeys.Contains(x)))
            {
                success = false;
                Console.WriteLine($"{altLang} does not contain a definition for {missingKey}");
            }
        }

        Assert.IsTrue(success);
    }

    [TestMethod]
    public void AlternativeLanguages_HaveNoKeys_NotInMainLanguage()
    {
        string defaultLang = GetDefaultLanguage();
        var defaultLangKeys = GetKeys(GetFolderForLanguage(defaultLang));
        bool success = true;

        foreach (string altLang in GetSupportedLanguages())
        {
            string altLangFolder = GetFolderForLanguage(altLang);
            IEnumerable<string> altLangKeys = GetKeys(altLangFolder);

            foreach (string additionalKey in altLangKeys.Where(x => !defaultLangKeys.Contains(x)))
            {
                success = false;
                Console.WriteLine($"{altLang} contains a definition for {additionalKey}, but {defaultLang} does not.");
            }
        }

        Assert.IsTrue(success);
    }

    private static List<string> GetKeys(string folderPath)
    {
        List<string> ret = [];

        IEnumerable<string> reswFiles = Directory.GetFiles(folderPath, "*.resw", SearchOption.AllDirectories);
        foreach (string filePath in reswFiles)
        {
            var doc = XDocument.Load(filePath);
            var dataTags = doc.Descendants("data");
            foreach (var dataTag in dataTags)
            {
                XAttribute? nameAttr = dataTag.Attributes().FirstOrDefault(x => x.Name == "name");
                if (nameAttr is null)
                {
                    Console.WriteLine($"{filePath}: Located a data tag with no name attribute");
                    continue;
                }

                ret.Add(nameAttr.Value);
            }
        }

        return ret;
    }

    private static string GetDefaultLanguage()
    {
        string csProjPath = Path.GetFullPath(Path.Combine(RepoRootPath, "src", "RssApp.Native.Windows", "RssApp.Native.Windows.csproj"));
        var doc = XDocument.Load(csProjPath);
        var defaultLanguageElement = doc.Descendants("DefaultLanguage").FirstOrDefault();
        ArgumentNullException.ThrowIfNull(defaultLanguageElement);
        return defaultLanguageElement.Value;
    }

    private static string GetFolderForLanguage(string language)
    {
        return Path.Join(StringsFolderPath, language);
    }

    private static IEnumerable<string> GetSupportedLanguages()
    {
        return Directory.GetDirectories(StringsFolderPath, "*", SearchOption.TopDirectoryOnly)
            .Select(Path.GetFileName)
            .Where(x => x is not null)
            .Cast<string>();
    }
}
