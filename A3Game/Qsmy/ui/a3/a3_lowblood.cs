using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameFramework;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using MuGame;
using Cross;
namespace MuGame
{
    class a3_lowblood : FloatUi
    {
        static public a3_lowblood instance;

        Image ig_blood;
        Animator m_ani;
        GameObject assassin_fx;
        GameObject net_link;
        bool m_hurt = false;
        public override void init()
        {
            inText();
            ig_blood = transform.FindChild("blood").GetComponent<Image>();
            m_ani = transform.GetComponent<Animator>();
            assassin_fx = transform.FindChild("FX_yingsheng").gameObject;
            net_link = transform.FindChild("net_link").gameObject;

            instance = this;
            gameObject.SetActive(false);
            assassin_fx.SetActive(false);
            ig_blood.gameObject.SetActive(false);
            net_link.gameObject.SetActive(false);

            //网络连接提示
            SXML xml = XMLMgr.instance.GetSXML("comm.wifi_notice");
            link_notice_time = xml.getInt("time_notice");
            link_lose_time = xml.getInt("time_lose");
            link_replace_time = xml.getInt("time_replace");

            CancelInvoke("ongetPing");
            InvokeRepeating("ongetPing", 0, link_replace_time);
        }

        void inText()
        {
            this.transform.FindChild("net_link/Text").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_lowblood_1");// 正在连接服务器，请稍候
        }

        public float time = 0;
        public float link_notice_time = 5;
        public float link_lose_time = 15;
        public float link_replace_time = 5; //发包间隔
        void ongetPing()
        {
            if (time >= link_notice_time)
            {
                gameObject.SetActive(true);
                net_link.gameObject.SetActive(true);
            }
            else if (time >= link_lose_time)
            {
                //gameObject.SetActive(false);
                net_link.gameObject.SetActive(false);
                InterfaceMgr.getInstance().ui_async_open(InterfaceMgr.DISCONECT);
            }
            else
            {
                //gameObject.SetActive(false);
                net_link.gameObject.SetActive(false);
            }

            GeneralProxy.getInstance().sendGetPing();
            time = time + link_replace_time;
        }

        public void refreshLinkTime()
        {
            time = 0;
        }


        public void begin_assassin_fx()
        {
            gameObject.SetActive(true);
            assassin_fx.SetActive(true);
        }

        public void end_assassin_fx()
        {
            //gameObject.SetActive(false);
            assassin_fx.SetActive(false);
        }

        public void begin()
        {
            if (m_hurt)
                return;

            transform.SetAsLastSibling();
            ig_blood.transform.GetComponent<CanvasGroup>().blocksRaycasts = false;
            m_ani.enabled = true;
            ig_blood.color = Color.red;
            ig_blood.gameObject.SetActive(true);
            gameObject.SetActive(true);

            m_hurt = true;

            CancelInvoke("onshowend");
            Invoke("onshowend", 5);

            CancelInvoke("onhurt");
            Invoke("onhurt", 10);
        }
        public void end()
        {
            m_ani.enabled = false;
            //gameObject.SetActive(false);
            ig_blood.gameObject.SetActive(false);
        }

        void onhurt()
        {
            m_hurt = false;
        }
        void onshowend()
        {
            end();
        }
    }
}
