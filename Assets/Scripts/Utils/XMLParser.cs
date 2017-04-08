using System.Collections;

namespace UnityMVC.Utils
{
    public static class XMLParser : object
    {
        private const char LT = '<';
        private const char GT = '>';
        private const char SQR = ']';
        //private const char SQL = '[';
        private const char DASH = '-';
        private const char SPACE = ' ';
        private const char QUOTE = '"';
        private const char SLASH = '/';
        private const char QMARK = '?';
        private const char EQUALS = '=';
        private const char NEWLINE = '\n';
        private const char EXCLAMATION = '!';

        public static XMLNode Parse(string content)
        {
            // Set up variables
            bool inMetaTag = false;
            bool inComment = false;
            bool inCDATA = false;
            bool inElement = false;
            bool collectNodeName = false;
            bool collectAttributeName = false;
            bool collectAttributeValue = false;
            bool quoted = false;
            string attName = "";
            string attValue = "";
            string nodeName = "";
            string textValue = "";
            //string nodeContents = "";

            XMLNodeList parents = new XMLNodeList();

            XMLNode rootNode = new XMLNode();
            rootNode["_text"] = "";

            XMLNode currentNode = rootNode;

            // Process Input
            for (int i = 0; i < content.Length; i++)
            {
                // Store current and nearby characters
                char c, cn, cnn, cp;
                cn = cnn = cp = '\x00';
                c = content[i];
                if ((i + 1) < content.Length) cn = content[i + 1];
                if ((i + 2) < content.Length) cnn = content[i + 2];
                if (i > 0) cp = content[i - 1];


                // Process Meta Tag information
                if (inMetaTag)
                {
                    if (c == QMARK && cn == GT)
                    { // End of Meta Tag
                        inMetaTag = false;
                        i++;
                    }
                    continue;
                }
                else
                {
                    if (!quoted && c == LT && cn == QMARK)
                    { // Start of Meta Tag
                        inMetaTag = true;
                        continue;
                    }
                }


                // Process Comment information
                if (inComment)
                {
                    if (cp == DASH && c == DASH && cn == GT)
                    { // End of comment
                        inComment = false;
                        i++;
                    }
                    continue;
                }
                else
                {
                    if (!quoted && c == LT && cn == EXCLAMATION)
                    { // Start of comment or CDATA 
                        if (content.Length > (i + 9) && content.Substring(i, 9) == "<![CDATA[")
                        {
                            inCDATA = true;
                            i += 8;
                        }
                        else
                        {
                            inComment = true;
                        }
                        continue;
                    }
                }


                // Process CDATA information
                if (inCDATA)
                {
                    if (c == SQR && cn == SQR && cnn == GT)
                    {
                        inCDATA = false;
                        i += 2;
                        continue;
                    }
                    textValue += c;
                    continue;
                }


                // Process Elements
                if (inElement)
                {

                    if (collectNodeName)
                    {
                        if (c == SPACE)
                        {
                            collectNodeName = false;
                        }
                        else if (c == GT)
                        {
                            collectNodeName = false;
                            inElement = false;
                        }


                        if (!collectNodeName && nodeName.Length > 0)
                        {
                            if (nodeName[0] == SLASH)
                            {
                                // close tag
                                if (textValue.Length > 0)
                                {
                                    currentNode["_text"] += textValue;
                                }

                                textValue = "";
                                nodeName = "";
                                currentNode = parents.Pop();
                            }
                            else
                            {

                                if (textValue.Length > 0)
                                {
                                    currentNode["_text"] += textValue;
                                }
                                textValue = "";
                                XMLNode newNode = new XMLNode();
                                newNode["_text"] = "";
                                newNode["_name"] = nodeName;

                                if (!currentNode.ContainsKey(nodeName))
                                {
                                    currentNode[nodeName] = new XMLNodeList();
                                }
                                XMLNodeList a = currentNode[nodeName] as XMLNodeList;
                                a.Push(newNode);
                                parents.Push(currentNode);
                                currentNode = newNode;
                                nodeName = "";
                            }
                        }
                        else
                        {
                            nodeName += c;
                        }
                    }
                    else
                    {

                        if (!quoted && c == SLASH && cn == GT)
                        {
                            inElement = false;
                            collectAttributeName = false;
                            collectAttributeValue = false;
                            if (attName != "")
                            {
                                if (attValue != "")
                                {
                                    currentNode["@" + attName] = attValue;
                                }
                                else
                                {
                                    currentNode["@" + attName] = true;
                                }
                            }

                            i++;
                            currentNode = parents.Pop();
                            attName = "";
                            attValue = "";
                        }
                        else if (!quoted && c == GT)
                        {
                            inElement = false;
                            collectAttributeName = false;
                            collectAttributeValue = false;
                            if (attName != "")
                            {
                                currentNode["@" + attName] = attValue;
                            }

                            attName = "";
                            attValue = "";
                        }
                        else
                        {
                            if (collectAttributeName)
                            {
                                if (c == SPACE || c == EQUALS)
                                {
                                    collectAttributeName = false;
                                    collectAttributeValue = true;
                                }
                                else
                                {
                                    attName += c;
                                }
                            }
                            else if (collectAttributeValue)
                            {
                                if (c == QUOTE)
                                {
                                    if (quoted)
                                    {
                                        collectAttributeValue = false;
                                        currentNode["@" + attName] = attValue;
                                        attValue = "";
                                        attName = "";
                                        quoted = false;
                                    }
                                    else
                                    {
                                        quoted = true;
                                    }
                                }
                                else
                                {
                                    if (quoted)
                                    {
                                        attValue += c;
                                    }
                                    else
                                    {
                                        if (c == SPACE)
                                        {
                                            collectAttributeValue = false;
                                            currentNode["@" + attName] = attValue;
                                            attValue = "";
                                            attName = "";
                                        }
                                    }
                                }
                            }
                            else if (c == SPACE)
                            {

                            }
                            else
                            {
                                collectAttributeName = true;
                                attName = "" + c;
                                attValue = "";
                                quoted = false;
                            }
                        }
                    }

                }
                else
                {
                    if (c == LT)
                    { // Start of new element
                        inElement = true;
                        collectNodeName = true;
                    }
                    else
                    {
                        textValue += c; // text between elements
                    }
                }
            }
            return rootNode;
        }
    }
}
