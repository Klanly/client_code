using System;
using System.Collections.Generic;
using System.Xml;
using System.Text;
using UnityEngine;
using Cross;
namespace MuGame
{
    public class SXMLAttr
    {
        public string str;
        public int intvalue;
        public uint uintvalue;
        public float floatvalue;
    }


    public class SXML
    {
        public static List<string> lStr;

        public bool hasFound;
        public ByteArray m_root = new ByteArray();

        private string m_strNodeName = "";





        public Dictionary<string, SXMLAttr> m_dAtttr = new Dictionary<string, SXMLAttr>();
        public Dictionary<string, List<SXML>> m_dNode = new Dictionary<string, List<SXML>>();
        public SXML()
        {
        }

        public void checkInited()
        {
            if (m_strNodeName != "")
                return;

            m_root.position = 0;
            int idx = m_root.readInt();
            m_strNodeName = lStr[idx];
            int curtype = 0;

            while (m_root.bytesAvailable > 4)
            {
                int ididx = m_root.readInt();
                if (ididx == -1)
                {
                    curtype++;
                    continue;
                }

                if (curtype == 0)
                {
                    m_dAtttr[lStr[ididx]] = getAttr(m_root);
                }
                else if (curtype == 1)
                {
                    SXML node = new SXML();
                    string id = lStr[ididx];

                    if (!m_dNode.ContainsKey(id))
                        m_dNode[id] = new List<SXML>();
                    m_dNode[id].Add(node);

                    int offset = m_root.readInt();
                    m_root.readBytes(node.m_root, m_root.position, offset);
                }
            }
        }

        private SXMLAttr getAttr(ByteArray ba)
        {
            SXMLAttr d = new SXMLAttr();
            int type = ba.readByte();
            if (type == 1)
            {
                d.intvalue = ba.readByte();
                d.uintvalue = (uint)d.intvalue;
                d.floatvalue = (float)d.intvalue;
                d.str = d.intvalue.ToString();
            }
            else if (type == 2)
            {
                d.intvalue = ba.readShort();
                d.uintvalue = (uint)d.intvalue;
                d.floatvalue = (float)d.intvalue;
                d.str = d.intvalue.ToString();
            }
            else if (type == 3)
            {
                d.intvalue  = ba.readInt();
                d.str = d.intvalue.ToString();
                d.uintvalue = (uint)d.intvalue;
                d.floatvalue = (float)d.intvalue;
            }
            else if (type == 4)
            {
                d.str = lStr[ba.readInt()];
            }
            else if (type == 5)
            {
                d.uintvalue = ba.readUnsignedInt();
                d.intvalue = 0;
                d.floatvalue = 0;
                d.str = d.uintvalue.ToString();
            }
            else if (type == 6)
            {
                d.floatvalue = ba.readFloat();
                d.uintvalue = 0;
                d.intvalue = 0;
                d.str = d.floatvalue.ToString();
            }

            return d;
        }

        public void forEach(Action<SXML> handle)
        {
            checkInited();
            foreach (List<SXML> list in m_dNode.Values)
            {
                foreach (SXML xml in list)
                {
                    handle(xml);
                }
            }
        }

      static  public List<SXML> Filer(List<SXML> list, string filter)
        {
            if (filter == "" || filter == null)
                return list;

            int m_nfilt_Oper = 0;//1"==" 2"!=" 3">" 4"<" 5">=" 6"<="
            string m_filt_name = "";
            string m_filt_strv = "";
            int m_filt_nv = 0;

            List<SXML> listXml = new List<SXML>();

            int nstep = 0;
            char[] c_arr = filter.ToCharArray();
            for (int i = 0; i < c_arr.Length; i++)
            {
                char curkey = c_arr[i];
                switch (curkey)
                {
                    case '=':
                        {
                            nstep = 1;
                            i++;
                            m_nfilt_Oper = 1; //"=="
                        } break;
                    case '!':
                        {
                            nstep = 1;
                            i++;
                            m_nfilt_Oper = 2; //"!="
                        } break;
                    case '>':
                        {
                            nstep = 1;
                            if ('=' == c_arr[i + 1])
                            {
                                i++;
                                m_nfilt_Oper = 5; //">="
                            }
                            else
                            {
                                m_nfilt_Oper = 3; //">"
                            }
                        } break;
                    case '<':
                        {
                            nstep = 1;
                            if ('=' == c_arr[i + 1])
                            {
                                i++;
                                m_nfilt_Oper = 6; //"<="
                            }
                            else
                            {
                                m_nfilt_Oper = 4; //"<"
                            }
                        } break;
                    default:
                        {
                            if (1 == nstep)
                            {
                                m_filt_strv += curkey;
                            }
                            else
                            {
                                m_filt_name += curkey;
                            }
                        } break;
                }
            }

            if (m_nfilt_Oper >= 3 && m_nfilt_Oper <= 6)
            {
                m_filt_nv = int.Parse(m_filt_strv);
            }

            int nsrcvalue;
            foreach (SXML xml in list)
            {
                switch (m_nfilt_Oper)
                {
                    case 1:
                        if (xml.getString(m_filt_name).Equals(m_filt_strv))
                            listXml.Add(xml);
                        break;
                    case 2:
                        if (!xml.getString(m_filt_name).Equals(m_filt_strv))
                            listXml.Add(xml);
                        break;
                    case 3:
                        nsrcvalue = xml.getInt(m_filt_name);
                        if (nsrcvalue > m_filt_nv)
                            listXml.Add(xml);
                        break;
                    case 4:
                        nsrcvalue = xml.getInt(m_filt_name);
                        if (nsrcvalue < m_filt_nv)
                            listXml.Add(xml);
                        break;
                    case 5:
                        nsrcvalue = xml.getInt(m_filt_name);
                        if (nsrcvalue >= m_filt_nv)
                            listXml.Add(xml);
                        break;
                    case 6:
                        nsrcvalue = xml.getInt(m_filt_name);
                        if (nsrcvalue <= m_filt_nv)
                            listXml.Add(xml);
                        break;

                }

            }
            return listXml;
        }



