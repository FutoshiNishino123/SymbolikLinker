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
                Description = "�����N���쐬����t�H���_�܂��̓t�@�C�������",
                CommandText = "����",
                AllowDrop = true,
                Dock = DockStyle.Fill,
            };
            var page2 = new Page1
            {
                Description = "�����N�̏o�͐�t�H���_�����",
                CommandText = "����",
                AllowDrop = true,
                Dock = DockStyle.Fill,
            };
            var page3 = new Page1
            {
                Description = "�����N������́i�󔒂Ȃ烊���N��Ɠ����j",
                CommandText = "�쐬",
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
                MessageBox.Show("���݂��Ȃ��f�B���N�g���܂��̓t�@�C���ł��B", "�x��", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
                MessageBox.Show("���݂��Ȃ��f�B���N�g���ł��B", "�x��", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
                MessageBox.Show("�����̃f�B���N�g���܂��̓t�@�C�������łɑ��݂��܂��B", "�x��", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (MessageBox.Show(string.Join("\r\n", "�V���{���b�N�����N���쐬���܂��B�i�Ǘ��Ҍ������K�v�ł��j", "", $"����: {srcPath}", $"�o��: {dstPath}", "", "���s���܂����H"),
                                "�m�F", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
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
                MessageBox.Show("�V���{���b�N�����N�̍쐬�Ɏ��s���܂����B", "�G���[", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            if (Directory.Exists(dstPath) || File.Exists(dstPath))
            {
                MessageBox.Show("�V���{���b�N�����N���쐬���܂����B", "����", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            Close();
        }
    }
}