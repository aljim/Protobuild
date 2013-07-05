using System;
using System.Reflection;
using System.IO;

namespace Protobuild
{
    public static class ResourceExtractor
    {
        public static void ExtractAll(string path, string projectName)
        {
            using (var stream = Assembly.GetExecutingAssembly()
                .GetManifestResourceStream("Protobuild.BuildResources.Main.proj"))
            {
                using (var writer = new StreamWriter(Path.Combine(path, "Main.proj")))
                {
                    using (var reader = new StreamReader(stream))
                    {
                        var text = reader.ReadToEnd();
                        text = text.Replace("{MODULE_NAME}", projectName);
                        writer.Write(text);
                        writer.Flush();
                    }
                }
            }
            if (!Directory.Exists(Path.Combine(path, "Projects")))
                Directory.CreateDirectory(Path.Combine(path, "Projects"));
            var module = new ModuleInfo { Name = projectName };
            module.Save(Path.Combine(path, "Module.xml"));
        }
    }
}

