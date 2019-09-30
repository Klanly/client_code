using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameFramework;
using UnityEngine;
using UnityEngine.UI;
using Object = UnityEngine.Object;

namespace MuGame
{
    class a3_pet_skin : Window
    {
        private  A3_PetModel petmodel;

        private  uint feedid;
        private  uint levelid;
        private  uint stageid;

        private uint stagestep;

        private SXML currentStage;
        private SXML currentLevel;

        private RenderTexture petTexture;
        private GameObject avatarCamera;
        private GameObject pet;
        public  Toggle autobuy;
        public  Toggle autofeed;
        private GameObject prefab;
        public static a3_pet_skin instan;

        public override void init()
        {
            instan = this;
            petmodel = A3_PetModel.getInstance();

            feedid = petmodel.GetFeedItemTpid();
            levelid = petmodel.GetLevelItemTpid();
            stageid = petmodel.GetStageItemTpid();

            currentLevel = petmodel.CurrentLevelConf();
            currentStage = petmodel.CurrentStageConf();
            stagestep = currentStage.getUint("crystal_step");

            BaseButton upBtn = new BaseButton(getTransformByPath("exp_con/upgrade"));
            upBtn.onClick = OnUpgrade;

            BaseButton onekeyBtn = new BaseButton(getTransformByPath("exp_con/onekey"));
            onekeyBtn.onClick = OnOnekey;

            BaseButton feedBtn = new BaseButton(getTransformByPath("lampoil"));
            feedBtn.onClick = OnFeed;

            BaseButton stageBtn = new BaseButton(getTransformByPath("stage_con/improve"));
            stageBtn.onClick = OnStage;

            BaseButton helpBtn = new BaseButton(getTransformByPath("title/help"));
            helpBtn.onClick = OnHelp;

            BaseButton close_btn = new BaseButton(getTransformByPath("close"));
            close_btn.onClick = onclose;

            autofeed = getComponentByPath<Toggle>("light_hint/toggle");
            autofeed.onValueChanged.AddListener(OnAutoFeedToggleChange);

            autobuy = getComponentByPath<Toggle>("light_hint/toggle2");
            autobuy.onValueChanged.AddListener(OnAutoBuyToggleChange);

            prefab = getGameObjectByPath("att/a3_pet_att");
        }

        public override void onShowed()
        {
            A3_PetProxy.getInstance().GetPets();
            petmodel.OnExpChange += OnExpChange;
            //petmodel.OnStarvationChange += OnStarvationChange;
           // petmodel.OnAutoFeedChange += OnAutoFeedChange;
            petmodel.OnAutoFeedChange -= OnAutoBuyChange;
            petmodel.OnLevelChange += OnLevelChange;
            petmodel.OnStageChange += OnStageChange;

            OnExpChange();
            OnStarvationChange();
            OnAutoFeedChange();
            OnAutoBuyChange();
            OnLevelChange();
            OnStageChange();
            OnPetItemChange(null);

            BagProxy.getInstance().addEventListener(BagProxy.EVENT_ITEM_CHANGE, OnPetItemChange);

            //if (petmodel.Starvation < 20)
            //{
            //    String hint = ContMgr.getCont("pet_hungry");
            //    flytxt.instance.fly(hint);
            //}
        }

        public override void onClosed()
        {
            petmodel.OnExpChange -= OnExpChange;
            petmodel.OnStarvationChange -= OnStarvationChange;
            //petmodel.OnAutoFeedChange -= OnAutoFeedChange;
            //petmodel.Onauto_buyChange -= OnAutoBuyChange;
            petmodel.OnLevelChange -= OnLevelChange;
            petmodel.OnStageChange -= OnStageChange;

            BagProxy.getInstance().removeEventListener(BagProxy.EVENT_ITEM_CHANGE, OnPetItemChange);

            if (petTexture != null)
            {
                GameObject.Destroy(petTexture);
                petTexture = null;
            }
            if (avatarCamera != null)
            {
                GameObject.Destroy(avatarCamera);
                avatarCamera = null;
            }
            if (pet != null)
            {
                GameObject.Destroy(pet);
                pet = null;
            }
        }

        private void OnExpChange()
        {
            int maxExp = petmodel.GetMaxExp();
            int curExp = petmodel.Exp;
            
            Slider slider = getComponentByPath<Slider>("exp_con/expbar/slider");
            slider.value = (float) curExp/maxExp;

            Text text = getComponentByPath<Text>("exp_con/expbar/text");
            text.text = curExp + "/" + maxExp;
        }

        public void OnStarvationChange()
        {
            int starvation = petmodel.Starvation;
            Image image = getComponentByPath<Image>("light1/image");
            image.fillAmount = (float) starvation/100.0f;
        }

