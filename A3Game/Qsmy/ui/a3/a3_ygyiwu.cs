using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using GameFramework;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.EventSystems;
using Cross;
using DG.Tweening;

namespace MuGame
{
    class a3_ygyiwu : Window
    {

        static public a3_ygyiwu instan;
        int CloseIndex = 0;
        int index = 0; //0神王，1人王
        Dictionary<int, GameObject> God_obj = new Dictionary<int, GameObject>();
        Dictionary<int, GameObject> Pre_obj = new Dictionary<int, GameObject>();
        Image studybar;
        GameObject plane ;
        private GameObject scene_Camera;
        private GameObject scene_Obj;
        private GameObject m_SelfObj;
        private GameObject isthis;

        ScrollControler scrollControler;
        ScrollControler scrollControler1;
        GameObject IconHint;

        public override void init()
        {

            getComponentByPath<Text>("btn/doup/Text").text = ContMgr.getCont("a3_ygyiwu_0");
            getComponentByPath<Text>("tab_01/God/help_text/help/descTxt").text = ContMgr.getCont("a3_ygyiwu_1") + "\n" + ContMgr.getCont("a3_ygyiwu_2") + "\n" + ContMgr.getCont("a3_ygyiwu_3");

            instan = this;
            plane = this.transform.FindChild("tab_01/info").gameObject;
            BaseButton close = new BaseButton(this.transform.FindChild("over"));
            IconHint = this.transform.FindChild("btn/per_btn/IconHint").gameObject;
            close.onClick = onClose;
            BaseButton God_btn = new BaseButton(this.transform.FindChild("btn/God_btn"));
            God_btn.onClick = (GameObject go) =>
            {
                index = 0;
                ref_topText();
            };
            if (FunctionOpenMgr.instance.Check(FunctionOpenMgr.JL_SL))
            {
                getGameObjectByPath("btn/lockp").SetActive(false);
            }
            else
                getGameObjectByPath("btn/lockp").SetActive(true);
            new BaseButton(getTransformByPath("btn/lockp")).onClick = (GameObject go) =>
              {
                  flytxt.instance.fly(ContMgr.getCont("func_limit_44"));
              };
            isthis = this.transform.FindChild("btn/this").gameObject;
            isthis.SetActive(false);
            BaseButton per_btn = new BaseButton(this.transform.FindChild("btn/per_btn"));
            per_btn.onClick = (GameObject go) =>
            {
                index = 1;
                ref_topText();
            };
            BaseButton study_btn = new BaseButton(this.transform.FindChild("tab_02/go"));
            study_btn.onClick = onStudy;
            
            BaseButton tachback = new BaseButton(this.transform.FindChild("tab_01/info/tach"));
            BaseButton back = new BaseButton(this.transform.FindChild("tab_01/info/back"));

            new BaseButton(transform.FindChild("tab_01/God/help_God")).onClick = (GameObject go) => 
            {
                transform.FindChild("tab_01/God/help_text").gameObject.SetActive(true);
            };
            new BaseButton(transform.FindChild("tab_01/per/help_Pre")).onClick = (GameObject go) => 
            {
                transform.FindChild("tab_01/per/help_text").gameObject.SetActive(true);
            };

            new BaseButton(transform.FindChild("tab_01/God/help_text/close")).onClick =
            new BaseButton(transform.FindChild("tab_01/God/help_text/tach")).onClick = (GameObject go) => { transform.FindChild("tab_01/God/help_text").gameObject.SetActive(false); };
            new BaseButton(transform.FindChild("tab_01/per/help_text/close")).onClick =
            new BaseButton(transform.FindChild("tab_01/per/help_text/tach")).onClick = (GameObject go) => { transform.FindChild("tab_01/per/help_text").gameObject.SetActive(false); };
            back.onClick = tachback.onClick = (GameObject go) =>
            {
                if (index == 0)
                {
                    this.transform.FindChild("tab_01/God").gameObject.SetActive(true);
                }
                else if (index == 1)
                {
                    this.transform.FindChild("tab_01/per").gameObject.SetActive(true);
                }
                this.transform.FindChild("tab_01/info").gameObject.SetActive(false);
                this.transform.FindChild("btn").gameObject.SetActive(true);
            };
            BaseButton do_book = new BaseButton(this.transform.FindChild("btn/doup"));
            do_book.onClick = (GameObject go) =>
            {
                CloseIndex = 1;
                this.transform.FindChild("tab_01").gameObject.SetActive(false);
                this.transform.FindChild("btn").gameObject.SetActive(false);
                this.transform.FindChild("tab_02").gameObject.SetActive(true);
            };
            this.getEventTrigerByPath("tach").onDrag = OnDrag;
            studybar = this.transform.FindChild("tab_02/bar").GetComponent<Image>();
            into_God();
            into_per();
            A3_ygyiwuProxy.getInstance().SendYGinfo(2);
            refreshinfo(null);

            scrollControler = new ScrollControler();
            ScrollRect scroll = transform.FindChild("tab_01/God/view").GetComponent<ScrollRect>();
            scrollControler.create(scroll);

            scrollControler1 = new ScrollControler();
            ScrollRect scroll1 = transform.FindChild("tab_01/per/view").GetComponent<ScrollRect>();
            scrollControler1.create(scroll1);
        }
         
