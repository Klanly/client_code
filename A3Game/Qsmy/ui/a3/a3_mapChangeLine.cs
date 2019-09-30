using GameFramework;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace MuGame
{
    public class a3_mapChangeLine : Window
    {

        /*主城，副本以及战斗过程中不可切线 */
        GameObject contain,
                     grid;
        BaseButton btnClose;


        ScrollControler scrollControler;

        public static a3_mapChangeLine instance;
        public override void init()
        {
            inText();
            instance = this;
            contain = getGameObjectByPath("panel/ScrollPanel/content");
            grid = getGameObjectByPath("panel/ScrollPanel/grid");


            btnClose = new BaseButton(getTransformByPath("title/btnClose"));
            btnClose.onClick = onBtnCancelClick;

            scrollControler  = new ScrollControler();
            scrollControler.create(getComponentByPath<ScrollRect>("panel/ScrollPanel"));
        }
        void inText() {
            this.transform.FindChild("title/Text").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_mapChangeLine_1");// 分线

        }
        public override void onShowed()
        {
            ChangeLineProxy.getInstance().sendLineProxy();
        }
        public override void onClosed()
        {

        }
        public   void creatrvegrids(int line_num)
        {
            DesObj();
            if (line_num <= 0)
                return;
            for (int i = 0; i < ChangeLineProxy.getInstance().Line_people.Count; i++)
            {
                if (i == PlayerModel.getInstance().line)
                    continue;
                GameObject objclone = GameObject.Instantiate(grid) as GameObject;
                objclone.name = (i + 1).ToString();
                objclone.SetActive(true);
                objclone.transform.SetParent(contain.transform, false);
                creatrvegrids_infos(objclone);
                //new BaseButton(objclone.transform).onClick = (GameObject go) =>
                 // {
                     // onBtnOKClick(go);
                 // };
            }
            a3_runestone.commonScroview(getGameObjectByPath("panel/ScrollPanel/content"), ChangeLineProxy.getInstance().Line_people.Count-1);
        }
        void creatrvegrids_infos(GameObject go)
        {
            string sr = "  {0}      {1}       {2}";

            string map_name = string.Empty;
            if (GRMap.curSvrConf != null&&GRMap.curSvrConf.ContainsKey("map_name"))
                map_name = GRMap.curSvrConf["map_name"]._str;
          
            string lingtype = string.Empty;
            uint people_num = ChangeLineProxy.getInstance().Line_people[int.Parse(go.name) - 1];
            int num1 = XMLMgr.instance.GetSXML("comm.fx_state_3").getInt("val");
            int num2 = XMLMgr.instance.GetSXML("comm.fx_state_2").getInt("val");
            int num3 = XMLMgr.instance.GetSXML("comm.fx_state_1").getInt("val");
            int numx = 0;
            if (people_num>=0&&people_num< num1)
            {
                numx = 0;
                lingtype = XMLMgr.instance.GetSXML("comm.fx_state_3").getString("c");
            }
            else if(people_num >= num1 && people_num < num2)
            {
                numx = 1;
                lingtype = XMLMgr.instance.GetSXML("comm.fx_state_2").getString("c");
            }
            else
            {
                numx = 2;
                lingtype = XMLMgr.instance.GetSXML("comm.fx_state_1").getString("c");
            }
            Text txt = go.transform.FindChild("Label").GetComponent<Text>();

            txt.text = string.Format(sr, ContMgr.getCont("a3_mapChangeLine11") + go.name, map_name, lingtype);
            new BaseButton(go.transform).onClick = (GameObject gos) =>
            {
                 onBtnOKClick(gos, numx);
            };
        }
         public  void onBtnCancelClick(GameObject go)
        {
            InterfaceMgr.getInstance().close(InterfaceMgr.A3_MAPCHANGELINE);
            DesObj();
        }
        public   void DesObj()
        {
            for(int i=0; i< contain.transform.childCount;i++)
            {
                GameObject.Destroy(contain.transform.GetChild(i).gameObject);
            }
        }

        void onBtnOKClick(GameObject go,int nums)
        {
            if(nums>=2)
            {
                flytxt.instance.fly(ContMgr.getCont("xianluwenti"));
                return;
            }
            for (int i = 0; i < contain.transform.childCount; i++)
            {
                contain.transform.GetChild(i).gameObject.transform.FindChild("Image").gameObject.SetActive(false);
            }
            go.transform.FindChild("Image").gameObject.SetActive(true);
            ChangeLineProxy.getInstance().select_line(uint.Parse(go.name)-1);
            //this.gameObject.SetActive(false);
        }

        public static bool canline()
        {
            print("两次攻击的时间间隔是:" + (NetClient.instance.CurServerTimeStamp - BattleProxy.getInstance().hurt_old_time));
            if (BattleProxy.getInstance().hurt_old_time!=-1)
            {

                if (NetClient.instance.CurServerTimeStamp - BattleProxy.getInstance().hurt_old_time >XMLMgr.instance.GetSXML("comm.leave_atk_time").getInt("val"))
                       return true;
                else
                    return false;
            }
            else
               return true;
        }


    }
}
