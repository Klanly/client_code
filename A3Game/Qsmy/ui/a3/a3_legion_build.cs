using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameFramework;
using UnityEngine;
using UnityEngine.UI;
using Cross;
using System.Collections;
using DG.Tweening;
using UnityEngine.EventSystems;


namespace MuGame
{
    class a3_legion_build:Window
    {

        Text today_build_count,
             personal_give,
             today_build_money;

        GameObject exp;


        Dictionary<int, GameObject> away = new Dictionary<int, GameObject>();
        Dictionary<int, build_award> dic_away = new Dictionary<int, build_award>();
        Dictionary<int, GameObject> build = new Dictionary<int, GameObject>();

        Transform reward_parent,
                  build_parent;


        public static a3_legion_build instance;
        public override void init()
        {

            getComponentByPath<Text>("centre/contain/0/name").text = ContMgr.getCont("a3_legion_build2");
            getComponentByPath<Text>("centre/contain/1/name").text = ContMgr.getCont("a3_legion_build3");
            getComponentByPath<Text>("centre/contain/2/name").text = ContMgr.getCont("a3_legion_build4");
            getComponentByPath<Text>("centre/contain/0/Button/txt").text = ContMgr.getCont("a3_legion_build7");
            getComponentByPath<Text>("centre/contain/1/Button/txt").text = ContMgr.getCont("a3_legion_build7");
            getComponentByPath<Text>("centre/contain/2/Button/txt").text = ContMgr.getCont("a3_legion_build7");
            getComponentByPath<Text>("down/Text").text = ContMgr.getCont("a3_legion_build8");
            getComponentByPath<Text>("down/Image/txt").text = ContMgr.getCont("a3_legion_build9");


            exp = getGameObjectByPath("down/exp/exp");
            reward_parent = getTransformByPath("down/reward");
            build_parent = getTransformByPath("centre/contain");
            today_build_count = getComponentByPath<Text>("top/count");
            personal_give = getComponentByPath<Text>("top/nub");
            today_build_money = getComponentByPath<Text>("down/Image/num");

            new BaseButton(getTransformByPath("close")).onClick = (GameObject go) => { InterfaceMgr.getInstance().close(InterfaceMgr.A3_LEGION_BUILD); };





            creatrveBuild();
            creatrveAward();
        }


        public override void onShowed()
        {
            instance = this;
            RefreshTxt();
            RefresAward();
        }




        #region 建设
        void creatrveBuild()
        {
           

            for (int j = 0; j < build_parent.childCount; j++)
            {
                SXML x = XMLMgr.instance.GetSXML("clan.clan_donate", "id=="+(j+1));
                build[j + 1] = build_parent.GetChild(j).gameObject;
                build_parent.GetChild(j).transform.FindChild("zijing").GetComponent<Text>().text =ContMgr.getCont("a3_legion_build5") + x.getString("clan_money");
                build_parent.GetChild(j).transform.FindChild("gongxian").GetComponent<Text>().text = ContMgr.getCont("a3_legion_build6") + x.getString("donate");
                string costType_file = string.Empty;
                switch (x.getInt("cost_type"))
                {
                    case 2: costType_file = "icon_coin_coin1"; break;
                    case 3: costType_file = "icon_coin_coin3"; break;
                    case 4: costType_file = "icon_coin_coin2"; break;
                }
                build_parent.GetChild(j).transform.FindChild("Button/Image").GetComponent<Image>().sprite=GAMEAPI.ABUI_LoadSprite(costType_file);
                build_parent.GetChild(j).transform.FindChild("Button/Image/num").GetComponent<Text>().text =x.getString("cost_num");
                new BaseButton(build_parent.GetChild(j).transform.FindChild("Button")).onClick = (GameObject go) =>
                  {
                      buildBtnOnClick(int.Parse(go.transform.parent.name));
                  };
            }
        }

