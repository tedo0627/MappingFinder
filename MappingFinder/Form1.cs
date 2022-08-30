using System;
using System.IO;
using System.Windows.Forms;

namespace MappingFinder
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            openFileDialog1.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
            DialogResult dr = openFileDialog1.ShowDialog();
            if (dr == System.Windows.Forms.DialogResult.OK)
            {
                String text = openFileDialog1.FileName;
                textBox1.Text = text;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string text = textBox1.Text;
            if (!File.Exists(text))
            {
                MessageBox.Show($"ファイル {text} が見つかりません");
                return;
            }

            Hide();

            Form2 form = new Form2(text);
            form.ShowDialog();

            Close();
        }
    }
}
