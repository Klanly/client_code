using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameFramework;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace MuGame
{
    class maploading:LoadingUI
    {
        static public maploading instance;
        static public maploading isshow;
		public List<string> tips = new List<string>();
		Text tiptext;
        Image bgimage;

		private Image loadingMc;
		private Text loadingtext;
        private GameObject go_info;

        //List<Sprite> bglist = new List<Sprite>();
        //   private Text txtTips;
        int count_sprite = 0;
        public override void init()
        {
            instance = this;
            //Sprite[] l = new Sprite[2];
            //l[0] = GAMEAPI.ABUI_LoadSprite("loading_1");
            //l[1] = GAMEAPI.ABUI_LoadSprite("loading_2");
            //bglist.AddRange(l);
            count_sprite = XMLMgr.instance.GetSXML("comm.loading").getInt("num");
            go_info = this.transform.FindChild("info").gameObject;
            go_info.SetActive(true);
            loadingtext = this.getTransformByPath("info/loadingBar/Text").GetComponent<Text>();
			loadingMc = this.getTransformByPath("info/loadingBar").GetComponent<Image>();
            //   txtTips = this.getTransformByPath("txtTip").GetComponent<Text>();
            bgimage = this.transform.FindChild("info/bg_r").GetComponent<Image>();

            if (cemaraRectTran == null)
                cemaraRectTran = GameObject.Find("Canvas_overlay").GetComponent<RectTransform>();

            RectTransform cv = cemaraRectTran;

            RectTransform bg = this.getTransformByPath("info/bg_r").GetComponent<RectTransform>();
            bg.sizeDelta = new Vector2(cv.rect.width, cv.rect.height);


			var xml = XMLMgr.instance.GetSXML("tips");
			var lxml = xml.GetNodeList("t");
			foreach (var v in lxml) {
				var ss = v.getString("info");
				tips.Add(ss);
			}
			tiptext = this.getComponentByPath<Text>("info/tip/bg/text");

            //解决有些手机第一次显示不出来的bug
            this.gameObject.SetActive(true);
            this.transform.localScale = this.transform.localScale;
            //CancelInvoke("showui_phone");
            //Invoke("showui_phone", 0.1f);
        }

        public void showui_phone()
        {
            if(isshow)
                this.gameObject.SetActive(true);
        }

        public void setPercent(float cur,float max)
        {
			//float per = cur / max;
			//if (per > 1f)
			//    per = 1f;

			//loadingtext.text = Mathf.Floor(per * 100) + "%";
			//loadingMc.fillAmount = per;
			
        }

        public void setTips(string str)
        {
       //     txtTips.text = str;
        }

		public override void onShowed() {
			base.onShowed();
			RandomTip();
            Randombg();

            isshow = this;

            StartCoroutine(loading());
        }

        float cur = 0;
        bool b_close = false;
        bool b_load = true;
        IEnumerator loading()
        {
            loadingMc.fillAmount = 0;
            loadingtext.text = 0 + "%";
            cur = 0;
            b_close = false;
            bool waiting = true; 
            while (waiting)
            {
                if (b_close && b_load)
                {
                    if (cur >= 50)
                        cur = cur + 8f;
                    else
                        cur = cur + 4f;

                    if (cur >= 100)
                    {
                        InterfaceMgr.getInstance().close(InterfaceMgr.MAP_LOADING);
                        waiting = false;
                        yield break;
                    }
                }
                else
                {
                    if (cur > 97)
                        cur = 98;
                    else if (cur >= 50)
                        cur = cur + 0.3f;
                    else
                        cur = cur + 3;
                }
                
                loadingMc.fillAmount = cur/100;
                loadingtext.text = Math.Ceiling((double)cur) + "%";
                yield return new WaitForEndOfFrame();
            }
        }

        void Randombg()
        {
           bgimage.sprite = GAMEAPI.ABUI_LoadSprite("loading_"+ UnityEngine.Random.Range(1,count_sprite+1));//bglist[UnityEngine.Random.Range(0, bglist.Count)];
        }
		void RandomTip() {
			tiptext.text = tips[UnityEngine.Random.Range(0, tips.Count)];
		}

        public override void onClosed()
        {
            base.onClosed();
            isshow = null;

        }

        public void closeLoadWait(float time)
        {
            CancelInvoke("wait_close");
            Invoke("wait_close", time);
        }

        void wait_close()
        {
            b_close = true;
            //关闭第一次预加载的ui
            InterfaceMgr.getInstance().closeUiFirstTime();
        }


        public void loadingUi(List<string> m_first_ui, List<string> m_first_ui_lua, bool b_proxy = false)
        {
            b_load = false;
            StartCoroutine(openFirstUi(m_first_ui, m_first_ui_lua, b_proxy));
        }

        IEnumerator openFirstUi(List<string> m_first_ui, List<string> m_first_ui_lua, bool b_proxy = false)
        {
            //if (debug.instance != null)
            //    debug.instance.showMsg(ContMgr.getOutGameCont("debug5"),2);

            for (int i = 0; i < m_first_ui_lua.Count; i++)
            {
                InterfaceMgr.openByLua(m_first_ui_lua[i]);
                //yield return new WaitForSeconds(0.1f);
                yield return null;
            }

            for (int i = 0; i < m_first_ui.Count; i++)
            {
                InterfaceMgr.getInstance().ui_async_open(m_first_ui[i]);
                //yield return new WaitForSeconds(0.1f);
                yield return null;
            }

            if (b_proxy)
                initproxy_ui();
        }
        public void initproxy_ui()
        {
            b_load = true;
            IconHintMgr.getInsatnce().inituiisok = true; ;
            IconHintMgr.getInsatnce().initui();
            //关闭第一次预加载的ui
            InterfaceMgr.getInstance().closeUiFirstTime();


            a3_activeOnlineProxy.getInstance().SendProxy(1);
            A3_signProxy.getInstance().sendproxy(1, 0);
            A3_SevenDayProxy.getInstance().SendProcy(1);
            LotteryProxy.getInstance().sendlottery((int)LotteryType.CurrentInfo);
            ExchangeProxy.getInstance().GetExchangeInfo();
            welfareProxy.getInstance().sendWelfare(welfareProxy.ActiveType.selfWelfareInfo);
            A3_AwardCenterServer.getInstance().SendMsg( A3_AwardCenterServer.SERVER_SELFDATA );//福利数据

            //新手进主城检测摄像机
            SceneCamera.CheckLoginCam();
        }
    }
}
