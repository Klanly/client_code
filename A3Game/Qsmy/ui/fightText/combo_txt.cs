
//using GameFramework;
//using UnityEngine.UI;
//using System.Collections;
//using UnityEngine;
//using Cross;
//using DG.Tweening;
//namespace MuGame
//{
//    public class combo_txt : FightTextLayer
//    {

//        private Text txt;
//        private RectTransform rect;
//        processStruct process;
//        override public void init()
//        {
//            alain();
//            txt = this.getComponentByPath<Text>("Text");
//            rect = this.getComponentByPath<RectTransform>("Text");
//            process = new processStruct(onUpdate, "fb_main");

//        }

//        public override void onShowed()
//        {
//            showed = true;
//            comboNum = 1;
//            lastNum = -1;
//            (CrossApp.singleton.getPlugin("processManager") as processManager).addProcess(process);
//            InterfaceMgr.setUntouchable(this.gameObject);
//        }

//        public override void onClosed()
//        {
//            showed = false;
//            (CrossApp.singleton.getPlugin("processManager") as processManager).removeProcess(process);
//        }

//        public static void clear()
//        {
//            curTick = -1;
//        }

//        private int lastNum = -1;
//        void onUpdate(float s)
//        {
//            curTick--;
//            if (curTick < 0)
//            {
//                curTick = 0;
//                InterfaceMgr.getInstance().close(InterfaceMgr.COMBO_TEXT);
//            }

//            if (lastNum < comboNum)
//            {
//                lastNum++;
//                refresh();
//            }

//        }
//        Vector3 vec = new Vector3(2, 2, 2);
//        void refresh()
//        {
//            txt.text = lastNum.ToString();
//            DOTween.Sequence()
//                //  .AppendInterval(.3f)
//        .Append(rect.DOScale(2, 0.3f))
//          .Append(rect.DOScale(1, 0.3f).SetEase(Ease.OutBack));
//        }


//        private static int curTick = 0;
//        private static int maxTick = 120;
//        private static int comboNum = 0;
//        private static bool showed = false;
//        public static void play()
//        {
//            if (showed == false)
//                InterfaceMgr.getInstance().open(InterfaceMgr.COMBO_TEXT);
//            curTick = maxTick;
//            comboNum++;


//        }



//    }
//}
