using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameFramework;
using MuGame.Qsmy.model;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace MuGame
{
    class a3_autoplay2 : Window
    {
        private Text nhpText;
        private Slider nhpSlider;
        private Text nmpText;
        private Slider nmpSlider;
        //private Text mhpText;
        //private Slider mhpSlider;
        private Toggle buyToggle;

        private Toggle avoidToggle;
        private Toggle pkToggle;
        private Toggle respawnToggle;
        private Toggle goldRespawnToggle;
        private Toggle upboundToggle;
        private InputField timesInputField;

        private GameObject[] skillChoosed;
        private Transform skList;
        private Vector3[] corners;
        private GameObject openListSkil;

        /*private Toggle oriToggle;
        private Toggle mediumToggle;
        private Toggle wholeToggle;*/ // Modified

        private AutoPlayModel apModel;

		int dragup;
		int dragdown;


        public override void init()
        {
            apModel = AutoPlayModel.getInstance();
            inText();
            nhpText = getComponentByPath<Text>("nhptxt");
            nhpSlider = getComponentByPath<Slider>("nhpSlider");
            nmpText = getComponentByPath<Text>("nmptxt");
            nmpSlider = getComponentByPath<Slider>("nmpSlider");
            //mhpText = getComponentByPath<Text>("mhptxt");
            //mhpSlider = getComponentByPath<Slider>("mhpSlider");
            buyToggle = getComponentByPath<Toggle>("buy");
            
            nhpSlider.onValueChanged.AddListener(OnNhpSliderChange);
            nmpSlider.onValueChanged.AddListener(OnNmpSliderChange);
            //mhpSlider.onValueChanged.AddListener(OnMhpSliderChange);
            buyToggle.onValueChanged.AddListener(OnBuyToggleChange);

            avoidToggle = getComponentByPath<Toggle>("avoid");
            pkToggle = getComponentByPath<Toggle>("pk");
            respawnToggle = getComponentByPath<Toggle>("respawn");
            goldRespawnToggle = getComponentByPath<Toggle>("goldrespawn");
            upboundToggle = getComponentByPath<Toggle>("upbound");
            timesInputField = getComponentByPath<InputField>("times");
            
            avoidToggle.onValueChanged.AddListener(OnAvoidToggleChange);
            pkToggle.onValueChanged.AddListener(OnPkToggleChange);
            respawnToggle.onValueChanged.AddListener(OnRespawnToggleChange);
            goldRespawnToggle.onValueChanged.AddListener(OnGoldRespawnToggleChange);
            upboundToggle.onValueChanged.AddListener(OnUpboundToggleChange);
            timesInputField.onValueChanged.AddListener(OnTimesInputField);
 
            skillChoosed = new GameObject[4];
            for(int i = 0 ; i < 4; i++)
            {
                skillChoosed[i] = getGameObjectByPath("skill/" + i);
				skillChoosed[i].transform.FindChild("mask").localPosition = Vector3.zero;
				var evli = EventTriggerListener.Get(skillChoosed[i].gameObject);
				evli.onDown = (GameObject g) => {
					dragdown = int.Parse(g.name);
					dragup = int.Parse(g.name);
				};
				evli.onDrag = (GameObject g, Vector2 d) => {
					var t = g.transform.FindChild("mask");
					t.transform.position += new Vector3(d.x,d.y,0);
				};
				evli.onEnter = (GameObject g) => {
					dragdown = int.Parse(g.name);
				};
				var cbg = getTransformByPath("skill/cbg");
				var cornerskk = new Vector3[4];
				if(cbg!=null)
					cbg.GetComponent<RectTransform>().GetWorldCorners(cornerskk);
				evli.onDragEnd = (GameObject g, Vector2 d) => {
					Vector3 mousePos = Input.mousePosition;
					if (mousePos.x < cornerskk[0].x ||
						mousePos.x > cornerskk[2].x ||
						mousePos.y < cornerskk[0].y ||
						mousePos.y > cornerskk[2].y) {
							dragdown = -1;
					}

					if (dragup != dragdown) {
						if (dragdown == -1) {
							//丢弃
							if (openListSkil != null) {
								openListSkil.transform.FindChild("mask").gameObject.SetActive(false);
								apModel.Skills[int.Parse(g.name)] = 0;
								openListSkil = null;
								skList.parent.gameObject.SetActive(false);
							}
						}
						else {
							//交换
						}
						
					}
					var t = g.transform.FindChild("mask");
					t.transform.localPosition = Vector3.zero;
				};
				evli.onClick = OnSkillChoosedClick;
               // BaseButton btn = new BaseButton(skillChoosed[i].GetComponent<Button>().transform);
               // btn.onClick = OnSkillChoosedClick;
            };

            /*oriToggle = getComponentByPath<Toggle>("ap_o");
            mediumToggle = getComponentByPath<Toggle>("ap_m");
            wholeToggle = getComponentByPath<Toggle>("ap_l");
            oriToggle.onValueChanged.AddListener(OnOriToggleChange);       
            mediumToggle.onValueChanged.AddListener(OnMediumToggleChange); 
            wholeToggle.onValueChanged.AddListener(OnWholeToggleChange);*/   // Modified

            BaseButton closeBtn = new BaseButton(getTransformByPath("closeBtn"));
            closeBtn.onClick = OnClose;

            BaseButton eqpBtn = new BaseButton(getTransformByPath("eqp"));
            eqpBtn.onClick = OnEqp;

            BaseButton pickBtn = new BaseButton(getTransformByPath("pick"));
            pickBtn.onClick = OnPick;

           // BaseButton startBtn = new BaseButton(getTransformByPath("start"));
            //startBtn.onClick = OnStart;

            //BaseButton stopBtn = new BaseButton(getTransformByPath("stop"));
           // stopBtn.onClick = OnStop;
        }

        void inText()
        {
            this.transform.FindChild("t1/Text").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_autoplay2_1");
            this.transform.FindChild("buy/Label").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_autoplay2_2");
            this.transform.FindChild("eqp/Text").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_autoplay2_3");
            this.transform.FindChild("pick/Text").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_autoplay2_4");
        }

        public override void onShowed()
        {
            transform.FindChild("ig_bg_bg").gameObject.SetActive(false);
            this.transform.SetAsLastSibling();
            RefreshRestore();
            //RefreshSkillList();
            RefreshBattle();
            RefreshScope();

            OnFSMStartStop(SelfRole.fsm.Autofighting);
            SelfRole.fsm.OnFSMStartStop += OnFSMStartStop;
        }

        void Update()
        {
            //if (Input.GetMouseButtonDown(0))
            //{
            //    Vector3 mousePos = Input.mousePosition;
            //    if (mousePos.x < corners[0].x ||
            //        mousePos.x > corners[2].x ||
            //        mousePos.y < corners[0].y ||
            //        mousePos.y > corners[2].y)
            //    {
            //        skList.parent.gameObject.SetActive(false);
            //        openListSkil = null;
            //    }
            //}
        }

        public override void onClosed()
        {
            apModel.WriteLocalData();
            SelfRole.fsm.OnFSMStartStop -= OnFSMStartStop;
        }

        private void RefreshScope()
        {
            /*oriToggle.isOn = false;
            mediumToggle.isOn = false;
            wholeToggle.isOn = false;
            switch (apModel.Scope)
            {
                case 0:
                    oriToggle.isOn = true;
                    break;
                case 1:
                    mediumToggle.isOn = true;
                    break;
                case 2:
                    wholeToggle.isOn = true;
                    break;
            }*/ // Modified
        }
        private void RefreshBattle()
        {
            avoidToggle.isOn = apModel.Avoid > 0;
            pkToggle.isOn = apModel.AutoPK > 0;
            respawnToggle.isOn = apModel.StoneRespawn > 0;
            goldRespawnToggle.isOn = apModel.GoldRespawn > 0;
            upboundToggle.isOn = apModel.RespawnLimit > 0;
            timesInputField.text = apModel.RespawnUpBound.ToString();
        }
        private void RefreshRestore()
        {
            nhpSlider.value = apModel.NHpLower;
            nhpText.text = ContMgr.getCont("autoplay_restore_0", new List<string> { apModel.NHpLower.ToString() });
            nmpSlider.value = apModel.NMpLower;
            nmpText.text = ContMgr.getCont("autoplay_restore_1", new List<string> { apModel.NMpLower.ToString() });
            //mhpSlider.value = apModel.MHpLower;
            //mhpText.text = ContMgr.getCont("autoplay_restore_2", new List<string> { apModel.MHpLower.ToString() });
            buyToggle.isOn = apModel.BuyDrug > 0;
        }
        private void RefreshSkillList()
        {
			skList = getTransformByPath("skillist/skillist");
            corners = new Vector3[4];
            skList.GetComponent<RectTransform>().GetWorldCorners(corners);

            int i = 0;
            foreach (skill_a3Data sk in Skill_a3Model.getInstance().skilldic.Values)
            {
                //!--非本职业，普通技能，buff技能，未学的技能不可见
                if (sk.carr != PlayerModel.getInstance().profession ||
                    sk.skill_id == a1_gamejoy.NORNAL_SKILL_ID ||
                    sk.now_lv == 0 ||
					sk.skillType2 == 1)
                    continue;

                int skid = sk.skill_id;
                Transform sktrans = skList.GetChild(i);
                sktrans.name = skid.ToString();
                sktrans.FindChild("bg/mask").gameObject.SetActive(true);
				sktrans.FindChild("bg").gameObject.SetActive(true);
                sktrans.FindChild("bg/mask/icon").GetComponent<Image>().sprite = GAMEAPI.ABUI_LoadSprite("icon_skill_" + skid.ToString());
                BaseButton btn = new BaseButton(sktrans.FindChild("bg"));
                btn.onClick = OnSkillListClick;
                i++;
            }

            for (int j = i; j < skList.childCount; j++)
            {
                Transform sktrans = skList.GetChild(j);
                sktrans.name = "0";
                sktrans.FindChild("bg/mask").gameObject.SetActive(false);
				sktrans.FindChild("bg").gameObject.SetActive(false);
                BaseButton btn = new BaseButton(sktrans.FindChild("bg"));
                btn.onClick = OnSkillListClick;
            }

            for (i = 0; i < 4; i++)
            {
                if (apModel.Skills[i] == 0)
                {
                    skillChoosed[i].transform.FindChild("mask").gameObject.SetActive(false);
                }
                else
                {
                    skillChoosed[i].transform.FindChild("mask").gameObject.SetActive(true);
                    skillChoosed[i].transform.FindChild("mask/icon").GetComponent<Image>().sprite = GAMEAPI.ABUI_LoadSprite("icon_skill_" + apModel.Skills[i].ToString());
                }
            }
        }

        #region 自动喝药UI事件

        private void OnNhpSliderChange(float v)
        {
            apModel.NHpLower = (int) v;
            nhpText.text = ContMgr.getCont("autoplay_restore_0", new List<string> { apModel.NHpLower.ToString() });
        }

        private void OnNmpSliderChange(float v)
        {
            apModel.NMpLower = (int) v;
            nmpText.text = ContMgr.getCont("autoplay_restore_1", new List<string> { apModel.NMpLower.ToString() });
        }

        private void OnMhpSliderChange(float v)
        {
            /*apModel.MHpLower = (int) v;
            mhpText.text = ContMgr.getCont("autoplay_restore_2", new List<string> { apModel.MHpLower.ToString() });*/
        }

        private void OnBuyToggleChange(bool v)
        {
            apModel.BuyDrug = v ? 1 : 0;
        }

        #endregion
        #region 自动战斗设置UI事件
        private void OnAvoidToggleChange(bool v)
        {
            apModel.Avoid = v ? 1 : 0;
        }

        private void OnPkToggleChange(bool v)
        {
            apModel.AutoPK = v ? 1 : 0;
        }

        private void OnRespawnToggleChange(bool v)
        {
            apModel.StoneRespawn = v ? 1 : 0;
        }

        private void OnGoldRespawnToggleChange(bool v)
        {
            apModel.GoldRespawn = v ? 1 : 0;
        }

        private void OnUpboundToggleChange(bool v)
        {
            apModel.RespawnLimit = v ? 1 : 0;
        }

        private void OnTimesInputField(string v)
        {
            if (!string.IsNullOrEmpty(v))
            {
                StateInit.Instance.RespawnTimes = apModel.RespawnUpBound = int.Parse(v);
            }
            
        }
        #endregion
        #region 技能设置UI事件

        private void OnSkillChoosedClick(GameObject go)
        {
            skList.parent.gameObject.SetActive(true);
            openListSkil = go;
        }

        private void OnSkillListClick(GameObject go)
        {
            if (openListSkil == null)
                return;

            int idx = int.Parse(openListSkil.name);
            int skid = int.Parse(go.transform.parent.name);
            if (skid == 0)
            {
                openListSkil.transform.FindChild("mask").gameObject.SetActive(false);
            }
            else
            {
                openListSkil.transform.FindChild("mask").gameObject.SetActive(true);
                openListSkil.transform.FindChild("mask/icon").GetComponent<Image>().sprite = GAMEAPI.ABUI_LoadSprite("icon_skill_" + skid.ToString());

                for (int i = 0; i < apModel.Skills.Length; i++)
                {
                    if (skid == apModel.Skills[i] && i != idx)
                    {
                        apModel.Skills[i] = 0;
                        skillChoosed[i].transform.FindChild("mask").gameObject.SetActive(false);
                    }
                }
            }
            apModel.Skills[idx] = skid;
            openListSkil = null;
			skList.parent.gameObject.SetActive(false);
        }

        #endregion
        #region 挂机范围UI事件
        /*private void OnOriToggleChange(bool v)
        {
            if (v)
            {
                oriToggle.interactable = false;
                mediumToggle.isOn = false;
                wholeToggle.isOn = false;
                apModel.Scope = 0;
            }
            else
            {
                oriToggle.interactable = true;
            }
        }

        private void OnMediumToggleChange(bool v)
        {
            if (v)
            {
                mediumToggle.interactable = false;
                oriToggle.isOn = false;
                wholeToggle.isOn = false;
                apModel.Scope = 1;
            }
            else
            {
                mediumToggle.interactable = true;
            }
        }

        private void OnWholeToggleChange(bool v)
        {
            if (v)
            {
                wholeToggle.interactable = false;
                oriToggle.isOn = false;
                mediumToggle.isOn = false;
                apModel.Scope = 2;
            }
            else
            {
                wholeToggle.interactable = true;
            }
        }*/
        #endregion
        #region 其他UI事件

        private void OnClose(GameObject go)
        {
            InterfaceMgr.getInstance().close(InterfaceMgr.A3_AUTOPLAY2);
        }

        private void OnEqp(GameObject go)
        {
			if (A3_VipModel.getInstance().Level >= 3)
				InterfaceMgr.getInstance().ui_async_open(InterfaceMgr.A3_AUTOPLAY_EQP);
			else flytxt.instance.fly(ContMgr.getCont("a3_autoplay2_vip"));
        }

        private void OnPick(GameObject go)
        {
            InterfaceMgr.getInstance().ui_async_open(InterfaceMgr.A3_AUTOPLAY_PICK);
        }

        private void OnStart(GameObject go)
        {
			if (GRMap.curSvrConf != null && GRMap.curSvrConf.ContainsKey("id") && GRMap.curSvrConf["id"] == 10){
				flytxt.instance.fly(ContMgr.getCont("a3_autoplay2"));
				return;
			}
            SelfRole.fsm.StartAutofight();
            OnClose(null);
			flytxt.flyUseContId("autoplay_start");
        }

        private void OnStop(GameObject go)
        {
            SelfRole.fsm.Stop();
            OnClose(null);
			flytxt.flyUseContId("autoplay_stop");

        }

        private void OnFSMStartStop(bool isRunning)
        {
           // getGameObjectByPath("start").SetActive(!isRunning);
            //getGameObjectByPath("stop").SetActive(isRunning);
        }
        #endregion

    }
}
