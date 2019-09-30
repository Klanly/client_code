using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cross;
using GameFramework;
using MuGame.Qsmy.model;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using DG.Tweening;

namespace MuGame
{
    class a3_bossranking : Window
    {

        GameObject image,
                   contain;

        public static a3_bossranking instance;
        public override void init()
        {
            new BaseButton(getTransformByPath("bossranking/close")).onClick = (GameObject go) => {
               // showui(false);
                InterfaceMgr.getInstance().close(InterfaceMgr.A3_BOSSRANKING); 
            };
            image = getGameObjectByPath("bossranking/Panel/Image");
            contain = getGameObjectByPath("bossranking/Panel/contain");
            initcreatrve();
            initdata();
        }

        public override void onShowed()
        {
            instance = this;
           // showui(true);
            transform.FindChild("ig_bg_bg").gameObject.SetActive(false);
            EliteMonsterProxy.getInstance().addEventListener(EliteMonsterProxy.EVENT_SHOW, creatrve);
            if(contain.transform.childCount<=0)
            {
                initcreatrve();
            }

        }
        public void showui(bool show)
        {
            getComponentByPath<RectTransform>("bossranking").DOScale(show?new Vector3(1,1,1): new Vector3(0, 0, 0), 0.1f);
        }


        bool ishavemy = false;

        Dictionary<int, GameObject> dic = new Dictionary<int, GameObject>();
        void initcreatrve()
        {

            for (int j = 0; j < 10; j++)
            {
                GameObject clone = GameObject.Instantiate(image) as GameObject;
                clone.transform.SetParent(contain.transform, false);
                dic[j] = clone;
            }
        }
      

        void initdata()
        {
            ishavemy = false;
            Dictionary<int, bossrankingdata> dic_bsks = A3_EliteMonsterModel.getInstance().dic_bsk;


            int lenth = dic_bsks.Count;
            foreach (int nu in dic.Keys)
            {
                if (nu >= lenth)
                    dic[nu].SetActive(false);
                else
                    dic[nu].SetActive(true);
            }

            int j = 0;
            foreach(int i in dic_bsks.Keys)
            {

               
                int cid = dic_bsks[i].cid;
                string name = dic_bsks[i].name;
                uint dmg = dic_bsks[i].dmg;
                int num = j;
                dic[j].transform.FindChild("ranking").GetComponent<Text>().text = (num + 1).ToString();
                dic[j].transform.FindChild("name").GetComponent<Text>().text = name;
                dic[j].transform.FindChild("hurt").GetComponent<Text>().text = dmg.ToString();
                if (PlayerModel.getInstance().cid == cid)
                {
                    ishavemy = true;
                    getComponentByPath<Text>("bossranking/down/name").text = (num + 1).ToString();
                    getComponentByPath<Text>("bossranking/down/shanghai").text = dmg.ToString();
                }
                j++;
            }
            a3_runestone.commonScroview(contain, lenth);
            if (!ishavemy)
            {
                getComponentByPath<Text>("bossranking/down/name").text = "未上榜";
                getComponentByPath<Text>("bossranking/down/shanghai").text = "";
            }

            CancelInvoke("hidebtn");
            Invoke("hidebtn", 10);

        }

        void creatrve(GameEvent e)
        {
            ishavemy = false;

            int lenth = e.data["dmg_list"].Count;
            foreach(int nu in dic.Keys)
            {
                if (nu >= lenth)
                    dic[nu].SetActive(false);
                else
                    dic[nu].SetActive(true);
            }


            for (int i =0;i<e.data["dmg_list"].Count;i++)
            {


                int cid = e.data["dmg_list"][i]["cid"]._int;
                string name= e.data["dmg_list"][i]["name"]._str;
                uint dmg = e.data["dmg_list"][i]["dmg"]._uint;
                int num = i;
                dic[i].transform.FindChild("ranking").GetComponent<Text>().text =(num + 1).ToString();
                dic[i].transform.FindChild("name").GetComponent<Text>().text = name;
                dic[i].transform.FindChild("hurt").GetComponent<Text>().text = dmg.ToString();
                if(PlayerModel.getInstance().cid==cid)
                {
                    ishavemy = true;
                    getComponentByPath<Text>("bossranking/down/name").text = (num + 1).ToString();
                    getComponentByPath<Text>("bossranking/down/shanghai").text = dmg.ToString();
                }
            }
            a3_runestone.commonScroview(contain, lenth);
            if(!ishavemy)
            {
                getComponentByPath<Text>("bossranking/down/name").text = "未上榜";
                getComponentByPath<Text>("bossranking/down/shanghai").text = "";
            }

            CancelInvoke("hidebtn");
            Invoke("hidebtn", 10);

        }

        bool isover = false;
        void hidebtn()
        {

            //a3_expbar.instance?.BossRankingBtn.SetActive(false);
            //InterfaceMgr.getInstance().close(InterfaceMgr.A3_BOSSRANKING);
            EliteMonsterProxy.isfristover = false;
            string path = "ui/interfaces/low/a1_low_fightgame";
            InterfaceMgr.doCommandByLua("a1_low_fightgame.bossrkCl", path, null);
            EliteMonsterProxy.isfristover = false;
            CancelInvoke("hidebtn");
            isover = true;
        }

        public override void onClosed()
        {
            if(isover)
            {
                for (int j = 0; j < 10; j++)
                {
                    Destroy(contain.transform.transform.GetChild(j).gameObject);
                }
                getComponentByPath<Text>("bossranking/down/name").text = "";
                getComponentByPath<Text>("bossranking/down/shanghai").text = "";
                isover = false;
            }


        }
    }
}
