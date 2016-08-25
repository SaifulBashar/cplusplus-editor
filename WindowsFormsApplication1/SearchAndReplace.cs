using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApplication1
{
    public class SearchAndReplace
    {
        public int flag = 0;
        public string replaceWord;
        public string regexExpression;
        public string RegexExpression
        {
            get
            {
                return regexExpression;
            }
            set
            {
                this.regexExpression = value;
            }
        }
        public string Replace
        {
            get
            {
                return this.replaceWord;
            }
            set
            {
                this.replaceWord = value;
            }
        }

    }
}
