using GameFramework;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using DG.Tweening;
using Cross;
namespace MuGame
{
    class loading_cloud : LoadingUI
    {
        public static loading_cloud instance;

        public static Action showhandle;


        private Image bga;

		public List<string> tips = new List<string>();
		Text tiptext;
		private Image loadingMc;
		private Text loadingtext;

        private int curState = 0;
        int count_sprite = 0;
        //List<Sprite> bglist = new List<Sprite>();
        public override void init()
        {
            bga = this.getComponentByPath<Image>("bga");
            //Sprite[] l = new Sprite[2];
            //l[0] = GAMEAPI.ABUI_LoadSprite("loading_1");
            //l[1] = GAMEAPI.ABUI_LoadSprite("loading_2");
            count_sprite = XMLMgr.instance.GetSXML("comm.loading").getInt("num");
            //bglist.AddRange(l);
            bga.GetComponent<RectTransform>().sizeDelta = new Vector2(Baselayer.uiWidth, Baselayer.uiHeight);

			var xml = XMLMgr.instance.GetSXML("tips");
			var lxml = xml.GetNodeList("t");
			foreach (var v in lxml) {
				var ss = v.getString("info");
				tips.Add(ss);
			}
			tiptext = this.getComponentByPath<Text>("tip/bg/text");
			loadingtext = this.getTransformByPath("loadingBar/Text").GetComponent<Text>();
			loadingMc = this.getTransformByPath("loadingBar").GetComponent<Image>();
        }

        public bool showed = false;
        public override void onShowed()
        {
            if (showed)
                return;
            showed = true;



            curState = 0;
            //   lgSelfPlayer.instance.stop();

            instance = this;
            base.onShowed();
            play();
        }

        public override void onClosed()
        {
            if (!showed)
                return;
            showed = false;




            instance = null;
            bga.gameObject.SetActive(true);
            curState = 0;
            _handle = null;

            UiEventCenter.getInstance().onMapChanged();



            base.onClosed();
        }

        

        public void play()
        {
            if (bga == null)
                init();

       //     MediaClient.getInstance().PlayMusicUrl("");
            NewbieModel.getInstance().hide();

            Randombg();
            onAniOver();
			RandomTip();
			PlayLoading();
        }
        void Randombg()
        {
            bga.sprite = GAMEAPI.ABUI_LoadSprite("loading_" + UnityEngine.Random.Range(1, count_sprite + 1));
        }
        int waitingTick = 0;
        void Update()
        {
            if (curState == 1)
            {
                curState = 0;

                //   MediaClient.getInstance().pause(true);
                if (showhandle != null)
                    showhandle();
                showhandle = null;

            }
            else if (curState == 2)
            {
                waitingTick++;
                if (waitingTick < 20)
                    return;

                waitingTick = 0;
                curState = 0;

                //   debug.Log("!!loading curState==2!!::" + lgSelfPlayer.instance.y + " " + debug.count);
                //  MediaClient.getInstance().pause(false);
                playOver();
                //if (lgSelfPlayer.instance != null)
                //{
                //    lgSelfPlayer.instance.stop();
                //    lgSelfPlayer.instance.x += 0.1f;
                //}


                ////判断出副本时是否有等级升级界面
                //PlayerModel.getInstance().isShowVipUpLayer();
                ////判断出副本时是否有战斗力提升界面
                //PlayerModel.getInstance().isShowFightingUp();
                ////判断是否有vip时间到了，英雄需要下阵
                ////HeroModel.getInstance().checkVipHeroGetOut();

                //lgSelfPlayer.instance.refreshIdle();
            }
            else if (curState == 3)
            {
                curState = 0;

                InterfaceMgr.getInstance().DisposeUI(InterfaceMgr.LOADING_CLOUD);
                if (a3_mapname.instance != null)
                    a3_mapname.instance.refreshInfo();

                DoAfterMgr.instacne.addAfterRender(() =>
                {
                    GameObject goeff = GameObject.Instantiate(worldmap.EFFECT_CHUANSONG2) as GameObject;
                    goeff.transform.SetParent(SelfRole._inst.m_curModel, false);
                    Destroy(goeff, 2f);

                    if (a3_expbar.instance != null) a3_expbar.instance.On_Btn_Down();
                    InterfaceMgr.doCommandByLua("a1_low_fightgame.setToggle", "ui/interfaces/low/a1_low_fightgame", true);
                    // if (a3_liteMinimap.instance != null) a3_liteMinimap.instance.onTogglePlusClick(false);

                    //新技能开启提示
                    if (a3_skillopen.instance != null)
                        a3_skillopen.instance.refreshInfo();

                  
                    if (a3_runeopen.instance != null)
                    {
                        a3_runeopen.instance.refreshInfo();
                    }

                });

               
            }
        }

        static private Action _handle;
        static public void showIt(Action onfin) 
        {
            if (instance != null)
                return;
            showhandle = onfin;
            InterfaceMgr.getInstance().ui_async_open(InterfaceMgr.LOADING_CLOUD);
        }


        void onAniOver()
        {
            curState = 1;
        }

		void RandomTip() {
			tiptext.text = tips[UnityEngine.Random.Range(0, tips.Count)];
		}

		public void PlayLoading() {
			loadingMc.fillAmount = 0;
			loadingtext.text = 0 + "%";
			var vv = DG.Tweening.DOTween.To(() => loadingMc.fillAmount, (float x) => {
				loadingMc.fillAmount = x;
				loadingtext.text = Mathf.Floor(x * 100) + "%";
			}, 0.9f, 1.2f);
		}

        public void hide(Action handle)
        {

            //combo_txt.clear();
            //AttackPointMgr.instacne.clear();

            //lgSelfPlayer.instance.needRefreshY = true;


            //LGCamera.instance.updateMainPlayerPos();

            //lgSelfPlayer.instance.grAvatar.setAni("idle", 0);
            curState = 2;
            _handle = handle;
        }



        private void playOver()
        {
          
            if (_handle != null)
                _handle();
            _handle = null;


			var vv = DG.Tweening.DOTween.To(() => loadingMc.fillAmount, (float x) => {
				loadingMc.fillAmount = x;
				loadingtext.text = Mathf.Floor(x * 100) + "%";
			}, 1f, 0.2f).OnComplete(() => {
				playOverHandle();
			});

        }

        private void playOverHandle()
        {

            curState = 3;
            //   loading_cloud._handle();

        }

    }
}
