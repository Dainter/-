using System;
using System.Collections.Generic;
using System.Xml;

namespace SmartTaskChain
{
    public class Utility
    {
        //工具函数，从xml节点中返回某个标签的标识的节点
        public static XmlElement GetNode(XmlElement curNode, string sLabel)
        {
            if (curNode == null)
            {
                return null;
            }
            //遍历子节点列表
            foreach (XmlElement xNode in curNode.ChildNodes)
            {
                if (xNode.Name == sLabel)
                {//查找和指定内容相同的标签，返回其Innner Text
                    return xNode;
                }
            }
            return null;
        }
        //工具函数，从xml节点中读取某个标签的InnerText
        public static string GetText(XmlElement curNode, string sLabel)
        {
            if (curNode == null)
            {
                return "";
            }
            //遍历子节点列表
            foreach (XmlElement xNode in curNode.ChildNodes)
            {
                if (xNode.Name == sLabel)
                {//查找和指定内容相同的标签，返回其Innner Text
                    return xNode.InnerText;
                }
            }
            return "";
        }
    }
}