        public override void onShowed()
        {
            if (skill_a3._instance && skill_a3._instance.toshilian)
                InterfaceMgr.getInstance().close(InterfaceMgr.SKILL_A3);
            //into_God();
            //into_per();   
            if (FunctionOpenMgr.instance.Check(FunctionOpenMgr.JL_SL))
            {
                getGameObjectByPath("btn/lockp").SetActive(false);
            }
            else
                getGameObjectByPath("btn/lockp").SetActive(true);
            A3_ygyiwuProxy.getInstance().addEventListener(A3_ygyiwuProxy.EVENT_YWINFO , refreshinfo);
            A3_ygyiwuProxy.getInstance().addEventListener(A3_ygyiwuProxy.EVENT_ZHISHIINFO, refoMy_yiwu);
            A3_ygyiwuProxy.getInstance().SendYGinfo(1);
            CloseIndex = 0;

            if (uiData != null && uiData.Count > 0)
            {
                index = (int)uiData[0];
            }
            else {
                index = 0;
            }
            ref_topText();
            GRMap.GAME_CAMERA.SetActive(false);
            InterfaceMgr.getInstance().changeState(InterfaceMgr.STATE_FUNCTIONBAR);
            transform.FindChild("ig_bg_bg").gameObject.SetActive(false);
            createAvatar();
            UiEventCenter.getInstance().onWinOpen(uiName);
            showIconHint();

        }

        public override void onClosed()
        {
            disposeAvatar();
            this.transform.FindChild("tab_01").gameObject.SetActive(true);
            this.transform.FindChild("btn").gameObject.SetActive(true);
            this.transform.FindChild("tab_02").gameObject.SetActive(false);
            this.transform.FindChild("tab_01/info").gameObject.SetActive(false);
            A3_ygyiwuProxy.getInstance().removeEventListener(A3_ygyiwuProxy.EVENT_YWINFO, refreshinfo);
            A3_ygyiwuProxy.getInstance().removeEventListener(A3_ygyiwuProxy.EVENT_ZHISHIINFO, refoMy_yiwu);
            InterfaceMgr.getInstance().changeState(InterfaceMgr.STATE_NORMAL);
            GRMap.GAME_CAMERA.SetActive(true);

            if (skill_a3._instance && skill_a3._instance.toshilian) {
                InterfaceMgr.getInstance().ui_async_open(InterfaceMgr.SKILL_A3);
                skill_a3._instance.toshilian = false;
            }
        }

        void showIconHint() {
            if (a3_ygyiwuModel.getInstance().canToNowPre())
            {
                IconHint.SetActive(true);
            }
            else {
                IconHint.SetActive(false);
            }
        }

