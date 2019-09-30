//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using GameFramework;
//using UnityEngine;
//using UnityEngine.UI;

//using System.Collections;
//namespace MuGame
//{
//    class plot_linkui : StoryUI
//    {
//        static public plot_linkui instant = null;

//        static public Dictionary<string, GameObject> s_plotui_Linker = new Dictionary<string, GameObject>();

//        static public void show(String plotui = "")
//        {
//            if (instant == null) return;

//            //如果已经存在就要关闭
//            if (s_plotui_Linker.ContainsKey(plotui))
//            {
//                Destroy(s_plotui_Linker[plotui]);
//                s_plotui_Linker.Remove(plotui);
//            }
//            else
//            {
//                GameObject plot_ui_pre = U3DAPI.U3DResLoad("plotui/" + plotui) as GameObject;
//                if (plot_ui_pre != null)
//                {
//                    GameObject pui = GameObject.Instantiate(plot_ui_pre) as GameObject;
//                    pui.transform.SetParent(instant.transform, false);

//                    s_plotui_Linker.Add(plotui, pui);
//                }
//            }
//        }

//        static public void ClearAll()
//        {
//            instant = null;
//            s_plotui_Linker.Clear();
//        }

//        public override void onShowed()
//        {
//            instant = this;
//            base.onShowed();
//        }

//        public override void onClosed()
//        {
//            instant = null;
//            base.onClosed();
//        }
//    }
//}
