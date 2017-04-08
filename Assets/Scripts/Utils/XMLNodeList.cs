using System.Collections;

namespace UnityMVC.Utils
{
    public class XMLNodeList : ArrayList
    {
        public XMLNode Pop()
        {
            XMLNode node = this[Count - 1] as XMLNode;
            this.RemoveAt(Count - 1);
            return node;
        }

        public void Push(XMLNode node)
        {
            Add(node);
        }
    }
}
