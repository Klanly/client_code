using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cross;
using GameFramework;

namespace MuGame
{
    class a3_activeDegreeProxy:BaseProxy<a3_activeDegreeProxy>
    {
        public const uint EVENT_GET_ALLPOINT = 0;
        public const uint ON_GET_ACTIVEDEGREE_PRIZE = 1;
       //public  List<ActiveDegreeData> itd = new List<ActiveDegreeData>();
        public Dictionary<uint, ActiveDegreeData> itd = new Dictionary<uint, ActiveDegreeData>();
        public List<int> point = new List<int>();
        public int huoyue_point;
        public a3_activeDegreeProxy()
        {
            addProxyListener(PKG_NAME.S2C_ACTIVEDEGREE_RES, onActivedegree_info);
        }

        public void SendGetPoint(int op)
        {
            Variant msg = new Variant();
            msg["op"] = op;         
            sendRPC(PKG_NAME.C2S_ACTIVEDEGREE_RES, msg);
        }

        public void SendGetReward(uint op,uint point)
        {
            Variant msg = new Variant();
            msg["op"] = op;
            msg["point"] = point;
            sendRPC(PKG_NAME.C2S_ACTIVEDEGREE_RES, msg);
        }
        private void onActivedegree_info(Variant data)
        {
            if (!data.ContainsKey("huoyue_point"))
            {
                return;
            }
            debug.Log("活跃度::" + data.dump());
            if (SelfRole._inst != null)
                SelfRole._inst.m_LockRole = null;
            if (data.ContainsKey("res"))
            {
                int res = data["res"];
                switch (res)
                {
                    case 0://获取总点数
                       // dispatchEvent(GameEvent.Create(EVENT_GET_ALLPOINT, this, data));
                        break;
                    case 1://获取各个活跃活动信息；
                        point.Clear();
                        itd.Clear();                      
                        huoyue_point = data["huoyue_point"];
                       
                        foreach (Variant v in data["huoyues"]._arr)
                        {                          
                            uint  id = v["active_id"];
                            uint count = v["count"];
                            ActiveDegreeData i = new ActiveDegreeData();
                            i.id = id;
                            i.count = count;
                            itd.Add(id,i);
                        }
                        foreach (int id in data["huoyue_reward"]._arr)
                        {
                            point.Add(id);
                        }
                       
                        if (a3_activeDegree.instance != null)
                        {
                            a3_activeDegree.instance.do_Active();
                            a3_activeDegree.instance.onLoad_Change();
                            a3_activeDegree.instance.onActive_Load();
                        }
                        // dispatchEvent(GameEvent.Create(EVENT_GET_ALLPOINT, this, data));
                       
                        //for (int i = 0; i < xmlreward.Count; i++)
                        //{
                        //    int index = i;
                        //    if (a3_activeDegreeProxy.getInstance().huoyue_point >= xmlreward[index].getInt("ac"))
                        //    {
                        //        if (!a3_activeDegreeProxy.getInstance().point.Contains(xmlreward[index].getInt("ac")))
                        //        {
                        //            InterfaceMgr.doCommandByLua("a1_low_fightgame.resh_huoyue", "ui/interfaces/low/a1_low_fightgame", data);
                        //        }
                        //        else
                        //        {
                        //            InterfaceMgr.doCommandByLua("a1_low_fightgame.resh_huoyue", "ui/interfaces/low/a1_low_fightgame", null);
                        //        }
                        //    }

                        //}
                      
                        point.Sort();
                        //List<SXML> xmlreward = XMLMgr.instance.GetSXMLList("huoyue.reward");
                        //if ((point.Count > 0 && huoyue_point >= point[point.Count - 1]+20)||(point.Count==0&&huoyue_point>= xmlreward[0].getInt("ac")))
                        //{
                        //    IconAddLightMgr.getInstance().showOrHideFire("resh_huoyue",data);
                        //    //InterfaceMgr.doCommandByLua("a1_low_fightgame.resh_huoyue", "ui/interfaces/low/a1_low_fightgame", data);
                        //}
                        //else if (point.Count > 0 && huoyue_point / 20 >= point.Count+1)
                        //{
                        //    IconAddLightMgr.getInstance().showOrHideFire("resh_huoyue", data);
                        //    //InterfaceMgr.doCommandByLua("a1_low_fightgame.resh_huoyue", "ui/interfaces/low/a1_low_fightgame", data);
                        //}
                        //else
                        //{
                        //    IconAddLightMgr.getInstance().showOrHideFire("resh_huoyue", data);
                        //    //InterfaceMgr.doCommandByLua("a1_low_fightgame.resh_huoyue", "ui/interfaces/low/a1_low_fightgame", null);
                        //}
                        bool isopenlight = false;
                        int max_ac = 0;
                        if (huoyue_point < 20)
                            isopenlight = false;
                        else
                        {
                            if (point.Count > 0)
                            {
                                max_ac = point[point.Count - 1];
                                if (max_ac == 100)
                                    isopenlight = false;
                                else
                                    isopenlight = huoyue_point >= max_ac + 20 ? true : false;
                            }
                            else
                                isopenlight = true;
                        }

                        IconAddLightMgr.getInstance().showOrHideFire(isopenlight? "open_light_huoyue": "close_light_huoyue",null);





                        break;
                    
                        
                      
                       
                }



            }


                //服务器发的表里读取所有活跃数据，存到A3_activeDegreeModel.getinstance().activedeg_info里
                //id为键  

            }


        //public int prized_zt(int id)
        //{
        //    return(int) A3_activeDegreeModel.getInstance().activedeg_info[id].state;
        //}
    }
}
