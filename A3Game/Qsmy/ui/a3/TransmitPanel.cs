using GameFramework;
using UnityEngine;
using UnityEngine.UI;
using Cross;
using System;
using System.Collections;
using System.Collections.Generic;

namespace MuGame
{
    /// <summary>
    /// 传送面板
    /// </summary>
    class TransmitPanel : Window
    {
        private static TransmitPanel instance;
        public static TransmitPanel Instance
        {
            get { return instance; }
            set { instance = value; }
        }
        private TransmitData data;
        public int curNeedMoney;
        private Text textCostMoney;
        private Text textTargetDesc;
        private int transmitMapPoint;
        private int targetMapId;
        public int TargetMapId
        {
            get
            {
                return targetMapId;
            }
            set
            {
                targetMapId = value;
                transmitMapPoint = targetMapId * 100 + 1;
            }
        }
        public Vector3 currentTargetPosition { get; set; }
        public Dictionary<int, int> dicMappoint;
        public override void init()
        {
            instance = this;
            textCostMoney = transform.FindChild("bt1/cost").GetComponent<Text>();
            textTargetDesc = transform.FindChild("desc").GetComponent<Text>();
            new BaseButton(transform.FindChild("bt0")).onClick = (GameObject go) =>
            {           
                if (data.handle_customized_afterTransmit != null)
                    data.handle_customized_afterTransmit();
                else if (data.targetPosition != Vector3.zero)
                    SelfRole.WalkToMap(data.mapId, data.targetPosition, handle: data.after_arrive);                
                data.after_clickBtnWalk?.Invoke();
                InterfaceMgr.getInstance().close(InterfaceMgr.TRANSMIT_PANEL);                
            };
            new BaseButton(transform.FindChild("bt1")).onClick = (GameObject go) =>
            {
                if (PlayerModel.getInstance().vip < 3 && PlayerModel.getInstance().money < curNeedMoney)
                {
                    flytxt.instance.fly(ContMgr.getCont("comm_nomoney"));
                    return;
                }
                if (SelfRole.fsm.Autofighting)
                    SelfRole.fsm.Stop();
                if (data.closeWinName != null)
                    for (int i = 0; i < data.closeWinName.Length; i++)
                        InterfaceMgr.getInstance().close(data.closeWinName[i]);
                SelfRole.Transmit(toid: dicMappoint[data.mapId], after: delegate ()
                {
                    if (data.handle_customized_afterTransmit != null)
                        data.handle_customized_afterTransmit();
                    else if (data.targetPosition != Vector3.zero)
                        SelfRole.WalkToMap(data.mapId, data.targetPosition,handle:data.after_arrive);                    
                });                
                data.after_clickBtnTransmit?.Invoke();
                InterfaceMgr.getInstance().close(InterfaceMgr.TRANSMIT_PANEL);                
            };
            new BaseButton(transform.FindChild("btclose")).onClick = (GameObject go) =>
            {
                InterfaceMgr.getInstance().close(InterfaceMgr.TRANSMIT_PANEL);                
            };
            
            dicMappoint = new Dictionary<int, int>();
            List<SXML> listMappoint = XMLMgr.instance.GetSXML("mappoint").GetNodeList("trans_remind");
            for (int i = 0; i < listMappoint.Count; i++)
            {
                int mapId,transId;
                if ((mapId = listMappoint[i].getInt("map_id")) != -1 && (transId = listMappoint[i].getInt("trance_id")) != -1)
                    dicMappoint.Add(mapId, 1 + transId * 100);
            }
            getComponentByPath<Text>("desc").text = ContMgr.getCont("worldmapsubwin_0");
            getComponentByPath<Text>("bt0/Text").text = ContMgr.getCont("worldmapsubwin_1");
            getComponentByPath<Text>("Text").text = ContMgr.getCont("worldmapsubwin_2");
        }

        public override void onShowed()
        {
            #region 获取open参数
            data = null;
            if (uiData.Count == 1)
                data = (TransmitData)uiData[0];
            else
            {
                InterfaceMgr.getInstance().close(InterfaceMgr.TRANSMIT_PANEL);
                debug.Log("invalid uidata");
                return;
            }
            #endregion
            if (data.check_beforeShow && !CheckPoint())
            {
                if (data.handle_customized_afterTransmit != null)
                    data.handle_customized_afterTransmit();
                else if (data.targetPosition != Vector3.zero)
                    SelfRole.WalkToMap(dicMappoint[data.mapId], data.targetPosition, handle: data.after_arrive);
                data.after_clickBtnWalk?.Invoke();
                InterfaceMgr.getInstance().close(InterfaceMgr.TRANSMIT_PANEL);
                return;
            }
            #region 面板上的文字说明
            bool isfree = PlayerModel.getInstance().vip >= 3;
            TargetMapId = data.mapId;
            SXML xml = XMLMgr.instance.GetSXML("mappoint.p", "id==" + dicMappoint[data.mapId]);
            Variant vmap = SvrMapConfig.instance.getSingleMapConf(xml.getUint("mapid"));
            string name = vmap.ContainsKey("map_name") ? vmap["map_name"]._str : "--";
            int basecost = xml.getInt("cost");
            curNeedMoney = basecost / 10 * (int)((float)PlayerModel.getInstance().lvl / 10) + basecost;
            textCostMoney.text = curNeedMoney.ToString();
            textTargetDesc.text = name;
            gameObject.SetActive(true);
            #endregion
            transform.SetAsLastSibling();
        }

        public override void onClosed()
        {
            uiData.Clear();
        }

        public virtual bool CheckPoint()
        {
            if (GRMap.instance == null)
            {
                InterfaceMgr.getInstance().close(InterfaceMgr.A3_TASKOPT);
                return false;
            }
            if (dicMappoint.Count == 0)
                return true;
            if (!dicMappoint.ContainsKey(GRMap.instance.m_nCurMapID))
                return false;
            if (GetMapIdByMappoint(dicMappoint[data.mapId]) != GRMap.instance.m_nCurMapID && GRMap.instance.m_nCurMapID != data.mapId && dicMappoint[data.mapId] != dicMappoint[GRMap.instance.m_nCurMapID])
                return true;            
            return false;
        }

        private int GetMapIdByMappoint(int mapId) => (mapId - 1) / 100;

    }
    public class TransmitData
    {
        public Action after_clickBtnWalk;                // 点击寻路按钮后的ui行为
        public Action after_clickBtnTransmit;            // 点击传送按钮后的ui行为
        public Action handle_customized_afterTransmit;   // 自定义传送到目的地后的行为,将替代默认寻路
        public Action after_arrive;                      // 角色抵达目标后的行为
        public bool check_beforeShow;                    // 在打开面板前用于检查面板是否需要打开
        public Vector3 targetPosition;
        public string[] closeWinName;
        public int mappointId { get { return _mappointId; } }
        public int mapId { set { _mappointId = 1 + 100 * (_mapId = value); } get { return _mapId; } }

        private int _mapId;
        private int _mappointId;

        public static explicit operator ArrayList(TransmitData data)
        {
            ArrayList arr = new ArrayList();
            arr.Add(data);
            return arr;
        }
    }
}
