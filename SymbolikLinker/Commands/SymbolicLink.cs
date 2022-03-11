using System.Diagnostics;

namespace SymbolicLinker.Commands
{
    internal class SymbolicLink
    {
        public string SourcePath { get; set; } = default!;
        public string TargetPath { get; set; } = default!;

        public void Create()
        {
            var commands = new[]
            {
                $"New-Item -Value '{SourcePath}' -Path '{TargetPath}' -ItemType SymbolicLink",
            };
            var info = new ProcessStartInfo
            {
                FileName = "powershell.exe",
                Verb = "RunAs",  // 管理者権限（シンボリックリンクの作成に必要）
                UseShellExecute = true,
                Arguments = string.Join(';', commands),
            };

            Process.Start(info)?.WaitForExit();
        }
    }
}