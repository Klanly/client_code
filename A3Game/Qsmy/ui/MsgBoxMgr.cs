using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;
using GameFramework;
using Cross;

namespace MuGame
{
    /***********
     *MsgBox公用类
     * *********/
    public class MsgBoxMgr
    {
        GameObject root;
        GameObject root_dart_awd;
        GameObject root_task_fb;
        GameObject root_swtz_fb;
        Button btn_yes;
        Button btn_no;
        Button btn_dart;
        Button btn_onekey;
        Text text_str;
        Text text_dart;
        UnityAction _call;
        UnityAction _handleNo;


        static private MsgBoxMgr _instance;
        static public MsgBoxMgr getInstance()
        {
            if (_instance == null)
            {
                _instance = new MsgBoxMgr();
                _instance.init();
            }
            return _instance;

        }
        void init()
        {
            Transform showLayer = GameObject.Find("winLayer").transform;

            GameObject iconPrefab = GAMEAPI.ABLayer_LoadNow_GameObject("uilayer_msgbox");
            iconPrefab.transform.FindChild("Text").GetComponent<Text>().text = ContMgr.getCont("msgbox_0");
            iconPrefab.transform.FindChild("yes/Text").GetComponent<Text>().text = ContMgr.getCont("ToSure_summon_1");
            iconPrefab.transform.FindChild("no/Text").GetComponent<Text>().text = ContMgr.getCont("ToSure_summon_2");

            root = GameObject.Instantiate(iconPrefab) as GameObject;
            root.transform.SetParent(showLayer, false);
            text_str = root.transform.FindChild("Text").GetComponent<Text>();
            btn_yes = root.transform.FindChild("yes").GetComponent<Button>();
            BaseButton bsYes = new BaseButton(btn_yes.transform);
            bsYes.onClick = null;
            btn_no = root.transform.FindChild("no").GetComponent<Button>();
            BaseButton bsNo = new BaseButton(btn_no.transform);
            bsNo.onClick = null;

            btn_onekey = root.transform.FindChild("confirm").GetComponent<Button>();
            BaseButton bsOnekey = new BaseButton(btn_onekey.transform);


            bsOnekey.onClick = null;

            btn_yes.onClick.AddListener(onClick);
            btn_no.onClick.AddListener(hide);
            btn_onekey.onClick.AddListener(onClick);
            root.SetActive(false);
            //================
            GameObject dart_get_awd = GAMEAPI.ABLayer_LoadNow_GameObject("uilayer_msgbox_dart");
            dart_get_awd.transform.FindChild("str").GetComponent<Text>().text = ContMgr.getCont("msgbox_dart_0");
            dart_get_awd.transform.FindChild("txt").GetComponent<Text>().text = ContMgr.getCont("msgbox_dart_1");
            dart_get_awd.transform.FindChild("yes/Text").GetComponent<Text>().text = ContMgr.getCont("ToSure_summon_1");

            root_dart_awd = GameObject.Instantiate(dart_get_awd) as GameObject;
            root_dart_awd.transform.SetParent(showLayer, false);
            root_dart_awd.SetActive(false);
            text_dart = root_dart_awd.transform.FindChild("str").GetComponent<Text>();
            btn_dart = root_dart_awd.transform.FindChild("yes").GetComponent<Button>();
            btn_dart.onClick.AddListener(onClick);
            //================
            GameObject task_fb_Prefab = GAMEAPI.ABLayer_LoadNow_GameObject("uilayer_msgbox_task_fb");
            task_fb_Prefab.transform.FindChild("confirm/Text").GetComponent<Text>().text = ContMgr.getCont("msgbox_task_fb_0");
            task_fb_Prefab.transform.FindChild("cancel/Text").GetComponent<Text>().text = ContMgr.getCont("msgbox_task_fb_1");
            root_task_fb = GameObject.Instantiate(task_fb_Prefab) as GameObject;
            root_task_fb.transform.SetParent(showLayer, false);
            root_task_fb.SetActive(false);
            //================
            GameObject swtz_fb_Prefab = GAMEAPI.ABLayer_LoadNow_GameObject("uilayer_msgbox_swtz_fb");
            swtz_fb_Prefab.transform.FindChild("confirm/Text").GetComponent<Text>().text = ContMgr.getCont("msgbox_task_fb_0");
            swtz_fb_Prefab.transform.FindChild("Text").GetComponent<Text>().text = ContMgr.getCont("msgbox_swtz_fb_0");
            swtz_fb_Prefab.transform.FindChild("cancel/Text").GetComponent<Text>().text = ContMgr.getCont("msgbox_task_fb_1");
            swtz_fb_Prefab.transform.FindChild("tuijian").GetComponent<Text>().text = ContMgr.getCont("msgbox_swtz_fb_1");
            root_swtz_fb = GameObject.Instantiate(swtz_fb_Prefab) as GameObject;
            root_swtz_fb.transform.SetParent(showLayer, false);
            root_swtz_fb.SetActive(false);



        }

