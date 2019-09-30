using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameFramework;
using UnityEngine;
using UnityEngine.UI;
using Cross;
namespace MuGame
{
    class cdtime:Window
    {
        private static Action _handle;
        public static void show(Action handle)
        {
            _handle = handle;
            InterfaceMgr.getInstance().ui_async_open(InterfaceMgr.CD_TIME);
        }

        Animator ani;
        processStruct process;
        public override void init()
        {
            ani = GetComponent<Animator>();
            process = new processStruct(onUpdate, "cdtime");
            base.init();
        }

        public override void onShowed()
        {
            (CrossApp.singleton.getPlugin("processManager") as processManager).addProcess(process);
            base.onShowed();
        }

        public override void onClosed()
        {
            (CrossApp.singleton.getPlugin("processManager") as processManager).removeProcess(process);
            base.onClosed();
        }

        private void onUpdate(float s)
        {
            if (ani.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f)
            {
                if (_handle != null)
                    _handle();
                InterfaceMgr.getInstance().close(InterfaceMgr.CD_TIME);
            }
        }
    }
}
