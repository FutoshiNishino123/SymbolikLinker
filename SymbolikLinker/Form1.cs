using SymbolicLinker.Commands;
using System.Diagnostics;

namespace SymbolicLinker
{
    public partial class Form1 : Form
    {
        private Dictionary<string, string> _params = new Dictionary<string, string>();

        public Form1()
        {
            InitializeComponent();

            var page1 = new Page1
            {
                Description = "リンクを作成するフォルダまたはファイルを入力",
                CommandText = "次へ",
                AllowDrop = true,
                Dock = DockStyle.Fill,
            };
            var page2 = new Page1
            {
                Description = "リンクの出力先フォルダを入力",
                CommandText = "次へ",
                AllowDrop = true,
                Dock = DockStyle.Fill,
            };
            var page3 = new Page1
            {
                Description = "リンク名を入力（空白ならリンク先と同名）",
                CommandText = "作成",
                AllowDrop = false,
                Dock = DockStyle.Fill,
            };

            page1.CommandInvoked += Page1_CommandInvoked;
            page2.CommandInvoked += Page2_CommandInvoked;
            page3.CommandInvoked += Page3_CommandInvoked;

            Controls.Add(page1);
            Controls.Add(page2);
            Controls.Add(page3);
        }

        private void Page1_CommandInvoked(object? sender, EventArgs e)
        {
            var page = sender as Page1;
            if (page == null) throw new InvalidOperationException();

            var path = page.InputValue;
            if (!Directory.Exists(path) && !File.Exists(path))
            {
                MessageBox.Show("存在しないディレクトリまたはファイルです。", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            _params["srcPath"] = path;

            Controls.Remove(page);
        }

        private void Page2_CommandInvoked(object? sender, EventArgs e)
        {
            var page = sender as Page1;
            if (page == null) throw new InvalidOperationException();

            var path = page.InputValue;
            if (!Directory.Exists(path))
            {
                MessageBox.Show("存在しないディレクトリです。", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            _params["outDir"] = path;

            Controls.Remove(page);
        }

        private void Page3_CommandInvoked(object? sender, EventArgs e)
        {
            var page = sender as Page1;
            if (page == null) throw new InvalidOperationException();

            var srcPath = _params["srcPath"];
            var name = page.InputValue;
            if (string.IsNullOrWhiteSpace(name))
            {
                name = Path.GetFileName(srcPath);
            }

            var outDir = _params["outDir"];
            var dstPath = Path.Combine(outDir, name);
            if (Directory.Exists(dstPath) || File.Exists(dstPath))
            {
                MessageBox.Show("同名のディレクトリまたはファイルがすでに存在します。", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (MessageBox.Show(string.Join("\r\n", "シンボリックリンクを作成します。（管理者権限が必要です）", "", $"入力: {srcPath}", $"出力: {dstPath}", "", "続行しますか？"),
                                "確認", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
                != DialogResult.Yes)
            {
                return;
            }

            try
            {
                new SymbolicLink
                {
                    SourcePath = srcPath,
                    TargetPath = dstPath,
                }.Create();
            }
            catch
            {
                MessageBox.Show("シンボリックリンクの作成に失敗しました。", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            if (Directory.Exists(dstPath) || File.Exists(dstPath))
            {
                MessageBox.Show("シンボリックリンクを作成しました。", "完了", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            Close();
        }
    }
}