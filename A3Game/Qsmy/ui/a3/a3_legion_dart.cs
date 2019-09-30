using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
using System.Linq;
using System.Text;
using GameFramework;
using UnityEngine;
using UnityEngine.UI;
using Cross;
namespace MuGame
{
    class a3_legion_dart : Window
    {
        public static a3_legion_dart instance;


        string str = ContMgr.getCont("a3_legion_dart_ys");
        string city = "candodart/scroll_view_dart/contain/UndergroundCity/enter/Text";
        string dark = "candodart/scroll_view_dart/contain/darkPalace/enter/Text";
        string wind = "candodart/scroll_view_dart/contain/coldWind/enter/Text";
        int legionLvl, length, one, three, five;
        SXML ss;
        List<SXML> listXml = new List<SXML>();
        Dictionary<int, clans> dicClan = new Dictionary<int, clans>();
        public bool isme = false;//判断是不是我开启活动的
        uint item_id;
        public bool isopen = false,ltes=false;
        public override void init()
        {
            inText();
            instance = this;
            #region ====主界面初始化====
            ss = XMLMgr.instance.GetSXML("clan_escort");
            listXml = ss.GetNodeList("line");
            length = listXml.Count;

           
            for (int i = 0; i < length; i++)
            {
                clans cla = new clans();
                cla.open_lv_clan = listXml[i].getInt("clan_lvl");
                cla.pathid = listXml[i].getUint("id");
                cla.target_map = listXml[i].getUint("target_map");
                cla.add_money_num = listXml[i].getInt("clan_money");
                cla.item_id = listXml[i].getUint("item_id");
                cla.item_num = listXml[i].getInt("item_num");
                if (!dicClan.ContainsKey(listXml[i].getInt("id")))
                    dicClan.Add(listXml[i].getInt("id"), cla);
            }
            one = dicClan[1].open_lv_clan;
            three = dicClan[2].open_lv_clan;
            five = dicClan[3].open_lv_clan;
            //Variant list = SvrMapConfig.instance.getSingleMapConf(dicClan[1].target_map);
            //getTransformByPath("candodart/scroll_view_dart/contain/UndergroundCity/bg/title").GetComponent<Text>().text = list["map_name"];
            //list = SvrMapConfig.instance.getSingleMapConf(dicClan[2].target_map);
            //getTransformByPath("candodart/scroll_view_dart/contain/darkPalace/bg/title").GetComponent<Text>().text = list["map_name"];
            //list = SvrMapConfig.instance.getSingleMapConf(dicClan[3].target_map);
            //getTransformByPath("candodart/scroll_view_dart/contain/coldWind/bg/title").GetComponent<Text>().text = list["map_name"];
            string stri = "candodart/scroll_view_dart/contain";
            List<SXML> lisx = XMLMgr.instance.GetSXMLList("item.item", "id==" + dicClan[1].item_id);
            item_id = dicClan[1].item_id;
            getTransformByPath(stri + "/UndergroundCity/award/2/icon").GetComponent<Image>().sprite = GAMEAPI.ABUI_LoadSprite("icon_item_" + lisx[0].getInt("icon_file"));
            getTransformByPath(stri + "/UndergroundCity/award/1/icon").GetComponent<Image>().sprite = GAMEAPI.ABUI_LoadSprite("icon_comm_1x1");
            getTransformByPath(stri + "/UndergroundCity/award/1/Text").GetComponent<Text>().text = dicClan[1].add_money_num.ToString();
            getTransformByPath(stri + "/UndergroundCity/award/2/Text").GetComponent<Text>().text = dicClan[1].item_num.ToString();
            new BaseButton(getTransformByPath(stri + "/UndergroundCity/award/2")).onClick = (GameObject go) =>
             {
                 ArrayList arr = new ArrayList();
                 arr.Add(dicClan[1].item_id);
                 arr.Add(1);
                 InterfaceMgr.getInstance().ui_async_open(InterfaceMgr.A3_MINITIP, arr);
             };
            lisx = XMLMgr.instance.GetSXMLList("item.item", "id==" + dicClan[2].item_id);
            item_id = dicClan[2].item_id;
            getTransformByPath(stri + "/darkPalace/award/2/icon").GetComponent<Image>().sprite = GAMEAPI.ABUI_LoadSprite("icon_item_" + lisx[0].getInt("icon_file"));
            getTransformByPath(stri + "/darkPalace/award/1/icon").GetComponent<Image>().sprite = GAMEAPI.ABUI_LoadSprite("icon_comm_1x1");
            getTransformByPath(stri + "/darkPalace/award/1/Text").GetComponent<Text>().text = dicClan[2].add_money_num.ToString();
            getTransformByPath(stri + "/darkPalace/award/2/Text").GetComponent<Text>().text = dicClan[2].item_num.ToString();
            new BaseButton(getTransformByPath(stri + "/darkPalace/award/2")).onClick = (GameObject go) =>
            {
                ArrayList arr = new ArrayList();
                arr.Add(dicClan[2].item_id);
                arr.Add(1);
                InterfaceMgr.getInstance().ui_async_open(InterfaceMgr.A3_MINITIP, arr);
            };
            lisx = XMLMgr.instance.GetSXMLList("item.item", "id==" + dicClan[3].item_id);
            item_id = dicClan[3].item_id;
            getTransformByPath(stri + "/coldWind/award/2/icon").GetComponent<Image>().sprite = GAMEAPI.ABUI_LoadSprite("icon_item_" + lisx[0].getInt("icon_file"));
            getTransformByPath(stri + "/coldWind/award/1/icon").GetComponent<Image>().sprite = GAMEAPI.ABUI_LoadSprite("icon_comm_1x1");
            getTransformByPath(stri + "/coldWind/award/1/Text").GetComponent<Text>().text = dicClan[3].add_money_num.ToString();
            getTransformByPath(stri + "/coldWind/award/2/Text").GetComponent<Text>().text = dicClan[3].item_num.ToString();
            new BaseButton(getTransformByPath(stri + "/coldWind/award/2")).onClick = (GameObject go) =>
            {
                ArrayList arr = new ArrayList();
                arr.Add(dicClan[3].item_id);
                arr.Add(1);
                InterfaceMgr.getInstance().ui_async_open(InterfaceMgr.A3_MINITIP, arr);
            };
            #endregion

            #region  ====button====
            new BaseButton(getTransformByPath("candodart/btn_close")).onClick = (GameObject go) =>
              {
                  InterfaceMgr.getInstance().close(InterfaceMgr.A3_LEGION_DART);
              };
            new BaseButton(getTransformByPath("bg")).onClick = (GameObject go) =>
            {
                InterfaceMgr.getInstance().close(InterfaceMgr.A3_LEGION_DART);
            };
            new BaseButton(getTransformByPath("cantdart/close")).onClick = (GameObject go) =>
            {
                InterfaceMgr.getInstance().close(InterfaceMgr.A3_LEGION_DART);
            };
            new BaseButton(getTransformByPath("cantdart/bg/back")).onClick = (GameObject go) =>
            {
                InterfaceMgr.getInstance().close(InterfaceMgr.A3_LEGION_DART);
            };

            new BaseButton(getTransformByPath("cantdart/bg/go")).onClick = (GameObject go) =>
            {
                if ((int)PlayerModel.getInstance().mapid == 10)
                {
                    //SelfRole.moveToNPc(10, 1003);
                }
                else
                {
                    SelfRole.Transmit(10 * 100 + 1);
                }
                ltes = true;
                InterfaceMgr.getInstance().close(InterfaceMgr.A3_LEGION_DART);
            };

            new BaseButton(getTransformByPath("candodart/scroll_view_dart/contain/UndergroundCity/enter")).onClick = (GameObject go) =>
              {
                  if(!a3_dartproxy.getInstance().canOpenDart)
                  {
                      flytxt.instance.fly(ContMgr.getCont("clan_12"));return;
                  }
                  else
                  {
                      if (A3_LegionModel.getInstance().myLegion.clanc < 3)//领袖为4,元老为3
                      {
                          flytxt.instance.fly(ContMgr.getCont("clan_10"));
                          InterfaceMgr.getInstance().close(InterfaceMgr.A3_LEGION_DART);
                      }
                      else
                      {
                          a3_dartproxy.getInstance().sendDartStart(dicClan[1].pathid);
                          a3_dartproxy.getInstance().isme = true; ltes = true;//ltes要改成服务器发送
                          InterfaceMgr.getInstance().close(InterfaceMgr.A3_LEGION_DART);
                      }
                  }
              };
            new BaseButton(getTransformByPath("candodart/scroll_view_dart/contain/darkPalace/enter")).onClick = (GameObject go) =>
            {
                if (!a3_dartproxy.getInstance().canOpenDart)
                {
                    flytxt.instance.fly(ContMgr.getCont("clan_12")); return;
                }
                else
                {
                    if (A3_LegionModel.getInstance().myLegion.clanc < 3 || A3_LegionModel.getInstance().myLegion.lvl < three)//领袖为4,元老为3
                    {
                        flytxt.instance.fly(ContMgr.getCont("clan_10"));
                        InterfaceMgr.getInstance().close(InterfaceMgr.A3_LEGION_DART);
                    }
                    else if (A3_LegionModel.getInstance().myLegion.clanc >= 3 && A3_LegionModel.getInstance().myLegion.lvl >= three)
                    {
                        a3_dartproxy.getInstance().sendDartStart(dicClan[2].pathid);
                        a3_dartproxy.getInstance().isme = true; ltes = true;
                        InterfaceMgr.getInstance().close(InterfaceMgr.A3_LEGION_DART);
                    }
                }
            };
            new BaseButton(getTransformByPath("candodart/scroll_view_dart/contain/coldWind/enter")).onClick = (GameObject go) =>
            {
                if (!a3_dartproxy.getInstance().canOpenDart)
                {
                    flytxt.instance.fly(ContMgr.getCont("clan_12")); return;
                }
                else
                {
                    if (A3_LegionModel.getInstance().myLegion.clanc < 3 || A3_LegionModel.getInstance().myLegion.lvl < five)//领袖为4,元老为3
                    {
                        flytxt.instance.fly(ContMgr.getCont("clan_10"));
                        InterfaceMgr.getInstance().close(InterfaceMgr.A3_LEGION_DART);
                    }
                    else if (A3_LegionModel.getInstance().myLegion.clanc >= 3 && A3_LegionModel.getInstance().myLegion.lvl >= five)
                    {
                        a3_dartproxy.getInstance().sendDartStart(dicClan[3].pathid);
                        a3_dartproxy.getInstance().isme = true; ltes = true;
                        InterfaceMgr.getInstance().close(InterfaceMgr.A3_LEGION_DART);
                    }
                }
            };
            #endregion
            if (a3_dartproxy.getInstance().show2)
            {
                getGameObjectByPath("candodart").SetActive(false);
                getGameObjectByPath("cantdart").SetActive(true);
            }
            #region ====事件监听====
            A3_LegionProxy.getInstance().addEventListener(A3_LegionProxy.EVENT_CREATE, creatLegion);
            A3_LegionProxy.getInstance().addEventListener(A3_LegionProxy.EVENT_LVUP, upLegion);
            a3_dartproxy.getInstance().addEventListener(a3_dartproxy.EVENT_GETINFO, info);
            #endregion
        }
        public override void onShowed()
        {
            a3_dartproxy.getInstance().sendDartGo();//查看军团镖车信息
            legionLvl = A3_LegionModel.getInstance().myLegion.lvl;
            InterfaceMgr.getInstance().changeState(InterfaceMgr.STATE_FUNCTIONBAR);
            initText(legionLvl);
        }
        void inText()
        {
            this.transform.FindChild("candodart/ziyuan/notice").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_legion_dart_1");//请根据军团实力选择押送镖车目的地：
            this.transform.FindChild("candodart/scroll_view_dart/contain/UndergroundCity/time").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_legion_dart_2");//耗时：
            this.transform.FindChild("candodart/scroll_view_dart/contain/UndergroundCity/time/Text").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_legion_dart_3");//10分钟
            this.transform.FindChild("candodart/scroll_view_dart/contain/UndergroundCity/lvl").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_legion_dart_4");//玩家等级推荐：
            this.transform.FindChild("candodart/scroll_view_dart/contain/UndergroundCity/award").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_legion_dart_5");//最高奖励：
            this.transform.FindChild("candodart/scroll_view_dart/contain/UndergroundCity/enter/Text").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_legion_dart_6");//押 送
            this.transform.FindChild("candodart/scroll_view_dart/contain/darkPalace/enter/Text").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_legion_dart_6");//押 送
            this.transform.FindChild("candodart/scroll_view_dart/contain/darkPalace/time").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_legion_dart_2");//耗时：
            this.transform.FindChild("candodart/scroll_view_dart/contain/darkPalace/time/Text").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_legion_dart_7");//15分钟
            this.transform.FindChild("candodart/scroll_view_dart/contain/darkPalace/lvl").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_legion_dart_4");//玩家等级推荐：
            this.transform.FindChild("candodart/scroll_view_dart/contain/darkPalace/award").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_legion_dart_5");//最高奖励：
        }

