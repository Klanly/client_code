using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cross;
using GameFramework;
using MuGame.Qsmy.model;
using UnityEngine;
using UnityEngine.UI;

namespace MuGame
{
    class a3_relive : Window
    {
        static public int backtown_end_tm = 0;
        private int origin_tm = 3;

        private float timer = 0;
        
        private Button btn_gld;
        private Button btn_stone;
        private Button btn_backleft;
        private Button btn_backmid;
        private Text info;
        GameObject recharge;
        BaseRole one;
        public static a3_relive instans;
        public GameObject FX;
        private int here_time;
        public Text num_time;
        Transform spost_relive;
        private int jdzc_time;
        SXML Xml_jdzc;
        public override void init()
        {
            //this.getEventTrigerByPath("ig_bg_bg").onClick = OnClose;



            getComponentByPath<Text>("here_relive/Text_bg").text = ContMgr.getCont("a3_relive_0");
            getComponentByPath<Text>("btn_gld/Text").text = ContMgr.getCont("a3_relive_1");
            getComponentByPath<Text>("btn_gld/Text_free").text = ContMgr.getCont("a3_relive_2");
            getComponentByPath<Text>("btn_stone/Text").text = ContMgr.getCont("a3_relive_3");
            getComponentByPath<Text>("btn_stone/Text_free").text = ContMgr.getCont("a3_relive_2");
            getComponentByPath<Text>("btn_backleft/Text").text = ContMgr.getCont("a3_relive_4");
            getComponentByPath<Text>("recharge/Text").text = ContMgr.getCont("a3_relive_5");
            getComponentByPath<Text>("recharge/no/Text").text = ContMgr.getCont("a3_relive_6");
            getComponentByPath<Text>("recharge/yes/Text").text = ContMgr.getCont("a3_relive_7");
            getComponentByPath<Text>("spost_relive/Text").text = ContMgr.getCont("a3_relive_8");







            btn_gld = getComponentByPath<Button>("btn_gld");
            BaseButton btn0 = new BaseButton(btn_gld.transform);
            btn0.onClick = OnGoldRespawn;

            btn_backleft = getComponentByPath<Button>("btn_backleft");
            BaseButton btn1 = new BaseButton(btn_backleft.transform);
            btn1.onClick = OnBackRespawn;

            btn_stone = getComponentByPath<Button>("btn_stone");
            BaseButton btn2 = new BaseButton(btn_stone.transform);
            btn2.onClick = OnStoneRespawn;

            btn_backmid = getComponentByPath<Button>("btn_backmid");
            BaseButton btn3 = new BaseButton(btn_backmid.transform);
            btn3.onClick = OnBackRespawn;

           // num_time = this.getComponentByPath<Text>("here_relive/Text");
            info = this.getComponentByPath<Text>("dialog");
            recharge = this.transform.FindChild("recharge").gameObject;
            FX = this.transform.FindChild("FX_").gameObject;
            new BaseButton(recharge.transform.FindChild("no")).onClick = (GameObject go) => 
            {
                recharge.SetActive(false);
            };

            new BaseButton(recharge.transform.FindChild("yes")).onClick = (GameObject go) => {
                InterfaceMgr.getInstance().ui_async_open(InterfaceMgr.A3_RECHARGE);
                recharge.SetActive(false);
            };

            spost_relive = this.transform.FindChild("spost_relive");
            Xml_jdzc = XMLMgr.instance.GetSXML("pointarena.arenatime");
        }

