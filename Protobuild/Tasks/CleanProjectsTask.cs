using System.IO;
using System.Xml;
using System.Linq;

namespace Protobuild.Tasks
{
    public class CleanProjectsTask : GenerateProjectsTask
    {
        public override bool Execute()
        {
            this.Log.LogMessage(
                "Starting clean of projects for " + this.Platform);

            var module = ModuleInfo.Load(Path.Combine(this.RootPath, "Build", "Module.xml"));
            var definitions = module.GetDefinitions();
            
            foreach (var definition in definitions.Select(x => x.Name))
            {
                this.Log.LogMessage("Cleaning: " + definition);
                var projectDoc = new XmlDocument();
                projectDoc.Load(Path.Combine(
                    this.SourcePath,
                    definition + ".definition"));
                if (projectDoc == null ||
                    projectDoc.DocumentElement.Name != "Project")
                    continue;
                var path = Path.Combine(
                    this.RootPath,
                    projectDoc.DocumentElement.Attributes["Path"].Value
                        .Replace('\\', Path.DirectorySeparatorChar)
                        .Replace('/', Path.DirectorySeparatorChar),
                    projectDoc.DocumentElement.Attributes["Name"].Value + "." +
                    this.Platform + ".csproj");
                if (File.Exists(path))
                    File.Delete(path);
            }

            this.Log.LogMessage(
                "Clean complete.");

            return true;
        }
    }
}

