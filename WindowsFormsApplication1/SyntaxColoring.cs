using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
namespace WindowsFormsApplication1
{
    public class SyntaxColoring
    {
        public string syntax = @"\b(abstract|public|private|array|auto|bool|break|case|catch|char|class|const|const_cast|continue|decltype|default|delegate|delete|do|double|else|enum|struct|event|false|float|for|each |in|friend|goto|if|inline|int|interface |long|namespace|new|return|short|signed|sizeof|static|struct|switch|this|throw|true|try|unsigned|using|virtual|void|while)\b";
        public MatchCollection match(string text)
        {
            MatchCollection keywordMatches = Regex.Matches(text, this.syntax);
            return keywordMatches;
        }
    }
}

	

