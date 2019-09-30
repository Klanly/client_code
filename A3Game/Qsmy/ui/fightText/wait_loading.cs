using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameFramework;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections;
using Cross;
using DG.Tweening;

namespace MuGame
{
    class wait_loading : Window
    {
        private float m_fOpenTime = 0f;
        public override void init()
        {
            getComponentByPath<Text>("shuoming").text = ContMgr.getCont("wait_loading_0");
        }

        void Update()
        {
            m_fOpenTime += Time.deltaTime;
            transform.SetAsLastSibling();
            if (m_fOpenTime > 16f)
            {
                m_fOpenTime = 0f;
                InterfaceMgr.getInstance().close(InterfaceMgr.WAIT_LOADING);
                //Debug.Log("wait_loadingwait_loadingwait_loadingwait_loadingwait_loading colse");
            }
        }

        public override void onShowed()
        {
            m_fOpenTime = 0f;
            transform.SetAsLastSibling();
            base.onShowed();
        }

        public override void onClosed()
        {
            base.onClosed();
        }
    }
}
