using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cross;
using GameFramework;
using UnityEngine;

namespace MuGame
{
    class A3_RideProxy : BaseProxy<A3_RideProxy>
    {
        public A3_RideProxy() {

            A3_RideModel.getInstance();

            this.addProxyListener( PKG_NAME.S2C_RIDE , S2CHandle);

        }

        private void S2CHandle( Variant s2cData ) {

            int res = s2cData["res"]; ;

            //Debug.LogError( "==============坐骑消息" +  s2cData.dump().ToString() );

            switch ( res )
            {
                case ( int )   S2Cenum.RIDE_INFO:  A3_RideModel.getInstance().SetRideInfoS2cData( s2cData );  break;

                case ( int ) S2Cenum.RIDE_UPGRADE: A3_RideModel.getInstance().ChangeRideLvlS2cData( s2cData );

                dispatchEvent( GameEvent.Create( ( int ) S2Cenum.RIDE_UPGRADE , this , s2cData ) );

                break;

                case ( int ) S2Cenum.RIDE_UPGRADEGIFT: A3_RideModel.getInstance().ChangRideGiftLvlS2cData( s2cData );

                dispatchEvent( GameEvent.Create( ( int ) S2Cenum.RIDE_UPGRADEGIFT , this , s2cData ) );

                break;

                case ( int ) S2Cenum.RIDE_UPDOWN: A3_RideModel.getInstance().ChangeRideStateS2cData( s2cData );

                dispatchEvent( GameEvent.Create( ( int ) S2Cenum.RIDE_UPDOWN , this , s2cData ) );

                Debug.LogError( s2cData.getValue( "mount" )._uint == 0 ? "下坐骑" : "上坐骑" );

                break;

                case ( int ) S2Cenum.RIDE_CHANGE: A3_RideModel.getInstance().ChangeRideDressS2cData( s2cData );

                dispatchEvent( GameEvent.Create( ( int ) S2Cenum.RIDE_CHANGE , this , s2cData ) );

                if (s2cData.ContainsKey("lock_dress")) dispatchEvent(GameEvent.Create((int)S2Cenum.RIDE_LIMIT, this, s2cData));  // 限时过期

                break;

                case ( int ) S2Cenum.RIDE_ADD:    A3_RideModel.getInstance().AddRideDressS2cData( s2cData );

                dispatchEvent( GameEvent.Create( ( int ) S2Cenum.RIDE_ADD , this , s2cData ) );

                break;

                case ( int ) S2Cenum.RIDE_GIFTPOINT: break;

                default:

                Globle.err_output(res);

                break;
            }

        }

        public void SendC2S( uint type , string _params = null , uint _paramsvalue = 0 , bool isDie = false , bool isAuto = false ) {

            if ( isDie  )
            {
                SendC2S(type, _params, _paramsvalue);

                return;

            } // 死亡强制下坐骑

            if ( GRMap.curSvrConf != null && GRMap.curSvrConf.ContainsKey( "maptype" ) && GRMap.curSvrConf[ "maptype" ]._int > 0 )
            {

                flytxt.instance.fly( ContMgr.getCont( "a3_ride_fb" ) );

                return;

            }

            if (SelfRole._inst != null && SelfRole._inst.invisible)  // 隐身状态
            {
                ErrText("a3_ride_fight", !isAuto);

                return;

            } //隐身技能放前  隐身不自动上坐骑

            if (SelfRole._inst != null && SelfRole.fsm.Autofighting)  //自动战斗
            {
                ErrText("a3_ride_fight", !isAuto);

                return;

            }

            if (ride_cd.isShow)
            {
                //flytxt.instance.fly( ContMgr.getCont( "状态切换中" ) );

                return;
            }

            if ( type  == (uint)S2Cenum.RIDE_UPDOWN && IsCanChangeRide(SelfRole._inst == null  ? -1 : SelfRole._inst.dianjiTime) == false )
            {
                if (isAuto)
                {
                    SendC2S(type, _params, _paramsvalue);  // 自动寻路 自动上坐骑
                }

                ErrText("a3_ride_fight", !isAuto);

                return;
            }

            SendC2S(type, _params, _paramsvalue);

        } // op 1-5 


        private void SendC2S(uint type, string _params = null, uint _paramsvalue = 0) {

            Variant _msg = new Variant();

            _msg["op"] = type;

            if (_params != null)
            {
                _msg[_params] = _paramsvalue;
            }

            sendRPC(PKG_NAME.C2S_RIDE, _msg);

        } // op 1-5 

        public void SendC2S( uint dress , uint unlock_type  )
        {   
            Variant msg = new Variant();
            msg[ "op" ]=6;
            msg[ "dress" ]=dress;

            if ( unlock_type != 0 )
            {
                msg[ "unlock_type" ]=unlock_type;
            }

            sendRPC( PKG_NAME.C2S_RIDE , msg );

        } // op 6

        public static bool IsCanChangeRide( int dianjiTime )
        {
            if ( dianjiTime != -1 )
            {
                if ( NetClient.instance.CurServerTimeStamp - dianjiTime > 5f )
                    return true;
                else
                    return false;
            }
            else {
                return true;
            }

        }  //计算脱离战斗 根据不放技能后的时长算的

        public void ErrText(string errValue, bool isShow ) {

            if (isShow)
            {
                flytxt.instance.fly(ContMgr.getCont(errValue));
            }
           
        }
    }

    public enum S2Cenum{

        RIDE_INFO = 1,
        RIDE_UPGRADE = 2,
        RIDE_UPGRADEGIFT = 3,
        RIDE_UPDOWN = 4,
        RIDE_CHANGE = 5,
        RIDE_ADD = 6,
        RIDE_GIFTPOINT = 7,

        RIDE_LIMIT = 8,// 限时 

    }

    public enum RIDESTATE
    {

        UP = 1,
        Down=0,
    }

}