        void buildBtnOnClick(int type)
        {
            A3_LegionProxy.getInstance().SendBuild((uint)(type+1));
        }
        public  void RefreshTxt()
        {
            int num = A3_VipModel.getInstance().vip_exchange_num(22);
            today_build_count.text =ContMgr.getCont("a3_legion_build0") + (num-A3_LegionModel.getInstance().build_count).ToString()+"/"+num;
            personal_give.text = ContMgr.getCont("a3_legion_build1") + A3_LegionModel.getInstance().build_my_get.ToString();
            today_build_money.text = A3_LegionModel.getInstance().build_clan_get.ToString();
        }
        #endregion


        #region 奖励
        void creatrveAward()
        {
            for (int i = 0; i < reward_parent.childCount; i++)
            {
                away[i + 1] = reward_parent.GetChild(i).gameObject;
                int nub = i;
                new BaseButton(reward_parent.GetChild(i).FindChild("this").transform).onClick = (GameObject go) =>
                {
                    getAwardBtnOnClick(nub + 1);
                };
            }
            SXML x = XMLMgr.instance.GetSXML("clan");
            List<SXML> xmls = XMLMgr.instance.GetSXMLList("clan.donate_awd");
            foreach (SXML xx in xmls)
            {
                build_award ba = new build_award();
                ba.id = xx.getInt("id");
                ba.limit_donate = xx.getInt("limit_donate");
                ba.item_id = xx.GetNode("RewardItem").getInt("item_id");
                ba.item_num = xx.GetNode("RewardItem").getInt("value");
                dic_away[ba.id] = ba;
                GameObject icon = IconImageMgr.getInstance().createA3ItemIcon((uint)ba.item_id, true, ba.item_num, 1);
                icon.transform.FindChild("iconbor").gameObject.SetActive(false);
                icon.transform.FindChild("wk").gameObject.SetActive(false);
                icon.transform.SetParent(away[ba.id].transform, false);
                icon.transform.SetAsFirstSibling();
                away[ba.id].transform.FindChild("num").GetComponent<Text>().text = ba.limit_donate.ToString();
                new BaseButton(icon.transform).onClick = (GameObject go) =>
                {
                    itemtipOnclick(go, (uint)ba.item_id, ba.item_num);
                };
            }

        }
        void itemtipOnclick(GameObject go, uint id, int num)
        {
            ArrayList arr = new ArrayList();
            arr.Add(id);
            arr.Add(num);
            InterfaceMgr.getInstance().ui_async_open(InterfaceMgr.A3_MINITIP, arr);
            a3_miniTip.Instance?.transform.SetAsLastSibling();
        }
        void getAwardBtnOnClick( int id )
        {
            A3_LegionProxy.getInstance().SendBuildAwd((uint)(id));
        }
        public void RefresAward()
        {

            for (int i = 0; i < dic_away.Count; i++)
            {
                if (A3_LegionModel.getInstance().build_clan_get >= dic_away[i + 1].limit_donate)
                {
                    away[i + 1].transform.FindChild("this").gameObject.SetActive(true);
                    away[i + 1].transform.FindChild("ok").gameObject.SetActive(false);
                }
                else
                {
                    away[i + 1].transform.FindChild("this").gameObject.SetActive(false);
                    away[i + 1].transform.FindChild("ok").gameObject.SetActive(false);

                }

            }
            if (A3_LegionModel.getInstance().build_awd.Count>0)
            {

                foreach(int key in A3_LegionModel.getInstance().build_awd.Keys)
                {
                    if (away.ContainsKey(key))
                    {
                        away[key].transform.FindChild("this").gameObject.SetActive(false);
                        away[key].transform.FindChild("ok").gameObject.SetActive(true);
                    }
                }

            }


            /*exp*/
            int x = dic_away[dic_away.Keys.Last()].limit_donate;
            float num = A3_LegionModel.getInstance().build_clan_get >= x ? 1 : (A3_LegionModel.getInstance().build_clan_get /(float) x);
            exp.transform.localScale = new Vector3(num, 1, 1);
        }
        

        #endregion







        public override void onClosed()
        {
           
        }

    }
    class build_award
    {
        public  int id;
        public int limit_donate;
        public int item_id;
        public int item_num;
    }
}