        private void OnAutoFeedChange()
        {
            Toggle toggle = getComponentByPath<Toggle>("light_hint/toggle");
            toggle.onValueChanged.RemoveListener(OnAutoFeedToggleChange);
            toggle.isOn = petmodel.Auto_feed;
            toggle.onValueChanged.AddListener(OnAutoFeedToggleChange);
        }

        private void OnAutoBuyChange() 
        {
            Toggle toggle = getComponentByPath<Toggle>("light_hint/toggle2");
            toggle.onValueChanged.RemoveListener(OnAutoBuyToggleChange);
            toggle.isOn = petmodel.Auto_buy;
            toggle.onValueChanged.AddListener(OnAutoBuyToggleChange);
        }
        private void OnLevelChange()
        {
            currentLevel = petmodel.CurrentLevelConf();

            //!--更新等级
            Text level = getComponentByPath<Text>("lvl");
            level.text = "Lv " + petmodel.Level;

            //!--更新升级消耗
            RefreshLevelUpCost();

            //!--刷新属性
            OnAttChange();

            //!--刷新经验
            OnExpChange();

            //!--检查是否需要显示升阶的面板
            CheckShowLevelOrStage();
        }

        private void OnStageChange()
        {
            currentStage = petmodel.CurrentStageConf();
            stagestep = currentStage.getUint("crystal_step");

            //!--更新宠物的阶
            Text stage = getComponentByPath<Text>("stage");
            stage.text = "(" + petmodel.Stage.ToString() + ContMgr.getCont("zhuan")+")";

            Text name = getComponentByPath<Text>("name");
            name.text = petmodel.CurrentStageConf().getString("name");

            //!--更新阶面板的金币
            Text gold = getComponentByPath<Text>("stage_con/gold/text");
            gold.text = currentStage.getString("gold_cost");

            //!--更新阶面板的启灵水晶消耗
            Text cost = getComponentByPath<Text>("exp_con/upgrade/text");
            uint costnum = currentStage.getUint("crystal");
            int havenum = a3_BagModel.getInstance().getItemNumByTpid(stageid);
            string color = (costnum <= havenum) ? "<color=#00ffff>" : "<color=#ff0000>";
            cost.text = costnum.ToString() ;

            //!--更新进阶成功率
            Text rate = getComponentByPath<Text>("stage_con/rate");
            uint urate = currentStage.getUint("rate")/100;
            rate.text = ContMgr.getCont("pet_succ", new List<string> { urate.ToString() });

            //!--检查是否需要显示升阶的面板
            CheckShowLevelOrStage();

            //!--刷新界面上宠物avatar
            RefreshAvatar();
        }

        private void OnAttChange()
        {
            Transform grid = getComponentByPath<Transform>("att/grid");
            for (int i = 0; i < grid.childCount; i++)
            {
                Object.Destroy(grid.GetChild(i).gameObject);
            }

            SXML next = petmodel.NextLevelConf();
            var etor = currentLevel.m_dAtttr.GetEnumerator();
            while (etor.MoveNext())
            {
                if (Globle.AttNameIDDic.ContainsKey(etor.Current.Key))
                {
                    if (etor.Current.Key == "mp_suck")
                    {
                        continue;
                    }
                    GameObject go = Object.Instantiate(prefab) as GameObject;
                    if (go == null) return;

                    go.SetActive(true);
                    Transform trans = go.transform;
                    int id = Globle.AttNameIDDic[etor.Current.Key];
                    string str = etor.Current.Value.str;
                   
                    if (id == 30 || id == 33)
                    {
                        str = (int.Parse(etor.Current.Value.str)/10f) + "%";                       
                    }
                        trans.FindChild("name").GetComponent<Text>().text = Globle.getAttrNameById(id);
                        trans.FindChild("cur").GetComponent<Text>().text = "+" + str;
                        Text text = trans.FindChild("next").GetComponent<Text>();

                        if (next != null)
                        {
                            if (id == 30 || id == 33)
                            {
                            text.text = "+" + (int.Parse(next.getString(etor.Current.Key)) / 10f) + "%";
                            }
                            else 
                            {
                                text.text = "+" + next.getString(etor.Current.Key);
                            }
                            
                        }
                        else
                        {
                            text.text = String.Empty;
                        }      
                    trans.SetParent(grid,false );
                }
            }
        }

        private void OnUpgrade(GameObject go) 
        {
            Toggle toggle = getComponentByPath<Toggle>("exp_con/toggle");
            A3_PetProxy.getInstance().Bless(toggle.isOn);
        }

        private void OnOnekey(GameObject go)
        {
            Toggle toggle = getComponentByPath<Toggle>("exp_con/toggle");
            A3_PetProxy.getInstance().OneKeyBless(toggle.isOn);
        }

