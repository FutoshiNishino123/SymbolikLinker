using System.Diagnostics;

namespace SymbolicLinker.Commands
{
    internal class DirectoryList
    {
        public string Path { get; set; } = default!;

        public void Show()
        {
            var commands = new[]
            {
                $"dir {Path}",
                "pause",
            };
            var info = new ProcessStartInfo
            {
                FileName = "powershell.exe",
                UseShellExecute = true,
                Arguments = string.Join(';', commands),
            };

            Process.Start(info)?.WaitForExit();
        }
    }
}