using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameFramework;
using UnityEngine;
using UnityEngine.UI;

namespace MuGame
{
    class a3_active_godlight : Window
    {
        Text time_txt;
        Text go_txt;
        int map_x;
        int map_y;
        int map_id;

        public override void init()
        {
            BaseButton btn_close = new BaseButton(transform.FindChild("btn_close"));
            btn_close.onClick = onclose;

            BaseButton btn_go= new BaseButton(transform.FindChild("text_go"));
            btn_go.onClick = onGo;
            time_txt = transform.FindChild("time").GetComponent<Text>();
            go_txt = transform.FindChild("text_go").GetComponent<Text>();
            inText();
        }
        void inText()
        {
            this.transform.FindChild("desc").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_active_godlight_1");
            this.transform.FindChild("text").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_active_godlight_2");
        }
        public override void onShowed()
        {
            InterfaceMgr.getInstance().changeState(InterfaceMgr.STATE_FUNCTIONBAR);
            map_x = 0;
            map_y = 0;
            map_id = 1;

            string map_name = "";

            SXML s_xml = XMLMgr.instance.GetSXML("god_light");
            List<SXML> s_xml_info = s_xml.GetNodeList("player_info");
            if (s_xml_info != null)
            {
                foreach (SXML x in s_xml_info)
                {
                    if (x.getInt("zhuan") < PlayerModel.getInstance().up_lvl)
                    {

                    }
                    else if (x.getInt("zhuan") == PlayerModel.getInstance().up_lvl)
                    {
                        if (x.getInt("lv") > PlayerModel.getInstance().lvl)
                            break;
                    }
                    else
                    {
                        break;
                    }
                    map_name = x.getString("map_name");
                    map_id = x.getInt("map_id");
                    map_x = x.getInt("map_x");
                    map_y = x.getInt("map_y");
                }
            }

            //go_txt.text = "推荐前往 " + map_name + " 挂机>>";
            go_txt.text = ContMgr.getCont("a3_active_godlight", new List<string> { map_name });
        }
        public override void onClosed()
        {
            InterfaceMgr.getInstance().changeState(InterfaceMgr.STATE_NORMAL);
        }
        void onclose(GameObject go)
        {
            InterfaceMgr.getInstance().close(InterfaceMgr.A3_ACTIVE_GODLIGHT);
        }

        void onGo(GameObject go) 
        {
            InterfaceMgr.getInstance().close(InterfaceMgr.A3_ACTIVE_GODLIGHT);
            if (MapModel.getInstance().dicMappoint?.ContainsKey(map_id) ?? false)
                SelfRole.Transmit(MapModel.getInstance().dicMappoint[map_id], after: () =>
                {
                    SelfRole.moveto(map_id, new Vector3(map_x, 0, map_y), handle: () =>
                    {
                        SelfRole.fsm.StartAutofight();
                    });
                });
        }

        void Update()
        {
            if (a3_liteMinimap.instance == null) return;
           
            if (Globle.formatTime((int)a3_liteMinimap.instance.active_leftTm) == "00:00:00")
            {
                IconAddLightMgr.getInstance().showOrHideFire("Light_btnCseth", false);
                if(System.DateTime.Now.Hour>=20|| System.DateTime.Now.Hour<=12)
                      time_txt.text = ContMgr.getCont("a3_active_godlight_nexttime1");
                else
                     time_txt.text = ContMgr.getCont("a3_active_godlight_nexttime2");
            }
            else
            {
                IconAddLightMgr.getInstance().showOrHideFire("Light_btnCseth", false);
                time_txt.text = Globle.formatTime((int)a3_liteMinimap.instance.active_leftTm);
            }
           



        }
    }
}
