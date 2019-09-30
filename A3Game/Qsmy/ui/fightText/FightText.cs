using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using Cross;
using GameFramework;
using UnityEngine;
using UnityEngine.UI;
namespace MuGame
{
    public class FightText
    {
        static public readonly string WARRIOR_TEXT = "ani_fightingText_warriortext";
        static public readonly string CRIT_TEXT = "ani_fightingText_crittext";
        static public readonly string MAGE_TEXT = "ani_fightingText_magetext";
        static public readonly string ASSI_TEXT = "ani_fightingText_assitext";
        static public readonly string ENEMY_TEXT = "ani_fightingText_enemy";
        static public readonly string HEAL_TEXT = "ani_fightingText_healtext";
        static public readonly string MONEY_TEXT = "ani_fightingText_money1";
        static public readonly string MISS_TEXT = "ani_fightingText_miss";
        static public readonly string SHEILD_TEXT = "ani_fightingText_warriortext";
        static public readonly string IMG_TEXT = "ani_fightingText_Imgtext";
        static public readonly string BUFF_TEXT = "ani_fightingText_Bufftext";
        static public readonly string ADD_IMG_TEXT = "ani_fightingText_Add_Imgtext";
        static public readonly string IMG_TEXT_2 = "ani_fightingText_Imgtext2";

        static public readonly string ZHS_TEXT_PG = "ani_fightingText_magetext_zhs";
        static public readonly string ZHS_TEXT_ZM = "ani_fightingText_Imgtext_zhs";
        static public readonly string ZHS_TEXT_JN = "ani_fightingText_Skilltext_zhs";

        //static public string userText="";
        static public string userText = "ani_fightingText_enemy";//临时处理jason

        static public readonly string MOUSE_POINT = "ani_mousetouch_mouse_pointa";

        static private Dictionary<String, List<FightTextTempSC>> normalFightTextPool = new Dictionary<String, List<FightTextTempSC>>();

        static private List<FightTextTempSC> playingPool = new List<FightTextTempSC>();

