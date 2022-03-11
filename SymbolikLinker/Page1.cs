namespace SymbolicLinker
{
    public partial class Page1 : UserControl
    {
        public string InputValue
        {
            get
            {
                return textBox1.Text;
            }
            set
            {
                textBox1.Text = value;
            }
        }

        public string Description
        {
            get
            {
                return label1.Text;
            }
            set
            {
                label1.Text = value;
            }
        }

        public string CommandText
        {
            get
            {
                return button1.Text;
            }
            set
            {
                button1.Text = value;
            }
        }

        public event EventHandler? CommandInvoked;

        public Page1()
        {
            InitializeComponent();
        }

        private void Page1_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data?.GetDataPresent(DataFormats.FileDrop) ?? false)
            {
                e.Effect = DragDropEffects.All;
            }
            else
            {
                e.Effect = DragDropEffects.None;
            }
        }

        private void Page1_DragDrop(object sender, DragEventArgs e)
        {
            if (e.Data == null) { return; }

            string[] files = (string[])e.Data.GetData(DataFormats.FileDrop, false);

            InputValue = files.FirstOrDefault() ?? "";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            CommandInvoked?.Invoke(this, EventArgs.Empty);
        }
    }
}
