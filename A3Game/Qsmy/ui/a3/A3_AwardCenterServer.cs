using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cross;
using GameFramework;
using System.Collections;

namespace MuGame
{
    class A3_AwardCenterServer : BaseProxy<A3_AwardCenterServer>
    {
        public static uint SERVER_AWARDLST = 1;
        public static uint SERVER_SELFDATA = 2;
        public static uint SERVER_GETAWARD = 3;
        public static uint SERVER_TUAN = 4;
        public static uint SERVER_EXCHANGE = 5;


        public static uint EVENT_AWARDLST = 1;
        public static uint EVENT_SELFDATA = 2;
        public static uint EVENT_GETAWARD = 3;
        public static uint EVENT_TUAN = 4;
        public static uint EVENT_EXCHANGE = 5;
        public static uint EVENT_TUAN_GETAWARD = 6;
        public static uint EVENT_TUAN_INFORM = 8;



        public static  uint enddbcz=0;
        public GameEvent Alldata;

        public A3_AwardCenterServer()
        {
            addProxyListener( PKG_NAME.C2S_A3_AWARD , AuctionOP );
        }
        //是不是第一次打开
        bool isfrist_over = false;
        //是否过期
        bool isdczyj = false;  
        public  bool isdbcz = false;
        //是否含有
        bool haveczyj = false;
       public  bool havedbcy = false;
        public void AuctionOP(Variant data)
        {

            debug.Log( "活动福利:"+data.dump()+"res:"+data["res"]);
            int res = data["res"];
            if(res<0)
            {
                Globle.err_output(res);
            }
            switch ( res )
            {
                case 1:
                    //dispatchEvent( GameEvent.Create( EVENT_AWARDLST , this , data ) );
                    //Alldata = GameEvent.Create( EVENT_AWARDLST , this , data );
                    Alldata = GameEvent.Create(EVENT_AWARDLST, this, data);
                    if (!isfrist_over)
                    {
                        if(data["actonline_conf"]._arr !=null)

                        {
                            for (int i=0;i< data["actonline_conf"]._arr.Count;i++)
                            {

                                debug.Log("活动福利tp:" + data["actonline_conf"][i]["tp"]._int + "名字：" + data["actonline_conf"][i]["name"]._str);

                               string name = data["actonline_conf"][i]["name"]._str;
                                int tp= data["actonline_conf"][i]["tp"]._int;
                                debug.Log("ddddddddddddddddddddddddddddddddddd:" + name+"c:"+ muNetCleint.instance.CurServerTimeStamp);
                                int currServerTime = muNetCleint.instance.CurServerTimeStamp;
                                if (tp == 1)
                                {
                                    havedbcy = true;
                                  uint end = data["actonline_conf"][i]["end"]._uint;
                                   A3_AwardCenterServer.enddbcz = end;
                                    uint start = data["actonline_conf"][i]["begin"]._uint;
                                    if (end-currServerTime<= 0)
                                    {
                                        isdbcz = true;                                      
                                    }

                                }
                                if (tp == 3)
                                {
                                    haveczyj = true;
                                    uint end = data["actonline_conf"][i]["end"]._uint;
                                    uint start = data["actonline_conf"][i]["begin"]._uint;
                                    if (end - currServerTime <= 0)
                                    {
                                        isdczyj = true;
                                    }

                                }

                            }
                            
                        }
                        if(haveczyj&& havedbcy)
                        {
                            if (isdczyj && isdbcz)
                            {

                            }
                            else if (!isdczyj && !isdbcz)
                            {
                                InterfaceMgr.getInstance().ui_async_open(InterfaceMgr.A3_ONERECHARGE);

                            }
                            else
                            {
                                ArrayList a = new ArrayList();
                                a.Add(isdczyj ? 0 : 1);
                                InterfaceMgr.getInstance().ui_async_open(InterfaceMgr.A3_ONERECHARGE, a);
                            }
                        }
                        else if(haveczyj&& !havedbcy)
                        {
                            ArrayList a = new ArrayList();
                            a.Add(1);
                            InterfaceMgr.getInstance().ui_async_open(InterfaceMgr.A3_ONERECHARGE, a);
                        }
                        else if(!haveczyj && havedbcy)
                        {
                            ArrayList a = new ArrayList();
                            a.Add(0);
                            InterfaceMgr.getInstance().ui_async_open(InterfaceMgr.A3_ONERECHARGE, a);
                        }
                        else
                        {
                          //  ArrayList a = new ArrayList();
                         //   a.Add(0);
                          //  a.Add(1);
                           // InterfaceMgr.getInstance().ui_async_open(InterfaceMgr.A3_ONERECHARGE, a);
                        }

                        isfrist_over = true;
                    }
                   
                    break;
                case 2:
                foreach ( var item in data[ "actonline" ]._arr )
                {
                    a3_awardCenter.roleData[ item["tp"] ] = item;

                    if ( item["lists"] != null )
                    {
                        Dictionary<int,int> ones = new Dictionary<int,int>();
                        foreach ( var _item in item[ "lists" ]._arr )
                        {
                           if(_item["id"]!=null&& _item["state"]!=null)
                                 ones[_item["id"]._int] = _item["state"]._int;                          
                            if ( _item[ "state" ]!= null &&  _item["state"] == 1 )
                            {
                                a3_awardCenter.finishNum++;
                            }
                        }
                            a3_onerecharge.dics[item["tp"]] = ones;
                        }
                }

                welfareProxy.getInstance().showIconLight( a3_awardCenter.finishNum != 0 );
                A3_AwardCenterServer.getInstance().SendMsg( A3_AwardCenterServer.SERVER_AWARDLST );
                break;
                case 3:
                    SendMsg(A3_AwardCenterServer.SERVER_SELFDATA);
                    a3_onerecharge._instance?.DataRefresh(data["act_type"]._int, data["award_id"]._int);
                break;
                case 4:
                dispatchEvent( GameEvent.Create( EVENT_TUAN , this , data ) );
                break;
                case 8:
                dispatchEvent( GameEvent.Create( EVENT_TUAN_INFORM , this , data ) );
                break;

            }

        }

        public void SendMsg(uint id,Variant data = null)
        {
            Variant msg = new Variant();
            msg[ "op" ] = id;
            switch ( id )
            {
                case 3:
                msg[ "act_type" ] = data[ "act_type" ];
                msg[ "award_id" ] = data[ "award_id" ];
                break;
                 
                case 5:
                msg[ "award_id" ] = data[ "award_id" ];
                break;
                case 6:
                msg[ "award_id" ] = data[ "award_id" ];
                break;
            }

            sendRPC( PKG_NAME.C2S_A3_AWARD , msg );

        }

    }
}
