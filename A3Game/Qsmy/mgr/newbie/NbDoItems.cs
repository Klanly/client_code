using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
namespace MuGame
{
    class NbDoItems
    {


        public static void showWithNorlCamera(object[] objs, Action forceDO)
        {
            showTeachWin(objs, forceDO, 0);
        }

        public static void showWithOutClick(object[] objs, Action forceDo)
        {
            GameObject trans = GameObject.Find(objs[1] as string);
            if (trans == null)
            {
                Debug.LogError("新手脚本错误：找不到该元件：" + objs[1]);
                NewbieModel.getInstance().hide();
                return;
            }

            RectTransform rect = trans.transform.GetComponent<RectTransform>();

            NewbieModel.getInstance().show(rect.position, rect.sizeDelta, objs[2] as string, false, objs[3] as string, null, 0);
            NewbieModel.getInstance().curItem.hideMarkClick();
        }

        public static void showWithClick(object[] objs, Action forceDo)
        {
            GameObject trans = GameObject.Find(objs[1] as string);
            if (trans == null)
            {
                Debug.LogError("新手脚本错误：找不到该元件：" + objs[1]);
                NewbieModel.getInstance().hide();
                return;
            }
            NewbieModel.getInstance().showTittle(objs[1] as string, forceDo);
        }

        public static void showWithOutAvatar(object[] objs, Action forceDo)
        {
            GameObject trans = GameObject.Find(objs[1] as string);
            if (trans == null)
            {
                Debug.LogError("新手脚本错误：找不到该元件：" + objs[1]);
                NewbieModel.getInstance().hide();
                return;
            }
            RectTransform rect = trans.transform.GetComponent<RectTransform>();
            NewbieModel.getInstance().showWithoutAvatar(rect.position, rect.sizeDelta, objs[2] as string, forceDo);
        }

        public static void showWithNext(object[] objs, Action forceDo)
        {
            GameObject trans = GameObject.Find(objs[1] as string);
            if (trans == null)
            {
                Debug.LogError("新手脚本错误：找不到该元件：" + objs[1]);
                NewbieModel.getInstance().hide();
                return;
            }
            RectTransform rect = trans.transform.GetComponent<RectTransform>();
            NewbieModel.getInstance().showNext(rect.position, rect.sizeDelta, objs[2] as string, int.Parse(objs[3].ToString()), forceDo);
        }

        public static void showTeachWin(object[] objs, Action forceDO, int cameraType = 0)
        {

            GameObject trans = GameObject.Find(objs[1] as string);
            if (trans == null)
            {
                // Time.timeScale = 0; 
                Debug.LogError("新手脚本错误：找不到该元件：" + objs[1]);
                NewbieModel.getInstance().hide();
                return;
            }

            RectTransform rect = trans.transform.GetComponent<RectTransform>();

            if (objs.Length == 3)
                NewbieModel.getInstance().show(rect.position, rect.sizeDelta, objs[2] as string, false, "", null, cameraType);
            else if (objs.Length == 4)
            {
                NewbieModel.getInstance().show(rect.position, rect.sizeDelta, objs[2] as string, cameraType == 1, objs[3] as string, forceDO, cameraType);
            }
            else if (objs.Length == 5)
            {
                NewbieModel.getInstance().show(rect.position, rect.sizeDelta, objs[2] as string, cameraType == 1, objs[3] as string, forceDO, cameraType, int.Parse(objs[4] as string) != 1);
            }
        }

        public static void closeAllwin(object[] objs, Action forceDo)
        {
            if (objs.Length >= 2)
                InterfaceMgr.getInstance().closeAllWin(objs[1] as string);
            else
                InterfaceMgr.getInstance().closeAllWin();

            MsgBoxMgr.getInstance().hide();
        }




        public static void hideTeachWin(object[] objs, Action forceDo)
        {
            NewbieModel.getInstance().hide();
        }


        public static void stopMove(object[] objs, Action forceDo)
        {
            if (a1_gamejoy.inst_joystick != null)
                a1_gamejoy.inst_joystick.OnDragOut(null);
        }
        public static void stopmove1(object[] objs, Action forceDo)
        {
            if (a1_gamejoy.inst_joystick != null)
                a1_gamejoy.inst_joystick.OnDragOut_wait();
        }
        public static void hidefloatUI(object[] objs, Action forceDo)
        {
            InterfaceMgr.getInstance().floatUI.transform.localScale = Vector3.zero;
        }

        public static void showfloatUI(object[] objs, Action forceDo)
        {
            InterfaceMgr.getInstance().floatUI.transform.localScale = Vector3.one;
        }

        public static void openSkill(object[] objs, Action forceDo)
        {
            if (objs.Length < 2)
                return;

            if (a1_gamejoy.inst_skillbar != null)
                a1_gamejoy.inst_skillbar.refreshAllSkills(int.Parse(objs[1].ToString()));

        }