        //切换tab
        void ref_topText()
        {
            transform.FindChild("tab_01/God/help_text").gameObject.SetActive(false);
            transform.FindChild("tab_01/per/help_text").gameObject.SetActive(false);
            if (index == 0)
            {
                //this.transform.FindChild("tab_01/top").GetComponent<Text>().text = "神王遗物";
                this.transform.FindChild("tab_01/per").gameObject.SetActive(false);
                this.transform.FindChild("tab_01/God").gameObject.SetActive(true);
                isthis.SetActive(true);           
                isthis.transform.SetParent(this.transform.FindChild("btn/God_btn"),false);
                isthis.transform.localPosition = Vector2.zero;
            }
            else if (index == 1)
            {
                //this.transform.FindChild("tab_01/top").GetComponent<Text>().text = "人王遗物";
                this.transform.FindChild("tab_01/per").gameObject.SetActive(true);
                this.transform.FindChild("tab_01/God").gameObject.SetActive(false);
                isthis.SetActive(true);
                isthis.transform.SetParent(this.transform.FindChild("btn/per_btn"), false);
                isthis.transform.localPosition = Vector2.zero;
            }
            disboosAvatar();
            SXML x = XMLMgr.instance.GetSXML("accent_relic.mod", "type==" + (index + 1));
            int bossid = x.getInt("id");
            float size = x.getFloat("scale");
            createAvatar_body(bossid, size);
        }
        void onClose(GameObject go)
        {
            if (CloseIndex == 0)
                InterfaceMgr.getInstance().close(InterfaceMgr.A3_YGYIWU);
            else
            {
                this.transform.FindChild("tab_01").gameObject.SetActive(true);
                this.transform.FindChild("btn").gameObject.SetActive(true);
                this.transform.FindChild("tab_02").gameObject.SetActive(false);
                this.transform.FindChild("tab_01/info").gameObject.SetActive(false);
                ref_topText();
                CloseIndex = 0;
            }
        }
        void into_God()
        {
            if (a3_ygyiwuModel.getInstance().Allywlist_God.Count > 0)
            {
                GameObject item = this.transform.FindChild("tab_01/God/view/item").gameObject;
                RectTransform con = this.transform.FindChild("tab_01/God/view/con").GetComponent<RectTransform>();
                if (con.childCount > 0)
                {
                    for (int j = 0; j < con.childCount; j++)
                    {
                        Destroy(con.GetChild(j).gameObject);
                    }
                }
                for (int i = 1; i <= a3_ygyiwuModel.getInstance().Allywlist_God.Count; i++)
                {
                    GameObject clon = (GameObject)Instantiate(item);
                    clon.SetActive(true);
                    clon.transform.SetParent(con, false);
                    clon.transform.FindChild("name").gameObject.GetComponent<Text>().text = a3_ygyiwuModel.getInstance().Allywlist_God[i].name;
                    //clon.transform.FindChild("place").gameObject.GetComponent<Text>().text = a3_ygyiwuModel.getInstance().Allywlist_God[i].place;
                    clon.transform.FindChild("icon").GetComponent <Image>().sprite = GAMEAPI.ABUI_LoadSprite("icon_ar_" + a3_ygyiwuModel.getInstance().Allywlist_God[i].iconid);
                    int id = a3_ygyiwuModel.getInstance().Allywlist_God[i].id;
                    clon.name = id.ToString ();

                    if (!God_obj.ContainsKey(id))
                        God_obj[id] = clon;

                    new BaseButton(clon.transform).onClick = (GameObject go) =>
                    {
                        this.transform.FindChild("tab_01/God").gameObject.SetActive(false);
                        plane.SetActive(true);
                        this.transform.FindChild("btn").gameObject.SetActive(false);
                        this.transform.FindChild("tab_01/info/info_text").GetComponent<Text>().text = a3_ygyiwuModel.getInstance().Allywlist_God[int.Parse(go.name)].des;
                        string name = a3_ygyiwuModel.getInstance().Allywlist_God[int.Parse(go.name)].name;
                        string place = a3_ygyiwuModel.getInstance().Allywlist_God[int.Parse(go.name)].place;
                        string awardName = a3_ygyiwuModel.getInstance().Allywlist_God[int.Parse(go.name)].awardName;
                        string awardDesc = a3_ygyiwuModel.getInstance().Allywlist_God[int.Parse(go.name)].awardDesc;
                        int iconid = a3_ygyiwuModel.getInstance().Allywlist_God[int.Parse(go.name)].iconid;
                        Click_info(0,name,int.Parse(go.name),awardDesc, iconid);
                    };
                }
                //float childSizex = con.GetComponent<GridLayoutGroup>().cellSize.x;
                //float childSpacing = con.GetComponent<GridLayoutGroup>().spacing.x;
                //Vector2 newSize = new Vector2(a3_ygyiwuModel.getInstance().Allywlist_God.Count * (childSizex + childSpacing)- childSpacing, con.sizeDelta.y);
                //con.sizeDelta = newSize;
            }
        }

