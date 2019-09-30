using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameFramework;
using Cross;

namespace MuGame
{
    //class LGDropItemManager 
    //{
    //    static muNetCleint g_mgr;
    //    static Dictionary<int, DropItem> _dropItemDic = new Dictionary<int, DropItem>();
    //    public static void into(muNetCleint m)
    //    {
    //        g_mgr = m;
    //        g_mgr.g_processM.addProcess(processStruct.create(updateProcess));
    //    }

    //    public static DropItem getDropItem(int id)
    //    {
    //        if (_dropItemDic.ContainsKey(id))
    //            return _dropItemDic[id];

    //        return null;
    //    }

    //    /// <summary>
    //    /// 创建掉落物品
    //    /// </summary>
    //    /// <param name="item">物品列表</param>
    //    public static  void createDropItem(params DropItem[] item)
    //    {
    //        createDropItem(new List<DropItem>(item));
    //    }

    //    public static void createDropItem(List<DropItem> items)
    //    {
    //        if (items == null || items.Count == 0)
    //            return;

    //        foreach (var i in items)
    //        {
    //            if (_dropItemDic.ContainsKey(i.dpid))
    //                continue;

    //            string qz = "";
    //            string hz = "";
    //            string color = "00FF00";
    //            string name = "";

    //            i.left_tm += (g_mgr.g_netM as muNetCleint).CurServerTimeStampMS;
    //            Variant v = null;
    //            if (i.gold > 0)
    //            {
    //                v = GraphManager.singleton.getCharacterConf("101");// ceshi
    //                i.igre = g_mgr.g_sceneM.world.createEntity(Define.GREntityType.STATIC_MESH);

    //                qz = i.gold.ToString();
    //                name = LanguagePack.getLanguageText("common", "gld");
    //                name = DebugTrace.Printf( name, qz);
    //            }
    //            else if (i.itm != null)
    //            {
    //                v = (g_mgr.g_gameConfM as muCLientConfig).localItems.get_item_conf(i.itm["id"]._uint);
    //                if (v == null)
    //                {
    //                    DebugTrace.print("度配表出错 tpid:" + i.dpid);
    //                    continue;
    //                }

    //                if (v.ContainsKey("show3D"))
    //                {
    //                    v = GraphManager.singleton.getCharacterConf(v["show3D"][0]["cid"]._str);
    //                    i.igre = g_mgr.g_sceneM.world.createEntity(Define.GREntityType.STATIC_MESH);
    //                }
    //                else
    //                {
    //                    v["file"] = v["icon"]._str;
    //                    i.igre = g_mgr.g_sceneM.world.createEntity(Define.GREntityType.BILLBOARD);
    //                }

    //                if (i.itm.ContainsKey("cnt") && i.itm["cnt"]._int> 1)
    //                     hz = "* " + i.itm["cnt"]._int.ToString() ;

    //                name = LanguagePack.getLanguageText("items_xml", i.itm["id"]._str);
    //                name = qz + name + hz;
    //            }
    //            else if (i.eqp != null)
    //            {
    //                v = (g_mgr.g_gameConfM as muCLientConfig).localItems.get_item_conf(i.eqp["id"]._uint);
    //                if (v == null)
    //                {
    //                    DebugTrace.print("度配表出错 tpid:" + i.dpid);
    //                    continue;
    //                }

    //                if (v.ContainsKey("show3D"))
    //                {
    //                    v = GraphManager.singleton.getCharacterConf(v["show3D"][0]["cid"]._str);
    //                    i.igre = g_mgr.g_sceneM.world.createEntity(Define.GREntityType.STATIC_MESH);
    //                }
    //                else
    //                {
    //                    v["file"] = v["icon"]._str;
    //                    i.igre = g_mgr.g_sceneM.world.createEntity(Define.GREntityType.BILLBOARD);
    //                }
                     
    //                if (i.eqp.ContainsKey("exatt") && i.eqp["exatt"]._bool)
    //                    qz += "(卓越`)";

    //                if (i.eqp.ContainsKey("flvl") && i.eqp["flvl"]._int > 0)
    //                    hz = " Lv" + i.eqp["flvl"]._int;
                     
    //                 name = LanguagePack.getLanguageText("items_xml", i.eqp["id"]._str);
    //                 name = qz + name + hz;
    //            }
    //            else
    //                continue;


    //            if (v == null)
    //            {
    //                DebugTrace.print("度配表出错 tpid:" + i.dpid);
    //                continue;
    //            }

    //            i.igre.load(v);
    //            i.igre.x = i.x;
    //            i.igre.z = i.y;
    //            i.igre.y = (float)g_mgr.g_sceneM.getZ(i.x * GameConstant.GEZI, i.y * GameConstant.GEZI);

    //            i.setClickFun(g_mgr.g_sceneM.getGraphCamera());
    //            i.setName(name, color);
    //            _dropItemDic[i.dpid] = i;
    //        }
    //    }

