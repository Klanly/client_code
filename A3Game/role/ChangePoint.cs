using System;
using GameFramework;
using System.Linq;
using System.Text;
using UnityEngine;
using System.Collections;
using Cross;
using System.Collections.Generic;
namespace MuGame
{

    public class ChangePoint : MonoBehaviour
    {
        public List<int> paramInts = new List<int>();
        public int triggerTimes = 1;
        public int type = 0;
        public List<GameObject> paramGameobjects = new List<GameObject>();

        public int curTrigger = 0;
        void Start()
        {
            BoxCollider box = gameObject.GetComponent<BoxCollider>();
            if (box == null)
                box = gameObject.AddComponent<BoxCollider>();
            box.isTrigger = true;

            gameObject.layer = EnumLayer.LM_PT;
        }



        public void OnTriggerEnter(Collider other)
        {
            //  int layer = LayerMask.NameToLayer("selfrole");
            if (other.gameObject.layer == EnumLayer.LM_BT_SELF)
            {
                if (NetClient.instance != null)
                {
                    if (GRMap.changeMapTimeSt == 0 || NetClient.instance.CurServerTimeStamp - GRMap.changeMapTimeSt < 2)
                        return;
                }
                if (this.gameObject .name == "triggermap4" && a1_gamejoy.inst_joystick!= null) {
                    a1_gamejoy.inst_joystick.Not_show();
                }
                if (triggerTimes == 0)
                    onTrigger();
                else if (triggerTimes > curTrigger)
                {
                  bool b=  onTrigger();
                  if (b)
                  {
                      curTrigger++;
                      if (curTrigger >= triggerTimes)
                      {
                          Destroy(gameObject);
                      }
                  }
                   
                }
            }
        }


        private bool onTrigger()
        {
            //if (type == 0)//切地图
            //{
            //    SelfRole._inst.canMove = false;
            //    SelfRole._inst.StopMove();
            //    loading_cloud.showhandle = changemap;
            //    InterfaceMgr.getInstance().open(InterfaceMgr.LOADING_CLOUD);
            //}
            //else if(type==1)//显示隐藏物件
            //{
            //    int idx = 0;
            //    foreach (GameObject go in paramGameobjects)
            //    {
            //        if (idx % 2 == 0)
            //        {
            //            if (go != null)
            //                go.SetActive(true);
            //        }
            //        else
            //        {
            //            if (go != null)
            //                go.SetActive(false);
            //        }

            //        idx++;
            //    }
            //}
            //else if (type == 2)//触发

            if (type == 0)
            {
                for (int i = 0; i < transform.childCount; i++)
                {
                    TriggerHanldePoint hd = transform.GetChild(i).GetComponent<TriggerHanldePoint>();
                    if (hd != null)
                        hd.onTriggerHanlde();
                }

                return true;
                //foreach (GameObject go in paramGameobjects)
                //{
                //    TriggerHanldePoint hd = go.GetComponent<TriggerHanldePoint>();
                //    if (hd != null)
                //        hd.onTriggerHanlde();
                //}
            }
            else if (type == 1)//svr地图跳转
            {
                if (paramInts.Count == 0)
                    return true;

                Variant v = SvrMapConfig.instance.getSingleMapConf((uint)paramInts[0]);

                if (v.ContainsKey("lv_up") && v["lv_up"] > PlayerModel.getInstance().up_lvl)
                {
                    flytxt.instance.fly(ContMgr.getCont("comm_nolvmap", v["lv_up"], v["lv"]), v["map_name"]);
                    return false;
                }

                if (v.ContainsKey("lv") && v["lv"] > PlayerModel.getInstance().lvl)
                {
                    flytxt.instance.fly(ContMgr.getCont("comm_nolvmap", v["lv_up"], v["lv"]), v["map_name"]);
                    return false;
                }


                loading_cloud.showIt(() =>
                {
                    MapProxy.getInstance().sendBeginChangeMap(paramInts[0]);
                });

                return true;
            }

            return true;
        }

        public void changemap()
        {

            SelfRole._inst.setPos(new Vector3(0, 0,-11));
            loading_cloud.instance.hide(null);
        //    SelfRole._inst.canMove = true;
        }


        [ContextMenu("脚本使用帮助")]
        public void helper()
        {
            debug.Log("type1:切换地图\n"
                + "type2:显示隐藏物件  go中双数为需要显示的，单数是需要隐藏的\n"
                 + "type3:触发脚本  go中为需要触发的脚本物件\n"
                );
        }
    }
}
