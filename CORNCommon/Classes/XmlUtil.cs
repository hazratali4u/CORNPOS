using System;
using System.Data;
using System.Xml;

namespace CORNCommon.Classes
{
	/// <summary>
	/// <author>Rizwan Ansari</author>
	/// <date>19-06-2007</date>
	/// </summary>
	public class XmlUtil
	{
		private string m_path;
		private string m_nodeText;
		private XmlDocument xmlDoc; 

		public XmlUtil()
		{
			
		}
		public void Save()
		{
			xmlDoc.Save(Path);

		}

		public XmlUtil(string p_path)
		{
			
			xmlDoc = new XmlDocument(); 
			xmlDoc.Load(p_path);
			Path = p_path;
		}

		public string Path
		{
			get
			{
				return m_path;
			}
			set 
			{
				m_path = value;
			}
			
		}

		public string GetNode(string p_nodeName)
		{
			XmlNode node;
			XmlNode lastNode;

			if(p_nodeName.Equals(""))
			{
				m_nodeText = "Node name missing";
				return m_nodeText;
			}
			
			m_nodeText = "";
			if(xmlDoc.HasChildNodes)
			{
				node = xmlDoc.DocumentElement;
				if(!node.HasChildNodes)
				{
					return m_nodeText;
				}
				else
				{
					node = xmlDoc.DocumentElement.FirstChild;
					
					if(node.Name == p_nodeName)
					{
						m_nodeText = node.InnerText;
					}
					else
					{
						lastNode = xmlDoc.DocumentElement.LastChild;
						do
						{
							if(node == lastNode && node.Name != p_nodeName)
							{
								m_nodeText = "";
								return m_nodeText;
							}
							node = node.NextSibling;
							
						}while(node.Name != p_nodeName);
						m_nodeText = node.InnerText;
					}

				}
				return m_nodeText;
			}
			else 
			{
				return m_nodeText = "Invalid XML Document";
			}
			
		}
		public void CloseDocument()
		{
			if(xmlDoc != null)
			{
				xmlDoc = null;
			}
		}
	}
}