    //    /// <summary>
    //    /// 移除掉落物品
    //    /// </summary>
    //    /// <param name="dpid">物品ID</param>
    //    public static  void romoveDropItem(int dpid)
    //    {
    //        if (_dropItemDic.ContainsKey(dpid))
    //        {
    //            g_mgr.g_sceneM.world.deleteEntity( _dropItemDic[dpid].igre.id);
    //            _dropItemDic[dpid].dispose();
    //            _dropItemDic.Remove(dpid);
    //        }
    //    }

    //    public static void romoveDropItemAll()
    //    {
    //        foreach (var id in _dropItemDic.Keys)
    //            romoveDropItem(id);
    //    }

    //    public static void updateProcess(float tmSlice)
    //    {
    //        if (_dropItemDic == null || _dropItemDic.Count == 0) return;

    //        long time = g_mgr.g_netM.CurServerTimeStampMS;
    //        List<DropItem> itemlist = new List<DropItem>(_dropItemDic.Values);
    //        List<DropItem> list = itemlist.FindAll(delegate(DropItem i) { return (i.left_tm <= time) || ((int)((g_mgr.g_gameM as muLGClient).g_selfPlayer.x / (GameConstant.GEZI / 1.2f)) == (int)i.x && (int)((g_mgr.g_gameM as muLGClient).g_selfPlayer.y / (GameConstant.GEZI / 1.2f)) == (int)i.y); });
    //        if (list != null)
    //        {
    //            foreach (DropItem it in list)
    //            {
    //                if (it.left_tm <= time)
    //                    romoveDropItem(it.dpid);
    //                else if (it._fun != null)
    //                    it._fun(it.dpid);
    //                else
    //                    DebugTrace.print("Error: DropItem dpid:" + it.dpid);
    //            }
    //        }

    //        foreach (var it in itemlist)
    //        {
    //            it.setNamePos();
    //        }
    //    }


    //}

    ////class DropItem
    ////{
    ////    private int _dpid;
    ////    private long _left_tm;
    ////    private float _x;
    ////    private float _y;
    ////    private Variant _eqp;
    ////    private Variant _itm;
    ////    private int _gold;
    ////    private IGREntity _igre;
    ////    private IUIText _name;
    ////    public Action<int> _fun;

    ////    public DropItem(int dpid, long left_tm, float x, float y, Action<int> fun, int gold , Variant eqp = null, Variant itm = null)
    ////    {
    ////        _dpid = dpid;
    ////      //  _left_tm = (this.m_mgr.g_netM as muNetCleint).CurServerTimeStampMS + left_tm;
    ////        _left_tm =  left_tm;
    ////        _x = x;
    ////        _y = y;
    ////        _eqp = eqp;
    ////        _itm = itm;
    ////        _gold = gold;
    ////        _fun = fun;
    ////    }

    ////    public DropItem(Variant v, Action<int> fun)
    ////    {
    ////        _dpid = v["dpid"]._int;
    ////        _left_tm = v["left_tm"]._int64 * 1000;
    ////        _x = (float)GameTools.inst.pixelToUnit(v["x"]._double * GameConstant.GEZI);
    ////        _y = (float)GameTools.inst.pixelToUnit(v["y"]._double * GameConstant.GEZI);
    ////        _eqp = v.ContainsKey("eqp") ? v["eqp"] : null;
    ////        _itm = v.ContainsKey("itm") ? v["itm"] : null;
    ////        _gold = v.ContainsKey("gold") ? v["gold"]._int : 0;
    ////        _fun = fun;
    ////    }

    ////    public int dpid
    ////    {
    ////        get { return _dpid; }
    ////    }

    ////    public long left_tm
    //    {
    //        get { return _left_tm; }
    //        set { _left_tm = value; }
    //    }

    //    public float x
    //    {
    //        get { return _x; }
    //    }

    //    public float y
    //    {
    //        get { return _y; }
    //    }

    //    public Variant eqp
    //    {
    //        get { return _eqp; }
    //    }

    //    public Variant itm
    //    {
    //        get { return _itm; }
    //    }

    //    public int gold
    //    {
    //        get { return _gold; }
    //    }

    //    public IGREntity igre
    //    {
    //        set { _igre = value; }
    //        get { return _igre; }
    //    }

    //    GRCamera3D _camera;
    //    public void setClickFun(GRCamera3D camera)
    //    {
    //        _camera = camera;
    //        os.sys.addGlobalEventListener(Define.EventType.MOUSE_DOWN, _onClickColl);
    //    }

    //    public void dispose()
    //    {
    //        _name.dispose();
    //        os.sys.removeGlobalEventListener(Define.EventType.MOUSE_DOWN, _onClickColl);
    //    }

    //    void _onClickColl(Event e)
    //    {
    //    }

    //    public void setName( string name, string color)
    //    {
    //        //_name = UIManager.singleton.createControl("text", "text_chaTitle") as IUIText;
    //        //_name.text = name;
    //        //_name.color = color;
    //        //_name.fontSize = 18;
    //        //setNamePos();
    //        //_name.visible = true;
    //    }

    //    public void setNamePos()
    //    {
    //        Vec3 v = _name.changePot(new Vec3(igre.x, igre.y + 2.6f, igre.z));
    //        _name.x = v.x;
    //        _name.y = CrossApp.singleton.height - v.y;
    //    }

    //}
}
