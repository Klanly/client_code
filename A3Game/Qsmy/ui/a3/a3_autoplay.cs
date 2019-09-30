using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cross;
using GameFramework;
using MuGame.Qsmy.model;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace MuGame
{
    class a3_autoplay : Window
    {
/*
        private static readonly uint ITEM_QUAL = 5;
        private AutoPlayModel apModel;

        private TabControl tab;

        //!--恢复面板相关控件
        private Toggle hpToggle1;
        private Text hpTog1Txt;
        private Slider hpSlider1;
        private RectTransform nhp;

        private Toggle mpToggle1;
        private Text mpTog1Txt;
        private Slider mpSlider1;
        private RectTransform nmp;

        private Toggle hpToggle2;
        private Text hpTog2Txt;
        private Slider hpSlider2;
        private RectTransform mhp;

        private Toggle hpDrugToggle;
        private Text hpDrugTogText;
        private Slider hpDrugSlider;
        private RectTransform buyhp;

        private Toggle mpDrugToggle;
        private Text mpDrugTogText;
        private Slider mpDrugSlider;
        private RectTransform buymp;

        private GridLayoutGroup nhpgrid;
        private GridLayoutGroup nmpgrid;

        //!--拾取面板相关控件
        private Toggle[] eqpToggles;
        private Toggle eqpAllToggle;
        private Toggle[] matToggles;
        private Toggle matAllToggle;
        private Toggle[] procToggles;
        private Toggle procAllToggle;
        private Slider eqpDoSlider;

        //!--战斗面板相关控件
        private Slider scopeSlider;
        private Image mapCon;
        private Toggle avoidToggle;
        private Toggle autoPkToggle;
        private Toggle stoneRespawnToggle;
        private Toggle goldRespawnToggle;
        private Toggle backRespawnToggle;
        private InputField respawnUpboundInput;
        private int[] skillIds;
        private GameObject[] skillIcons;
        
        public override void init()
        {
            apModel = AutoPlayModel.getInstance();

            tab = new TabControl();
            tab.onClickHanle = OnTabSwitch;
            tab.create(getGameObjectByPath("btnChoose"), this.gameObject);

            InitRestoreCon();
            InitPickCon();
            InitFightCon();

            getEventTrigerByPath("closeBtn").onClick = onClose;
            getEventTrigerByPath("autoplayBtn").onClick = OnStartAutoPlay;
            getEventTrigerByPath("stopBtn").onClick = OnStopAutoplay;
        }

        private void InitRestoreCon()
        {
            //!--恢复面板相关控件持有
            //!--1)普通HP药
            hpToggle1 = getComponentByPath<Toggle>("restoreCon/hpToggle1");
            hpToggle1.onValueChanged.AddListener((bool v) =>
            {
                apModel.NHpDrug = hpToggle1.isOn ? 1 : 0;
            });

            hpTog1Txt = getComponentByPath<Text>("restoreCon/hpToggle1/Label");

            hpSlider1 = getComponentByPath<Slider>("restoreCon/hpSlider1");
            hpSlider1.onValueChanged.AddListener((float v) =>
            {
                string str = string.Format("{0,3}", (int)v);
                hpTog1Txt.text = ContMgr.getCont("autoplay_restore_0", new List<string> { str });
                apModel.NHpLower = (int)hpSlider1.value;
            });

            nhp = getComponentByPath<RectTransform>("restoreCon/nhp");

            //!--2)普通MP药
            mpToggle1 = getComponentByPath<Toggle>("restoreCon/mpToggle1");
            mpToggle1.onValueChanged.AddListener((bool v) =>
            {
                apModel.NMpDrug = mpToggle1.isOn ? 1 : 0;
            });

            mpTog1Txt = getComponentByPath<Text>("restoreCon/mpToggle1/Label");

            mpSlider1 = getComponentByPath<Slider>("restoreCon/mpSlider1");
            mpSlider1.onValueChanged.AddListener((float v) =>
            {
                string str = string.Format("{0,3}", v);
                mpTog1Txt.text = ContMgr.getCont("autoplay_restore_1", new List<string> { str });
                apModel.NMpLower = (int)mpSlider1.value;
            });

            nmp = getComponentByPath<RectTransform>("restoreCon/nmp");

            //!--3)商城HP药
            hpToggle2 = getComponentByPath<Toggle>("restoreCon/hpToggle2");
            hpToggle2.onValueChanged.AddListener((bool v) =>
            {
                apModel.MHpDrug = hpToggle2.isOn ? 1 : 0;
            });

            hpTog2Txt = getComponentByPath<Text>("restoreCon/hpToggle2/Label");

            hpSlider2 = getComponentByPath<Slider>("restoreCon/hpSlider2");
            hpSlider2.onValueChanged.AddListener((float v) =>
            {
                string str = string.Format("{0,3}", (int)v);
                hpTog2Txt.text = ContMgr.getCont("autoplay_restore_0", new List<string> { str });
                apModel.MHpLower = (int)hpSlider2.value;
            });
            mhp = getComponentByPath<RectTransform>("restoreCon/mhp");

            //!--4)购买HP药
            hpDrugToggle = getComponentByPath<Toggle>("restoreCon/hpDrugToggle");
            hpDrugToggle.onValueChanged.AddListener((bool v) =>
            {
                apModel.BuyHpDrug = hpDrugToggle.isOn ? 1 : 0;
            });

            hpDrugTogText = getComponentByPath<Text>("restoreCon/hpDrugToggle/Label");
            
            hpDrugSlider = getComponentByPath<Slider>("restoreCon/hpDrugSlider");
            hpDrugSlider.onValueChanged.AddListener((float v) =>
            {
                string str = string.Format("{0,3}", (int)v);
                hpDrugTogText.text = ContMgr.getCont("autoplay_restore_2", new List<string> { str });
                apModel.BuyHpDrugAmount = (int)hpDrugSlider.value;
            });

            buyhp = getComponentByPath<RectTransform>("restoreCon/buyhp");

            //!--5)购买MP药
            mpDrugToggle = getComponentByPath<Toggle>("restoreCon/mpDrugToggle");
            mpDrugToggle.onValueChanged.AddListener((bool v) =>
            {
                apModel.BuyMpDrug = mpDrugToggle.isOn ? 1 : 0;
            });
            
            mpDrugTogText = getComponentByPath<Text>("restoreCon/mpDrugToggle/Label");
            
            mpDrugSlider = getComponentByPath<Slider>("restoreCon/mpDrugSlider");
            mpDrugSlider.onValueChanged.AddListener((float v) =>
            {
                string str = string.Format("{0,3}", (int)v);
                mpDrugTogText.text = ContMgr.getCont("autoplay_restore_3", new List<string> { str });
                apModel.BuyMpDrugAmount = (int)mpDrugSlider.value;
            });
            
            buymp = getComponentByPath<RectTransform>("restoreCon/buymp");

            //!--6)药品弹出框
            nhpgrid = getComponentByPath<GridLayoutGroup>("restoreCon/nhpchoose");
            nmpgrid = getComponentByPath<GridLayoutGroup>("restoreCon/nmpchoose");
        }

        private void InitPickCon()
        {
            eqpToggles = new Toggle[ITEM_QUAL];
            for (int i = 0; i < ITEM_QUAL; i++)
            {
                eqpToggles[i] = getComponentByPath<Toggle>("pickCon/eqptoggle" + i.ToString());
                eqpToggles[i].onValueChanged.AddListener(OnEqpToggle);
            }
            eqpAllToggle = getComponentByPath<Toggle>("pickCon/eqpAllToggle");
            eqpAllToggle.onValueChanged.AddListener(OnEqpAllToggle);

            matToggles = new Toggle[ITEM_QUAL];
            for (int i = 0; i < ITEM_QUAL; i++)
            {
                matToggles[i] = getComponentByPath<Toggle>("pickCon/mattoggle" + i.ToString());
                matToggles[i].onValueChanged.AddListener(OnMatToggle);
            }
            matAllToggle = getComponentByPath<Toggle>("pickCon/matAllToggle");
            matAllToggle.onValueChanged.AddListener(OnMatAllToggle);

            procToggles = new Toggle[ITEM_QUAL];
            for (int i = 0; i < ITEM_QUAL; i++)
            {
                procToggles[i] = getComponentByPath<Toggle>("pickCon/proctoggle" + i.ToString());
                procToggles[i].onValueChanged.AddListener(OnProcToggle);
            }
            procAllToggle = getComponentByPath<Toggle>("pickCon/procAllToggle");
            procAllToggle.onValueChanged.AddListener(OnProcAllToggle);

            eqpDoSlider = getComponentByPath<Slider>("pickCon/eqpDoSlider");
            eqpDoSlider.onValueChanged.AddListener((float v) =>
            {
                apModel.EqpProc = (int)eqpDoSlider.value;
            });
        }

        private void InitFightCon()
        {
            //!--战斗面板相关控件持有
            scopeSlider = getComponentByPath<Slider>("fightCon/scopeSlider");
            scopeSlider.onValueChanged.AddListener(OnScopeSliderValueChange);
            EventTriggerListener.Get(scopeSlider.gameObject).onDown = OnScopeSliderDown;
            EventTriggerListener.Get(scopeSlider.gameObject).onUp = OnScopeSliderUp;

            mapCon = getComponentByPath<Image>("fightCon/map");

            avoidToggle = getComponentByPath<Toggle>("fightCon/avoidToggle");
            avoidToggle.onValueChanged.AddListener((bool v) =>
            {
                apModel.Avoid = avoidToggle.isOn ? 1 : 0;
            });

            autoPkToggle = getComponentByPath<Toggle>("fightCon/autoPKToggle");
            autoPkToggle.onValueChanged.AddListener((bool v) =>
            {
                apModel.AutoPK = autoPkToggle.isOn ? 1 : 0;
            });

            stoneRespawnToggle = getComponentByPath<Toggle>("fightCon/stoneRespawnToggle");
            stoneRespawnToggle.onValueChanged.AddListener((bool v) =>
            {
                apModel.StoneRespawn = stoneRespawnToggle.isOn ? 1 : 0;
            });

            goldRespawnToggle = getComponentByPath<Toggle>("fightCon/goldRespawnToggle");
            goldRespawnToggle.onValueChanged.AddListener((bool v) =>
            {
                apModel.GoldRespawn = goldRespawnToggle.isOn ? 1 : 0;
            });

            backRespawnToggle = getComponentByPath<Toggle>("fightCon/backRespawnToggle");
            backRespawnToggle.onValueChanged.AddListener((bool v) =>
            {
                apModel.BackRespawn = backRespawnToggle.isOn ? 1 : 0;
            });

            respawnUpboundInput = getComponentByPath<InputField>("fightCon/respawnUpBound");
            respawnUpboundInput.onValueChange.AddListener((string s) =>
            {
                if (respawnUpboundInput.text.Length != 0)
                {
                    apModel.RespawnUpBound = int.Parse(respawnUpboundInput.text);
                }
            });

            //!--技能初始化
            skillIcons = new GameObject[5];
            for (int i = 0; i < 5; i++)
            {
                int index = i;

                GameObject skilIcon = getGameObjectByPath("fightCon/skill" + i.ToString());
                skilIcon.transform.GetComponent<Button>().onClick.AddListener(() =>
                {
                    InterfaceMgr.getInstance().open(InterfaceMgr.A3_AUTOPLAY_SKILL);
                    a3_autoplay_skill.SkillSeat = index;
                });

                skillIcons[i] = skilIcon;
            }
            a3_autoplay_skill.OnSkillChoose += OnSkillChoose;
        }

        private void OnSkillChoose(int skid, int index)
        {
            for (int i = 0; i < skillIds.Length; i++)
            {
                if (skid == skillIds[i] && i != index)
                {
                    //!--如果该技能已经在自动释放的技能列表里面,但是在另一个位置
                    //!--则取消该位置的技能
                    skillIds[i] = 0;
                    skillIcons[i].transform.FindChild("mask").gameObject.SetActive(false);
                }
            }
            
            GameObject icon = skillIcons[index];
            if (skid != 0)
            {
                icon.transform.FindChild("mask").gameObject.SetActive(true);
            }
            else
            {
                icon.transform.FindChild("mask").gameObject.SetActive(false);
            }
            skillIds[index] = skid;
        }

        public override void onShowed()
        {
            SetRestoreCon();
            SetPickCon();
            SetFightCon();

            BagProxy.getInstance().addEventListener(BagProxy.EVENT_ITEM_CHANGE, RefreshDrugsNum);

            //!--初始启动停止按钮，注册回调
            OnFSMStartStop(SelfRole.fsm.Autofighting);
            SelfRole.fsm.OnFSMStartStop += OnFSMStartStop;
        }

        public override void onClosed()
        {
            apModel.WriteLocalData();

            int cnt = nhpgrid.transform.childCount;
            for (int i = 0; i < cnt; i++)
            {
                Destroy(nhpgrid.transform.GetChild(i).gameObject);
            }
            cnt = nmpgrid.transform.childCount;
            for (int i = 0; i < cnt; i++)
            {
                Destroy(nmpgrid.transform.GetChild(i).gameObject);
            }

            BagProxy.getInstance().addEventListener(BagProxy.EVENT_ITEM_CHANGE, RefreshDrugsNum);
            SelfRole.fsm.OnFSMStartStop -= OnFSMStartStop;
        }

        private void SetRestoreCon()
        {
            string str;

            //!--喝HP药
            hpToggle1.isOn = (apModel.NHpDrug > 0);
            str = string.Format("{0,3}", apModel.NHpLower);
            hpTog1Txt.text = ContMgr.getCont("autoplay_restore_0", new List<string> { str });
            hpSlider1.value = apModel.NHpLower;

            //!--喝MP药
            mpToggle1.isOn = (apModel.NMpDrug > 0);
            str = string.Format("{0,3}", apModel.NMpLower);
            mpTog1Txt.text = ContMgr.getCont("autoplay_restore_1", new List<string> { str });
            mpSlider1.value = apModel.NMpLower;

            //!--喝商城药
            hpToggle2.isOn = (apModel.MHpDrug > 0);
            str = string.Format("{0,3}", apModel.MHpLower);
            hpTog2Txt.text = ContMgr.getCont("autoplay_restore_0", new List<string> { str });
            hpSlider2.value = apModel.MHpLower;

            //!--自动补充HP药
            hpDrugToggle.isOn = (apModel.BuyHpDrug > 0);
            str = string.Format("{0,3}", apModel.BuyHpDrugAmount);
            hpDrugTogText.text = ContMgr.getCont("autoplay_restore_2", new List<string> { str });
            hpDrugSlider.value = apModel.BuyHpDrugAmount;

            //!--自动补充MP药
            mpDrugToggle.isOn = (apModel.BuyMpDrug > 0);
            str = string.Format("{0,3}", apModel.BuyMpDrugAmount);
            mpDrugTogText.text = ContMgr.getCont("autoplay_restore_3", new List<string> { str });
            mpDrugSlider.value = apModel.BuyMpDrugAmount;

            //!--创建普通HP药ICON
            SetInUseHpDrug(AutoPlayModel.getInstance().NHpDrugId);

            //!--创建普通MP药ICON
            SetInUseMpDrug(AutoPlayModel.getInstance().NMpDrugId);

            //!--创建商城药ICON
            int tid = AutoPlayModel.getInstance().MHpDrugId;
            int n = a3_BagModel.getInstance().getItemNumByTpid((uint)tid);
            GameObject go = GetIcon(tid, n);
            go.transform.parent = mhp;
            go.transform.localPosition = Vector3.zero;

            //!--创建购买HP药ICON
            SetInBuyHpDrug(AutoPlayModel.getInstance().NHpDrugId);
            
            //!--创建购买MP药ICON
            SetInBuyMpDrug(AutoPlayModel.getInstance().NMpDrugId);

            //!--创建HP药列表
            List<SXML> nhpxml = AutoPlayModel.getInstance().AutoplayXml.GetNodeList("nhp","");
            foreach (SXML x in nhpxml)
            {
                int tpid = x.getInt("id");
                int num = a3_BagModel.getInstance().getItemNumByTpid((uint)tpid);
                go = GetIcon(tpid, num);
                GameObject goempty = new GameObject();
                goempty.AddComponent<RectTransform>();
                go.transform.parent = goempty.transform;
                goempty.transform.parent = nhpgrid.transform;
                go.transform.GetComponent<Button>().onClick.AddListener(() =>
                {
                    SetInUseHpDrug(tpid);
                    SetInBuyHpDrug(tpid);
                    OnCloseHpGrid();
                });
            }

            //!--创建MP药列表
            List<SXML> nmpxml = AutoPlayModel.getInstance().AutoplayXml.GetNodeList("nmp", "");
            foreach (SXML x in nmpxml)
            {
                int tpid = x.getInt("id");
                int num = a3_BagModel.getInstance().getItemNumByTpid((uint)tpid);
                go = GetIcon(tpid, num);
                GameObject goempty = new GameObject();
                goempty.AddComponent<RectTransform>();
                go.transform.parent = goempty.transform;
                goempty.transform.parent = nmpgrid.transform;
                go.transform.GetComponent<Button>().onClick.AddListener(() =>
                {
                    SetInUseMpDrug(tpid);
                    SetInBuyMpDrug(tpid);
                    OnCloseMpGrid();
                });
            }
        }

        private void SetInUseHpDrug(int tpid)
        {
            if (nhp.childCount > 0)
            {
                Transform child = nhp.GetChild(0);
                if (child.name == tpid.ToString()) return;
                Destroy(child.gameObject);
            }

            int num = a3_BagModel.getInstance().getItemNumByTpid((uint)tpid);
            GameObject go = GetIcon(tpid, num);
            go.transform.parent = nhp;
            go.transform.localPosition = Vector3.zero;
            go.transform.GetComponent<Button>().onClick.AddListener(() =>
            {
                CancelInvoke("onCloseHpGrid");
                nhpgrid.gameObject.SetActive(true);
                Invoke("onCloseHpGrid", 3.0f);
            });

            apModel.NHpDrugId = tpid;
        }

        private void SetInUseMpDrug(int tpid)
        {
            if (nmp.childCount > 0)
            {
                Transform child = nmp.GetChild(0);
                if (child.name == tpid.ToString()) return;
                Destroy(child.gameObject);
            }

            int num = a3_BagModel.getInstance().getItemNumByTpid((uint)tpid);
            GameObject go = GetIcon(tpid, num);
            go.transform.parent = nmp;
            go.transform.localPosition = Vector3.zero;
            go.transform.GetComponent<Button>().onClick.AddListener(() =>
            {
                CancelInvoke("onCloseMpGrid");
                nmpgrid.gameObject.SetActive(true);
                Invoke("onCloseMpGrid", 3.0f);
            });

            apModel.NMpDrugId = tpid;
        }

        private void SetInBuyHpDrug(int tpid)
        {
            if (buyhp.childCount > 0)
            {
                Transform child = buyhp.GetChild(0);
                if (child.name == tpid.ToString()) return;
                Destroy(child.gameObject);
            }

            int num = a3_BagModel.getInstance().getItemNumByTpid((uint)tpid);
            GameObject go = GetIcon(tpid, num);
            go.transform.parent = buyhp;
            go.transform.localPosition = Vector3.zero;
        }

        private void SetInBuyMpDrug(int tpid)
        {
            if (buymp.childCount > 0)
            {
                Transform child = buymp.GetChild(0);
                if (child.name == tpid.ToString()) return;
                Destroy(child.gameObject);
            }

            int num = a3_BagModel.getInstance().getItemNumByTpid((uint)tpid);
            GameObject go = GetIcon(tpid, num);
            go.transform.parent = buymp;
            go.transform.localPosition = Vector3.zero;
        }

        private void OnCloseHpGrid()
        {
            nhpgrid.gameObject.SetActive(false);
        }

        private void OnCloseMpGrid()
        {
            nmpgrid.gameObject.SetActive(false);
        }

        private GameObject GetIcon(int tpid, int num)
        {
            GameObject go = IconImageMgr.getInstance().createA3ItemIcon((uint)tpid, true, num, 0.6f, true);
            go.name = tpid.ToString();
            return go;
        }

        private void RefreshDrugsNum(GameEvent e)
        {
            RefreshIconNum(nhp.GetChild(0));
            RefreshIconNum(nmp.GetChild(0));
            RefreshIconNum(mhp.GetChild(0));
            RefreshIconNum(buyhp.GetChild(0));
            RefreshIconNum(buymp.GetChild(0));

            for (int i = 0; i < nhpgrid.transform.childCount; i++)
            {
                RefreshIconNum(nhpgrid.transform.GetChild(i).GetChild(0));
            }

            for (int i = 0; i < nmpgrid.transform.childCount; i++)
            {
                RefreshIconNum(nmpgrid.transform.GetChild(i).GetChild(0));
            }
        }

        private void RefreshIconNum(Transform icon)
        {
            uint tpid = uint.Parse(icon.name);
            Text numText = icon.FindChild("num").GetComponent<Text>();
            int num = a3_BagModel.getInstance().getItemNumByTpid(tpid);
            numText.text = num.ToString();
        }

        private void SetPickCon()
        {
            for (int i = 0; i < ITEM_QUAL; i++)
            {
                ForceSetToggle(eqpToggles[i], (apModel.PickEqp & (1 << i)) != 0, OnEqpToggle);
            }
            ForceSetToggle(eqpAllToggle, (apModel.PickEqp == (int)Quality.QAll), OnEqpAllToggle);

            for (int i = 0; i < ITEM_QUAL; i++)
            {
                ForceSetToggle(matToggles[i], (apModel.PickMat & (1 << i)) != 0, OnMatToggle);
            }
            ForceSetToggle(matAllToggle, (apModel.PickMat == (int)Quality.QAll), OnMatAllToggle);

            for (int i = 0; i < ITEM_QUAL; i++)
            {
                ForceSetToggle(procToggles[i], (apModel.EqpProcType & (1 << i)) != 0, OnProcToggle);
            }
            ForceSetToggle(procAllToggle, apModel.EqpProcType == (int)Quality.QAll, OnProcAllToggle);

            eqpDoSlider.value = (int) apModel.EqpProc;
        }

        private void SetFightCon()
        {
            scopeSlider.value = (int)apModel.Scope;
            avoidToggle.isOn = (apModel.Avoid > 0);
            autoPkToggle.isOn = (apModel.AutoPK > 0);
            stoneRespawnToggle.isOn = (apModel.StoneRespawn > 0);
            goldRespawnToggle.isOn = (apModel.GoldRespawn > 0);
            backRespawnToggle.isOn = (apModel.BackRespawn > 0);
            respawnUpboundInput.text = apModel.RespawnUpBound.ToString();

            skillIds = AutoPlayModel.getInstance().Skills;

            for (int i = 0; i < 5; i++)
            {
                if (skillIds[i] > 0)
                {
                    skillIcons[i].transform.FindChild("mask").gameObject.SetActive(true);
                }
                else
                {
                    skillIcons[i].transform.FindChild("mask").gameObject.SetActive(false);
                }
            }

        }

        private void OnEqpToggle(bool on)
        {
            bool v = true;
            for (int i = 0; i < ITEM_QUAL; i++)
            {
                if (eqpToggles[i].isOn == false)
                {
                    v = false;
                    break;
                }
            }
            ForceSetToggle(eqpAllToggle, v, OnEqpAllToggle);
            SaveALLPickToggle();
        }

        private void OnEqpAllToggle(bool on)
        {
            for (int i = 0; i < ITEM_QUAL; i++)
            {
                ForceSetToggle(eqpToggles[i],on,OnEqpToggle);
            }
            SaveALLPickToggle();
        }

        private void OnMatToggle(bool on)
        {
            bool v = true;
            for (int i = 0; i < ITEM_QUAL; i++)
            {
                if (matToggles[i].isOn == false)
                {
                    v = false;
                    break;
                }
            }
            ForceSetToggle(matAllToggle, v, OnMatAllToggle);
            SaveALLPickToggle();
        }

        private void OnMatAllToggle(bool on)
        {
            for (int i = 0; i < ITEM_QUAL; i++)
            {
                ForceSetToggle(matToggles[i], on, OnMatToggle);
            }
            SaveALLPickToggle();
        }

        private void OnProcToggle(bool on)
        {
            bool v = true;
            for (int i = 0; i < ITEM_QUAL; i++)
            {
                if (procToggles[i].isOn == false){
                    v = false;
                    break;
                }
            }

            ForceSetToggle(procAllToggle, v, OnProcAllToggle);
            SaveALLPickToggle();
        }

        private void OnProcAllToggle(bool on)
        {
            for (int i = 0; i < ITEM_QUAL; i++)
            {
                ForceSetToggle(procToggles[i], on, OnProcToggle);
            }
            SaveALLPickToggle();
        }

        private void ForceSetToggle(Toggle toggle, bool value, UnityAction<bool> cb)
        {//!--强制设置Toggle值,不触发onvaluechange事件(TODO Fix me!!! the pick toggle logic)
            toggle.onValueChanged.RemoveListener(cb);
            toggle.isOn = value;
            toggle.onValueChanged.AddListener(cb);
        }

        private void OnScopeSliderDown(GameObject go)
        {
            uint mapid = PlayerModel.getInstance().mapid;
            mapCon.sprite = GAMEAPI.ABUI_LoadSprite("map_map" + mapid).texture;
            mapCon.gameObject.SetActive(true);
            RefreshMapCon();
        }

        private void OnScopeSliderUp(GameObject go)
        {
            mapCon.gameObject.SetActive(false);
        }

        private void OnScopeSliderValueChange(float v)
        {
            apModel.Scope = (int)scopeSlider.value;
            RefreshMapCon();
        }

        private void RefreshMapCon()
        {
            RectTransform rt = mapCon.GetComponent<RectTransform>();
            RectTransform pos_rt = mapCon.transform.FindChild("mypos").GetComponent<RectTransform>();
            RectTransform ring_rt = mapCon.transform.FindChild("mypos/circle").GetComponent<RectTransform>();
            
            Vector2 mypos = SceneCamera.getPosOnMiniMap(SelfRole._inst.m_curModel.position);
            pos_rt.anchoredPosition = mypos;

            float diameter = SceneCamera.getLengthOnMinMap(scopeSlider.value) * 2;
            ring_rt.transform.localScale = new Vector3(diameter, diameter, diameter);
        }

        private void OnTabSwitch(TabControl t)
        {
            GameObject go0 = getGameObjectByPath("restoreCon");
            GameObject go1 = getGameObjectByPath("pickCon");
            GameObject go2 = getGameObjectByPath("fightCon");

            go0.SetActive(false);
            go1.SetActive(false);
            go2.SetActive(false);

            switch (t.getSeletedIndex())
            {
                case 0:
                    go0.SetActive(true);
                    break;
                case 1:
                    go1.SetActive(true);
                    break;
                case 2:
                    go2.SetActive(true);
                    break;
            }
        }

        private void OnStartAutoPlay(GameObject go)
        {
            SelfRole.fsm.StartAutofight();
            onClose(null);
        }

        private void OnStopAutoplay(GameObject go)
        {
            SelfRole.fsm.Stop();
            onClose(null);
        }

        private void OnFSMStartStop(bool isRunning)
        {
            getGameObjectByPath("autoplayBtn").SetActive(!isRunning);
            getGameObjectByPath("stopBtn").SetActive(isRunning);
        }

        private void onClose(GameObject go)
        {
            InterfaceMgr.getInstance().close(InterfaceMgr.A3_AUTOPLAY);
        }

        private void SaveALLPickToggle()
        {
            apModel.PickEqp = 0;
            apModel.PickMat = 0;
            apModel.EqpProcType = 0;
            for (int i = 0; i < ITEM_QUAL; i++)
            {
                int cur = (1 << i);

                if (eqpToggles[i].isOn)
                    apModel.PickEqp += cur;

                if (matToggles[i].isOn)
                    apModel.PickMat += cur;

                if (procToggles[i].isOn)
                    apModel.EqpProcType += cur;
            }
        }*/
    }
}