        void Click_info(int type, string name, int index,string awardDesc, int iconid)
        {
            //plane.transform.FindChild("item/name").GetComponent<Text>().text = name;
            plane.transform.FindChild("get_text").GetComponent<Text>().text = name;
            plane.transform.FindChild("getinfo").GetComponent<Text>().text = awardDesc;
            plane.transform.FindChild("icon").GetComponent<Image>().sprite = GAMEAPI.ABUI_LoadSprite("icon_ar_" + iconid);
            //plane.transform.FindChild("item/state").gameObject.SetActive(false);
            //plane.transform.FindChild("item/place").gameObject.SetActive(false);
            //plane.transform.FindChild("item/go").gameObject.SetActive(false);
            //plane.transform.FindChild("item/tishi").gameObject.SetActive(false);
            if (type == 0)//神王
            {
                if (a3_ygyiwuModel.getInstance().nowGod_id > index)
                {
                    plane.transform.FindChild("isGet").gameObject.SetActive(true);
                }
                //else if (a3_ygyiwuModel.getInstance().nowGod_id == index)
                //{
                //    plane.transform.FindChild("item/go").gameObject.SetActive(true);
                //    new BaseButton(plane.transform.FindChild("item/go")).onClick = (GameObject go) =>
                //    {
                //        onInGodFB();
                //    };
                //}
                else
                {
                    plane.transform.FindChild("isGet").gameObject.SetActive(false);
                    //plane.transform.FindChild("item/place").GetComponent<Text>().text = place;
                    //plane.transform.FindChild("item/place").gameObject.SetActive(true);
                }
            }
            if (type == 1) //人王
            {
                if (a3_ygyiwuModel.getInstance().nowPre_id > index)
                {
                    plane.transform.FindChild("isGet").gameObject.SetActive(true);
                    //plane.transform.FindChild("item/state").gameObject.SetActive(true);
                }
                //else if (a3_ygyiwuModel.getInstance().nowPre_id == index)
                //{
                //    if (PlayerModel.getInstance().up_lvl < a3_ygyiwuModel.getInstance().Allywlist_Pre[a3_ygyiwuModel.getInstance().nowPre_id].needuplvl)
                //    {
                //        plane.transform.FindChild("item/place").GetComponent<Text>().text = a3_ygyiwuModel.getInstance().Allywlist_Pre[index].needuplvl + "转" + a3_ygyiwuModel.getInstance().Allywlist_Pre[index].needlvl + "级开启";
                //        plane.transform.FindChild("item/place").gameObject.SetActive(true);
                //    }
                //    else if (PlayerModel.getInstance().up_lvl == a3_ygyiwuModel.getInstance().Allywlist_Pre[a3_ygyiwuModel.getInstance().nowPre_id].needuplvl)
                //    {
                //        if (PlayerModel.getInstance().lvl < a3_ygyiwuModel.getInstance().Allywlist_Pre[a3_ygyiwuModel.getInstance().nowPre_id].needlvl)
                //        {
                //            plane.transform.FindChild("item/place").GetComponent<Text>().text = a3_ygyiwuModel.getInstance().Allywlist_Pre[index].needuplvl + "转" + a3_ygyiwuModel.getInstance().Allywlist_Pre[index].needlvl + "级开启";
                //            plane.transform.FindChild("item/place").gameObject.SetActive(true);
                //        }
                //        else
                //        {
                //            plane.transform.FindChild("item/go").gameObject.SetActive(true);
                //            new BaseButton(plane.transform.FindChild("item/go")).onClick = (GameObject go) =>
                //            {
                //                onInPreFB();
                //            };
                //        }
                //    }
                //    else
                //    {
                //        plane.transform.FindChild("item/go").gameObject.SetActive(true);
                //        new BaseButton(plane.transform.FindChild("item/go")).onClick = (GameObject go) =>
                //        {
                //            onInPreFB();
                //        };
                //    }
                //}
                else
                {
                    plane.transform.FindChild("isGet").gameObject.SetActive(false);
                    //plane.transform.FindChild("item/tishi").gameObject.SetActive(true);
                }
            }
        }


        public void refreshGod()
        {
            foreach (int id in God_obj.Keys)
            {
                God_obj[id].transform.FindChild("t1").gameObject.SetActive(false);
                God_obj[id].transform.FindChild("t2").gameObject.SetActive(false);
                God_obj[id].transform.FindChild("t3").gameObject.SetActive(false);
                if (id < a3_ygyiwuModel.getInstance().nowGod_id)
                {
                    God_obj[id].transform.FindChild("t1").gameObject.SetActive(true);
                    God_obj[id].transform.FindChild("t1/skilldec").GetComponent<Text>().text = a3_ygyiwuModel.getInstance().Allywlist_God[id].awardDesc;
                }
                else if (id == a3_ygyiwuModel.getInstance().nowGod_id)
                {
                    God_obj[id].transform.FindChild("t2").gameObject.SetActive(true);
                    int lastexp = a3_ygyiwuModel.getInstance().GetYiWu_God(a3_ygyiwuModel.getInstance().nowGod_id).needexp;
                    float val = ((float)PlayerModel.getInstance().accent_exp / (float)lastexp);
                    God_obj[id].transform.FindChild("t2/jindu").GetComponent<Text>().text = val * 100 + "%";
                    God_obj[id].transform.FindChild("t2/barbg/bar").GetComponent<Image>().fillAmount=(float)PlayerModel.getInstance().accent_exp / (float)lastexp;
                    God_obj[id].transform.FindChild("t2/zhanli").GetComponent<Text>().text = a3_ygyiwuModel.getInstance().Allywlist_God[id].need_zdl.ToString();
                    God_obj[id].transform.FindChild("t2/go").gameObject.SetActive(false);
                    God_obj[id].transform.FindChild("t2/goo").gameObject.SetActive(false);
                    if (PlayerModel.getInstance().accent_exp >= lastexp)
                    {
                        God_obj[id].transform.FindChild("t2/go").gameObject.SetActive(true);
                        new BaseButton(God_obj[id].transform.FindChild("t2/go").transform).onClick = (GameObject go) =>
                        {
                            onInGodFB();
                        };
                    }
                    else
                    {
                        God_obj[id].transform.FindChild("t2/goo").gameObject.SetActive(true);
                    }

                }
                else if (id > a3_ygyiwuModel.getInstance().nowGod_id)
                {
                    God_obj[id].transform.FindChild("t3").gameObject.SetActive(true);
                }
            }
        }

