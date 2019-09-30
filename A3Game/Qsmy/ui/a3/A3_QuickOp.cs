using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GameFramework;
namespace MuGame
{
    class A3_QuickOp : FloatUi
    {
        Transform tfSkillbarCombat;
        GameObject goUpBtn;
        GameObject goBeStronger;
        A3_QuickOp instance;
        A3_QuickOp Instance
        {
            get
            {
                if (instance != null)
                    return instance;
                else
                {
                    (instance = new A3_QuickOp()).init();
                    return instance;
                }
            }
            set { instance = value; }
        }
        public override void init()
        {
            Instance = this;
            joinWorldInfo.instance.g_mgr.addEventListener(PKG_NAME.S2C_MAP_CHANGE, OnSceneChange);
            tfSkillbarCombat = a1_gamejoy.inst_skillbar.transform.FindChild("skillbar/combat");
            goBeStronger = A3_BeStronger.Instance.gameObject;
            goUpBtn = goBeStronger.transform.FindChild("upbtn").gameObject;
            new BaseButton(transform.FindChild("openAuction")).onClick = OpenAuction;
            new BaseButton(transform.FindChild("go2warehouse")).onClick = MoveToWarehouseNPC;
            new BaseButton(transform.FindChild("go2treasureMap")).onClick = MoveToTreasureMapNPC;
            new BaseButton(getTransformByPath("gotoDart")).onClick = (GameObject go) =>
             {
                 SelfRole.moveToNPc((int)PlayerModel.getInstance().mapid,1003);
             };

            if (PlayerModel.getInstance().mapid == 10)
            {
                A3_BeStronger.Instance.Owner = Instance.transform;
                if (!goBeStronger.activeSelf)
                {
                    goBeStronger.SetActive(true);
                    goUpBtn.SetActive(A3_BeStronger.Instance.CheckUpItem());
                }
            } else {
                gameObject.SetActive(false);
            }
        }


        public override void onShowed()
        {

            base.onShowed();

            this.transform.SetAsLastSibling();

        }
        public void OnSceneChange(GameEvent e)
        {
            if (e.data["mpid"]._uint == 10)
            {
                if (!A3_BeStronger.Instance.Owner.Equals(Instance.transform))
                {
                    A3_BeStronger.Instance.Owner = Instance.transform;
                    if (!goBeStronger.activeSelf)
                    {
                        goBeStronger.SetActive(true);
                        goUpBtn.SetActive(A3_BeStronger.Instance.CheckUpItem());
                    }
                }
                if (!gameObject.activeSelf)
                    gameObject.SetActive(true);

            }
            else
            {
                if (!A3_BeStronger.Instance.Owner.Equals(tfSkillbarCombat))
                    A3_BeStronger.Instance.Owner = tfSkillbarCombat;
                if (gameObject.activeSelf)
                    gameObject.SetActive(false);
            }
        }
        public void OpenAuction(GameObject go)
        {
            SelfRole.fsm.ChangeState(StateIdle.Instance);
            InterfaceMgr.getInstance().ui_async_open(InterfaceMgr.A3_AUCTION);
        }
        public void MoveToWarehouseNPC(GameObject go) =>
            SelfRole.moveToNPc((int)PlayerModel.getInstance().mapid, npcId: 1010);
        public void MoveToTreasureMapNPC(GameObject go) =>
            SelfRole.moveToNPc((int)PlayerModel.getInstance().mapid, npcId: 1001);
    }
}
