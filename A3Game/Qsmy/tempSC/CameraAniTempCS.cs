using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace MuGame
{
     [AddComponentMenu("QSMY/CameraAni")]
    class CameraAniTempCS : MonoBehaviour
    {
         public bool lookatUser = false;

         public bool stop = false;

         public void QSMY_LookAtUser(int b)
         {
             lookatUser = b==1;
         }

         public void QSMY_SetTimeScale(int f)
         {
             Globle.setTimeScale((float)f/100);
         }

         public void QSMY_STOP(int delta)
         {
             stop = true;
             GameCameraMgr.getInstance().stop();
         }
         public void QSMY_OPEN_WIN(string id)
         {
             InterfaceMgr.getInstance().closeAllWin();
             InterfaceMgr.getInstance().ui_async_open(id);
         }

         Dictionary<string, GameObject> dPreLab = new Dictionary<string, GameObject>(); 
         //public void QSMY_OPEN_PRELAB_id_path(string parm)
         //{
         //    string[] arr = parm.Split(new char[] { ',' });
         //    if (arr.Length != 2)
         //        return;

         //    GameObject temp = U3DAPI.U3DResLoad<GameObject>(arr[1]);

         //    if (temp == null)
         //        return;

         //    GameObject go = Instantiate(temp) as GameObject;
         //    go.transform.SetParent(InterfaceMgr.getInstance().winLayer, false);
         //    InterfaceMgr.setUntouchable(go);
         //    dPreLab[arr[0]] = go;
         //}

         public void QSMY_CLOSE_PRELAB_id(string id)
         {
             if (dPreLab.ContainsKey(id))
             {
                 Destroy(dPreLab[id]);
                 dPreLab.Remove(id);
             }
         }

         public void clearAllPrelab()
         {
             foreach (GameObject go in dPreLab.Values)
             {
                 Destroy(go);
             }
             dPreLab.Clear();
         }
    }
}