        public void refreshPre()
        {
            foreach (int id in Pre_obj.Keys)
            {
                Pre_obj[id].transform.FindChild("t1").gameObject.SetActive(false);
                Pre_obj[id].transform.FindChild("t2").gameObject.SetActive(false);
                Pre_obj[id].transform.FindChild("t3").gameObject.SetActive(false);
                //if (id == a3_ygyiwuModel.getInstance().nowPre_id)
                //{
                //    Pre_obj[id].transform.FindChild("state").gameObject.SetActive(false);
                //    Pre_obj[id].transform.FindChild("tishi").gameObject.SetActive(false);
                //    if (PlayerModel.getInstance().up_lvl < a3_ygyiwuModel.getInstance().Allywlist_Pre[a3_ygyiwuModel.getInstance().nowPre_id].needuplvl)
                //    {
                //        Pre_obj[id].transform.FindChild("place").gameObject.SetActive(true);
                //        Pre_obj[id].transform.FindChild("go").gameObject.SetActive(false);
                //    }
                //    else if (PlayerModel.getInstance().up_lvl == a3_ygyiwuModel.getInstance().Allywlist_Pre[a3_ygyiwuModel.getInstance().nowPre_id].needuplvl)
                //    {
                //        if (PlayerModel.getInstance().lvl < a3_ygyiwuModel.getInstance().Allywlist_Pre[a3_ygyiwuModel.getInstance().nowPre_id].needlvl)
                //        {
                //            Pre_obj[id].transform.FindChild("place").gameObject.SetActive(true);
                //            Pre_obj[id].transform.FindChild("go").gameObject.SetActive(false);
                //        }
                //        else
                //        {
                //            Pre_obj[id].transform.FindChild("go").gameObject.SetActive(true);
                //            Pre_obj[id].transform.FindChild("place").gameObject.SetActive(false);
                //            new BaseButton(Pre_obj[id].transform.FindChild("go").transform).onClick = (GameObject go) =>
                //            {
                //                //进入副本
                //                onInPreFB();
                //            };
                //        }
                //    }
                //    else if (PlayerModel.getInstance().up_lvl > a3_ygyiwuModel.getInstance().Allywlist_Pre[a3_ygyiwuModel.getInstance().nowPre_id].needuplvl) {
                //        Pre_obj[id].transform.FindChild("go").gameObject.SetActive(true);
                //        Pre_obj[id].transform.FindChild("place").gameObject.SetActive(false);
                //        new BaseButton(Pre_obj[id].transform.FindChild("go").transform).onClick = (GameObject go) =>
                //        {
                //            //进入副本
                //            onInPreFB();
                //        };
                //    }
                //}
                //else if (id > a3_ygyiwuModel.getInstance().nowPre_id)
                //{
                //    Pre_obj[id].transform.FindChild("state").gameObject.SetActive(false);
                //    Pre_obj[id].transform.FindChild("place").gameObject.SetActive(false);
                //    Pre_obj[id].transform.FindChild("go").gameObject.SetActive(false);
                //    Pre_obj[id].transform.FindChild("tishi").gameObject.SetActive(true);
                //}
                //else if (id < a3_ygyiwuModel.getInstance().nowPre_id)
                //{
                //    Pre_obj[id].transform.FindChild("state").gameObject.SetActive(true);
                //    Pre_obj[id].transform.FindChild("place").gameObject.SetActive(false);
                //    Pre_obj[id].transform.FindChild("go").gameObject.SetActive(false);
                //    Pre_obj[id].transform.FindChild("tishi").gameObject.SetActive(false);
                //}
                if (id < a3_ygyiwuModel.getInstance().nowPre_id)
                {
                    Pre_obj[id].transform.FindChild("t1").gameObject.SetActive(true);
                    Pre_obj[id].transform.FindChild("t1/skilldec").GetComponent<Text>().text = a3_ygyiwuModel.getInstance().Allywlist_Pre[id].awardDesc;
                }
                else if (id == a3_ygyiwuModel.getInstance().nowPre_id)
                {
                    if (a3_ygyiwuModel.getInstance().canToNowPre())
                    {
                        Pre_obj[id].transform.FindChild("t2").gameObject.SetActive(true);
                        Pre_obj[id].transform.FindChild("t2/zhanli").GetComponent<Text>().text = a3_ygyiwuModel.getInstance().Allywlist_Pre[id].need_zdl.ToString();
                        new BaseButton(Pre_obj[id].transform.FindChild("t2/go").transform).onClick = (GameObject go) =>
                        {
                            onInPreFB();
                        };
                    }
                    else
                    {
                        Pre_obj[id].transform.FindChild("t3").gameObject.SetActive(true);
                        Pre_obj[id].transform.FindChild("t3/openlvl").GetComponent<Text>().text = a3_ygyiwuModel.getInstance().Allywlist_Pre[id].needuplvl + ContMgr.getCont("zhuan") + a3_ygyiwuModel.getInstance().Allywlist_Pre[id].needlvl + ContMgr.getCont("a3_ygyiwu");
                    }

                }
                else if (id > a3_ygyiwuModel.getInstance().nowPre_id)
                {
                    Pre_obj[id].transform.FindChild("t3").gameObject.SetActive(true);
                    Pre_obj[id].transform.FindChild("t3/openlvl").GetComponent<Text>().text = a3_ygyiwuModel.getInstance().Allywlist_Pre[id].needuplvl + ContMgr.getCont("zhuan") + a3_ygyiwuModel.getInstance().Allywlist_Pre[id].needlvl + ContMgr.getCont("a3_ygyiwu");
                }
            }
        }

