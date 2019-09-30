using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameFramework;
using UnityEngine;
using System.Collections;

namespace MuGame
{
    class a3_resetLvLSuccess:FloatUi
    {
    
        public override void init()
        {
            StartCoroutine(timeGo());
        }

        IEnumerator timeGo()
        {
            yield return new WaitForSeconds(2.0f);
            this.gameObject.SetActive(false);
            if (PlayerModel.getInstance().up_lvl > 0 && PlayerModel.getInstance().pt_att > 0)
                UiEventCenter.getInstance().onAddPoint();
            FunctionOpenMgr.instance.onLvUp((int)PlayerModel.getInstance().up_lvl, (int)PlayerModel.getInstance().lvl, true);
        }
    }
}
