﻿using System;
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
        public string filepath;
        public string result;
        SearchAndReplace s = new SearchAndReplace();
        SyntaxColoring color = new SyntaxColoring();
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
            
            button1.Focus();
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
            OpenFileDialog open = new OpenFileDialog();
            open.Filter = "Text files (*.*)|*.*";
            if (open.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                richTextBox1.LoadFile(open.FileName, RichTextBoxStreamType.PlainText);

            }
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFile1 = new SaveFileDialog();

            // Initialize the SaveFileDialog to specify the RTF extension for the file.
            saveFile1.DefaultExt = "*.cpp";
            saveFile1.Filter = "C++ Files|*.cpp";

            // Determine if the user selected a file name from the saveFileDialog.
            if (saveFile1.ShowDialog() == System.Windows.Forms.DialogResult.OK &&
               saveFile1.FileName.Length > 0)
            {
                filepath = saveFile1.FileName;
                //filepath = filepath.Replace(@"\", @"\\");
                MessageBox.Show(filepath);
                // Save the contents of the RichTextBox into the file.
                richTextBox1.SaveFile(saveFile1.FileName, RichTextBoxStreamType.PlainText);
            }
        }

        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {

            try
            {
                // create the ProcessStartInfo using "cmd" as the program to be run,
                // and "/c " as the parameters.  & C:\\Users\\student\\Desktop\\i.exe
                //new System.Diagnostics.ProcessStartInfo("cmd", "/c cd\\ & g++ -o  C:\\Users\\gg\\Desktop\\main " + filepath + " & C:\\Users\\gg\\Desktop\\main.exe");
                // Incidentally, /c tells cmd that we want it to execute the command that follows,
                // and then exit.
                MessageBox.Show(" cd\\ & g++ -o  C:\\Users\\gg\\Desktop\\main " + filepath);
                File.WriteAllText("t.txt", "cd\\ & g++ -o  C:\\Users\\gg\\Desktop\\main " + filepath);
                System.Diagnostics.ProcessStartInfo procStartInfo =

                new System.Diagnostics.ProcessStartInfo("cmd", " /c cd\\ & g++ -o  C:\\Users\\gg\\Desktop\\main " + filepath + " & C:\\Users\\gg\\Desktop\\main.exe");
                // The following commands are needed to redirect the standard output.
                // This means that it will be redirected to the Process.StandardOutput StreamReader.
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
            }
            catch (Exception objException)
            {
                MessageBox.Show("SOMETHING WRONG");

            }

            //string strCmdText;
            //strCmdText = "/c g++ -o  main.exe " + filepath + " & main.exe";
            //System.Diagnostics.Process.Start("CMD.exe", strCmdText);







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
                    richTextBox1.Text = richTextBox1.Text.Insert(sel, "\n   \n");
                    e.Handled = true;
                    richTextBox1.SelectionStart = sel + "   ".Length;
                    curslyBracesKeyPressed = false;
                    number += 1;
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
            w.regexExpression = @"[\W]+";
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

            f2.getObjectFromForm2(ref s);


        }


        private void button2_Click(object sender, EventArgs e)
        {
            Regex rgx = new Regex(@"\b" + s.RegexExpression + @"\b");
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


    }
}
