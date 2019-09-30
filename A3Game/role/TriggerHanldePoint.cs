using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
namespace MuGame
{

    public class TriggerHanldePoint : MonoBehaviour
    {

        public int type = 0;
        public List<int> paramInts = new List<int>();
        public List<float> paramFloat = new List<float>();
        public List<GameObject> paramGo = new List<GameObject>();
        public List<string> paramStr = new List<string>();
        public bool paramBool;

        public int waitCodesid;
        public int dialogid;


        public static List<GameObject> lGo;

        void Start() {

        }

        virtual public void onTriggerHanlde()
        {
            //  MonsterMgr._inst.AddMonster(transform);

            if (type == 1)
            {
                if (paramInts.Count == 0)
                    return;


                MonsterMgr._inst.AddMonster(paramInts[0], transform.position, 0, transform.localEulerAngles.y);
            }
            else if (type == 2)//enable
            {
                if (paramGo.Count == 0)
                    return;

                foreach (GameObject go in paramGo)
                {
                    go.SetActive(true);
                }
            }
            else if (type == 3)//unenable
            {
                if (paramGo.Count == 0)
                    return;

                foreach (GameObject go in paramGo)
                {
                    HiddenItem hideitem = go.GetComponent<HiddenItem>();
                    if (hideitem != null)
                        hideitem.hide();
                    else
                        go.SetActive(false);
                }
            }
            else if (type == 4)//nav
            {
                if (paramInts.Count == 0)
                    return;

                int idx = 0;
                for (int i = 0; i < paramInts.Count; i++)
                {
                    idx += NavmeshUtils.listARE[paramInts[i]];
                }

                if (idx == 0)
                    return;

                SelfRole._inst.setNavLay(idx);
            }
            else if (type == 5)
            {
                if (paramGo.Count == 0)
                    return;

                Transform con = paramGo[0].transform;
                for (int i = 0; i < con.childCount; i++)
                {
                    GameObject tempgi = con.GetChild(i).gameObject;
                    tempgi.AddComponent<BrokenIten>();
                }
            }
            else if (type == 6)
            {
                if (paramGo.Count == 0)
                    return;

                if (paramFloat.Count == 0)
                    return;

                if (paramBool)
                {
                    InterfaceMgr.getInstance().changeState(InterfaceMgr.STATE_HIDE_ALL);
                    a1_gamejoy.inst_joystick.stopDrag();
                }

                paramGo[1].SetActive(true);
                SceneCamera.changeAniCamera(paramGo[0], paramFloat[0]);
            }

            else if (type == 8)
            {
                if (paramGo.Count == 0)
                    return;

                paramGo[0].SetActive(true);

                HiddenItem hideitem = paramGo[0].GetComponent<HiddenItem>();
                if (hideitem != null)
                    hideitem.hide();
                else
                    paramGo[0].SetActive(false);
            }
            else if (type == 9)
            {
                if (paramFloat.Count < 2)
                    return;
                if (paramInts.Count < 1)
                    return;

                SceneCamera.cameraShake(paramFloat[0], paramInts[0], paramFloat[1]);
            }
            else if (type == 10)//eff
            {
                if (paramFloat.Count < 1)
                    return;
                if (paramGo.Count < 1)
                    return;
                GameObject tempGo = GameObject.Instantiate(paramGo[0]) as GameObject;

                tempGo.transform.SetParent(SelfRole._inst.m_curModel, false);
                Destroy(tempGo, paramFloat[0]);
            }
            else if (type == 11)//dialog
            {
                if (dialogid <= 0)
                    return;
                SXML ncpxml = XMLMgr.instance.GetSXML("dialog");
                SXML xml = ncpxml.GetNode("dialog", "id==" + dialogid);
                List<string> lstr = new List<string>();
                if (xml != null)
                {
                    List<SXML> l = new List<SXML>();
                    l = xml.GetNodeList("log");
                    foreach (SXML s in l)
                    {
                        lstr.Add(s.getString("value"));
                    }
                }
                List<string> ldialog = lstr;

                NpcRole npc = null;
                if (paramGo != null && paramGo.Count > 0 && paramGo[0] != null)
                    npc = paramGo[0].GetComponent<NpcRole>();

                DoAfterMgr.instacne.addAfterRender(() =>
                {

                    dialog.showTalk(ldialog, null, npc);
                });


            }
            else if (type == 12)//act
            {
                if (paramStr.Count < 1)
                    return;

                if (paramGo == null || paramGo.Count == 0 || paramGo[0] == null)
                    SelfRole._inst.m_curAni.SetTrigger(paramStr[0]);
                else
                {
                    Animator anim = paramGo[0].GetComponent<Animator>();
                    if (anim != null)
                        anim.SetTrigger(paramStr[0]);
                }

            }
            else if (type == 13)//newbiecode
            {
                if (waitCodesid == 0 )
                    return;


                NewbieTeachMgr.getInstance().add(1,waitCodesid, -1);
            }
            else if (type == 14)//floatui
            {
                if (!paramBool)
                {
                    //InterfaceMgr.getInstance().floatUI.transform.localScale = Vector3.zero;
                    InterfaceMgr.getInstance().changeState(InterfaceMgr.STATE_STORY);
                    if (a1_gamejoy.inst_joystick != null)
                        a1_gamejoy.inst_joystick.OnDragOut();
                }
                else
                {
                    InterfaceMgr.getInstance().changeState(InterfaceMgr.STATE_NORMAL);
                    //InterfaceMgr.getInstance().floatUI.transform.localScale = Vector3.one;
                }

            }
            else if (type == 15)//gos
            {
                lGo = paramGo;
            }
            else if (type == 16)//group
            {
                if (paramGo.Count == 0)
                    return;

                foreach (GameObject go in paramGo)
                {
                    TriggerHanldePoint tri = go.GetComponent<TriggerHanldePoint>();
                    if (tri != null)
                        tri.onTriggerHanlde();
                }
            }
            else if (type == 17)
            {
                //延迟0.5秒关闭loading界面。解决avatar一闪和一些初始化的东西卡住的问题
                if (maploading.instance != null)
                    maploading.instance.closeLoadWait(0.5f);
            }
        }
    }
}