        static public BaseRole CurrentCauseRole { set; get; } // 引发此次飘字的对象
        static private List<Vector3> offsetPos;
        static private int posIdx = 0;
        static private TickItem process;
        static public void play(String id, Vector3 pos, int num, bool criatk = false, int type = -1 , string skill = null, INameObj avatar= null)
        {
            //战斗飘字高度在头顶位置的基础上提高屏幕的10分之1
            pos.y = pos.y + Screen.height/10;
            //return;

            //if (id == HERO_TEXT)
            //{
            //    debug.Log("aa");
            //}

            //if (offsetPos == null)
            //{
            //    offsetPos = new List<Vector3>();
            //    Vector3 v;
            //    for (int i = 0; i < 20; i++)
            //    {
            //        v = new Vector3(ConfigUtil.getRandom(-30, 30), ConfigUtil.getRandom(-30, 10), 0);
            //        offsetPos.Add(v);
            //    }
            //}


            if (instacne == null)
            {

                instacne = GameObject.Find("fightText").transform;
                process = new TickItem(onUpdate);
                TickMgr.instance.addTick(process);

                mousePointCon = GameObject.Find("mouseTouchAni").transform;
                InterfaceMgr.setUntouchable(mousePointCon.gameObject);
            }


            if (!normalFightTextPool.ContainsKey(id + type))
                normalFightTextPool[id + type] = new List<FightTextTempSC>();

            List<FightTextTempSC> l = normalFightTextPool[id + type];
            GameObject root;
            FightTextTempSC sc;
            if (l.Count == 0)
            {
                GameObject commonUIPrefab = GAMEAPI.ABUI_LoadPrefab(id);

                root = GameObject.Instantiate(commonUIPrefab) as GameObject;
                if (type != -1)
                {
                    Sprite icon_Image = GAMEAPI.ABUI_LoadSprite("icon_rune_fight_" + type);
                    if (icon_Image != null && U3DAPI.DEF_SPRITE != icon_Image)
                        root.transform.FindChild("Text/Image").GetComponent<Image>().sprite = icon_Image;
                    else
                    {
                        GameObject.Destroy(root);
                        return;
                    }
                }
                sc = root.GetComponent<FightTextTempSC>();
                if (sc == null) sc = root.AddComponent<FightTextTempSC>();

                if (id == MOUSE_POINT || id == MISS_TEXT)
                {
                    // root.transform.SetParent(mousePointCon, false);
                    root.transform.SetParent(instacne, false);
                    sc.init(FightTextTempSC.TYPE_ANI);

                }
                else
                {
                    root.transform.SetParent(instacne, false);
                    sc.init(FightTextTempSC.TYPE_TEXT);
                }
                sc.pool = l;
                sc.playingPool = playingPool;
            }
            else
            {
                sc = l[0];
                sc.setActive(true);
                l.RemoveAt(0);
            }
            sc.timer = Time.time;
            playingPool.Add(sc);
            //Vec3 v = imgnum.changePot(m_char.pos + new Vec3(0, 1.8f, 0));
            //imgnum.x = v.x;
            //imgnum.y = 720 - v.y;
            //imgnum.num = num + "";
            //  Camera.main.WorldToScreenPoint(v3)
            if (skill != null)
            {//飘技能字
                sc.transform.FindChild("Text/skill").GetComponent<Text>().text = skill;
            }
            Vector2 offsetByHit = Vector2.one;
            if (CurrentCauseRole != null)
            {
                Vector3 originPos;
                try {
                    originPos = GRMap.GAME_CAM_CAMERA.WorldToScreenPoint(CurrentCauseRole.m_curModel.position);
                }
                catch (Exception) { // --此异常通常由分辨率突然变更引起
                    return;
                }
                offsetByHit = new Vector2(
                        x: (originPos.x - pos.x) > 0 ? 1 : -1,
                        y: (originPos.y - pos.y) > 0 ? 1 : -1
                );
            }
            sc.ani.SetFloat("HorizontalValue", offsetByHit.x);
            sc.ani.SetFloat("VerticalValue", offsetByHit.y);
            if (avatar != null) {
                sc._avatar = avatar;
            }
            sc.play(pos, num, criatk);

            //sc.play(pos + (id == MOUSE_POINT ? Vector3.zero : offsetPos[posIdx]), num, criatk);

            posIdx++;
            if (posIdx >= 20)
                posIdx = 0;
        }
        static public void play1(String id, Vector3 pos, int num, bool criatk = false, int type = -1)
        {
            if (offsetPos == null)
            {
                offsetPos = new List<Vector3>();
                Vector3 v;
                for (int i = 0; i < 20; i++)
                {
                    v = new Vector3(ConfigUtil.getRandom(-30, 30), ConfigUtil.getRandom(-30, 10), 0);
                    offsetPos.Add(v);
                }
            }
            if (instacne == null)
            {
                instacne = GameObject.Find("fightText").transform;
                process = new TickItem(onUpdate);
                TickMgr.instance.addTick(process);

                mousePointCon = GameObject.Find("mouseTouchAni").transform;
                InterfaceMgr.setUntouchable(mousePointCon.gameObject);
            }
            GameObject root;
            FightTextTempSC sc;
            GameObject commonUIPrefab = GAMEAPI.ABUI_LoadPrefab(id);
            root = GameObject.Instantiate(commonUIPrefab) as GameObject;
            if (type != -1)
            {
                Sprite icon_Image = GAMEAPI.ABUI_LoadSprite("icon_rune_fight_" + type);
                if (icon_Image != null)
                    root.transform.FindChild("Text/Image").GetComponent<Image>().sprite = icon_Image;
            }
            sc = root.GetComponent<FightTextTempSC>();
            if (sc == null) sc = root.AddComponent<FightTextTempSC>();
            if (id == MOUSE_POINT || id == MISS_TEXT)
            {
                // root.transform.SetParent(mousePointCon, false);
                root.transform.SetParent(instacne, false);
                sc.init(FightTextTempSC.TYPE_ANI);

            }
            else
            {
                root.transform.SetParent(instacne, false);
                sc.init(FightTextTempSC.TYPE_TEXT);
            }
            sc.play(pos + (id == MOUSE_POINT ? Vector3.zero : offsetPos[posIdx]), num, criatk);
            posIdx++;
            if (posIdx >= 20)
                posIdx = 0;
        }

        public static void clear()
        {
            if (instacne == null)
                return;
            FightTextTempSC t;
            for (int i = 0; i < playingPool.Count; i++)
            {
                t = playingPool[i];
                t.onAniOver();
                i--;
            }
        }

        private static Transform instacne;

        private static Transform mousePointCon;

        private static float tick;
        private static void onUpdate(float s)
        {
            //  tick++;
            //if (tick < 30)
            //    return;
            float timer = Time.time;
            FightTextTempSC t;
            for (int i = 0; i < playingPool.Count; i++)
            {
                t = playingPool[i];
                if (timer - t.timer > 2)
                {
                    t.onAniOver();
                    i--;
                }
            }


        }
    }
}
