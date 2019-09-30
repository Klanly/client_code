using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.EventSystems;
using GameFramework;
namespace MuGame
{
    class NewbieModel
    {
        public BgItem curItem;
        public BgItem bgitem;
        public bool first_show = false ;

        public NewbieModel()
        {
            Transform transCon = GameObject.Find("newbieLayer").transform;


            //GameObject temp = U3DAPI.U3DResLoad<GameObject>("prefab/newbiebg");
            GameObject temp = GAMEAPI.ABLayer_LoadNow_GameObject("uilayer_newbiebg");
            GameObject goBg = GameObject.Instantiate(temp) as GameObject;
            bgitem = new BgItem(goBg.transform, transCon);
            goBg = GameObject.Instantiate(temp) as GameObject;




        }
        public static Dictionary<int, int> doneId = new Dictionary<int, int>();
        public static bool getDoneId(int id)
        {
            return doneId.ContainsKey(id);
        }
        public bool getIsOpenHeroNb()
        {
            Transform heroLayerNb = GameObject.Find("newbieHeroLayer").transform;
            if (heroLayerNb.GetChild(0).gameObject.activeSelf)
                return true;
            else
                return false;
        }

        private bool inited = false;
        public void initNewbieData()
        {
            if (inited)
                return;

            string str = FileMgr.loadString(FileMgr.TYPE_NEWBIE, "n");
            if (str != "")
            {
                string[] arr = str.Split(',');
                for (int i = 0; i < arr.Length; i++)
                {
                    doneId[int.Parse(arr[i])] = 1;
                }
            }



            inited = true;
            List<SXML> xml = XMLMgr.instance.GetSXMLList("newbie.n");
            //SXML xmls = xml.GetNode("skill");
            if (xml != null)
            {
                foreach (SXML x in xml)
                {
                    int id = x.getInt("id");
                    if (getDoneId(id))
                        continue;

                    NewbieTeachMgr.getInstance().add(x.getString("p"), id);
                }
            }
        }

        public void show(Vector3 pos, Vector2 size, string text = "", bool force = false, string clickItemName = "", Action clickMaskHandle = null, int cameraType = 0, bool autoClose = true)
        {
            a3_task_auto.instance.stopAuto = true;
            curItem = bgitem;

            if (a1_gamejoy.inst_joystick != null && SelfRole._inst != null)
                a1_gamejoy.inst_joystick.OnDragOut(null);

            curItem.show(pos, size, text, force, clickItemName, clickMaskHandle, cameraType, autoClose);
        }

        public void showNext(Vector3 pos, Vector2 size, string text = "", int type = 0, Action clickMaskHandle = null)
        {
            curItem.showNext(pos, size, text, type, clickMaskHandle);
        }

        public void showTittle(string clickItemName = "", Action clickMaskHandle = null)
        {
            curItem.showTittle(clickItemName, clickMaskHandle);
        }

        public void showWithoutAvatar(Vector3 pos, Vector2 size, string clickItemName = "", Action clickMaskHandle = null)
        {
            curItem.showWithoutAvatar(pos, size, clickItemName, clickMaskHandle);
        }

        public void hide()
        {
            a3_task_auto.instance.stopAuto = false;
            if (curItem != null)
            {
                curItem.hide();
                curItem = null;
            }
        }


        public static NewbieModel instanceaaa;

        public static NewbieModel getInstance()
        {
            if (instanceaaa == null)
                instanceaaa = new NewbieModel();
            return instanceaaa;
        }
    }

    class BgItem : Skin
    {
        Text txt;
        RectTransform rectTxtCon;
        RectTransform rectUp;
        RectTransform rectDown;
        RectTransform rectRight;
        RectTransform rectleft;
        RectTransform rectMask;

        RectTransform rect;
        Transform transCon;

        RectTransform bg;
        GameObject goBg;
        GameObject goMask;
        Transform txtBg;
        GameObject goNext;
        Text goNextTxt;

        public bool showing = false;
        public BgItem(Transform trans, Transform con)
            : base(trans)
        {
            transCon = con;

            goBg = trans.gameObject;

            goBg.transform.SetParent(transCon, false);

            bg = transform.FindChild("bg").GetComponent<RectTransform>();
            goMask = bg.transform.FindChild("mask").gameObject;
            rect = goBg.GetComponent<RectTransform>();
            txt = goBg.transform.FindChild("txt").FindChild("txt").GetComponent<Text>();
            txtBg = goBg.transform.FindChild("txt").FindChild("bg");
            rectTxtCon = goBg.transform.FindChild("txt").GetComponent<RectTransform>();
            goNext = goBg.transform.FindChild("go_next").gameObject;
            goNextTxt = goNext.transform.FindChild("txt").GetComponent<Text>();
            goBg.SetActive(false);
            goMask.SetActive(true);
            rectUp = bg.transform.FindChild("up").GetComponent<RectTransform>();
            rectDown = bg.transform.FindChild("down").GetComponent<RectTransform>();
            rectRight = bg.transform.FindChild("right").GetComponent<RectTransform>();
            rectleft = bg.transform.FindChild("left").GetComponent<RectTransform>();
            rectMask = bg.transform.FindChild("mask").GetComponent<RectTransform>();
            EventTriggerListener.Get(rectMask.gameObject).onClick = onMaskClick;
            EventTriggerListener.Get(goNext.transform.FindChild("btn_next").gameObject).onClick = onGoNext;
        }

