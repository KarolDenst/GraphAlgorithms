using GraphAlgorithms.Common;
using System.CommandLine;
using System.Text.Json;

namespace GraphAlgorithms.Commands
{
    internal static class ConfigCommand
    {
        public static Command Create()
        {
            var pyPathOption = OptionsFactory.CreatePyPathOption();

            var command = new Command("config", "Configure the program.")
            {
                pyPathOption
            };
            command.SetHandler(Run, pyPathOption);

            return command;
        }

        private static void Run(string pyPath)
        {
            var serializerOptions = new JsonSerializerOptions { WriteIndented = true };
            string jsonString = JsonSerializer.Serialize(new PythonSettings { Path = pyPath }, serializerOptions);

            Directory.CreateDirectory(Locations.ConfigDirPath);
            File.WriteAllText(Locations.PythonSettingsFilePath, jsonString);
        }
    }
}
