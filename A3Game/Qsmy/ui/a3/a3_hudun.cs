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
using System.Xml;
using DG.Tweening;

namespace MuGame
{
    class a3_hudun : Window
    {
        private BaseButton btnClose;

        private BaseButton btnUpLevel;//强化
        private BaseButton btnAddEnergy;//充能

        private Text huDunLevel;//护盾等级text
        private Text huDunCount;//当前护盾等级当前护盾值和最大护盾值
        private Text mjCount;//强化所需魔晶数量

        public Toggle isAuto;//自动充能

        private HudunModel HudunModel;
        public a3_BagModel bagModel;
        private PlayerModel playerModel;
        private A3_HudunProxy hudunProxy;
        public static a3_hudun _instance;
        public static a3_hudun isshow;
        Animator ani;
        Image bar;

        public override void init()
        {
            _instance = this;
            btnClose = new BaseButton(this.getTransformByPath("btn_close"));
            btnClose.onClick = OnCLoseClick;
            btnUpLevel = new BaseButton(this.getTransformByPath("ig_bg1/qianghua"));
            btnUpLevel.onClick = UpLevel;
            btnAddEnergy = new BaseButton(this.getTransformByPath("ig_bg1/chongneng"));
            btnAddEnergy.onClick = AddEnergy;
            new BaseButton(this.getTransformByPath("ig_bg1/help")).onClick = onHelp;
            new BaseButton(this.getTransformByPath("ig_bg1/tishi/close")).onClick = close_tishi;


            huDunLevel = this.getComponentByPath<Text>("ig_bg1/top/topImage/Text");
            huDunCount = this.getComponentByPath<Text>("ig_bg1/top/shuzhi");
            mjCount = this.getComponentByPath<Text>("ig_bg1/mjText/count");
            isAuto = this.getComponentByPath<Toggle>("ig_bg1/Toggle");
            bar = this.getComponentByPath<Image>("ig_bg1/bar/bar_n");
            isAuto.onValueChanged.AddListener(add_isAuto);

            HudunModel = HudunModel.getInstance();
            playerModel = PlayerModel.getInstance();
            hudunProxy = A3_HudunProxy.getInstance();
            bagModel = a3_BagModel.getInstance();
            hudunProxy.sendinfo(0);
            transform.FindChild("ig_bg_bg").gameObject.SetActive(false);
            updata_hd(HudunModel.NowCount);
            base.init();
        }

        public override void onShowed()
        {
            isshow = this;

            updata_hd(HudunModel.NowCount);
            showbtnUpLevel();
            
            InterfaceMgr.getInstance().changeState(InterfaceMgr.STATE_FUNCTIONBAR);
            base.onShowed();

            GRMap.GAME_CAMERA.SetActive(false);
            createSence();
            createShield();
            //CancelInvoke("showCam");
            //Invoke("showCam", 0.2f);
        }
        //void showCam()
        //{
        //    if (isshow != null)
        //    {
        //        createSence();
        //        createShield();
        //    }
        //}
        public override void onClosed()
        {
            isshow = null;
            //a3_herohead.instance.refreshSheild();
            
            disposeAvatar();
            InterfaceMgr.getInstance().changeState(InterfaceMgr.STATE_NORMAL);
            base.onClosed();

            GRMap.GAME_CAMERA.SetActive(true);
        }

        void add_isAuto(bool v)
        {
            if (isAuto.isOn)
            {
                HudunModel.is_auto = true;
                hudunProxy.sendinfo(3, 1);
                a3_herohead.instance.Add_energy_auto(HudunModel.auto_time, HudunModel.is_auto);
            }
            else
            {
                HudunModel.is_auto = false;
                hudunProxy.sendinfo(3, 0);
            }
        }
        //是否显示强化按钮
        private void showbtnUpLevel()
        {
            if (HudunModel.Level >= HudunModel.hdData.Count)
                btnUpLevel.interactable = false;
            else
                btnUpLevel.interactable = HudunModel.getInstance().CheckLevelupAvailable();
        }