        void OnDrag(GameObject go, Vector2 delta)
        {
            if (m_SelfObj != null)
            {
                m_SelfObj.transform.Rotate(Vector3.up, -delta.x);
            }
        }
        void createAvatar()
        {
            if (scene_Obj ==null)
            {
                GameObject obj_prefab;
                obj_prefab = GAMEAPI.ABFight_LoadPrefab("model_avatar_ui_scene_ui_camera");
                scene_Camera = GameObject.Instantiate(obj_prefab) as GameObject;
                obj_prefab = GAMEAPI.ABFight_LoadPrefab("model_avatar_ui_show_scene");
                scene_Obj = GameObject.Instantiate(obj_prefab, new Vector3(-77.38f, -0.49f, 15.1f), new Quaternion(0, 180, 0, 0)) as GameObject;
                foreach (Transform tran in scene_Obj.GetComponentsInChildren<Transform>())
                {
                    if (tran.gameObject.name == "scene_ta")
                        tran.gameObject.layer = EnumLayer.LM_ROLE_INVISIBLE;
                    else
                        tran.gameObject.layer = EnumLayer.LM_FX;// 更改物体的Layer层
                }
                scene_Obj.transform.FindChild("scene_ta").localPosition = new Vector3(-2.2f, -0.112f, 0.166f);
                scene_Obj.transform.FindChild("sc_tz_lg").localPosition = new Vector3(-2.2f, -0.112f, 0.166f);
                scene_Obj.transform.FindChild("fx_sc").localPosition = new Vector3(-2.21f, 0f, 0f);
            }
        }

        void createAvatar_body(int objid,float size)
        {
            GameObject obj_prefab;
            obj_prefab = GAMEAPI.ABModel_LoadNow_GameObject("monster_" + objid);
            m_SelfObj = Instantiate(obj_prefab, new Vector3(-75.26f, -0.561f, 14.91f), new Quaternion(0, 90, 0, 0)) as GameObject;
            foreach (Transform tran in m_SelfObj.GetComponentsInChildren<Transform>())
            {
                tran.gameObject.layer = EnumLayer.LM_ROLE_INVISIBLE;// 更改物体的Layer层
            }
            Transform cur_model = m_SelfObj.transform.FindChild("model");
            if (objid == 10070)
            {
                cur_model.FindChild("body/FX_idle").gameObject.SetActive(false);
            }
            cur_model.localScale = new Vector3(size, size, size);
            cur_model.gameObject.AddComponent<Summon_Base_Event>();
            if (cur_model != null)
            {
                var animm = cur_model.GetComponent<Animator>();
                if (animm != null)
                {
                    animm.Rebind();
                }
                animm.cullingMode = AnimatorCullingMode.AlwaysAnimate;
            }
        }

        public void disposeAvatar()
        {
            if (m_SelfObj != null) GameObject.Destroy(m_SelfObj);
            if (scene_Obj != null) GameObject.Destroy(scene_Obj);
            if (scene_Camera != null) GameObject.Destroy(scene_Camera);
        }
        public void disboosAvatar()
        {
            if (m_SelfObj != null) GameObject.Destroy(m_SelfObj);
        }

