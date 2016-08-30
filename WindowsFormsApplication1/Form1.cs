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
        public Form1()
        {
            InitializeComponent();


        }



        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {
            //activate tab key in richtextbox
            richTextBox1.AcceptsTab = true;
            //color is a syntaxcolouring object
            //color.match return array of match keyword
            MatchCollection keywordMatches = color.match(richTextBox1.Text);
            //save current caret position
            int originalIndex = richTextBox1.SelectionStart;
            /*Getting this property returns the number of characters in the current selection.
             * Setting this property adjusts the length of the current selection to the specified value,
             * keeping the beginning of the selection fixed.
             * In general, when the specified selection length causes the selection to end in an invalid position 
             * (for example, between a carriage return and a new-line character, or inside a tag),
             * the selection length automatically 
             * adjusts so that the resulting selection starts and ends in valid positions.*/
            int originalLength = richTextBox1.SelectionLength;
            Color originalColor = Color.Black;
            //avoid blinking
            runButton.Focus();
            // removes any previous highlighting (so modified words won't remain highlighted)
            richTextBox1.SelectionStart = 0;
            richTextBox1.SelectionLength = richTextBox1.Text.Length;
            richTextBox1.SelectionColor = originalColor;

            foreach (Match m in keywordMatches)
            {
                //select match keyword
                //colour it to blue
                richTextBox1.SelectionStart = m.Index;
                richTextBox1.SelectionLength = m.Length;
                richTextBox1.SelectionColor = Color.Blue;
            }
            // restoring the original colors, for further writing
            richTextBox1.SelectionStart = originalIndex;
            richTextBox1.SelectionLength = originalLength;
            richTextBox1.SelectionColor = originalColor;
            // giving back the focus
            richTextBox1.Focus();
            
            

        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {

            this.openFile();
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
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
                build = 1;


            }
            catch (Exception objException)
            {
                MessageBox.Show("SOMETHING WRONG"+objException);

            }

        }
        public static Boolean curslyBracesKeyPressed = false;
        public static Boolean semi = false;
        /*KeyPress Event : This event is raised for character keys while the key is pressed 
         * and then released. This event is not raised by noncharacter keys, 
         * unlike KeyDown and KeyUp, which are also raised for noncharacter keys*/
        private void bracket(object sender, KeyPressEventArgs e)
        {
            //convert keyboard charachter to string
            String s = e.KeyChar.ToString();
            //current position of caret
            int sel = richTextBox1.SelectionStart;
            //switch case for "("
            //switch case for "{"
            //switch case for "["
            //switch case for "'"
            switch (s)
            {
                case "(":
                    //auto complete bracket
                    richTextBox1.Text = richTextBox1.Text.Insert(sel, "()");
                    //Set Handled to true to cancel the KeyPress event.
                    e.Handled = true;
                    //previous selection start is here
                    //      |(
                    //than insert |()
                    //now selectionstart+1 means 
                    //      (|)
                    richTextBox1.SelectionStart = sel + 1;
                    break;

                case "{":
                    //complete braces
                    //also invoke keydown action for enter input
                    richTextBox1.Text = richTextBox1.Text.Insert(sel, "{}");
                    //Set Handled to true to cancel the KeyPress event.
                    e.Handled = true;
                    //previous selection start is here
                    //      |{
                    //than insert |{}
                    //now selectionstart+1 means 
                    //      {|}
                    richTextBox1.SelectionStart = sel + 1;
                    //boolen flag to keydown action that use press curslybrace
                    curslyBracesKeyPressed = true;
                    break;

                case "[":
                    //complete square bracket
                    richTextBox1.Text = richTextBox1.Text.Insert(sel, "[]");
                    //Set Handled to true to cancel the KeyPress event.
                    e.Handled = true;
                    //previous selection start is here
                    //      |[
                    //than insert |[]
                    //now selectionstart+1 means 
                    //      [|]
                    richTextBox1.SelectionStart = sel + 1;
                    break;

                case "'": richTextBox1.Text = richTextBox1.Text.Insert(sel, "''");
                    //Set Handled to true to cancel the KeyPress event.
                    e.Handled = true;
                    //previous selection start is here
                    //      |'
                    //than insert |''
                    //now selectionstart+1 means 
                    //      '|'
                    richTextBox1.SelectionStart = sel + 1;
                    break;

            }




        }
        
        /*KeyDown Event : This event raised as soon as the user presses a key on the keyboard, 
        it repeats while the user keeps the key depressed.*/
        private void keydown(object sender, KeyEventArgs e)
        {
            
            //current caret position
            int sel = richTextBox1.SelectionStart;
            //if keyboard get enter input
            if (e.KeyCode == Keys.Enter)
            {
                //chech did user input curslyBraces
                if (curslyBracesKeyPressed == true)
                {

                    richTextBox1.Text = richTextBox1.Text.Insert(sel, "\n       \n");
                    //Set Handled to true to cancel the KeyPress event.
                    e.Handled = true;
                    richTextBox1.SelectionStart = sel + "        ".Length;
                    //curslybraces is false
                    curslyBracesKeyPressed = false;
                    
                }



            }
            //it is for keyboard shortcut
            //CTRL+O for open file
            //CTRL+S for save file
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
        /*KeyUp Event : This event is raised after the user releases a key on the keyboard.*/
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
            //create form2 object
            Form2 f2 = new Form2();
            //display form2
            f2.Show();
            //current form2  object as parameter
            f2.getForm1(this);
            
            f2.getObjectFromForm2(ref s);
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
                    MessageBox.Show("SOMETHING WRONG"+ objException);

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

        private void lineNumberOnSelectionChanged(object sender, EventArgs e)
        {
            int i = richTextBox1.SelectionStart;
            
            line.Text = "Line number :" + (richTextBox1.GetLineFromCharIndex(i)+1).ToString();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }


    }
}

