using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameFramework;
using UnityEngine;
using UnityEngine.UI;
using Object = UnityEngine.Object;

//ReSharper disable once CheckNamespace
namespace MuGame
{
    enum PetFunc
    {
        PetList,
        PetTrain,
        PetUpgrade
    }

    /// <summary>
    /// 宠物界面中宠物展示的对象
    /// </summary>
    class PetShow
    {
        private GridLayoutGroup _parentGrid;
        private GameObject _me;
        private uint _petiid;

        private Image _petImg;
        private Text _rankTxt;
        private Transform _canUpgradeHint;
        private Transform _hungryHint;
        private Text _levelTxt;
        private Button _petBtn;

        private Action<uint> _onClick = null;
        public PetShow(GridLayoutGroup petShowGrid, GameObject petShowPrefab, uint iid, Action<uint> onClick)
        {
            _me = Object.Instantiate(petShowPrefab) as GameObject;
            if (_me == null)
            {
                //Debug.Log("[宠物界面] 宠物show预制错误！");
                return;
            }
            _me.transform.parent = petShowGrid.gameObject.transform;
            _parentGrid = petShowGrid;
            _petiid = iid;

            //!-- 获取PetShow下面的控件对象
            _petImg = _me.transform.FindChild("petimg").GetComponent<Image>();
            _petBtn = _petImg.GetComponent<Button>();
            BaseButton b_petShowBtn = new BaseButton(_petBtn.transform, 0, 0);
            b_petShowBtn.onClick = OnClickPetShow;
            _onClick = onClick;

            _rankTxt = _me.transform.FindChild("rank/ranktxt").GetComponent<Text>();
            _canUpgradeHint = _me.transform.FindChild("rank/hint");
            _hungryHint = _me.transform.FindChild("hungryHint");
            _levelTxt = _me.transform.FindChild("levelTxt").GetComponent<Text>();
        }

        public void Refresh()
        {
            PetData pd = PetModel.getInstance().GetPetDataByIid(_petiid);
            _rankTxt.text = pd.rank.ToString();
            _levelTxt.text = "Lv." + pd.level.ToString();

            bool b = PetModel.getInstance().GetCanUpgrade(_petiid);
            _canUpgradeHint.gameObject.SetActive(b);

            b = PetModel.getInstance().IsHungry(_petiid);
            _hungryHint.gameObject.SetActive(b);
        }

        private void OnClickPetShow(GameObject go)
        {
            _onClick(_petiid);

            //TODO 显示选中状态
        }
    }

    class pet : Window
    {
        private Transform _closeBtn;
        private Transform _petListBtn;
        private Transform _petTrainBtn;
        private Transform _petUpgradeBtn;

        private Transform _petListCon;
        private Transform _petTrainCon;
        private Transform _petUpgradeCon;

        private GameObject _petShowPrefab; 
        private GridLayoutGroup _petShowGrid;
        private ContentSizeFitter _petShowContentSizeFitter;
        private Dictionary<uint, PetShow> _petShows = new Dictionary<uint, PetShow>(); 

        private PetFunc _curFunc = PetFunc.PetList;
        private uint _curPetIid = 0;
        private PetData _curPetData = null;

        //!-- 宠物列表Container相关
        private Text _petListNameTxt;
        private Text _petListLevelTxt;
        private Slider _hungrySlider;
        private Text _hungrySliderTxt;
        private Slider _expSlider;
        private Text _expSliderTxt;
        private GameObject _petListAtt;
        private Toggle _hungryAutoUse;
        private Button _feedBtn;
        private Button _recyleBtn;
        private Button _changeNameBtn;
        private Button _useBtn;
 
        //!-- 宠物升级Container相关
        private Text _petTrainNameTxt;
        private Text _petTrainLevelTxt;
        private GameObject _petTrainAtt;
        private Slider _blessSlider;
        private Text _blessSliderTxt;
        private Text _costGoldTxt;
        private Text _costBlessTxt;
        private Text _blessLeftTxt;
        private Toggle _diamondAutoUse;
        private Button _blessBtn;

        //!-- 宠物进阶Container相关
        private GameObject _pet3DShowImg;
        private GameObject _pet3DShowPoint;
        private GameObject _pet3D;
        private Text _petUpgrdeSuccTxt;
        private Slider _crystalSlider;
        private Text _petUpgradeGlodTxt;
        private Text _crystalCostTxt;
        private Text _crystalLeftTxt;
        private Text _upgradeBtn;
        private Image _peticon;