        private void onInGodFB()
        {
            int needExp = a3_ygyiwuModel.getInstance().GetYiWu_God(a3_ygyiwuModel.getInstance().nowGod_id).needexp;
            if (PlayerModel.getInstance().accent_exp >= needExp)
            {
                debug.Log("Enter");
                Variant sendData = new Variant();
                sendData["mapid"] = 3334;
                sendData["npcid"] = 0;
                sendData["ltpid"] = a3_ygyiwuModel.getInstance().nowGodFB_id;
                sendData["diff_lvl"] = 1;
                LevelProxy.getInstance().sendCreate_lvl(sendData);
                if (skill_a3._instance)
                    skill_a3._instance.toshilian = false;
            }
            else { flytxt.instance.fly(ContMgr.getCont("a3_ygyiwu_canont")); }
        }

        private void onInPreFB()
        {
            if (PlayerModel.getInstance().up_lvl < a3_ygyiwuModel.getInstance().Allywlist_Pre[a3_ygyiwuModel.getInstance().nowPre_id].needuplvl)
            {
                flytxt.instance.fly(ContMgr.getCont("a3_ygyiwu_canont"));
                return;
            }
            if (PlayerModel.getInstance().up_lvl == a3_ygyiwuModel.getInstance().Allywlist_Pre[a3_ygyiwuModel.getInstance().nowPre_id].needuplvl)
            {
                if (PlayerModel.getInstance().lvl < a3_ygyiwuModel.getInstance().Allywlist_Pre[a3_ygyiwuModel.getInstance().nowPre_id].needlvl)
                {
                    flytxt.instance.fly(ContMgr.getCont("a3_ygyiwu_canont"));
                    return;
                }
            }
            debug.Log("Enter");
            Variant sendData = new Variant();
            sendData["mapid"] = 3334;
            sendData["npcid"] = 0;
            sendData["ltpid"] = a3_ygyiwuModel.getInstance().nowPreFB_id;
            sendData["diff_lvl"] = 1;
            LevelProxy.getInstance().sendCreate_lvl(sendData);
            if(skill_a3._instance)
                skill_a3._instance.toshilian = false;
        }


        void refreshinfo(GameEvent e)
        {
            refreshGod();
            refreshPre();
            //into_My_yiwu();
        }

        void into_per()
        {

            if (a3_ygyiwuModel.getInstance().Allywlist_Pre.Count > 0)
            {
                GameObject item = this.transform.FindChild("tab_01/per/view/item").gameObject;
                RectTransform con = this.transform.FindChild("tab_01/per/view/con").GetComponent<RectTransform>();
                if (con.childCount > 0)
                {
                    for (int j = 0; j < con.childCount; j++)
                    {
                        Destroy(con.GetChild(j).gameObject);
                    }
                }
                for (int i = 1; i <= a3_ygyiwuModel.getInstance().Allywlist_Pre.Count; i++)
                {
                    GameObject clon = (GameObject)Instantiate(item);
                    clon.SetActive(true);
                    clon.transform.SetParent(con, false);
                    clon.transform.FindChild("name").gameObject.GetComponent<Text>().text = a3_ygyiwuModel.getInstance().Allywlist_Pre[i].name;
                    //clon.transform.FindChild("place").gameObject.GetComponent<Text>().text = a3_ygyiwuModel.getInstance().Allywlist_Pre[i].needuplvl + "转" + a3_ygyiwuModel.getInstance().Allywlist_Pre[i].needlvl + "级开启";
                    clon.transform.FindChild("icon").GetComponent<Image>().sprite = GAMEAPI.ABUI_LoadSprite("icon_ar_" + a3_ygyiwuModel.getInstance().Allywlist_Pre[i].iconid);
                    int id = a3_ygyiwuModel.getInstance().Allywlist_Pre[i].id;// info[i].getInt("id");
                    clon.name = id.ToString ();
                    if (!Pre_obj.ContainsKey(id))
                        Pre_obj[id] = clon;

                    new BaseButton(clon.transform).onClick = (GameObject go) =>
                    {
                        this.transform.FindChild("tab_01/per").gameObject.SetActive(false);
                        GameObject plane = this.transform.FindChild("tab_01/info").gameObject;
                        plane.SetActive(true);
                        this.transform.FindChild("btn").gameObject.SetActive(false);
                      
                        this.transform.FindChild("tab_01/info/info_text").GetComponent<Text>().text = a3_ygyiwuModel.getInstance().Allywlist_Pre[int.Parse(go.name)].des;
                        string name = a3_ygyiwuModel.getInstance().Allywlist_Pre[int.Parse(go.name)].name;
                        //string place = a3_ygyiwuModel.getInstance().Allywlist_Pre[int.Parse(go.name)].place;
                        //string awardName = a3_ygyiwuModel.getInstance().Allywlist_Pre[int.Parse(go.name)].awardName;
                        string awardDesc = a3_ygyiwuModel.getInstance().Allywlist_Pre[int.Parse(go.name)].awardDesc;
                        int iconid = a3_ygyiwuModel.getInstance().Allywlist_Pre[int.Parse(go.name)].iconid;
                        Click_info(1, name,int.Parse (go.name), awardDesc, iconid);
                    };
                }
                //float childSizex = con.GetComponent<GridLayoutGroup>().cellSize.x;
                //float childSpacing = con.GetComponent<GridLayoutGroup>().spacing.x;
                //Vector2 newSize = new Vector2(a3_ygyiwuModel.getInstance().Allywlist_Pre.Count * (childSizex + childSpacing) - childSpacing, con.sizeDelta.y);
                //con.sizeDelta = newSize;
            }
        }