        public void onMaskClick(GameObject go)
        {
            string tempName = _clickItemName;

            GameObject findobj = GameObject.Find(_clickItemName);
            if (findobj == null)
            {
                debug.Log("新手模块错误：" + _clickItemName);
            }

            if (_autoClose)
                hide();

            //InterfaceMgr.getInstance().close(InterfaceMgr.VIP_UP);
            //InterfaceMgr.getInstance().close(InterfaceMgr.GETTING);
            //InterfaceMgr.getInstance().close(InterfaceMgr.UPLEVEL);
            if (findobj != null)
            {
                if ((findobj.name == "bt0" || findobj.name == "bt1") && findobj.transform.parent.gameObject.name == "combat")
                {

                    ExecuteEvents.Execute<IPointerDownHandler>(findobj, new PointerEventData(EventSystem.current), ExecuteEvents.pointerDownHandler);
                    //修改手机上新手本第一攻击会停不下来的bug
                    ExecuteEvents.Execute<IPointerUpHandler>(findobj, new PointerEventData(EventSystem.current), ExecuteEvents.pointerUpHandler);
                }
                else
                {
                    ExecuteEvents.Execute<IPointerClickHandler>(findobj, new PointerEventData(EventSystem.current), ExecuteEvents.pointerClickHandler);

                }

                if (tempName == "10051")
                {
                    

                }
            }





            if (_clickMaskHandle != null)
            {
                Action fun = _clickMaskHandle;
                _clickMaskHandle = null;
                fun();
            }
        }

        public void onGoNext(GameObject go)
        {
            if (_autoClose)
                hide();

            if (_clickMaskHandle != null)
            {
                Action fun = _clickMaskHandle;
                _clickMaskHandle = null;
                fun();
            }
        }

        private bool _autoClose = true;
        private Action _clickMaskHandle;
        private string _clickItemName = "";

        //public void showWithHeroCamera(Vector3 pos, Vector2 size, string text = "", bool force = false, string clickItemName = "", Action clickMaskHandle = null)
        //{
        //    show(pos, size, text, force, clickItemName, clickMaskHandle, 1);
        //}


        private int curtype = -1;
        public void show(Vector3 pos, Vector2 size, string text = "", bool force = false, string clickItemName = "", Action clickMaskHandle = null, int cameraType = 0, bool autoClose = true)
        {
            a3_task_auto.instance.stopAuto = true;
            
            if (showing == true)
                return;
            showing = true;
            goBg.SetActive(true);
            goNext.SetActive(false);
            txt.gameObject.SetActive(true);
            txtBg.gameObject.SetActive(true);
            txt.text = text;

            showMarkClick();

            // rect.position = pos;
            //if (force)
            //{
            //    rect.position = pos;
            //}
            //else
            //{
            // rect.position = pos;
            if (NewbieModel.getInstance().first_show)
            {
                bg.position = pos;
                NewbieModel.getInstance().first_show = false;
            }
            else 
                bg.DOMove(pos, 0.6f).OnComplete(() => { goMask.SetActive(true); });
            //}
            _clickItemName = clickItemName;
            //if (clickItemName != "")
            //goMask.SetActive(true);
            //else
            //    goMask.SetActive(false);

            _autoClose = autoClose;
            _clickMaskHandle = clickMaskHandle;
            rectMask.sizeDelta = size;


            DoAfterMgr.instacne.addAfterRender(() =>
            {

                //InterfaceMgr.getInstance().close(InterfaceMgr.GETTING);
                //InterfaceMgr.getInstance().close(InterfaceMgr.UPLEVEL);

            });




            //float tempx = size.x / 2 + 130f;
            //if (pos.x > Baselayer.halfuiWidth || cameraType == 1)
            //{
            //    txtBg.localScale = Vector3.one;
            //    tempx = -tempx;
            //}
            //else
            //{
            //    txtBg.localScale = new Vector3(-1, 1, 1);
            //}

            //float tempy = 60;
            //if (pos.y > 0)
            //    tempy = -60;

            //if (clickItemName == "dialog(Clone)")
            //    rectTxtCon.localPosition = new Vector3(-tempx / 2, tempy + 140f);
            //else if (clickItemName == "btn_get")
            //    rectTxtCon.localPosition = new Vector3(tempx + 190f, tempy + 40f);
            //else
            //    rectTxtCon.localPosition = new Vector3(tempx, tempy);


            Vector3 vec = rectUp.localPosition;
            vec.y = 1000 + size.y / 2;
            rectUp.localPosition = vec;

            vec = rectDown.localPosition;
            vec.y = -1000 - size.y / 2;
            rectDown.localPosition = vec;

            vec = rectRight.localPosition;
            vec.x = -1000 - size.x / 2;
            rectRight.localPosition = vec;
            Vector2 v2 = new Vector2(2000, size.y);
            rectRight.sizeDelta = v2;
            rectleft.sizeDelta = v2;

            vec = rectleft.localPosition;
            vec.x = 1000 + size.x / 2;
            rectleft.localPosition = vec;

            if(pos.x > Baselayer.halfuiWidth)
                createAvatar(true);
            else
                createAvatar(false);
        }