        override public void init()
        {
            #region --右侧功能区初始化
            _petListCon = transform.FindChild("petListCon");
            _petTrainCon = transform.FindChild("petTrainCon");
            _petUpgradeCon = transform.FindChild("petUpgradeCon");
            #endregion

            #region --左侧列表初始化

            _petListBtn = transform.FindChild("leftList/scroll_view/container/petListBtn");
            BaseButton b_petListBtn = new BaseButton(_petListBtn, 0, 0);
            b_petListBtn.onClick = OnClickPetListBtn;

            _petTrainBtn = transform.FindChild("leftList/scroll_view/container/petTrainBtn");
            BaseButton b_petTrainBtn = new BaseButton(_petTrainBtn, 0, 0);
            b_petTrainBtn.onClick = OnClickPetTrainBtn;

            _petUpgradeBtn = transform.FindChild("leftList/scroll_view/container/petUpgradeBtn");
            BaseButton b_petUpgradeBtn = new BaseButton(_petUpgradeBtn, 0, 0);
            b_petUpgradeBtn.onClick = OnClickPetUpgradeBtn;

            #endregion

            #region --中间宠物展示初始化
            _petShowPrefab = Resources.Load("prefab/pet_show") as GameObject;
            _petShowGrid = transform.FindChild("petList/scroll_view/grid").GetComponent<GridLayoutGroup>();
            _petShowContentSizeFitter = _petShowGrid.GetComponent<ContentSizeFitter>();
            #endregion

            #region --宠物列表Container初始化
            _petListNameTxt = transform.FindChild("petListCon/nametxt").GetComponent<Text>();
            _petListLevelTxt = transform.FindChild("petListCon/leveltxt").GetComponent<Text>();
            _hungrySlider = transform.FindChild("petListCon/hungrySlider").GetComponent<Slider>();
            _hungrySliderTxt = transform.FindChild("petListCon/hungrySlider/hungryValTxt").GetComponent<Text>();
            _expSlider = transform.FindChild("petListCon/expSlider").GetComponent<Slider>();
            _expSliderTxt = transform.FindChild("petListCon/expSlider/expValTxt").GetComponent<Text>();
            _petListAtt = transform.FindChild("petListCon/pet_att/petatts").gameObject;
            _hungryAutoUse = transform.FindChild("petListCon/hungryAutoUse").GetComponent<Toggle>();
            _feedBtn = transform.FindChild("petListCon/feedBtn").GetComponent<Button>();
            _recyleBtn = transform.FindChild("petListCon/recyleBtn").GetComponent<Button>();
            _changeNameBtn = transform.FindChild("petListCon/changeNameBtn").GetComponent<Button>();
            _useBtn = transform.FindChild("petListCon/useBtn").GetComponent<Button>();
            #endregion

            #region --宠物培养Container初始化
            _petTrainNameTxt = transform.FindChild("petTrainCon/nametxt").GetComponent<Text>();
            _petTrainLevelTxt = transform.FindChild("petTrainCon/leveltxt").GetComponent<Text>();
            _petTrainAtt = transform.FindChild("petTrainCon/pet_att/petatts").gameObject;
            _blessSlider = transform.FindChild("petTrainCon/blessBar").GetComponent<Slider>();
            _blessSliderTxt = transform.FindChild("petTrainCon/blessBar/blessVal").GetComponent<Text>();
            _costGoldTxt = transform.FindChild("petTrainCon/costGoldVal").GetComponent<Text>();
            _costBlessTxt = transform.FindChild("petTrainCon/costBlessVal").GetComponent<Text>();
            _blessLeftTxt = transform.FindChild("petTrainCon/blessLeftVal").GetComponent<Text>();
            _diamondAutoUse = transform.FindChild("petTrainCon/diamondAutoUse").GetComponent<Toggle>();
            _blessBtn = transform.FindChild("petTrainCon/blessBtn").GetComponent<Button>();
            #endregion

            #region --宠物进阶Container初始化
            _pet3DShowImg = transform.FindChild("petUpgradeCon/showbg").gameObject;
            _pet3DShowPoint = transform.FindChild("petUpgradeCon/showbg/showPoint").gameObject;
            _petUpgrdeSuccTxt = transform.FindChild("petUpgradeCon/showbg/upgradeRateBg/successRate").GetComponent<Text>();
            _peticon = transform.FindChild("petUpgradeCon/petbg/peticon").GetComponent<Image>();
            _crystalSlider = transform.FindChild("petUpgradeCon/crystalSlider").GetComponent<Slider>();
            _petUpgradeGlodTxt = transform.FindChild("petUpgradeCon/gldval").GetComponent<Text>();
            _crystalCostTxt = transform.FindChild("petUpgradeCon/crystalVal").GetComponent<Text>();
            _crystalLeftTxt = transform.FindChild("petUpgradeCon/crystalLeftBg/crystalLeftVal").GetComponent<Text>();
            _upgradeBtn = transform.FindChild("petUpgradeCon/upgradeBtn").GetComponent<Text>();

            EventTriggerListener.Get(_pet3DShowImg).onDrag = OnDragPet3D;
            #endregion

            //!--关闭按钮
            _closeBtn = transform.FindChild("closeBtn");
            BaseButton b_closeBtn = new BaseButton(_closeBtn, 0, 0);
            b_closeBtn.onClick = OnClose;

            //TODO 测试用代码
            PetModel.getInstance().DummyData();
        }