        public override void onShowed()
        {
            closeWindow();
            instans = this;
            this.transform.FindChild("btn_gld/Text_free").gameObject.SetActive(false);
            this.transform.FindChild("btn_stone/Text_free").gameObject.SetActive(false);
            recharge.SetActive(false);
            InterfaceMgr.getInstance().floatUI.localScale = Vector3.zero;
            if (GameObject.Find("GAME_CAMERA/myCamera"))
            {
                GameObject cameraOBJ = GameObject.Find("GAME_CAMERA/myCamera");
                if (!cameraOBJ.GetComponent<DeathShader>())
                {
                    cameraOBJ.AddComponent<DeathShader>();
                }
                else
                {
                    cameraOBJ.GetComponent<DeathShader>().enabled = true;
                }
            }
            //timer = 0;
            //origin_tm = 3;
            here_time = 30;
            jdzc_time = Xml_jdzc.getInt("revive");
            btn_gld.gameObject.SetActive(false);
            btn_stone.gameObject.SetActive(false);
            btn_backleft.gameObject.SetActive(false);
            btn_backmid.gameObject.SetActive(false);
            spost_relive.gameObject.SetActive(false);
            //btn_stone.interactable = false;
            //btn_gld.interactable = false;
            //btn_backmid.interactable = false;
            //btn_backleft.interactable = false;

            RefreshBackTownBtn();
            if (!PlayerModel.getInstance().inFb) RefreshOriginBtn();

            if (CanReviveOrigin() == 0)
            {//地图不允许原地复活
                btn_backmid.gameObject.SetActive(true);
            }
            else if (CanReviveOrigin() == 1)
            {
                if (HasRespawnStone())
                {
                    btn_stone.gameObject.SetActive(true);
                    btn_backleft.gameObject.SetActive(true);
                }
                else
                {
                    btn_gld.gameObject.SetActive(true);
                    btn_backleft.gameObject.SetActive(true);
                }
            } else if (CanReviveOrigin() == 3)
            {
                //不可复活，自动本场景复活点复活
                spost_relive.gameObject.SetActive(true);
            }

           // BattleProxy.getInstance().addEventListener(BattleProxy.EVENT_DIE, refInfo);

            if (uiData != null)
            {
                one = (BaseRole)uiData[0];
            }
            refInfo();
            InterfaceMgr.getInstance().closeAllWin(InterfaceMgr.A3_RELIVE);

            if (A3_SummonModel.getInstance().GetSummons().ContainsKey(A3_SummonModel.getInstance().nowShowAttackID))
            {
                A3_SummonModel.getInstance().toAttackID = A3_SummonModel.getInstance().nowShowAttackID;
                A3_SummonProxy.getInstance().sendZhaohui();
            }

            NewbieModel.getInstance().hide();
        }

        public override void onClosed()
        {
            this.transform.FindChild("btn_stone/cnt").gameObject.SetActive(true);
            this.transform.FindChild("btn_stone/icon").gameObject.SetActive(true);
            this.transform.FindChild("btn_gld/cnt").gameObject.SetActive(true);
            this.transform.FindChild("btn_gld/icon").gameObject.SetActive(true);
            this.transform.FindChild("btn_gld/Text").gameObject.SetActive(true);       
            this.transform.FindChild("btn_gld/Text").gameObject.SetActive(true);
          
            InterfaceMgr.getInstance().floatUI.localScale = Vector3.one;
            if (GameObject.Find("GAME_CAMERA/myCamera"))
            {
                GameObject cameraOBJ = GameObject.Find("GAME_CAMERA/myCamera");
                if (cameraOBJ.GetComponent<DeathShader>())
                {
                    cameraOBJ.GetComponent<DeathShader>().enabled = false;
                }
            }
            instans = null;

            if (A3_SummonModel.getInstance().GetSummons().ContainsKey(A3_SummonModel.getInstance().toAttackID))
            {
                A3_SummonProxy.getInstance().sendChuzhan(A3_SummonModel.getInstance().toAttackID);
                A3_SummonModel.getInstance().toAttackID = 0;
            }

        }


        public void Update()
        {
            if (PlayerModel.getInstance().inFb &&  (!PlayerModel .getInstance ().inSpost || !PlayerModel.getInstance().inCityWar ))
                return;
            timer += Time.deltaTime;
            if (timer > 1.0f)
            {
                timer -= 1.0f;
                here_time--;
                jdzc_time--;
                if (spost_relive.gameObject.activeSelf == false) {
                    if (here_time > 0)
                    {
                        this.transform.FindChild("btn_stone/Text").gameObject.SetActive(true);
                    }
                    else
                    {
                        this.transform.FindChild("btn_stone/cnt").gameObject.SetActive(false);
                        this.transform.FindChild("btn_stone/icon").gameObject.SetActive(false);
                        this.transform.FindChild("btn_stone/Text").gameObject.SetActive(false);
                        this.transform.FindChild("btn_stone/Text_free").gameObject.SetActive(true);
                        this.transform.FindChild("btn_gld/cnt").gameObject.SetActive(false);
                        this.transform.FindChild("btn_gld/icon").gameObject.SetActive(false);
                        this.transform.FindChild("btn_gld/Text").gameObject.SetActive(false);
                        this.transform.FindChild("btn_gld/Text_free").gameObject.SetActive(true);

                        here_time = 0;
                    }
                }
                //origin_tm--;
                //backtown_end_tm--;
              
                RefreshBackTownBtn();
                if(!PlayerModel.getInstance().inFb) RefreshOriginBtn();
              //  num_time.text = here_time.ToString();
              
              //  AutoRespawn();
            }
        }