        public void showNext(Vector3 pos, Vector2 size, string text = "", int type = 0, Action clickMaskHandle = null)
        {
            a3_task_auto.instance.stopAuto = true;

            if (showing == true)
                return;
            showing = true;
            goBg.SetActive(true);
            goNext.SetActive(true);
            txt.gameObject.SetActive(false);
            txtBg.gameObject.SetActive(false);
            goNextTxt.text = text;

            hideMarkClick();

            _clickMaskHandle = clickMaskHandle;

            // rect.position = pos;
            if (NewbieModel.getInstance().first_show)
            {
                bg.position = pos;
                NewbieModel.getInstance().first_show = false;
            }
            else
                bg.DOMove(pos, 0.6f).OnComplete(() => { goMask.SetActive(true); });
            goNext.transform.localPosition = new Vector3(-rect.localPosition.x, -rect.localPosition.y, 0);

            rectMask.sizeDelta = size;

            //float tempx = size.x / 2 + 130f;
            //if (pos.x > Baselayer.halfuiWidth)
            //{
            //    txtBg.localScale = Vector3.one;
            //    tempx = -tempx;
            //}
            //else
            //{
            //    txtBg.localScale = new Vector3(-1, 1, 1);
            //}

            //float tempy = 60;
            //if (pos.y > 0)
            //    tempy = -60;

           
            //rectTxtCon.localPosition = new Vector3(tempx, tempy);

            Vector3 vec = rectUp.localPosition;
            vec.y = 1000 + size.y / 2;
            rectUp.localPosition = vec;

            vec = rectDown.localPosition;
            vec.y = -1000 - size.y / 2;
            rectDown.localPosition = vec;

            vec = rectRight.localPosition;
            vec.x = -1000 - size.x / 2;
            rectRight.localPosition = vec;
            Vector2 v2 = new Vector2(2000, size.y);
            rectRight.sizeDelta = v2;
            rectleft.sizeDelta = v2;

            vec = rectleft.localPosition;
            vec.x = 1000 + size.x / 2;
            rectleft.localPosition = vec;

            if (pos.x > Baselayer.halfuiWidth)
                createAvatar(true);
            else
                createAvatar(false);
        }

        public void showTittle(string clickItemName = "", Action clickMaskHandle = null)
        {
            _clickMaskHandle = clickMaskHandle;

            GameObject findobj = GameObject.Find(clickItemName);
           
            if (findobj == null)
            {
                debug.Log("新手模块错误：" + _clickItemName);
            }

            _clickItemName = clickItemName;

            if (findobj != null)
            {
                findobj.transform.DOScale(Vector3.one, 0.1f).OnComplete(() => {
                    if ((findobj.name == "bt0" || findobj.name == "bt1") && findobj.transform.parent.gameObject.name == "combat")
                    {

                        ExecuteEvents.Execute<IPointerDownHandler>(findobj, new PointerEventData(EventSystem.current), ExecuteEvents.pointerDownHandler);
                    }
                    else
                    {
                        ExecuteEvents.Execute<IPointerClickHandler>(findobj, new PointerEventData(EventSystem.current), ExecuteEvents.pointerClickHandler);

                    }

                    if (_clickMaskHandle != null)
                    {
                        Action fun = _clickMaskHandle;
                        _clickMaskHandle = null;
                        fun();
                    }
                });
            }
        }