        public static bool isShow_guide = false;
        public void showConfirmWithContId(string id, UnityAction handleYes, UnityAction handleNo = null, List<string> l = null, int type = 0)
        {
            showConfirm(ContMgr.getCont(id, l), handleYes, handleNo, type);
        }

        public void showConfirm(string str, UnityAction handleYes, UnityAction handleNo = null, int type = 0)
        {
            clear();
            root.transform.SetAsLastSibling();
            text_str.text = str;
            root.SetActive(true);
            if (type == 0)
            {
                btn_yes.gameObject.SetActive(true);
                btn_no.gameObject.SetActive(true);
                btn_onekey.gameObject.SetActive(false);
                _handleNo = handleNo;
                _call = handleYes;
            }
            if (type == 1)
            {
                btn_yes.gameObject.SetActive(false);
                btn_no.gameObject.SetActive(false);
                btn_onekey.gameObject.SetActive(true);
                _handleNo = handleNo;
            }
        }

        public void showTask_fb_confirm(string title, string str, bool guide, UnityAction handleYes, UnityAction handleNo = null)
        {
            root_task_fb.transform.SetAsLastSibling();
            root_task_fb.transform.FindChild("title").GetComponent<Text>().text = title;
            root_task_fb.transform.FindChild("Text").GetComponent<Text>().text = str;
            if (guide)
            {
                isShow_guide = true;
                a3_liteMinimap.instance.setGuide();
                root_task_fb.transform.FindChild("confirm/guide_task_info").gameObject.SetActive(true);
            }
            else
            {
                root_task_fb.transform.FindChild("confirm/guide_task_info").gameObject.SetActive(false);
                isShow_guide = false;
                a3_liteMinimap.instance.setGuide();
            }
            new BaseButton(root_task_fb.transform.FindChild("confirm")).onClick = (GameObject g) =>
            {
                handleYes(); root_task_fb.SetActive(false);
                if (a3_liteMinimap.instance)
                {
                    isShow_guide = false;
                    a3_liteMinimap.instance.setGuide();
                }
            };
            new BaseButton(root_task_fb.transform.FindChild("cancel")).onClick = (GameObject g) =>
            {
                root_task_fb.SetActive(false);
                if (a3_liteMinimap.instance)
                {
                    isShow_guide = false;
                    a3_liteMinimap.instance.setGuide();
                }
            };
            new BaseButton(root_task_fb.transform.FindChild("bg")).onClick = (GameObject g) =>
            {
                root_task_fb.SetActive(false);
                if (a3_liteMinimap.instance)
                {
                    isShow_guide = false;
                    a3_liteMinimap.instance.setGuide();
                }
            };
            new BaseButton(root_task_fb.transform.FindChild("yes")).onClick = (GameObject g) =>
            {
                handleYes(); root_task_fb.SetActive(false);
                if (a3_liteMinimap.instance)
                {
                    isShow_guide = false;
                    a3_liteMinimap.instance.setGuide();
                }
            };
            new BaseButton(root_task_fb.transform.FindChild("no")).onClick = (GameObject g) =>
            {
                handleNo(); root_task_fb.SetActive(false);
                if (a3_liteMinimap.instance)
                {
                    isShow_guide = false;
                    a3_liteMinimap.instance.setGuide();
                }
            };
            root_task_fb.SetActive(true);
        }

