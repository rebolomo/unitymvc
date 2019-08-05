using System.Collections;
using UnityEngine;

namespace UnityMVC.Utils
{
    public class XMLNode : Hashtable
    {

        public XMLNodeList GetNodeList(string path)
        {
            return (GetObject(path) as XMLNodeList);
        }

        public XMLNode GetNode(string path)
        {
            return (GetObject(path) as XMLNode);
        }

        public string GetValue(string path)
        {
            return (GetObject(path) as string);
        }


        private object GetObject(string path)
        {
            XMLNode currentNode = this;
            XMLNodeList currentNodeList = null;
            bool inList = false;
            object ob = null;

            string[] bits = path.Split('>');

            for (int i = 0; i < bits.Length; i++)
            {
                if (inList)
                {
                    ob = currentNode = (currentNodeList[int.Parse(bits[i])] as XMLNode);
                    inList = false;
                }
                else
                {
                    ob = currentNode[bits[i]];

                    if (ob is XMLNodeList)
                    {
                        currentNodeList = ob as XMLNodeList;
                        inList = true;
                    }
                    else
                    {
                        // reached a leaf node/attribute
                        if (i != (bits.Length - 1))
                        {
                            // unexpected leaf node
                            string actualPath = "";
                            for (int j = 0; j <= i; j++)
                            {
                                actualPath = actualPath + ">" + bits[j];
                            }
#if UNITY_EDITOR
                           ClientLogger.Info("xml path search truncated. Wanted: " + path + " got: " + actualPath);
#endif
                        }
                        return ob;
                    }

                }
            }

            if (inList) return currentNodeList;
            else return currentNode;
        }
    }
}
