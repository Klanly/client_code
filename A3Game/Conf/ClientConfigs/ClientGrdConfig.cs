using System;
using GameFramework;
using Cross;
using System.IO;

using System.Collections.Generic;

namespace MuGame
{
    public class ClientGrdConfig : configParser
    {
        protected Dictionary<string, List<string>> m_map = new Dictionary<string, List<string>>();
        protected List<string> m_list;
        
        //protected int m_width;
        //protected int m_height;

        public float[] m_hdtdata = null;
        //protected int m_grayWidth;
        //protected int m_grayHeight;
        
        protected short[] m_grd;
        protected float m_min;
        protected float m_max;
        public float m_hdt_z = 0.01f;

        public ClientGrdConfig(ClientConfig m)
            : base(m)
        {

        }

        public static IObjectPlugin create(IClientBase m)
        {
            return new ClientGrdConfig(m as ClientConfig);
        }

        //public int width
        //{
        //    get { return m_width; }
        //}

        //public int height
        //{
        //    get { return m_height; }
        //}

        //public float[] map_hdt
        //{
        //    get { return m_hdtdata; }
        //}

        //public int grayWidth
        //{
        //    get { return m_grayWidth;  }
        //}

        //public int grayHeight
        //{
        //    get { return m_grayHeight; }
        //}

        public short[] grd
        {
            get { return m_grd; }
        }

        public void get_value(string id, Action onfin)
        {
            resolve(id);

            if (!m_map.ContainsKey(id))
            {
                GameTools.PrintCrash( "map grd file load err!" );
				onfin();
                return;
            }

            List<string> l = m_map[id];
            dataGrd(l[0], () =>
            {
                dataHdt(l[1], () =>
                {
                    onfin();
                });
            });
        }

        public void dataGrd(string path, Action onfin)
        {
            IURLReq urlreqimpl = m_mgr.g_netM.getUrlImpl();
            urlreqimpl.url = path;
			urlreqimpl.dataFormat = "binary";//text, binary, assetbundle

            debug.Log("处理地图的阻挡数据，数据的文件位置" + path);

            urlreqimpl.load(
				(IURLReq r, object ret) =>
				{
					byte[] data = ret as byte[];

                    //解压地图阻挡数据
                    ByteArray msk_data = new ByteArray(data);
                    msk_data.uncompress();

                    m_grd = new short[msk_data.length / 2];
                    int n = 0;
                    while (msk_data.bytesAvailable > 0)
                    {
                        m_grd[n++] = msk_data.readShort();// br.ReadInt16();
                    }

                    msk_data.clear();

					onfin();
				},
				( IURLReq url, float prog )=>{
					Variant v = new Variant();
					v["tp"] = GameConstant.LOADING_MAP_GRD;
					v["prog"] = prog;

					GameTools.PrintNotice( "LOADING_MAP_GRD load prog:" + prog );
					
					m_mgr.g_gameM.dispatchEvent(
						GameEvent.Createimmedi( GAME_EVENT.GAME_LOADING, this, v )
					);					 
				},
				( IURLReq url, string err )=>{
					Variant v = new Variant();
					v["tp"] = GameConstant.LOADING_MAP_GRD;
					v["err"] = err;
					m_mgr.g_gameM.dispatchEvent(
						GameEvent.Createimmedi( GAME_EVENT.GAME_LOADING, this, v )
					);		
				}
			);
        }

        public void dataHdt(string path, Action onfin)
        {
            if ("null" == path)
            {
                debug.Log("此地图不需要处理，地表的高度信息");
                m_hdtdata = null;
                onfin();
                return;
            }

            IURLReq urlreqimpl = m_mgr.g_netM.getUrlImpl();
            urlreqimpl.url = path;
			urlreqimpl.dataFormat = "binary";

            debug.Log("处理地图的高度数据，数据的文件位置" + path);

            urlreqimpl.load((IURLReq r, object ret) =>
            {
                //old 久的高度图处理
                //byte[] data = ret as byte[];
                //BinaryReader br = new BinaryReader(new MemoryStream(data));
                //char[] magic = br.ReadChars(4);
                //if (magic[0] != 'C' || magic[1] != 'R' || magic[2] != 'H' || magic[3] != 'M')
                //{
                //    DebugTrace.add(Define.DebugTrace.DTT_ERR, "Could not support hdt file anymore, please use CRHM: " + path);
                //    return;
                //}

                //ushort ver = br.ReadUInt16();
                //if (ver < 2)
                //{
                //    DebugTrace.add(Define.DebugTrace.DTT_ERR, "Could not support old version CRHM file: " + path);
                //    return;
                //}

                //this.m_grayWidth = (int)br.ReadUInt16();
                //this.m_grayHeight = (int)br.ReadUInt16();

                //int len = m_grayWidth * m_grayHeight;
                //this.m_gray = new byte[len];
                //br.Read(this.m_gray, 0, len);

                ////this.m_width = m_grayWidth / 5;
                ////this.m_height = m_grayHeight / 5;

                //onfin();


                byte[] data = ret as byte[];
                //解压地图阻挡数据
                ByteArray hdt_data = new ByteArray(data);
                hdt_data.uncompress();

                m_hdtdata = new float[hdt_data.length / 4];
                int n = 0;
                while (hdt_data.bytesAvailable > 0)
                {
                    m_hdtdata[n++] = hdt_data.readFloat();// br.ReadInt16();
                }

                hdt_data.clear();

                onfin();
            });
        }
        
        public float min
        {
            get { return m_min; }
        }

        public float max
        {
            get { return m_max; }
        }

        public void resolve(string id)
        {
            Variant conf1 = this.m_mgr.g_sceneM.getMapInfo;
            for (int i = 0; i < conf1.Count; i++)
            {
                if (conf1[i]["id"]._str == id)
                {
                    m_min = conf1[i]["min"];
                    m_max = conf1[i]["max"];
                    m_hdt_z = conf1[i]["hdt_z"];

                    if (!m_map.ContainsKey(id))
                    {
                        m_list = new List<string>();
                        string file1 = conf1[i]["mskfile"];
                        m_list.Add(file1);

                        //没有高度信息就用统一的高度进行行走
                        if (conf1[i].ContainsKey("hdtfile"))
                        {
                            string file2 = conf1[i]["hdtfile"];
                            m_list.Add(file2);
                        }
                        else
                        {
                            m_list.Add("null");
                        }

                        m_map[id] = m_list;
                    }
                }
            }
        }
    }
}
