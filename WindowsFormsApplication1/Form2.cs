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
        SearchAndReplace s = new SearchAndReplace();
        Form1 f1 = new Form1();
        public Form2()
        {
            InitializeComponent();
        }
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

            s.RegexExpression = searchtextBox.Text;
            s.replaceWord = replacetextBox.Text;
            
            Regex rgx = new Regex(@"" + s.RegexExpression + "");
            if ( rgx.IsMatch(h.richTextBox1.Text))
            {



                h.richTextBox1.Text = rgx.Replace(h.richTextBox1.Text, s.Replace);
                s.flag = 0;
            }
            if (rgx.IsMatch(h.richTextBox1.Text) == false )
            {
                MessageBox.Show("No Match");
                MessageBox.Show(h.richTextBox1.Text);
            }
            
            
            this.Hide();
        }

        private void searchtextBox_TextChanged(object sender, EventArgs e)
        {

        }
        public Form1 h = new Form1();
        public void getForm1(Form1 f1)
        {
            h = f1;
        }
    }
}