        public void showTask_fb_confirm(string title, string str, bool guide, int zhanli, UnityAction handleYes, UnityAction handleNo = null)
        {
            root_swtz_fb.transform.SetAsLastSibling();
            root_swtz_fb.transform.FindChild("title").GetComponent<Text>().text = title;
            root_swtz_fb.transform.FindChild("Text").GetComponent<Text>().text = str;

            root_swtz_fb.transform.FindChild("zhandouli").GetComponent<Text>().text = zhanli.ToString();
            if (guide)
            {
                root_swtz_fb.transform.FindChild("confirm/guide_task_info").gameObject.SetActive(true);
                isShow_guide = true;
                a3_liteMinimap.instance.setGuide();
            }
            else
            {
                root_swtz_fb.transform.FindChild("confirm/guide_task_info").gameObject.SetActive(false);
                isShow_guide = false;
                a3_liteMinimap.instance.setGuide();
            }
            new BaseButton(root_swtz_fb.transform.FindChild("confirm")).onClick = (GameObject g) =>
            {
                handleYes(); root_swtz_fb.SetActive(false);
                if (a3_liteMinimap.instance)
                {
                    isShow_guide = false;
                    a3_liteMinimap.instance.setGuide();
                }
            };
            new BaseButton(root_swtz_fb.transform.FindChild("cancel")).onClick = (GameObject g) =>
            {
                root_swtz_fb.SetActive(false);
                if (a3_liteMinimap.instance)
                {
                    isShow_guide = false;
                    a3_liteMinimap.instance.setGuide();
                }
            };
            new BaseButton(root_swtz_fb.transform.FindChild("bg")).onClick = (GameObject g) =>
            {
                root_swtz_fb.SetActive(false);
                if (a3_liteMinimap.instance)
                {
                    isShow_guide = false;
                    a3_liteMinimap.instance.setGuide();
                }
            };
            new BaseButton(root_swtz_fb.transform.FindChild("yes")).onClick = (GameObject g) =>
            {
                handleYes(); root_swtz_fb.SetActive(false);
                if (a3_liteMinimap.instance)
                {
                    isShow_guide = false;
                    a3_liteMinimap.instance.setGuide();
                }
            };
            new BaseButton(root_swtz_fb.transform.FindChild("no")).onClick = (GameObject g) =>
            {
                handleNo(); root_swtz_fb.SetActive(false);
                if (a3_liteMinimap.instance)
                {
                    isShow_guide = false;
                    a3_liteMinimap.instance.setGuide();
                }
            };
            root_swtz_fb.SetActive(true);
        }

        public void showDartGetAwd(string str,uint item_id,int num,int per)
        {
            _call = null;
            root_dart_awd.transform.FindChild("2/icon").GetComponent<Image>().sprite = GAMEAPI.ABUI_LoadSprite("icon_item_"+item_id);
            root_dart_awd.transform.FindChild("2/Text").GetComponent<Text>().text = num.ToString();
            root_dart_awd.transform.FindChild("1/Text").GetComponent<Text>().text = per.ToString();
            root_dart_awd.transform.SetAsLastSibling();
            //text_dart.text = str;
            root_dart_awd.SetActive(true);
            btn_dart.gameObject.SetActive(true);
            new BaseButton(root_dart_awd.transform.FindChild("2")).onClick = (GameObject go) =>
              {
                  ArrayList arr = new ArrayList();
                  arr.Add(item_id);
                  arr.Add(1);
                  InterfaceMgr.getInstance().ui_async_open(InterfaceMgr.A3_MINITIP, arr);
              };


        }
        public void show(string str, UnityAction call)
        {
            clear();
            root.transform.SetAsLastSibling();
            _call = call;
            //这里先存str，到时候颜色字段在改
            text_str.text = str;
            root.SetActive(true);
        }
        public void showGoldNeed(string id, int goldenNeedNum, UnityAction handleYes, UnityAction handleNo = null, List<string> contPram = null)
        {
            showConfirm(ContMgr.getCont(id, contPram), () =>
            {
                if (goldenNeedNum > PlayerModel.getInstance().gold)
                {
                    flytxt.flyUseContId("rechange_noGolden", new List<string> { goldenNeedNum.ToString() });
                    //InterfaceMgr.getInstance().open(InterfaceMgr.RECHARGE);
                    hide();
                    return;
                }
                if (handleYes != null)
                    handleYes();
            }, handleNo);
        }
        public void showMoneyNeed()
        {

        }
        public void hide()
        {
            if (_handleNo != null)
                _handleNo();
            root.SetActive(false);
            root_task_fb.SetActive(false);
            root_swtz_fb.SetActive(false);
            CreateModel();
        }
        private void CreateModel()
        {
            if (InterfaceMgr.getInstance().checkWinOpened(InterfaceMgr.A3_LOTTERY))
            {
                a3_lottery.mInstance.CreateModel();
                return;
            }
        }

        protected void onClick()
        {
            if (_call != null)
                _call();
            root.SetActive(false);
            root_task_fb.SetActive(false);
            root_swtz_fb.SetActive(false);
            root_dart_awd.SetActive(false);            
        }


        void clear()
        {
            _call = null;
            _handleNo = null;
            text_str.text = "";
        }
    }
}
