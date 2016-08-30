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

namespace WindowsFormsApplication1
{
    public partial class Form2 : Form 
    {   
        //form1 class object
        public Form1 h = new Form1();
        SearchAndReplace s = new SearchAndReplace();
        public Form2()
        {
            InitializeComponent();
        }
        //passing reference of searchandreplaceclass object
        public void getObjectFromForm2(ref SearchAndReplace a )
        {
            s = a;
        }

        private void Form2_Load(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            //s is a object of searchandreplace class
            s.RegexExpression = searchtextBox.Text;
            s.replaceWord = replacetextBox.Text;
            //initializing patter
            Regex rgx = new Regex(@"" + s.RegexExpression + "");
            //chech pattern
            if ( rgx.IsMatch(h.richTextBox1.Text))
            {
                //replace matching string
                h.richTextBox1.Text = rgx.Replace(h.richTextBox1.Text, s.Replace);   
            }
            //if no match found
            else
            {
                MessageBox.Show("no match");
            }
            this.Hide();
        }

        private void searchtextBox_TextChanged(object sender, EventArgs e)
        {

        }
        
        public void getForm1(Form1 f1)
        {
            h = f1;
        }
    }
}