        //强化
        private void UpLevel(GameObject go)
        {
            if (OnMjCountOk(HudunModel.GetNeedMjMun(HudunModel.Level + 1)))
            {
                if (HudunModel.Level >= HudunModel.hdData.Count)
                {
                    flytxt.instance.fly(ContMgr.getCont("a3_hudun_maxhudun"), 1);
                }
                else
                {
                    hudunProxy.sendinfo(1);
                }
            }
            else
            {
                flytxt.instance.fly(ContMgr.getCont("a3_hudun_nomj"), 1);
            }
        }

        bool isCan = false;
        public void AniUpLvl()
        {
            ani.SetBool("isopen",true);
            if (FX_open_Clon != null)
            {
                FX_open_Clon.SetActive(false);
            }
            FX_open_Clon = Instantiate(FX_open);
            FX_open_Clon.SetActive(true);
            FX_open_Clon.transform.SetParent(Shield_obj.transform,false);
            Destroy(FX_open_Clon,2);
            //FX_idle.SetActive(false);
            isCan = true;
        }


        void Update()
        {
            if (isCan) {
                AnimatorStateInfo info = ani.GetCurrentAnimatorStateInfo(0);
                // 判断动画是否播放
                if (info.IsName("open"))
                {
                    ani.SetBool("isopen", false);
                    isCan = false;
                }
            }

            if (HudunModel != null && HudunModel.Level != 0)
            {
                goUp();
            }
        }

        //充能
        private void AddEnergy(GameObject go)
        {
            if (!HudunModel.isNoAttack)
            {
                flytxt.instance.fly(ContMgr.getCont("a3_hudun_nocn"), 1);
            }
            else
            {
                if (HudunModel.Level <= 0)
                {
                    flytxt.instance.fly(ContMgr.getCont("a3_hudun_minhd"), 1);
                }
                else
                {
                    if (HudunModel.NowCount >= HudunModel.GetMaxCount(HudunModel.Level))
                    {
                        flytxt.instance.fly(ContMgr.getCont("a3_hudun_maxnl"), 1);
                    }
                    else
                    {
                        if (OnMjCountOk(HudunModel.hdData[HudunModel.Level].addcount))
                        {
                            hudunProxy.sendinfo(2);
                        }
                        else
                        {
                            flytxt.instance.fly(ContMgr.getCont("a3_hudun_nomj"), 1);
                        }

                    }
                }
            }
        }
        //更新数值
        public void updata_hd(int oldcount)
        {
            showbtnUpLevel();
            if (HudunModel.Level == 0)
            {
                huDunCount.text = "0/0";
                bar.fillAmount = 0;
            }
            else
            {
                huDunCount.text = HudunModel.NowCount.ToString() + "/" + HudunModel.GetMaxCount(HudunModel.Level).ToString();
                refbar(oldcount);
            }
            huDunLevel.text = HudunModel.Level.ToString();
            isAuto.isOn = HudunModel.is_auto;
            if (HudunModel.Level >= HudunModel.hdData.Count)
            {
                mjCount.transform.parent.gameObject.SetActive(false);
            }
            else
            {
                mjCount.text = HudunModel.GetNeedMjMun(HudunModel.Level + 1).ToString();
            }
        }



