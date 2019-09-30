using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameFramework;
using UnityEngine.UI;
using UnityEngine;
using Cross;
namespace MuGame
{
    class relive:Window
    {
        private float curCd;
        processStruct process;
        Text txt;
        public override void init()
        {
            txt = this.getComponentByPath<Text>("txt");
            process = new processStruct(onUpdate, "relive");
        }


        public override void onShowed()
        {
            this.getComponentByPath<Button>("bt").onClick.AddListener(onClick);
            curCd = Time.time + 30;
            (CrossApp.singleton.getPlugin("processManager") as processManager).addProcess(process);
            base.onShowed();
            hasSend = false;
            refresh();
        }
        private bool hasSend = false;
        void refresh()
        {
            int cd = (int)(curCd-Time.time);
            if (cd <= 0 && hasSend==false)
            {
                hasSend = true;
                MapProxy.getInstance().sendRespawn();
                //InterfaceMgr.getInstance().close(InterfaceMgr.RELIVE);
                return;
            }

            txt.text = cd + "";
        }

        void onUpdate(float s)
        {
            refresh();
        }

        public override void onClosed()
        {
            this.getComponentByPath<Button>("bt").onClick.RemoveListener(onClick);
            (CrossApp.singleton.getPlugin("processManager") as processManager).addProcess(process);
            base.onClosed();
        }

        void onClick()
        {
            if (hasSend == true)
                return;
            MapProxy.getInstance().sendRespawn(true);
            hasSend = true;

            //SelfRole._inst.onRelive();
            //InterfaceMgr.getInstance().close("relive");
        }

    }
}
