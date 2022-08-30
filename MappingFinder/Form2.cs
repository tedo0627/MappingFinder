using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace MappingFinder
{
    public partial class Form2 : Form
    {
        private Dictionary<string, string> classMapping = new Dictionary<string, string>();
        private Dictionary<string, List<ClassMember>> memberMapping = new Dictionary<string, List<ClassMember>>();

        public Form2(string file)
        {
            InitializeComponent();

            StreamReader sr = new StreamReader(file, Encoding.UTF8);
            string currentClassName = "";
            while (sr.Peek() >= 0)
            {
                string text = sr.ReadLine();
                if (text.StartsWith("#")) continue; // コメントだった場合

                if (!text.StartsWith("    ")) // クラスだった場合
                {
                    string[] classSplit = text.Remove(text.Length - 1).Split(" -> ");
                    string[] classPackage = classSplit[0].Split(".");
                    string className = classPackage[classPackage.Length - 1];
                    classMapping[className] = classSplit[1];

                    currentClassName = className;
                    continue;
                }

                // フィールドかメソッドだった場合
                string[] memberSplit = text.Remove(0, 4).Split(" -> ");
                if (memberSplit.Length == 1)
                {
                    MessageBox.Show(text);
                }
                string[] typeAndName = memberSplit[0].Split(" ");
                string[] typeSplit = typeAndName[0].Split(".");
                string type = typeSplit[typeSplit.Length - 1];
                
                string name = typeAndName[1];
                if (name.Contains("("))
                {
                    string[] nameArgumentSplit = name.Remove(name.Length - 1).Split("(");
                    string[] argumentSplit = nameArgumentSplit[1].Split(",");
                    List<string> newArguments = new List<string>();
                    foreach (string argumentPackage in argumentSplit)
                    {
                        string[] packageSplit = argumentPackage.Split(".");
                        newArguments.Add(packageSplit[packageSplit.Length - 1]);
                    }
                    name = $"{nameArgumentSplit[0]}({string.Join(", ", newArguments)})";
                }
                string mapping = memberSplit[1];
                bool isField = !type.Contains(":");
                if (!isField)
                {
                    string[] coronSplit = type.Split(":");
                    type = coronSplit[coronSplit.Length - 1];
                }

                if (memberMapping.ContainsKey(currentClassName))
                {
                    memberMapping[currentClassName].Add(new ClassMember(name, mapping, type, isField));
                }
                else
                {
                    List<ClassMember> list = new List<ClassMember>();
                    list.Add(new ClassMember(name, mapping, type, isField));
                    memberMapping[currentClassName] = list;
                }
            }

            UpdateShowClassNames();
        }

        private void textBox1_TextChanged(object sender, System.EventArgs e)
        {
            UpdateShowClassNames();
        }

        private void UpdateShowClassNames()
        {
            string text = textBox1.Text;

            List<string> filter = new List<string>();
            foreach (string key in classMapping.Keys)
            {
                if (key.StartsWith(text)) filter.Add($"{key} -> {classMapping[key]}");
            }
            filter.Sort();

            richTextBox1.Clear();
            richTextBox1.Lines = filter.ToArray();

            richTextBox2.Clear();
            if (!memberMapping.ContainsKey(text)) return;

            List<ClassMember> list = memberMapping[text];

            List<string> strList = new List<string>();
            foreach (ClassMember member in list)
            {
                strList.Add($"{member.Name}: {member.ReturnType} -> {member.Mapping}");
                strList.Add("");
            }
            richTextBox2.Lines = strList.ToArray();
        }

        private void richTextBox1_MouseDown(object sender, MouseEventArgs e)
        {
            int line = richTextBox1.GetLineFromCharIndex(richTextBox1.SelectionStart);

            string text = textBox1.Text;

            List<string> filter = new List<string>();
            foreach (string key in classMapping.Keys)
            {
                if (key.StartsWith(text)) filter.Add(key);
            }
            filter.Sort();

            if (filter.Count <= line) return;

            textBox1.Text = filter[line];
        }
    }
}