        private GameObject scene_Camera;
        private GameObject scene_Obj;
        private GameObject Shield_obj;
        public void createSence()
        {
            GameObject obj_prefab;
            obj_prefab = GAMEAPI.ABFight_LoadPrefab("model_avatar_ui_scene_ui_camera");
            scene_Camera = GameObject.Instantiate(obj_prefab) as GameObject;

            scene_Camera.transform.FindChild("Main_Avatar_Camera").GetComponent<Camera>().orthographicSize = 0.86f;
            obj_prefab = GAMEAPI.ABFight_LoadPrefab("model_avatar_ui_show_scene");
            scene_Obj = GameObject.Instantiate(obj_prefab, new Vector3(-77.38f, -0.49f, 15.1f), new Quaternion(0, 180, 0, 0)) as GameObject;

            foreach (Transform tran in scene_Obj.GetComponentsInChildren<Transform>())
            {
                if (tran.gameObject.name == "scene_ta")
                    tran.gameObject.layer = EnumLayer.LM_ROLE_INVISIBLE;
                else
                    tran.gameObject.layer = EnumLayer.LM_FX;// 更改物体的Layer层
            }
            scene_Obj.transform.FindChild("scene_ta").localPosition = new Vector3(-1.08f,0.39f,-0.166f);
            scene_Obj.transform.FindChild("scene_ta").localScale = new Vector3(0.6f,0.6f,0.6f);
            scene_Obj.transform.FindChild("sc_tz_lg").localPosition = new Vector3(-1.08f, 0.39f, -0.166f);
            scene_Obj.transform.FindChild("sc_tz_lg").localScale = new Vector3(1, 1, 1);
            scene_Obj.transform.FindChild("fx_sc").localPosition = new Vector3(-1.08f, 0.461f, -0.32f);
            scene_Obj.transform.FindChild("fx_sc").localScale = new Vector3(0.6f, 0.6f, 0.6f);

        }


        //private GameObject FX_idle;
        private GameObject FX_open;
        private GameObject FX_open_Clon;
        public void createShield()
        {
            GameObject obj_prefab;
            obj_prefab = GAMEAPI.ABModel_LoadNow_GameObject("npc_npc_shield");
            Shield_obj = GameObject.Instantiate(obj_prefab, new Vector3(-76.3f, 0.195f, 15.266f), new Quaternion(0, 180, 0, 0)) as GameObject;
            foreach (Transform tran in Shield_obj.GetComponentsInChildren<Transform>())
            {
                tran.gameObject.layer = EnumLayer.LM_FX;// 更改物体的Layer层
            }
            ani = Shield_obj.transform.FindChild("model/model").GetComponent<Animator>();
            //FX_idle = Shield_obj.transform.FindChild("FX_npc_shield_idle").gameObject;
            FX_open = Shield_obj.transform.FindChild("FX_npc_shield_open").gameObject;
        }

        public void disposeAvatar()
        {
            // m_proAvatar = null;
            ani = null;
           // FX_idle = null;
            FX_open = null;
            FX_open_Clon = null;
            if (Shield_obj != null) GameObject.Destroy(Shield_obj);
            if (scene_Obj != null) GameObject.Destroy(scene_Obj);
            if (scene_Camera != null) GameObject.Destroy(scene_Camera);
        }

        //判断所需魔晶数量是否足够
        private bool OnMjCountOk(int needcount) => needcount <= bagModel.getItemNumByTpid(1540);
        public bool CheckMjCount(int needcount) => OnMjCountOk(needcount);

        int speed = 0;
        int oldCount = 0;
        int newCount = 0;
        public void refbar(int oldCount)
        {
            //bar.fillAmount = (float) HudunModel.NowCount/ HudunModel.GetMaxCount(HudunModel.Level);
            this.oldCount = oldCount;
            newCount = HudunModel.NowCount;
            speed = (int)Math.Ceiling((double)(newCount - oldCount) / 20);
        }

        void goUp()
        {
            if (newCount > oldCount)
            {
                oldCount += speed;
                bar.fillAmount = (float)oldCount / HudunModel.GetMaxCount(HudunModel.Level);
            }
            else if (newCount <= oldCount)
            {
                bar.fillAmount = (float)newCount / HudunModel.GetMaxCount(HudunModel.Level);
            }
        }

        private void OnCLoseClick(GameObject go)
        {
            InterfaceMgr.getInstance().close(InterfaceMgr.A3_HUDUN);
        }
        public void onHelp(GameObject go)
        {
            this.transform.FindChild("ig_bg1/tishi").gameObject.SetActive(true);
        }
        public void close_tishi(GameObject go)
        {
            this.transform.FindChild("ig_bg1/tishi").gameObject.SetActive(false);
        }
    }
}