        void into_My_yiwu()
        {
            Transform con = this.transform.FindChild("tab_02/scrollview/con");
            if (con.childCount > 0)
            {
                for (int j = 0; j < con.childCount; j++)
                    Destroy(con.GetChild(j).gameObject);
                {
                }
            }
            GameObject item = this.transform.FindChild("tab_02/scrollview/item").gameObject;
            foreach (int id in a3_ygyiwuModel.getInstance().yiwuList_Pre.Keys)
            {
                GameObject clon = (GameObject)Instantiate(item);
                clon.SetActive(true);
                clon.name = id.ToString();
                clon.transform.SetParent(con, false);
                clon.transform.FindChild("name").GetComponent<Text>().text = a3_ygyiwuModel.getInstance().yiwuList_Pre[id].name;
                clon.transform.FindChild("info").GetComponent<Text>().text = a3_ygyiwuModel.getInstance().yiwuList_Pre[id].eff;
            }
            foreach (int id in a3_ygyiwuModel.getInstance().yiwuList_God .Keys)
            {
                GameObject clon = (GameObject)Instantiate(item);
                clon.SetActive(true);
                clon.name = id.ToString();
                clon.transform.SetParent(con, false);
                clon.transform.FindChild("name").GetComponent<Text>().text = a3_ygyiwuModel.getInstance().yiwuList_God[id].name;
                clon.transform.FindChild("info").GetComponent<Text>().text = a3_ygyiwuModel.getInstance().yiwuList_God[id].eff;        
            }
        }


        public void refoMy_yiwu(GameEvent e)
        {
            this.transform.FindChild("tab_02/lvl/tex").GetComponent<Text>().text = a3_ygyiwuModel.getInstance().yiwuLvl +ContMgr.getCont("ji");
            //studybar.fillAmount = (float)a3_ygyiwuModel.getInstance().studyTime / a3_ygyiwuModel.getInstance().studyTime_all;
            ref_StudyBtn();
            if (a3_ygyiwuModel.getInstance().studyTime > 0)
            InvokeRepeating("timess", 0, 1);
        }

        void timess()
        {
            a3_ygyiwuModel.getInstance().studyTime--;
            if (a3_ygyiwuModel.getInstance().studyTime <= 0)
            {
                studybar.fillAmount = 1;
                CancelInvoke("timess");
            }
            else
            {
                //double tm = add_time * nowtime;
                studybar.fillAmount = (float)(a3_ygyiwuModel.getInstance().GetZTime((int)a3_ygyiwuModel.getInstance().yiwuLvl) - a3_ygyiwuModel.getInstance().studyTime) / a3_ygyiwuModel.getInstance().GetZTime((int)a3_ygyiwuModel.getInstance().yiwuLvl);//(float)Math.Round(tm, 3);
                //debug.Log("SY"+ a3_ygyiwuModel.getInstance().studyTime+"ZD"+ a3_ygyiwuModel.getInstance().studyTime_all+"YJX"+ (a3_ygyiwuModel.getInstance().studyTime_all - a3_ygyiwuModel.getInstance().studyTime));
            }
        }
        public void ref_StudyBtn()
        {
            if (a3_ygyiwuModel.getInstance().studyTime > 0)
            {
                isCanStudy = false;
                this.transform.FindChild("tab_02/go/Text1").gameObject.SetActive(false);
                this.transform.FindChild("tab_02/go/Text2").gameObject.SetActive(true);
                InvokeRepeating("timess", 0, 1);
            }
            else
            {
                isCanStudy = true;
                this.transform.FindChild("tab_02/go/Text1").gameObject.SetActive(true);
                this.transform.FindChild("tab_02/go/Text2").gameObject.SetActive(false);
                CancelInvoke("timess");
                studybar.fillAmount = 1;
            }
        }
        bool isCanStudy = true;
        void onStudy(GameObject go)
        {
            if (isCanStudy)
            {
                //点击开始研究
                A3_ygyiwuProxy.getInstance().SendYGinfo(3);
            }
            else
            {
                //点击加速研究
                A3_ygyiwuProxy.getInstance().SendYGinfo(4);
            }

        }
    }
}