        public override void onClosed()
        {
            InterfaceMgr.getInstance().changeState(InterfaceMgr.STATE_NORMAL);
        }

        private void creatLegion(GameEvent e)
        {
            getTransformByPath(city).GetComponent<Text>().text = str;
            //
            //
        }
        private void upLegion(GameEvent e)
        {
            legionLvl = A3_LegionModel.getInstance().myLegion.lvl;
            initText(legionLvl);
        }
        private void info(GameEvent e)
        {
            isopen = e.data["finish"];
            if (isopen)     
            {
                a3_liteMinimap.instance.getGameObjectByPath("goonDart").SetActive(false);
            }
        }

        private void getNum(GameEvent e)
        {
            List<SXML> list = XMLMgr.instance.GetSXMLList("item.item", "id==" + item_id);
            string stri = list[0].getString("item_name");
            flytxt.instance.fly(ContMgr.getCont("clan_11", e.data["item_num"]) +stri);
        }
        void initText(int lvl)
        {
            if (lvl >= 1 && lvl < 3)
            {
                getTransformByPath(city).GetComponent<Text>().text = str;
                getTransformByPath(dark).GetComponent<Text>().text = three + ContMgr.getCont("a3_legion_dart_jtstart");
                getTransformByPath(wind).GetComponent<Text>().text = five + ContMgr.getCont("a3_legion_dart_jtstart");
                getTransformByPath("candodart/scroll_view_dart/contain/darkPalace/enter").GetComponent<Button>().interactable = false;
                getTransformByPath("candodart/scroll_view_dart/contain/coldWind/enter").GetComponent<Button>().interactable = false;
            }
            if (lvl <= 4 && lvl >= 3)
            {
                getTransformByPath(city).GetComponent<Text>().text = str;
                getTransformByPath(dark).GetComponent<Text>().text = str;
                getTransformByPath(wind).GetComponent<Text>().text = five + ContMgr.getCont("a3_legion_dart_jtstart");
                getTransformByPath("candodart/scroll_view_dart/contain/darkPalace/enter").GetComponent<Button>().interactable = true;
                getTransformByPath("candodart/scroll_view_dart/contain/coldWind/enter").GetComponent<Button>().interactable = false;
            }
            else if (lvl <= 6 && lvl >= 5)
            {
                getTransformByPath(city).GetComponent<Text>().text = str;
                getTransformByPath(dark).GetComponent<Text>().text = str;
                getTransformByPath(wind).GetComponent<Text>().text = str;
                getTransformByPath("candodart/scroll_view_dart/contain/darkPalace/enter").GetComponent<Button>().interactable = true;
                getTransformByPath("candodart/scroll_view_dart/contain/coldWind/enter").GetComponent<Button>().interactable = true;
            }
            else if (lvl > 6)//留给后面要继续加的镖车等级
            {
                getTransformByPath(city).GetComponent<Text>().text = str;
                getTransformByPath(dark).GetComponent<Text>().text = str;
                getTransformByPath(wind).GetComponent<Text>().text = str;
                getTransformByPath("candodart/scroll_view_dart/contain/darkPalace/enter").GetComponent<Button>().interactable = true;
                getTransformByPath("candodart/scroll_view_dart/contain/coldWind/enter").GetComponent<Button>().interactable = true;
            }

        }
    }
}
public class clans
{
    public int open_lv_clan;//开启的军团等级
    public uint pathid;//相应path的id
    public uint target_map;//目标地图的mapid
    public int add_money_num;//相应加的军团资金
    public uint item_id;//奖励物品的id
    public int item_num;//奖励物品的数量

}

