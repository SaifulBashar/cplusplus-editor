using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using System.Diagnostics;
using System.IO;

namespace WindowsFormsApplication1
{
    public partial class Form1 : Form
    {
        public int build;
        public string filepath;
        public string result;
        SearchAndReplace s = new SearchAndReplace();
        SyntaxColoring color = new SyntaxColoring();
        Undo u = new Undo();
        public Form1()
        {
            InitializeComponent();


        }



        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {

            richTextBox1.AcceptsTab = true;

            MatchCollection keywordMatches = color.match(richTextBox1.Text);

            int originalIndex = richTextBox1.SelectionStart;
            int originalLength = richTextBox1.SelectionLength;
            Color originalColor = Color.Black;

            runButton.Focus();
            richTextBox1.SelectionStart = 0;
            richTextBox1.SelectionLength = richTextBox1.Text.Length;
            richTextBox1.SelectionColor = originalColor;

            foreach (Match m in keywordMatches)
            {
                richTextBox1.SelectionStart = m.Index;
                richTextBox1.SelectionLength = m.Length;
                richTextBox1.SelectionColor = Color.Blue;
            }

            richTextBox1.SelectionStart = originalIndex;
            richTextBox1.SelectionLength = originalLength;
            richTextBox1.SelectionColor = originalColor;

            richTextBox1.Focus();

        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //OpenFileDialog open = new OpenFileDialog();
            //open.Filter = "Text files (*.*)|*.*";
            //if (open.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            //{
            //    richTextBox1.LoadFile(open.FileName, RichTextBoxStreamType.PlainText);
            //    filepath = open.FileName;

            //}
            ////word count after load file
            ////create object of WORDCOUNT
            //WordCount w = new WordCount();
            //w.regexExpression = @"\w+";
            //MatchCollection wordCollection = Regex.Matches(richTextBox1.Text, w.regexExpression);

            //countWordLabel.Text = w.wordCounter(wordCollection);

            this.openFile();
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //SaveFileDialog saveFile1 = new SaveFileDialog();


            //saveFile1.DefaultExt = "*.cpp";
            //saveFile1.Filter = "C++ Files|*.cpp";

            //// Determine if the user selected a file name from the saveFileDialog.
            //if (saveFile1.ShowDialog() == System.Windows.Forms.DialogResult.OK &&
            //   saveFile1.FileName.Length > 0)
            //{
            //    filepath = saveFile1.FileName;

            //    // Save the contents of the RichTextBox into the file.
            //    richTextBox1.SaveFile(saveFile1.FileName, RichTextBoxStreamType.PlainText);
            //}
            this.savefile();
        }

        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {

            try
            {
                // create the ProcessStartInfo using "cmd" as the program to be run, and "/c " as the
                // parameters. & C:\\Users\\student\\Desktop\\i.exe

                // Incidentally, /c tells cmd that we want it to execute the command that follows,
                // and then exit.
                //System.Diagnostics.ProcessStartInfo procStartInfo =
                //    new System.Diagnostics.ProcessStartInfo("cmd", "/K cd\\ & g++ -o  C:\\Users\\gg\\Desktop\\main " + filepath );
                System.Diagnostics.ProcessStartInfo procStartInfo =
                    new System.Diagnostics.ProcessStartInfo("cmd", "/K cd\\ & g++ -o " + filepath.Substring(0, filepath.Length - 4) + " " + filepath);
                // The following commands are needed to redirect the standard output. This means that
                // it will be redirected to the Process.StandardOutput StreamReader.
                procStartInfo.RedirectStandardOutput = true;
                procStartInfo.UseShellExecute = false;
                // Do not create the black window.
                procStartInfo.CreateNoWindow = false;

                // Now we create a process, assign its ProcessStartInfo and start it
                System.Diagnostics.Process proc = new System.Diagnostics.Process();
                proc.StartInfo = procStartInfo;
                proc.Start();
                proc.WaitForExit();
                // Get the output into a string
                result = proc.StandardOutput.ReadToEnd();
                // Display the command output.
                build = 1;


            }
            catch (Exception objException)
            {
                MessageBox.Show("SOMETHING WRONG");

            }

        }
        public static Boolean curslyBracesKeyPressed = false;
        public static Boolean semi = false;
        private void bracket(object sender, KeyPressEventArgs e)
        {



            String s = e.KeyChar.ToString();

            int sel = richTextBox1.SelectionStart;

            switch (s)
            {
                case "(":
                    richTextBox1.Text = richTextBox1.Text.Insert(sel, "()");
                    e.Handled = true;
                    richTextBox1.SelectionStart = sel + 1;
                    break;

                case "{":
                    String t = "{}";
                    richTextBox1.Text = richTextBox1.Text.Insert(sel, t);
                    e.Handled = true;
                    richTextBox1.SelectionStart = sel + t.Length - 1;
                    curslyBracesKeyPressed = true;
                    break;

                case "[": richTextBox1.Text = richTextBox1.Text.Insert(sel, "[]");
                    e.Handled = true;
                    richTextBox1.SelectionStart = sel + 1;
                    break;

                case "'": richTextBox1.Text = richTextBox1.Text.Insert(sel, "''");
                    e.Handled = true;
                    richTextBox1.SelectionStart = sel + 1;
                    break;

            }




        }
        public int number = 0;
        private void keydown(object sender, KeyEventArgs e)
        {

            if (richTextBox1.Text.Length == 0)
            {
                number = 0;
                curslyBracesKeyPressed = false;
            }
            int sel = richTextBox1.SelectionStart;
            if (e.KeyCode == Keys.Enter)
            {
                if (curslyBracesKeyPressed == true)
                {
                    richTextBox1.Text = richTextBox1.Text.Insert(sel, "\n       \n");
                    e.Handled = true;
                    richTextBox1.SelectionStart = sel + "        ".Length;
                    curslyBracesKeyPressed = false;
                    number += 1;
                }



            }

            if (e.Modifiers == Keys.Control)
            {
                switch (e.KeyCode)
                {
                    // you can add what ever keys you want to handle here
                    case Keys.O:
                        this.openFile();
                        e.Handled = true;
                        break;
                    case Keys.S:

                        e.Handled = true;
                        this.savefile();
                        break;
                    //case Keys.Z:
                    //    richTextBox1.Undo();
                    //    e.Handled = true;
                    //    break;
                    default:
                        break;
                }
            }
        }

