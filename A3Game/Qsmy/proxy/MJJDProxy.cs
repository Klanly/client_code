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

namespace MuGame
{
    class MJJDProxy : BaseProxy<MJJDProxy>
    {
        public void sendMsgtoServer(int id, int param = 0)
        {
            debug.Log("Send Message_244 , type == " + id );
            Variant msg = new Variant();
            msg["operation"] = id;
            sendRPC(PKG_NAME.C2S_GET_LVL_INFO_RES, msg);
        }
    }
}