        private void OnFeed(GameObject go)
        {
            int havenum = a3_BagModel.getInstance().getItemNumByTpid(feedid);
            if (havenum > 0)
            {
                if (havenum == 1)
                {
                    if (A3_PetModel.getInstance().Auto_buy == false)
                    {
                    //String hint = ContMgr.getCont("pet_no_feed");
                   // if (flytxt.instance != null)
                      //  flytxt.instance.fly(hint);
                    }
                }
                A3_PetProxy.getInstance().Feed();
            }
            else
            {
                a3_store.itm_tpid = feedid;
                InterfaceMgr.getInstance().ui_async_open(InterfaceMgr.A3_STORE);
            }
        }

        private void OnStage(GameObject go)
        {
            uint costnum = currentStage.getUint("crystal");
            A3_PetProxy.getInstance().Stage((int)costnum);
        }

        private void OnHelp(GameObject go)
        {
            InterfaceMgr.getInstance().ui_async_open(InterfaceMgr.A3_PET_DESC);
        }

        private void OnAutoFeedToggleChange(bool isOn)
        {
            if (isOn)
            {
                A3_PetProxy.getInstance().SetAutoFeed(1);
            }
            else
            {
                A3_PetProxy.getInstance().SetAutoFeed(0);
            }
        }

        private void OnAutoBuyToggleChange(bool isOn) 
        {
            if (isOn)
            {
                A3_PetProxy.getInstance().SetAutoBuy(1);
            }
            else 
            { 
                A3_PetProxy.getInstance().SetAutoBuy(0);
            }
        }
        private void OnPetItemChange(GameEvent e)
        {
            int num = a3_BagModel.getInstance().getItemNumByTpid(feedid);
            getComponentByPath<Text>("lampoil/num").text = num.ToString();

            RefreshLevelUpCost();

            Text cost = getComponentByPath<Text>("stage_con/stageitm/text");
            uint costnum = currentStage.getUint("crystal");
            int havenum = a3_BagModel.getInstance().getItemNumByTpid(stageid);
            string color = (costnum <= havenum) ? "<color=#00ffff>" : "<color=#ff0000>";
            cost.text = costnum.ToString() + color + "/" + havenum.ToString() + "</color>";
        }

        private void RefreshLevelUpCost()
        {
            Text gold = getComponentByPath<Text>("exp_con/gold/text");
            gold.text = currentLevel.getString("cost_gold");

            Text item = getComponentByPath<Text>("exp_con/upgrade/text");
            int havenum = a3_BagModel.getInstance().getItemNumByTpid(levelid);
            uint costnum = currentLevel.getUint("cost_item_num");
            string color = (costnum <= havenum) ? "<color=#00ffff>" : "<color=#ff0000>";
            item.text = costnum.ToString();
        }

        private void RefreshAvatar()
        {
            RawImage petAvatar = getComponentByPath<RawImage>("avatar");
            if (petTexture == null){
                petTexture = new RenderTexture(380, 420, 16);
            }
            if (avatarCamera == null){
                GameObject prefab = GAMEAPI.ABFight_LoadPrefab("model_avatar_ui_avatar_ui_camera");
                avatarCamera = Object.Instantiate(prefab) as GameObject;
            }
            Camera camera = avatarCamera.GetComponentInChildren<Camera>();
            camera.targetTexture = petTexture;
            petAvatar.texture = petTexture;

            string avapath = petmodel.GetPetAvatar((int)petmodel.Tpid,0);
            GameObject petPrefab = GAMEAPI.ABModel_LoadNow_GameObject("profession_" + avapath);
            if (pet != null){
                Object.Destroy(pet);
            }
            pet = GameObject.Instantiate(petPrefab, new Vector3(-128f, 0f, 0f), Quaternion.identity) as GameObject;
            pet.transform.localScale = Vector3.one;
            pet.transform.Rotate(new Vector3(20,40,-30));
            foreach (Transform tran in pet.GetComponentsInChildren<Transform>())
            {
                tran.gameObject.layer = EnumLayer.LM_FX;
            }
        }
        void onclose(GameObject go)
        {
            InterfaceMgr.getInstance().close(InterfaceMgr.A3_PET_SKIN);
        }
        private void CheckShowLevelOrStage()
        {
            uint nextLvl = currentStage.getUint("to_next_stage_level");
            SXML nextLvlSXML = petmodel.NextLevelConf();
            bool showStage = (nextLvlSXML != null && petmodel.Level >= nextLvl);
            GameObject expCon = getGameObjectByPath("exp_con");
            GameObject stageCon = getGameObjectByPath("stage_con");
            expCon.SetActive(!showStage);
            stageCon.SetActive(showStage);
        }
    }
}
