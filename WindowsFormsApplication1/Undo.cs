using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApplication1
{
    public class Undo
    {
        public Stack<string> undoList = new Stack<string>();
        public void addTextInStack(string s)
        {
            undoList.Push(s);
        }
        public string popTextStack()
        {
            return undoList.Pop();
        }
    }
}