        private void menuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void keyUp(object sender, KeyEventArgs e)
        {
            WordCount w = new WordCount();
            //w.regexExpression = @"[\W]+";
            w.regexExpression = @"\w+";
            MatchCollection wordCollection = Regex.Matches(richTextBox1.Text, w.regexExpression);

            countWordLabel.Text = w.wordCounter(wordCollection);
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void fileToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void replaceToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form2 f2 = new Form2();

            f2.Show();
            f2.getForm1(this);

            f2.getObjectFromForm2(ref s);


        }


        private void button2_Click(object sender, EventArgs e)
        {
            //Regex rgx = new Regex(@"\b" + s.RegexExpression + @"\b");
            Regex rgx = new Regex(@"" + s.RegexExpression + "");
            if (s.flag == 1 && rgx.IsMatch(richTextBox1.Text))
            {



                richTextBox1.Text = rgx.Replace(richTextBox1.Text, s.Replace);
                s.flag = 0;
            }
            if (rgx.IsMatch(richTextBox1.Text) == false && s.flag == 1)
            {
                MessageBox.Show("No Match");
            }
            if (s.flag == 0)
            {

            }
        }

        private void run(object sender, KeyEventArgs e)
        {

        }
        public void savefile()
        {
            //assign file path
            string path = @"" + filepath + "";
            //checking file for its existence
            if (!File.Exists(path))
            {
                //Creating object of save file dialog
                SaveFileDialog saveFile1 = new SaveFileDialog();
                //default save option ".cpp"
                saveFile1.DefaultExt = "*.cpp";
                saveFile1.Filter = "C++ Files|*.cpp";

                // Determine if the user selected a file name from the saveFileDialog.
                if (saveFile1.ShowDialog() == System.Windows.Forms.DialogResult.OK
                    && saveFile1.FileName.Length > 0)
                {
                    filepath = saveFile1.FileName;

                    // Save the contents of the RichTextBox into the file.
                    richTextBox1.SaveFile(saveFile1.FileName, RichTextBoxStreamType.PlainText);
                }
            }
            else
                //Write file or overwrite
                File.WriteAllText(path, richTextBox1.Text);

        }
        public void openFile()
        {
            //object of open file dialog
            OpenFileDialog open = new OpenFileDialog();
            //show all file in Dialog
            open.Filter = "Text files (*.*)|*.*";
            //Confirmation
            if (open.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                //Load file from directory 
                richTextBox1.LoadFile(open.FileName, RichTextBoxStreamType.PlainText);
                //save file path from open.filename
                filepath = open.FileName;

            }
            
            //word count after load file
            //create object of WORDCOUNT class
            WordCount w = new WordCount();
            //select word from richtextbox1
            w.regexExpression = @"\w+";
            //wordcollection is array
            //it matches all text from richtextbox and expression
            MatchCollection wordCollection = Regex.Matches(richTextBox1.Text, w.regexExpression);
            //show count number in word label
            countWordLabel.Text = w.wordCounter(wordCollection);

        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            if (build == 1)
            {
                try
                {
                    // create the ProcessStartInfo using "cmd" as the program to be run, and "/c " as
                    // the parameters. & C:\\Users\\student\\Desktop\\i.exe

                    // Incidentally, /c tells cmd that we want it to execute the command that
                    // follows, and then exit.
                    System.Diagnostics.ProcessStartInfo procStartInfo =
                        new System.Diagnostics.ProcessStartInfo("cmd", "/c cd\\ & " + filepath.Substring(0, filepath.Length - 4));

                    // The following commands are needed to redirect the standard output. This means
                    // that it will be redirected to the Process.StandardOutput StreamReader.
                    procStartInfo.RedirectStandardOutput = true;
                    procStartInfo.UseShellExecute = false;
                    // Do not create the black window.
                    procStartInfo.CreateNoWindow = false;

                    // Now we create a process, assign its ProcessStartInfo and start it
                    System.Diagnostics.Process proc = new System.Diagnostics.Process();
                    proc.StartInfo = procStartInfo;
                    proc.Start();
                    proc.WaitForExit();
                    // Get the output into a string
                    result = proc.StandardOutput.ReadToEnd();
                    // Display the command output.

                    MessageBox.Show(result);
                    build = 0;
                }
                catch (Exception objException)
                {
                    MessageBox.Show("SOMETHING WRONG");

                }
            }
            else
                MessageBox.Show("Build first","Warning",MessageBoxButtons.OK,MessageBoxIcon.Warning,MessageBoxDefaultButton.Button1);

        }

        private void vScrollBar1_Scroll(object sender, ScrollEventArgs e)
        {

        }

        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFile1 = new SaveFileDialog();


            saveFile1.DefaultExt = "*.cpp";
            saveFile1.Filter = "C++ Files|*.cpp";

            // Determine if the user selected a file name from the saveFileDialog.
            if (saveFile1.ShowDialog() == System.Windows.Forms.DialogResult.OK &&
               saveFile1.FileName.Length > 0)
            {
                filepath = saveFile1.FileName;

                // Save the contents of the RichTextBox into the file.
                richTextBox1.SaveFile(saveFile1.FileName, RichTextBoxStreamType.PlainText);
            }
        }


    }
}