        override public void onShowed()
        {
            ShowLeftContainer();
            RefreshAllPetsShow();
        }

        override public void dispose()
        {
        }

        void OnClose(GameObject go)
        {
            InterfaceMgr.getInstance().close(InterfaceMgr.PET);
        }

        void OnClickPetListBtn(GameObject go)
        {
            _curFunc = PetFunc.PetList;
            ShowLeftContainer();
        }

        void OnClickPetTrainBtn(GameObject go)
        {
            _curFunc = PetFunc.PetTrain;
            ShowLeftContainer();
        }

        void OnClickPetUpgradeBtn(GameObject go)
        {
            _curFunc = PetFunc.PetUpgrade;
            ShowLeftContainer();
        }

        void OnClickPetShow(uint iid)
        {
            _curPetIid = iid;
            _curPetData = PetModel.getInstance().GetPetDataByIid(iid);
            RefreshPetListCon();
            RefreshPetTrainCon();
            RefreshPetUpgrade();
        }

        void RefreshPetListCon()
        {
            _petListNameTxt.text = _curPetData.name;
            _petListLevelTxt.text = _curPetData.level.ToString();
            _hungrySlider.value = _curPetData.hungry/100.0f;
            _hungrySliderTxt.text = _curPetData.hungry + "/" + 100;
            //TODO, 这里应该获取配置里面的最大经验值
            _expSlider.value = _curPetData.exp/1000.0f;
            _expSliderTxt.text = _curPetData.exp + "/" + 1000;
            //TODO,宠物属性
        }

        void RefreshPetTrainCon()
        {
            _petTrainNameTxt.text = _curPetData.name;
            _petTrainLevelTxt.text = _curPetData.level.ToString();
            //TODO,宠物属性
            //TODO,Slider
        }

        void RefreshPetUpgrade()
        {
            //TODO
            GameObject petPrefab = Resources.Load<GameObject>("profession/eagle");
            _pet3D = GameObject.Instantiate(petPrefab, _pet3DShowPoint.transform.position, Quaternion.identity) as GameObject;
            _pet3D.transform.localScale = new Vector3(30, 30, 30);
            _pet3D.transform.forward = new Vector3(0, 0, -1);
            _pet3D.transform.parent = _pet3DShowPoint.transform;
            int uilayer = LayerMask.NameToLayer("UI");
            _pet3D.layer = uilayer; //UI
            Transform[] childs = _pet3D.GetComponentsInChildren<Transform>();
            foreach (var trans in childs)
            {
                trans.gameObject.layer = uilayer;
            }
        }

        void OnDragPet3D(GameObject go, Vector2 delta)
        {
            if (_pet3D == null) return;
            float speed = 2.0f;
            _pet3D.transform.localRotation = Quaternion.Euler(0f, -0.5f * delta.x * speed, 0f) * _pet3D.transform.localRotation;
        }

        void RefreshAllPetsShow()
        {
            Dictionary<uint, PetData> allpet = PetModel.getInstance().Allpets;
            var etor = allpet.GetEnumerator();
            while (etor.MoveNext())
            {
                uint iid = etor.Current.Key;
                PetShow ps = null;
                if (!_petShows.TryGetValue(iid, out ps))
                {
                    ps = new PetShow(_petShowGrid, _petShowPrefab, etor.Current.Key, OnClickPetShow);
                    _petShows[iid] = ps;
                }
                ps.Refresh();
            }
        }

        void RefreshOnePetShow(GameObject petShow, PetData pd)
        {
            
        }

        void ShowLeftContainer()
        {
            HideAllLeftContainer();
            switch (_curFunc)
            {
                case PetFunc.PetList:
                    _petListCon.gameObject.SetActive(true);
                    break;
                case PetFunc.PetTrain:
                    _petTrainCon.gameObject.SetActive(true);
                    break;
                case PetFunc.PetUpgrade:
                    _petUpgradeCon.gameObject.SetActive(true);
                    break;
            }
        }

        private void HideAllLeftContainer()
        {
            _petListCon.gameObject.SetActive(false);
            _petTrainCon.gameObject.SetActive(false);
            _petUpgradeCon.gameObject.SetActive(false);
        }


    }
}
