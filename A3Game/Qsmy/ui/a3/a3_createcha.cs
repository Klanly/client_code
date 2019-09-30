using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using Cross;
using GameFramework;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace MuGame
{
    class a3_createcha : FloatUi
    {
        private TabControl chooseTab;
        private InputField nameInput;
        private Text descText;
        Text _txtKeyWordWaring;
        private Slider[] attSliders;
        //  private Text[] attTxts;

        private string[] lFirstName;
        private string[] lLastName0;
        private string[] lLastName1;

        private GameObject fg;

        private AsyncOperation async;
        private GameObject cameras = null;

        private SXML createChaSxml = null;

        private GameObject att;

        public override void init()
        {

            inText();
            if (createChaSxml == null)
            {
                createChaSxml = XMLMgr.instance.GetSXML("creat_character");
            }

            fg = getGameObjectByPath("fg");
            fg.SetActive(true);

            if (debug.instance != null)
                debug.instance.showMsg(ContMgr.getOutGameCont("debug2"));

            fg.GetComponent<RectTransform>().sizeDelta = new Vector2(Baselayer.uiWidth, Baselayer.uiHeight);

            InitUI();
        }

        void inText()
        {
            this.transform.FindChild("createBtn/Text").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_createcha_1");//创建角色
            this.transform.FindChild("nameInput/Placeholder").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_createcha_2");//输入角色名
            this.transform.FindChild("txtKeyWordWaring").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_createcha_3");//含有屏蔽字符,换一个试试吧...
            this.transform.FindChild("btnChoose/warriorBtn/Text").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_createcha_4");//狂战士
            this.transform.FindChild("btnChoose/magicBtn/Text").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_createcha_5");//法师
            this.transform.FindChild("btnChoose/rogueBtn/Text").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_createcha_6");//暗影
        }


        private IEnumerator LoadScene()
        {
            fg.SetActive(true);

            GAMEAPI.LoadAsset_Async("cr_scene.assetbundle", "cr_scene");
            while (GAMEAPI.Res_Async_Loaded() == false)
            {
                yield return new WaitForSeconds(0.1f);
            }

            debug.Log("加载创角场景");
            async = SceneManager.LoadSceneAsync("cr_scene");
            yield return async;
            fg.SetActive(false);

            GAMEAPI.Unload_Asset("cr_scene.assetbundle");
        }

        public override void onShowed()
        {
            GRMap.DontDestroyBaseGameObject();
            StartCoroutine(LoadScene());

            muNetCleint.instance.charsInfoInst.addEventListener(UI_EVENT.UI_ACT_CREATE_CHAR, OnCreateChar);
        }

        public override void onClosed()
        {
            cameras = null;
            async = null;
            muNetCleint.instance.charsInfoInst.removeEventListener(UI_EVENT.UI_ACT_CREATE_CHAR, OnCreateChar);
        }

        void Update()
        {
            if (async != null && async.isDone)
            {
                async = null;
                cameras = GameObject.Find("cameras");

                //!--加载完成后,刷新一次,显示默认的角色
                RefreshAfterSelect();
            }
            string oldname = nameInput.text;
            string newname = oldname.Replace(" ","");
            nameInput.text = newname;
        }

        void OnDestroy()
        {

        }

        private void InitUI()
        {
            //attSliders = new Slider[4];
            //   attTxts = new Text[4];
            //for (int i = 0; i < 4; i++)
            //{
            //    attSliders[i] = getComponentByPath<Slider>("atts/attSlider" + i.ToString());
            ////    attTxts[i] = getComponentByPath<Text>("atts/att" + i.ToString());
            //}
            att = this.transform.FindChild("att_pic").gameObject;
            nameInput = getComponentByPath<InputField>("nameInput");
            descText = getComponentByPath<Text>("descText");
            _txtKeyWordWaring = getComponentByPath<Text>("txtKeyWordWaring");
            _txtKeyWordWaring.gameObject.SetActive(false);
            getEventTrigerByPath("returnBtn").onClick = OnReturnClick;
            getEventTrigerByPath("createBtn").onClick = OnCreateClick;
            getEventTrigerByPath("randomBtn").onClick = OnNameRandomClick;

            chooseTab = new TabControl();
            chooseTab.onClickHanle = onChooseCha;
            chooseTab.create(this.getGameObjectByPath("btnChoose"), this.gameObject);
        }

        private void onChooseCha(TabControl t)
        {
            RefreshAfterSelect();
        }

        private void OnReturnClick(GameObject go)
        {
            //RectTransform rtf = gameObject.GetComponent<RectTransform>();
            //rtf.localScale = new UnityEngine.Vector3(11, 11, 11);
            //return;
            //MediaClient.instance.clearMusic();
            InterfaceMgr.getInstance().close(InterfaceMgr.A3_CREATECHA);
            InterfaceMgr.getInstance().ui_async_open(InterfaceMgr.A3_SELECTCHA);
        }

        void OnCreateChar(GameEvent e)
        {
            Variant data = e.data;
            debug.Log(data["res"]._int.ToString());
            debug.Log(data.dump());
            if (data["res"]._int == -153)
                flytxt.instance.fly(ContMgr.getCont("a3_createcha_tongming"));
            if (data["res"]._int > 0)
            {
                uint cid = data["cha"]["cid"]._uint;
                PlayerModel.getInstance().cid = cid;
                //  LGPlatInfo.inst.logSDKAP("roleCreate");

                InterfaceMgr.getInstance().close(InterfaceMgr.A3_CREATECHA);
                InterfaceMgr.getInstance().DisposeUI(InterfaceMgr.A3_CREATECHA);

                PlayerModel.getInstance().profession = chooseTab.getSeletedIndex() + 2;
                PlayerModel.getInstance().name = nameInput.text;

                GameSdkMgr.record_createRole(data["cha"]);

                //当创建角色成功，先不要进入游戏
                //debug.Log("当创建角色成功，先不要进入游戏，进入单机副本");
                SelfRole.s_bStandaloneScene = true;

                //资源会再前面加载，这里必须使用异步加载 map_loading
                InterfaceMgr.getInstance().ui_async_open(InterfaceMgr.MAP_LOADING);
                //InterfaceMgr.getInstance().ui_async_open("joystick");
                //InterfaceMgr.getInstance().ui_async_open("skillbar");
                InterfaceMgr.getInstance().ui_async_open("a1_gamejoy");
                LGMap lgmap = GRClient.instance.g_gameM.getObject(OBJECT_NAME.LG_MAP) as LGMap;
                lgmap.EnterStandalone();
                MediaClient.instance.StopMusic();
                //MediaClient.instance.clearMusic();

                //if (debug.instance != null)
                //    debug.instance.showMsg(ContMgr.getOutGameCont("debug4"), 3);
            }
        }

        private void OnCreateClick(GameObject go)
        {
            //TODO TOBECHECK
            bool isContainKeyWord = KeyWord.isContain(nameInput.text);
            _txtKeyWordWaring.gameObject.SetActive(isContainKeyWord);
            if (isContainKeyWord)
            {
                return;
            }

            //flytxt.instance.fly("jjjjjjjjjjjjjjj");
            int carr = chooseTab.getSeletedIndex() + 2;
            muNetCleint.instance.outGameMsgsInst.createCha(
                  nameInput.text,
                  (uint)carr,//职业
                  0
               );
        }

        protected Variant firstname = null;
        protected Variant lastname = null;

        private void OnNameRandomClick(GameObject go)
        {
            nameInput.text = RandomName();
        }

        private string RandomName()
        {
            if (lFirstName == null)
            {
                lFirstName = XMLMgr.instance.GetSXML("comm.ranName.firstName").getString("value").Split(',');
                lLastName0 = XMLMgr.instance.GetSXML("comm.ranName.lastName", "sex==0").getString("value").Split(',');
                lLastName1 = XMLMgr.instance.GetSXML("comm.ranName.lastName", "sex==1").getString("value").Split(',');
            }

            System.Random r = new System.Random();
            int first = r.Next(0, lFirstName.Length);
            int last = r.Next(0, lLastName0.Length);
            return lFirstName[first] + lFirstName[last];
        }

        private void ShowDescAndAtt()
        {
            int curChaIndex = chooseTab.getSeletedIndex();

            for (int i = 0; i < att.transform.childCount; i++)
            {
                att.transform.GetChild(i).gameObject.SetActive(false);
            }
            att.transform.FindChild("cre" + curChaIndex).gameObject.SetActive(true);

            SXML curChaXml = createChaSxml.GetNode("character", "job_type==" + (curChaIndex + 2));

            descText.text = curChaXml.getString("desc");
            //List<SXML> attsXml = curChaXml.GetNodeList("character", null);
            //for(int i = 0; i < attsXml.Count; i++)
            //{
            //    int att_tpid = attsXml[i].getInt("att_type");
            //   // attTxts[i].text = Globle.getAttrNameById(att_tpid);
            //    attSliders[i].value = attsXml[i].getInt("att_per") / 100.0f;
            //}
        }

        private void RefreshAfterSelect()
        {
            ShowCameraAnim();
            ShowDescAndAtt();
            OnNameRandomClick(null);
        }


        private void ShowBtenIamg(int a)
        {
            for (int i = 0; i < 4; i++)
            {
                if (i == a)
                {
                    this.transform.FindChild("btnChoose").GetChild(i).FindChild("isthis").gameObject.SetActive(true);
                    this.transform.FindChild("btnChoose").GetChild(i).FindChild("isnull").gameObject.SetActive(false);
                }
                else
                {
                    this.transform.FindChild("btnChoose").GetChild(i).FindChild("isthis").gameObject.SetActive(false);
                    this.transform.FindChild("btnChoose").GetChild(i).FindChild("isnull").gameObject.SetActive(true);
                }
            }
        }
        private void ShowCameraAnim()
        {
            if (cameras == null)
                return;

            int curChaIndex = chooseTab.getSeletedIndex();

            Transform assa_camera = cameras.transform.FindChild("assa_Camera");
            Transform mage_camera = cameras.transform.FindChild("mage_Camera");
            Transform warrior_camera = cameras.transform.FindChild("warrior_Camera");

            if (assa_camera == null || mage_camera == null || warrior_camera == null)
            {
                debug.Log("some camera is missing in create role scene");
                return;
            }

            assa_camera.gameObject.SetActive(false);
            mage_camera.gameObject.SetActive(false);
            warrior_camera.gameObject.SetActive(false);

            switch (curChaIndex)
            {
                case 0:
                    warrior_camera.gameObject.SetActive(true);
                    ShowBtenIamg(0);
                    break;
                case 1:
                    mage_camera.gameObject.SetActive(true);
                    ShowBtenIamg(1);
                    break;
                case 2:
                    //TODO
                    break;
                case 3:
                    assa_camera.gameObject.SetActive(true);
                    ShowBtenIamg(3);
                    break;
            }
        }
    }
}
