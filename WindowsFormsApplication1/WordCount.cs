using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace WindowsFormsApplication1
{
    class WordCount
    {
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
        public string wordCounter(MatchCollection a){
            return a.Count.ToString();
        }
    }
}