        public bool nextOne()
        {
            return false;
        }

        public SXML GetNode(string command, string filter = "")
        {
            List<SXML> list = GetNodeList(command, filter);
            if (list == null)
                return null;

            if (list.Count > 0)
                return list[0];
            return null;
        }

        public List<SXML> GetNodeList(string command, string filter = "")
        {
            checkInited();
            string[] cmds = command.Split('.');

            if (cmds.Length < 1) return null;

            SXML curNode = this;

            List<SXML> list = null;
            for (int i = 0; i < cmds.Length; i++)
            {
                if (curNode.m_dNode.ContainsKey(cmds[i]))
                {
                    list = curNode.m_dNode[cmds[i]];
                    curNode = list[0];
                    curNode.checkInited();
                }
                else
                    return null;
            }
            return Filer(list, filter);
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        public string getString(string key)
        {
            checkInited();
            if (m_dAtttr.ContainsKey(key))
                return m_dAtttr[key].str;
            return "null";
        }

        public float getFloat(string key)
        {
            checkInited();
            if (m_dAtttr.ContainsKey(key))
                return m_dAtttr[key].floatvalue;
            return -1f;
        }

        public int getInt(string key)
        {
            checkInited();
            if (m_dAtttr.ContainsKey(key))
                return m_dAtttr[key].intvalue;
            return -1;
        }

        public uint getUint(string key)
        {
            checkInited();
            if (m_dAtttr.ContainsKey(key))
                return (uint)m_dAtttr[key].uintvalue;
            return 0;
        }

        public bool hasValue(string key) => m_dAtttr.ContainsKey(key);
    }

    public class XMLMgr
    {
        public static XMLMgr instance = new XMLMgr();

        private Dictionary<string, SXML> m_allConf = new Dictionary<string, SXML>();

        public void init(ByteArray ba)
        {

            List<string> lStr = new List<string>();
            ba.position = 0;
            string str = "";
            while (ba.bytesAvailable > 4)
            {
                int len = ba.readInt();
                if (len == -1)
                    break;


                str = ba.readUTF8Bytes(len);

                lStr.Add(str);
            }
            SXML.lStr = lStr;

            while (ba.bytesAvailable > 4)
            {
                int id = ba.readInt();
                if (id == -1)
                    break;

                string xmlname = SXML.lStr[id];
                SXML xml = new SXML();
                int len = ba.readInt();

                ByteArray root = new ByteArray();
                ba.readBytes(root, ba.position, len);
                xml.m_root = root;
                m_allConf[xmlname] = xml;
            }
        }


        public List<SXML> GetSXMLList(string id, string filter = "")
        {
            string[] cmds = id.Split('.');

            if (cmds.Length < 1) return null;
            SXML curxml = null;
            if (m_allConf.ContainsKey(cmds[0]))
                curxml = m_allConf[cmds[0]];
            else
                return null;

            SXML curNode = curxml;

            curNode.checkInited();

          

            List<SXML> list = null;

            if (cmds.Length == 1)
            {
                 list = new List<SXML>();
                 list.Add(curNode);
            }

            for (int i = 1; i < cmds.Length; i++)
            {
                if (curNode.m_dNode.ContainsKey(cmds[i]))
                {
                    list = curNode.m_dNode[cmds[i]];
                    curNode = list[0];
                    curNode.checkInited();
                }
                else
                    return null;   
            }
            return SXML.Filer(list, filter);
        }

        public SXML GetSXML(string id, string filter = "")
        {
            List<SXML> l = GetSXMLList(id, filter);
            if (l == null || l.Count==0)
                return null;
            return l[0];
        }

        public void AddXmlData(string key, ref string data)
        {
            //XmlDocument xmlDoc = new XmlDocument();
            ////string xml = Encoding.UTF8.GetString(data);
            //xmlDoc.LoadXml(data);

            //m_allConf[key] = xmlDoc;
            ////Debug.Log("成功处理表 " + key);
        }
    }
}