        public void showWithoutAvatar(Vector3 pos, Vector2 size, string clickItemName = "", Action clickMaskHandle = null)
        {
            a3_task_auto.instance.stopAuto = true;

            if (showing == true)
                return;
            showing = true;
            goBg.SetActive(true);
            txt.gameObject.SetActive(false);
            txtBg.gameObject.SetActive(false);
            goNext.SetActive(false);

            showMarkClick();

            _clickItemName = clickItemName;
            _clickMaskHandle = clickMaskHandle;

            // rect.position = pos;
            if (NewbieModel.getInstance().first_show)
            {
                bg.position = pos;
                NewbieModel.getInstance().first_show = false;
            }
            else
                bg.DOMove(pos, 0.6f).OnComplete(() => { goMask.SetActive(true); });
            goNext.transform.localPosition = new Vector3(-rect.localPosition.x, -rect.localPosition.y, 0);

           
            Vector3 vec = rectUp.localPosition;
            vec.y = 1000 + size.y / 2;
            rectUp.localPosition = vec;

            vec = rectDown.localPosition;
            vec.y = -1000 - size.y / 2;
            rectDown.localPosition = vec;

            vec = rectRight.localPosition;
            vec.x = -1000 - size.x / 2;
            rectRight.localPosition = vec;
            Vector2 v2 = new Vector2(2000, size.y);
            rectRight.sizeDelta = v2;
            rectleft.sizeDelta = v2;

            vec = rectleft.localPosition;
            vec.x = 1000 + size.x / 2;
            rectleft.localPosition = vec;
        }

        public void hide()
        {
            a3_task_auto.instance.stopAuto = false;
            if (showing == false)
                return;
            showing = false;
            goBg.SetActive(false);
            _clickItemName = "";

            disposeAvatar();
        }


        public void hideMarkClick()
        {
            rectMask.GetComponent<CanvasGroup>().blocksRaycasts = false;
        }
        public void showMarkClick()
        {
            rectMask.GetComponent<CanvasGroup>().blocksRaycasts = true;
        }

        private GameObject m_Obj;
        private GameObject m_skmesh_camera;//拍摄avatar的摄像机
        public List<Vector3> lAvPos = new List<Vector3>() { new Vector3(-127.74f, -1.82f, -128f), new Vector3(-132.7f, -1.82f, -128f) };
        public List<Vector3> txtPos = new List<Vector3>() { new Vector3(-167f, -12f, 0), new Vector3(284f, -12f, 0) };

        private bool m_left;
        public void createAvatar( bool left) //true为左，false为右
        {
            //创建一个显示UI上角色的摄像机
            GameObject shizhuang_camera_prefab = GAMEAPI.ABUI_LoadPrefab("camera_newbie_camera");
            m_skmesh_camera = GameObject.Instantiate(shizhuang_camera_prefab) as GameObject;
            m_skmesh_camera.transform.localPosition = new Vector3(-129.7f, 1.34f, -124.98f);

            if (m_Obj == null)
            {
                m_left = left;
                GAMEAPI.ABModel_LoadGameObject("npc_125", Model_LoadedOK, null);
            }

        }

        private void Model_LoadedOK(UnityEngine.Object model_obj, System.Object data)
        {
            //由于异步加载有可能在关闭后才加载到模型。这样会出现bug。加个保护
            if (showing == false)
                return;
            if (m_Obj != null) GameObject.Destroy(m_Obj);
            m_Obj = null;

            GameObject obj_prefab = model_obj as GameObject;
            m_Obj = GameObject.Instantiate(obj_prefab) as GameObject;
            GameObject.Destroy(m_Obj.GetComponent<NavMeshAgent>());
            if (m_left)
            {
                m_Obj.transform.position = lAvPos[0];
                rectTxtCon.localPosition = new Vector3(txtPos[0].x - rect.localPosition.x, txtPos[0].y - rect.localPosition.y + 70);
                txtBg.localScale = new Vector3(-1, 1, 1);
            }

            else
            {
                m_Obj.transform.position = lAvPos[1];
                rectTxtCon.localPosition = new Vector3(txtPos[1].x - rect.localPosition.x, txtPos[1].y - rect.localPosition.y + 70);
                txtBg.localScale = new Vector3(1, 1, 1);
            }

            m_Obj.transform.eulerAngles = Vector3.zero;


            Transform[] listTN = m_Obj.GetComponentsInChildren<Transform>();
            foreach (Transform tran in listTN)
            {
                tran.gameObject.layer = EnumLayer.LM_DEFAULT;
            }
        }

        public void disposeAvatar()
        {
            if (m_Obj != null) GameObject.Destroy(m_Obj);
            if (m_skmesh_camera != null) GameObject.Destroy(m_skmesh_camera);
       
            m_Obj = null;
            m_skmesh_camera = null;
        }
    }
}
