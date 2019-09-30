using System.Collections.Generic;
using GameFramework;
using UnityEngine.UI;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
namespace MuGame
{
    public class debug : LoadingUI
    {
        public static int count = 0;//用于控制帧率的显示速度的count
        private static float milliSecond = 0;//毫秒数
        public static float fps = 0;//帧率值
        private static float deltaTime = 0.0f;//用于显示帧率的deltaTime

        public  bool monstersPos = false;
        public static debug instance;

        public static bool show_debug = false;
        public static void Log(string msg)
        {
            if(show_debug)
                Debug.Log(msg);
        }

        public static void initLog()
        {
            if (PlayeLocalInfo.checkKey(PlayeLocalInfo.DEBUG_SHOW))
                show_debug = PlayeLocalInfo.loadInt(PlayeLocalInfo.DEBUG_SHOW) == 1 ? true : false;
            else
                show_debug = true;
        }

        Text txt;
        GameObject msg;
        Text msg_txt;
        public override void init()
        {
            alain();
            txt = this.getComponentByPath<Text>("Text");
            txt.text = "";
            msg = this.getGameObjectByPath("msg");
            msg_txt = msg.transform.FindChild("txt").GetComponent<Text>();

            initLog();
        }
        public void changetxt()
        {
            if(txt.gameObject.activeSelf)
                txt.gameObject.SetActive(false);
            else
                txt.gameObject.SetActive(true);
        }
        public override void onShowed()
        {
            instance = this;
            base.onShowed();
            txt.gameObject.SetActive(false);
            msg.SetActive(false);
        }

        public override void onClosed()
        {
            instance = null;
            base.onClosed();
        }

        public void showMsg(string txt, float time = 1.0f)
        {
            transform.SetAsLastSibling();
            msg.SetActive(true);
            msg_txt.text = txt;

            CancelInvoke("hideMsg");
            Invoke("hideMsg", time);
        }

        public void hideMsg()
        {
            msg.SetActive(false);
        }

        void Update()
        {
            //return;
            //if (Globle.DebugMode == 2 || Globle.isHardDemo)
            //{
                deltaTime += (Time.deltaTime - deltaTime) * 0.1f;
                //左上方帧数显示
                if (++count > 10)
                {
                    count = 0;
                    milliSecond = deltaTime * 1000.0f;
                    fps = 1.0f / deltaTime;
                }
               string text = string.Format(" 当前每帧渲染间隔：{0:0.0} ms ({1:0.} 帧每秒)", milliSecond, fps);
                if (SelfRole._inst != null && SelfRole._inst.m_curModel!=null)
                {
                    int px = (int)(SelfRole._inst.m_curModel.position.x * 53.3);
                    int py = (int)(SelfRole._inst.m_curModel.position.z * 53.3);
                    int cx = px/32;
                     int cy = py/32;

                     text += "\n pos:" + SelfRole._inst.m_curModel.position.x + "(" + px + "  " + cx + ")" + SelfRole._inst.m_curModel.position.z + "(" + py +"  "+ cy+ ")";

                    if (monstersPos)
                    {
                        Dictionary<uint, LGAvatarMonster> d = LGMonsters.instacne.getMons();
                        foreach (LGAvatarMonster mon in d.Values)
                        {
                            text += "\n monsterpos iid:" + mon.iid+" x:" + mon.x + " y:" + mon.y ;
                        }
                    }

                }

                txt.text = text;
            //}
            //else
            //{
            //    gameObject.SetActive(false);
            //}

        }
        
    }
}