        public static void closeexpbar(object[] objs, Action forceDo)
        {
            if (a3_expbar.instance != null)
                a3_expbar.instance.On_Btn_Up();
            InterfaceMgr.doCommandByLua("a1_low_fightgame.setToggle", "ui/interfaces/low/a1_low_fightgame", false);
            //if (a3_liteMinimap.instance != null)
            //    a3_liteMinimap.instance.onTogglePlusClick(true);
        }

        public static void openexpbar(object[] objs, Action forceDo)
        {
            if (a3_expbar.instance != null)
                a3_expbar.instance.On_Btn_Down();
            InterfaceMgr.doCommandByLua("a1_low_fightgame.setToggle", "ui/interfaces/low/a1_low_fightgame", true);
            //if (a3_liteMinimap.instance != null)
            //    a3_liteMinimap.instance.onTogglePlusClick(false);
        }

        public static void skillDraw(object[] objs, Action forceD)
        {
            skill_a3.show_teack_ani = true;
            if (skill_a3._instance != null)
                skill_a3._instance.showTeachAni(true);
        }

        public static void showTeachLine(object[] objs, Action forceDo)
        {
            teachline.show(objs[1].ToString(), float.Parse(objs[2].ToString()));
        }


        public static void showObj(object[] objs, Action forceDo)
        {
            if (objs.Length < 3)
                return;

            if (int.Parse(objs[2].ToString()) == 1)
            {

                DoAfterMgr.instacne.addAfterRender(() =>
                {
                    GameObject temp2 = TriggerHanldePoint.lGo[int.Parse(objs[1].ToString())];
                    debug.Log(":::::::temp2::::::" + temp2);
                    temp2.SetActive(true);
                });
                return;
            }


            GameObject go = GameObject.Find(objs[1].ToString());
            if (go == null)
            {
                debug.Log("showObj：：未找到" + objs[1].ToString());
                return;
            }

            DoAfterMgr.instacne.addAfterRender(() =>
            {
                go.SetActive(int.Parse(objs[2].ToString()) == 1);

            });

        }

        public static void clearLGo(object[] objs, Action forceDo)
        {
            NbDoItems.cacheCameraAni = null;
            if (TriggerHanldePoint.lGo == null)
                return;
            TriggerHanldePoint.lGo.Clear();
            TriggerHanldePoint.lGo = null;
        }

        public static void endNewbie(object[] objs, Action forceDo)
        {

            PlayerModel.getInstance().LeaveStandalone_CreateChar();
        }


        public static void playact(object[] objs, Action forceDo)
        {
            if (objs.Length < 3)
                return;

            GameObject go = null;
            if (objs[1].ToString() == "#usr")
                go = SelfRole._inst.m_curModel.gameObject;
            else
            {
                if (GameObject.Find(objs[1].ToString()).transform.childCount > 0)
                    go = GameObject.Find(objs[1].ToString()).transform.GetChild(0)?.FindChild("model").gameObject;
                else
                    go = null;
            }
                
                
            if (go == null)
                return;

            Animator anim = go.GetComponent<Animator>();
            if (anim == null)
                return;
            anim.SetTrigger(objs[2].ToString());
        }

        public static void playEff(object[] objs, Action forceDo)
        {
            if (objs.Length < 3)
                return;





            GameObject go = null;
            if (objs[1].ToString() == "#usr")
                go = SelfRole._inst.m_curModel.gameObject;
            else
                go = GameObject.Find(objs[1].ToString()).transform.GetChild(0).FindChild("model").gameObject;
            if (go == null)
                return;

            GameObject res = GAMEAPI.ABFight_LoadPrefab(objs[2].ToString());
            if (res == null)
            {
                Debug.LogError("playEff failed " + objs[2].ToString());
                return;
            }

            GameObject eff = GameObject.Instantiate(res) as GameObject;
            eff.transform.SetParent(go.transform, false);
            GameObject.Destroy(eff, 3);





        }

        public static Animator cacheCameraAni;
        public static void playCameani(object[] objs, Action forceDo)
        {


            cacheCameraAni.speed = float.Parse(objs[1].ToString());
            NbDoItems.cacheCameraAni = null;
        }

        public static void endStory(object[] objs, Action forceDo)
        {
            SceneCamera.endStory(float.Parse(objs[1].ToString()));
        }


        public static void ContinueDo(object[] objs, Action forceDo)
        {
            DoAfterMgr.instacne.addAfterRender((Action)(() =>
            {
                InterfaceMgr.getInstance().ui_async_open((string)InterfaceMgr.DIALOG);
            }));
        }

        public static void showDialog(object[] objs, Action forceDo)
        {
            NpcRole npc = null;

            GameObject go = GameObject.Find(objs[1].ToString());
            if (go == null)
                return;

            npc = go.GetComponent<NpcRole>();

            if (npc == null)
                return;

            dialog.showTalk(new List<string> { objs[2].ToString() }, null, npc, true);
        }

    }
}
