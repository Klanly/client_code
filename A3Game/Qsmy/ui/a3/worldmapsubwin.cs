using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameFramework;
using UnityEngine;
using UnityEngine.UI;
using Cross;
namespace MuGame
{
    class worldmapsubwin : Window
    {
        public static int id;
        public static int mapid;
        public Text txtBt1;
        public Text txtBt2;
        public Text txt;

        bool isfree = false;
        private Variant vmap;
        private SXML xml;

        private int AREA;

        public int needMoney = 0;
        public override void init()
        {
            BaseButton bt1 = new BaseButton(getTransformByPath("bt0"));
            bt1.onClick = onfreeclick;
            BaseButton bt2 = new BaseButton(getTransformByPath("bt1"));
            bt2.onClick = onmoneyclick;
            txtBt1 = getComponentByPath<Text>("bt0/Text");
            txtBt1.text = ContMgr.getCont("worldmapsubwin_1");
            txtBt2 = getComponentByPath<Text>("bt1/Text");
            txt = getComponentByPath<Text>("desc");
            txt.text = ContMgr.getCont("worldmapsubwin_0");
            AREA = XMLMgr.instance.GetSXML("comm.worldmap").getInt("area");
            EventTriggerListener.Get(getGameObjectByPath("btclose")).onClick = onClose;
            getComponentByPath<Text>("Text").text = ContMgr.getCont("worldmapsubwin_2");
        }

        public override void onShowed()
        {
            isfree = PlayerModel.getInstance().vip >= 3;
            xml = XMLMgr.instance.GetSXML("mappoint.p", "id==" + id);
            vmap = SvrMapConfig.instance.getSingleMapConf(xml.getUint("mapid"));
            string name = vmap.ContainsKey("map_name") ? vmap["map_name"]._str : "--";
            txt.text = name;
            txtBt1.text = ContMgr.getCont("worldmap_bt1");
            transform.SetAsLastSibling();
            // if (isfree)
            //{
            //   txtBt2.text = ContMgr.getCont("worldmap_bt2");
            //   needMoney = 0;
            //}
            //else
            // {
            int basecost = xml.getInt("cost");
            needMoney = basecost / 10 * (int)((float)PlayerModel.getInstance().lvl / 10) + basecost;

            txtBt2.text = needMoney.ToString();
            //}
            if (a3_active.onshow)
            {
                if (a3_active_findbtu.instans?.monobj != null) {
                    a3_active_findbtu.instans.monobj.SetActive(false);
                }
            }

        }

        public override void onClosed()
        {
            id = 0;
            mapid = 0;
            if (a3_active.onshow)
            {
                if (a3_active_findbtu.instans?.monobj != null)
                {
                    a3_active_findbtu.instans.monobj.SetActive(true);
                }
            }
        }

        void onfreeclick(GameObject go)
        {
            Vector3 vec = new Vector3(xml.getFloat("ux"), 0f, xml.getFloat("uy"));
            SelfRole.WalkToMap(vmap["id"],vec);
            InterfaceMgr.getInstance().close(InterfaceMgr.WORLD_MAP);
            InterfaceMgr.getInstance().close(InterfaceMgr.WORLD_MAP_SUB);
            if (a3_active.onshow)
                InterfaceMgr.getInstance().close(InterfaceMgr.A3_ACTIVE);
        }


        public static void onCD(cd item)
        {
            int temp = (int)(cd.secCD - cd.lastCD) / 100;
            item.txt.text = ContMgr.getCont("worldmap_cd", ((float)temp / 10f).ToString());

        }

        void onClose(GameObject go)
        {
            InterfaceMgr.getInstance().close(InterfaceMgr.WORLD_MAP_SUB);

        }
        int toid = 0;
        void onmoneyclick(GameObject go)
        {
            if (!FindBestoModel.getInstance().Canfly) //宝图活动战斗状态禁止传送
            {
                flytxt.instance.fly(FindBestoModel.getInstance().nofly_txt);
                InterfaceMgr.getInstance().close(InterfaceMgr.WORLD_MAP);
                InterfaceMgr.getInstance().close(InterfaceMgr.WORLD_MAP_SUB);
                return;
            }
            if (PlayerModel.getInstance().money < needMoney)
            {
                flytxt.instance.fly(ContMgr.getCont("comm_nomoney"));
            }
            else
            {
                float dis = Vector3.Distance(SelfRole._inst.m_curModel.position, new Vector3(xml.getFloat("ux"), SelfRole._inst.m_curModel.position.y, xml.getFloat("uy")));
                //  debug.Log(":::::" + dis);
                toid = id;
                if (mapid == GRMap.instance.m_nCurMapID && dis < AREA)
                {
                    GameObject goeff = GameObject.Instantiate(worldmap.EFFECT_CHUANSONG1) as GameObject;
                    goeff.transform.SetParent(SelfRole._inst.m_curModel, false);
                    MsgBoxMgr.getInstance().showConfirm(ContMgr.getCont("worldmap_tooclose"), () =>
                    {
                        cd.updateHandle = onCD;
                        cd.show(() =>
                        {
                            MapProxy.getInstance().sendBeginChangeMap(toid, true);
                        }, 2.8f,false,
                         () =>
                         {
                             Destroy(goeff);
                         }
                        );
                        InterfaceMgr.getInstance().close(InterfaceMgr.WORLD_MAP);
                        InterfaceMgr.getInstance().close(InterfaceMgr.WORLD_MAP_SUB);
                        if (a3_active.onshow)
                            InterfaceMgr.getInstance().close(InterfaceMgr.A3_ACTIVE);
                    });
                }
                else
                {
                    cd.updateHandle = onCD;
                    GameObject goeff = GameObject.Instantiate(worldmap.EFFECT_CHUANSONG1) as GameObject;
                    goeff.transform.SetParent(SelfRole._inst.m_curModel, false);
                    cd.show(() =>
                    {
                        MapProxy.getInstance().sendBeginChangeMap(toid, true);
                    }, 2.8f,false,
                    () =>
                    {
                        Destroy(goeff);
                    }
                    );

                    InterfaceMgr.getInstance().close(InterfaceMgr.WORLD_MAP);
                    InterfaceMgr.getInstance().close(InterfaceMgr.WORLD_MAP_SUB);
                    if(a3_active.onshow)
                        InterfaceMgr.getInstance().close(InterfaceMgr.A3_ACTIVE);
                }

            }



            //InterfaceMgr.getInstance().close(InterfaceMgr.WORLD_MAP);
            //InterfaceMgr.getInstance().close(InterfaceMgr.WORLD_MAP_SUB);
        }

    }
}