        void closeWindow()
        {
            for ( int i = 0; i < this.transform.parent.childCount; i++ )
            {
                if (this.transform.parent.GetChild(i).gameObject.activeSelf == true)
                {
                    if (this.transform.parent.GetChild(i).gameObject == this.gameObject)
                    {
                        continue;
                    }
                    this.transform.parent.GetChild(i).gameObject.SetActive(false);
                    GRMap.GAME_CAMERA.SetActive(true);
                }
            }
        }
        private void AutoRespawn()
        {
            //!--没有挂机不考虑自动复活
            if (!SelfRole.fsm.Autofighting)
                return;
           
            if (here_time<= 0)
            {//!--倒计时结束
             //AutoPlayModel apmodel = AutoPlayModel.getInstance();

                //if (apmodel.StoneRespawn == 0 ||    //!--未勾选原地复活
                //    !CanReviveOrigin() ||   //!--地图不允许原地复活
                //    (apmodel.RespawnLimit > 0 && StateInit.Instance.RespawnTimes <= 0))   //!--自动原地复活次数用尽
                //{//!--停止自动挂机,等待手动或服务器自动回城复活
                //    SelfRole.fsm.Stop();
                //    return;
                //}

                //if (HasRespawnStone())
                //{//!--有复活石,使用复活石原地复活
                //    OnStoneRespawn(null);
                //    StateInit.Instance.RespawnTimes--;
                //}
                //else if(apmodel.GoldRespawn > 0 && 
                //    PlayerModel.getInstance().gold >= 20)
                //{//!--没有复活石,但是勾选了使用金币复活，并且金币足够
                //    OnGoldRespawn(null);
                //    StateInit.Instance.RespawnTimes--;
                //}
                //else
                //{//!--等待自动回城复活,关闭挂机
                //    SelfRole.fsm.Stop();
                //}
                //  MapProxy.getInstance().sendRespawn(true);
               
               
            }

        }

        void RefreshOriginBtn()
        {
            //if (origin_tm == 0)
            //{
            //    btn_stone.interactable = true;
            //    btn_gld.interactable = true;
            //}

            //if (origin_tm < 0) 
            //    return;

            btn_stone.transform.FindChild("Text").GetComponent<Text>().text = GetOriText();
            btn_gld.transform.FindChild("Text").GetComponent<Text>().text = GetOriText();
        }

        void RefreshBackTownBtn()
        {
            //if (backtown_end_tm <= 0)
            //{
            //    btn_backmid.interactable = true;
            //    btn_backleft.interactable = true;

            //    backtown_end_tm = 0;
            //}

            btn_backleft.transform.FindChild("Text").GetComponent<Text>().text = GetBackText();
            btn_backmid.transform.FindChild("Text").GetComponent<Text>().text = GetBackText();
            spost_relive.FindChild("time").GetComponent<Text>().text = jdzc_time.ToString();


        }

        void OnClose(GameObject go)
        {
            InterfaceMgr.getInstance().close(InterfaceMgr.A3_RELIVE);
        }

        private void OnGoldRespawn(GameObject go)
        {
           
            if (here_time>0&&PlayerModel.getInstance().gold < 20)
            {
                flytxt.instance.fly(ContMgr.getCont("a3_relive_nolive"));
                recharge.SetActive(true);
                return;
            }
            MapProxy.getInstance().sendRespawn(true);
        }
          
        private void OnBackRespawn(GameObject go)
        {
            MapProxy.getInstance().sendRespawn(false);
            //PlayerModel.getInstance().c
            if (MapModel.getInstance().curLevelId == 108 || MapModel.getInstance().curLevelId == 109 || MapModel.getInstance().curLevelId == 110 || MapModel.getInstance().curLevelId == 111)
                TeamProxy.getInstance().SendLeaveTeam(PlayerModel.getInstance().cid);
        }

        private void OnStoneRespawn(GameObject go)
        {
            MapProxy.getInstance().sendRespawn(true);
        }

        private string GetOriText()
        {
            string str = ContMgr.getCont("a3_relive_live");

            if (here_time <= 0)
            {
                str = ContMgr.getCont("a3_relive_livefree");
                return str;
            }
             else  

          //  return str + "(" + origin_tm + ")";
            return str+ ContMgr.getCont("a3_relive_live2", new List<string>() { here_time.ToString() }); ;
        }

        private string GetBackText()
        {
            string str = ContMgr.getCont("a3_relive_live1");
            //if (backtown_end_tm == 0)
            //    return str;

            return str/* + "(" + backtown_end_tm + ")"*/;
        }

      

        void refInfo()
        {
            string str = "";
            if (one != null)
                str = ContMgr.getCont("a3_relive_die", new List<string>() { one.roleName.ToString() });
            //str = "你被"+ one.roleName +"杀死了";
            info.text = str;
        }

        private int CanReviveOrigin()
        {
            uint mapid = (uint)GRMap.instance.m_nCurMapID;
            Variant mapConf = SvrMapConfig.instance.getSingleMapConf(mapid);
            
            if (!mapConf.ContainsKey("revive"))
                return 0;

            int revive = mapConf["revive"];
            return revive;
        }

        private bool HasRespawnStone()
        {
            int num = a3_BagModel.getInstance().getItemNumByTpid(1508);
            return num >= 1;
        }
    }
}
