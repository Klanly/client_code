using GameFramework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace MuGame
{ 
    class a3_RollItem : FloatUi
    {
        public static a3_RollItem single = null;
        private BaseButton _btnOpen = null;
        private GameObject _downContaninerGo , _itemContaninerGo , _itemGo ,_imageGo= null;
        private bool isOpen = false;
        private Dictionary<uint,RollItemGo> _goMapping = new Dictionary<uint, RollItemGo>();

        public override void init()
        {
            single = this;
            OnFindChildGo();
            OnAddOnClick();
        }

        public override void onShowed()
        {
            single = this;

            OnBtnOpenHandler(_btnOpen.gameObject);

            BuildRollItemGo(A3_RollModel.getInstance().rollMapping);

            OnAddEventListener();

        }

        public override void onClosed()
        {
            OnDispose();
        }

        private  void OnFindChildGo() {

            _btnOpen = new BaseButton(this.getTransformByPath("Container/ImageTitle/Button_Open"));
            _downContaninerGo = this.getGameObjectByPath("Container/Item_Container");
            _itemContaninerGo = this.getGameObjectByPath("Container/Item_Container/Image_Mask/Scroll_Container");
            _itemGo = this.getGameObjectByPath("drop_Item");
            _imageGo = this.getGameObjectByPath("Container/ImageTitle/Image_Orientation");
        }

        private void OnAddOnClick() {

            _btnOpen.onClick = OnBtnOpenHandler;

        }

        private void OnAddEventListener()
        {
            TeamProxy.getInstance().addEventListener(18, UpdateRoll);
        }

        private void RemoveEventListener()
        {
            TeamProxy.getInstance().removeEventListener(18, UpdateRoll);
        }

        private void UpdateRoll(GameEvent _event) {

            if (_goMapping.ContainsKey(_event.data["dpid"]._uint))
            {
                _goMapping[_event.data["dpid"]._uint].ChangeBtnState();
            }

        }

        private void OnBtnOpenHandler(GameObject go)
        {
            isOpen = !isOpen;

            _imageGo.transform.localEulerAngles = new Vector3(0f, isOpen ? 180f : 0f , 0f);

            _downContaninerGo.SetActive(isOpen);

        }

        private void BuildRollItemGo ( Dictionary<uint, ROllItem> rollItemMapping) {

            foreach (var item in rollItemMapping)
            {
                AddRollItemGo(item.Value);
            }
        }

        public void AddRollItemGo(ROllItem itemData) {

            if (_goMapping.ContainsKey(itemData.dpid)) return;

            GameObject itemgo = GameObject.Instantiate(_itemGo);

            RollItemGo rollGo = new RollItemGo( itemgo , itemData, _itemContaninerGo);

            _goMapping[itemData.dpid] = rollGo;

            this.gameObject.SetActive(_goMapping.Count > 0);

            StartCoroutine(rollGo.TimeBegin());

        }

        private void ClearRollItemGo() {

            foreach (var go in _goMapping)
            {
                StopCoroutine(go.Value.TimeBegin());
                go.Value.Dispose();
            }

            _goMapping.Clear();

            A3_RollModel.getInstance().RemoveRollDropItem(0,true);
        }

        public void RemoveRollItemGO(uint dpid) {

            if (_goMapping.ContainsKey(dpid))
            {
                StopCoroutine(_goMapping[dpid].TimeBegin());
                _goMapping[dpid].Dispose();
                _goMapping.Remove(dpid);
            }

            A3_RollModel.getInstance().RemoveRollDropItem(dpid);

            this.gameObject.SetActive( _goMapping.Count > 0 );

        }

        private void OnDispose() {

            single = null;

            RemoveEventListener();

            ClearRollItemGo();

            isOpen = false;

        }
       
    }

    class RollItemGo 
    {
        private GameObject _itemGo, _iconGo, _iconGoParent = null;
        private ROllItem _rollItemdata = null;
        private Text _textitemName ,_textResult = null;
        private BaseButton _btnNeed, _btnGreed = null;
        private float _endTime=0f;

        public RollItemGo(GameObject go, ROllItem data ,GameObject parentGo)
        {
            _itemGo = go;
            _rollItemdata = data;
            _endTime = (float)_rollItemdata.left_tm;

            this._itemGo.gameObject.SetActive(true);
            this._itemGo.transform.SetParent(parentGo.transform);
            this._itemGo.transform.localScale = Vector3.one;
            this._itemGo.transform.localPosition = Vector3.zero;

            Init();
            OnAddOnClick();
            CreateIconGo(_iconGoParent.transform);

        }

        private void Init() {

            _iconGoParent = this._itemGo.transform.FindChild("Item_Icon").gameObject;
            _textitemName = this._itemGo.transform.FindChild("Text_ItemName").GetComponent<Text>();
            _textResult = this._itemGo.transform.FindChild("Text_Result").GetComponent<Text>();
            _btnNeed = new BaseButton(this._itemGo.transform.FindChild("Button_Need"));
            _btnGreed = new BaseButton(this._itemGo.transform.FindChild("Button_Greedy"));
            _textitemName.text = (_rollItemdata.eqpData == null ? _rollItemdata.itemData : _rollItemdata.eqpData).confdata.item_name;

            if( _rollItemdata.eqpData != null ) _btnNeed.interactable = a3_EquipModel.getInstance().checkisSelfEquip(_rollItemdata.eqpData.confdata);

        }

        private void CreateIconGo(Transform parent)
        {
            a3_BagItemData data = _rollItemdata.eqpData == null ? _rollItemdata.itemData : _rollItemdata.eqpData;

            _iconGo = IconImageMgr.getInstance().createA3ItemIcon(data, true, data.num,0.75f);

            _iconGo.transform.SetParent(parent);
            _iconGo.transform.localPosition = Vector3.zero;
            _iconGo.transform.localScale = new Vector3(0.65f,0.65f,0.65f);

            if (data.num <= 1)
                _iconGo.transform.FindChild("num").gameObject.SetActive(false);

            BaseButton bs_bt = new BaseButton(_iconGo.transform);

            bs_bt.onClick = delegate (GameObject go) {

                if (data.isEquip)
                {
                    ArrayList uidata = new ArrayList();
                    uidata.Add(data);
                    uidata.Add(equip_tip_type.Comon_tip);
                    InterfaceMgr.getInstance().ui_async_open(InterfaceMgr.A3_EQUIPTIP, uidata);
                }
                else
                {
                    ArrayList uidata = new ArrayList();
                    uidata.Add(data);
                    uidata.Add(equip_tip_type.Comon_tip);
                    InterfaceMgr.getInstance().ui_async_open(InterfaceMgr.A3_ITEMTIP, uidata);
                }

            };

        }

        public void ChangeBtnState() {

            _btnNeed.gameObject.SetActive(false);
            _btnGreed.gameObject.SetActive(false);
            var result = A3_RollModel.getInstance().rollReusultMapping[_rollItemdata.dpid];
            if (result == null ) { return; }
            _textResult.gameObject.SetActive(true);

            if( result.roll_type != 0 ) _textResult.text = string.Format(ContMgr.getCont("a3_roll_get") + "({0})({1})", result.roll.ToString(), ContMgr.getCont("a3_roll_type" + result.roll_type.ToString()) );

            if (result.max_roll != 0 )
            {
                _textResult.gameObject.SetActive(false);

                SetRollResult();

            } // 已经有了结果

        }

        private void OnAddOnClick()
        {
            _btnNeed.onClickFalse = (go) =>
            {
                Globle.err_output(-1201);
            };

            _btnNeed.onClick = (go) =>
            {
                if (_rollItemdata.eqpData != null && ! a3_EquipModel.getInstance().checkisSelfEquip(_rollItemdata.eqpData.confdata)) {

                    Globle.err_output(-1201);

                    return;

                }

                TeamProxy.getInstance().SendRoll(1,_rollItemdata.dpid);
            };

            _btnGreed.onClick = (go) =>
            {
                TeamProxy.getInstance().SendRoll(2, _rollItemdata.dpid);
            };

        }

        private void SetRollResult() {

            if (!_textResult.IsActive())
            {
                _btnNeed.gameObject.SetActive(false);
                _btnGreed.gameObject.SetActive(false);
                _textResult.gameObject.SetActive(true);

                Roll result = null;

                if (A3_RollModel.getInstance().rollReusultMapping.ContainsKey(_rollItemdata.dpid))
                {
                    result = A3_RollModel.getInstance().rollReusultMapping[_rollItemdata.dpid];

                }

                _textResult.text = result == null ? ContMgr.getCont("a3_roll_not_hava") : string.Format(" {0}" + ContMgr.getCont("a3_roll_get") + "({1})" + ContMgr.getCont("a3_roll_youget") + "({2})", result.name, result.max_roll.ToString(), result.roll.ToString());

                if (A3_RollModel.getInstance().rollMapping.ContainsKey(_rollItemdata.dpid))
                {
                    if (result == null)
                    {
                        A3_RollModel.getInstance().rollMapping[_rollItemdata.dpid].isCanPick = true;
                    }
                    else if (PlayerModel.getInstance().cid == result.roll_owner)
                    {
                        A3_RollModel.getInstance().rollMapping[_rollItemdata.dpid].isCanPick = true;
                    }
                }

            }

        }

        public void Dispose()
        {
            _rollItemdata = null;
            _iconGo = null;
            _iconGoParent = null;
            _btnNeed.removeAllListener();
            _btnGreed.removeAllListener();
            _btnNeed = null;
            _btnGreed = null;
            _textitemName = null;
            _textResult = null;
            GameObject.Destroy(this._itemGo);
            _itemGo = null;
            _endTime = 0f;
        }

        public IEnumerator TimeBegin()
        {
            while (_endTime > 0f )
            {
                _endTime -= Time.deltaTime;

                if ( _rollItemdata.roll_tm <= NetClient.instance.CurServerTimeStamp )
                {
                    SetRollResult();
                }

                if ( _rollItemdata.roll_tm >= NetClient.instance.CurServerTimeStamp )
                {
                    _textitemName.text = (_rollItemdata.eqpData == null ? _rollItemdata.itemData : _rollItemdata.eqpData).confdata.item_name + " " + (_rollItemdata.roll_tm- NetClient.instance.CurServerTimeStamp).ToString()+"s";
                }

                yield return 1;

            }

            a3_RollItem.single.RemoveRollItemGO(_rollItemdata.dpid);

        }
    }

}
