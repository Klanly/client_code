using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cross;
using UnityEngine;

namespace MuGame
{
    class A3_RideModel : ModelBase<A3_RideModel>
    {
        //loacl config

        public Dictionary<int, ConfigVo> rideMapping = null;
        public Dictionary<int, ConfigVo> levelMapping = null;
        public Dictionary<int, ConfigVo> rideGiftMapping = null;
        public Dictionary<int, ConfigVo> rideMaxGiftMapping = null;
        public uint upGradeRideItemId =  0;  //坐骑升级  itemId
        public uint upGradeGiftItemId =  0;  //天赋升级  itemId

        public Dictionary<int, WeaponVo> weaponTransform = null;

        public int _maxExp = 0;

        private int _eachExp = 0;

        public int eachExp {

            get {

                if ( _eachExp == 0 )
                {
                    _eachExp = a3_BagModel.getInstance().getItemDataById( upGradeRideItemId ).main_effect;

                }

                return _eachExp;
                    
                    }
        }

        public int maxExp {

          get {

                if ( rideInfo == null )
                {
                    return 0;
                }

                _maxExp = 0;

                for ( uint i = rideInfo.lvl ; rideInfo.lvl < levelMapping.Count ; i++ )
                {   
                    if ( levelMapping.ContainsKey((int)i) )
                    {   
                        var lvlConfigVo = ( levelMapping[ (int)i ] as LevelConfigVo) ;

                        if ( lvlConfigVo.exp == -1   )
                        {
                            return _maxExp;
                        }

                        _maxExp = _maxExp + lvlConfigVo.exp;
                    }
                }

                return _maxExp;
            }

        }

        //s2c Data

        private RideInfoData rideInfo;


        public A3_RideModel()
        {
            InitData();
        }

        private void InitData() {

            rideMapping = new Dictionary<int , ConfigVo>();
            levelMapping = new Dictionary<int , ConfigVo>();
            rideGiftMapping = new Dictionary<int , ConfigVo>();
            //rideMaxGiftMapping = new Dictionary<int , ConfigVo>();
            weaponTransform = new Dictionary<int , WeaponVo>();

            GetAllRideConfig();
            GetRideLevelConfig();
            GetRideGiftConig();
            GetWeaponTransform();

            //GetRideMaxGiftConig();
        }

        //所有坐骑配置config
        private void GetAllRideConfig()
        {

            List<SXML> xml = XMLMgr.instance.GetSXMLList("ride.mount");

            foreach ( SXML x in xml )
            {
                RideConfigVo infos = new RideConfigVo();
                infos.conditionLst=new List<ConditionVO>();

                infos.id = x.getInt( "id" );
                infos.desc = x.getString( "desc" );
                infos.name = x.getString( "name" );
                infos.avatar = x.getString( "avatar" );
                infos.icon = x.getString( "icon" );

                int condition = x.getInt("condition");

                infos.condition = condition;

                if ( condition != -1 ) // 多 （单） 条件
                {
                    foreach ( var _x in x.GetNodeList( "unlock" ) )
                    {
                        ConditionVO vo = new ConditionVO();
                        vo.type =_x.getUint( "type" );
                        vo.lvl =_x.getUint( "lvl" );
                        vo.need_item =_x.getUint( "need_item" );
                        vo.need_num =_x.getUint( "need_num" );
                        infos.conditionLst.Add( vo );
                    }
                }
                else
                {
                    List<SXML> conditionLst = x.GetNodeList( "unlock" );
                    if ( conditionLst.Count > 0 ) {
                        ConditionVO vo = new ConditionVO();
                        SXML _x = conditionLst[ 0 ];
                        vo.type =_x.getUint( "type" );
                        vo.lvl =_x.getUint( "lvl" );
                        vo.need_item =_x.getUint( "need_item" );
                        vo.need_num =_x.getUint( "need_num" );
                        infos.conditionLst.Add( vo );
                    }
                }

                AddMapping( infos.id , rideMapping , infos );
            }

            GetAllTransformConfigVo();


        }

        //所有坐骑配置Transformconfig
        private void GetAllTransformConfigVo()
        {

            List<SXML> xml = XMLMgr.instance.GetSXMLList("ride.transform");

            foreach ( SXML x in xml )
            {
                int id = x.getInt("id");

                ConfigVo rideConfigvo;

                if ( rideMapping.TryGetValue( id , out rideConfigvo ) )
                {
                    RideConfigVo _vo = rideConfigvo as RideConfigVo;

                    if ( _vo.transformMapping == null )
                    {
                        _vo.transformMapping = new Dictionary<int , TransformConfigVo>();
                    }

                    List<SXML> _xml =  x.GetNodeList( "career" );

                    foreach ( var _x in _xml )
                    {
                        TransformConfigVo vo = new TransformConfigVo();

                        vo.postionMapping = new Dictionary<string , float>();
                        vo.rotationMapping = new Dictionary<string , float>();
                        vo.scaleMapping = new Dictionary<string , float>();
                        vo.yinziscaleMapping = new Dictionary<string , float>();
                        vo.yinzipostionMapping = new Dictionary<string , float>();

                        vo.carrer =  _x.getInt( "id" );

                        vo.baseoffest =  _x.getFloat( "baseoffest" );

                        string pos =_x.getString( "postion" );
                        var  posLst =   GetConfigLst( pos );
                        vo.postionMapping.Add( "x" , posLst[ 0 ] );
                        vo.postionMapping.Add( "y" , posLst[ 1 ] );
                        vo.postionMapping.Add( "z" , posLst[ 2 ] );   //位置

                        string rotation =_x.getString( "rotation" );
                        var  rotaLst =   GetConfigLst( rotation );
                        vo.rotationMapping.Add( "x" , rotaLst[ 0 ] );
                        vo.rotationMapping.Add( "y" , rotaLst[ 1 ] );
                        vo.rotationMapping.Add( "z" , rotaLst[ 2 ] ); // 旋转

                        string scale =_x.getString( "scale" );
                        var  scaleLst =   GetConfigLst( scale );
                        vo.scaleMapping.Add( "x" , scaleLst[ 0 ] );
                        vo.scaleMapping.Add( "y" , scaleLst[ 1 ] );
                        vo.scaleMapping.Add( "z" , scaleLst[ 2 ] ); // scale

                        //影子

                        string yinzipos =_x.getString( "yinziposition" );
                        var  yinziposLst =   GetConfigLst( yinzipos );
                        vo.yinzipostionMapping.Add( "x" , yinziposLst[ 0 ] );
                        vo.yinzipostionMapping.Add( "y" , yinziposLst[ 1 ] );
                        vo.yinzipostionMapping.Add( "z" , yinziposLst[ 2 ] );   //位置

                        string yinzisca =_x.getString( "yinziscale" );
                        var  yinziscaLst =   GetConfigLst( yinzisca );
                        vo.yinziscaleMapping.Add( "x" , yinziscaLst[ 0 ] );
                        vo.yinziscaleMapping.Add( "y" , yinziscaLst[ 1 ] );
                        vo.yinziscaleMapping.Add( "z" , yinziscaLst[ 2 ] );   //位置

                        string runpos =_x.getString( "runpostion" );

                        if ( runpos != "null" )
                        {
                            vo.runpostionMapping = new Dictionary<string , float>();
                            var  runposLst =   GetConfigLst( runpos );
                            vo.runpostionMapping.Add( "x" , runposLst[ 0 ] );
                            vo.runpostionMapping.Add( "y" , runposLst[ 1 ] );
                            vo.runpostionMapping.Add( "z" , runposLst[ 2 ] );   //位置
                        }

                        _vo.transformMapping.Add( vo.carrer , vo );

                    }


                }

            }

        }

        //每个职业的武器配置
        private void GetWeaponTransform() {

            List<SXML> xml = XMLMgr.instance.GetSXMLList("ride.Weapon");

            foreach ( var x in xml )
            {
                WeaponVo vo = new WeaponVo();

                vo.carrer = x.getInt( "id" );

                SXML rightSxml = x.GetNode( "Rigth" );
                SXML leftSxml = x.GetNode( "Left" );

                if ( rightSxml != null )
                {
                    vo.rightVo = new WeaponTransform();
                    vo.rightVo.postionMapping = new Dictionary<string , float>();
                    vo.rightVo.rotationMapping = new Dictionary<string , float>();

                    string pos =rightSxml.getString( "postion" );
                    var  posLst =   GetConfigLst( pos );
                    AddVe3Mapping( posLst , vo.rightVo.postionMapping );

                    string rotation =rightSxml.getString( "rotation" );
                    var  rotaLst =   GetConfigLst( rotation );
                    AddVe3Mapping( rotaLst , vo.rightVo.rotationMapping );


                }

                if ( leftSxml != null )
                {
                    vo.leftVo = new WeaponTransform();
                    vo.leftVo.postionMapping = new Dictionary<string , float>();
                    vo.leftVo.rotationMapping = new Dictionary<string , float>();

                    string pos =leftSxml.getString( "postion" );
                    var  posLst =   GetConfigLst( pos );
                    AddVe3Mapping( posLst , vo.leftVo.postionMapping );

                    string rotation =leftSxml.getString( "rotation" );
                    var  rotaLst =   GetConfigLst( rotation );
                    AddVe3Mapping( rotaLst , vo.leftVo.rotationMapping );

                }

                weaponTransform.Add( vo.carrer , vo );

            }

        }

        //坐骑等级配置config
        private void GetRideLevelConfig()
        {
            List<SXML> xml = XMLMgr.instance.GetSXMLList("ride.lvl_info");

            foreach ( SXML x in xml )
            {
                LevelConfigVo lvlvo = new LevelConfigVo();
                lvlvo.attMappingLst = new Dictionary<int, List<AttConfigVo>>();

                lvlvo.lvl = x.getInt( "lvl" );
                lvlvo.exp = x.getInt( "exp" );
                lvlvo.speed = x.getFloat( "speed" );
                lvlvo.dressment = x.getInt( "dressment" );

                //if ( lvlvo.exp == -1 && maxExp == 0 )
                //{
                //    if ( levelMapping.ContainsKey( lvlvo.lvl -1 ) )
                //    {
                //        maxExp = ( levelMapping[ lvlvo.lvl - 1 ] as LevelConfigVo ).exp;
                //    }
                //}

                foreach (SXML _carr_att in x.GetNodeList("carr_att"))
                {
                        int carr =  _carr_att.getInt("carr");

                         List<AttConfigVo> attLst = new List<AttConfigVo>();

                        foreach (SXML _x in _carr_att.GetNodeList("att"))
                        {
                            
                            AttConfigVo att = new AttConfigVo();
                            att.att_type = _x.getInt("att_type");
                            att.add = _x.getInt("add");
                            attLst.Add(att);

                        if (!lvlvo.attMappingLst.ContainsKey(carr))
                        {
                            lvlvo.attMappingLst.Add(carr, attLst);

                        }else {

                            lvlvo.attMappingLst[carr] = attLst;

                            }
                        
                        }
                }

                AddMapping( lvlvo.lvl , levelMapping , lvlvo );

            }

            //if ( maxExp == 0 )
            //{
            //    Debug.LogError("没有匹配到最大exp值");
            //}

        }

        //坐骑灵性配置config
        private void GetRideGiftConig() {

            List<SXML> xml = XMLMgr.instance.GetSXMLList("ride.gift_info");

            foreach ( SXML x in xml )
            {
                RideGiftConfigVo lvlvo = new RideGiftConfigVo();

                lvlvo.attMaping = new Dictionary<int , List<AttConfigVo>>();

                lvlvo.lvl = x.getInt( "lvl" );
                lvlvo.num = x.getInt( "num" );

                List<SXML> typeLst=x.GetNodeList( "gift_type" );

                foreach ( SXML currGift in typeLst ) {

                    int type = currGift.getInt( "type" );

                    List<AttConfigVo> attLst  = new List<AttConfigVo>();

                    foreach ( SXML _x in currGift.GetNodeList( "att" ) )
                    {
                        AttConfigVo att= new AttConfigVo();

                        att.att_type = _x.getInt( "att_type" );

                        att.add = _x.getInt( "add" );

                        attLst.Add( att );
                    }

                    if ( lvlvo.attMaping.ContainsKey( type ) )
                    {
                        lvlvo.attMaping[ type ] = attLst;
                    } else {
                        lvlvo.attMaping.Add( type , attLst );
                    }

                }

                AddMapping( lvlvo.lvl , rideGiftMapping , lvlvo );
            }
        }

        //坐骑灵性maxlvl 奖励
        private void GetRideMaxGiftConig()
        {

            List<SXML> xml = XMLMgr.instance.GetSXMLList("ride.max_gift");

            foreach ( SXML x in xml )
            {
                RideMaxGiftConfigVo maxGift = new RideMaxGiftConfigVo();
                maxGift.attLst = new List<AttConfigVo>();
                maxGift.type = x.getInt( "type" );
                maxGift.max = x.getInt( "max" );
                maxGift.desc = x.getString( "desc" );

                foreach ( SXML _x in x.GetNodeList( "att" ) )
                {
                    AttConfigVo attVo =  new AttConfigVo();
                    attVo.att_type = _x.getInt( "att_type" );
                    attVo.add = _x.getInt( "add" );

                }

                AddMapping( maxGift.type , rideMaxGiftMapping , maxGift );
            }

        }

        private void AddVe3Mapping( float [] lst , Dictionary<string , float> mapping) {

            if ( lst == null || mapping == null )
            {
                return;
            }

            mapping.Add( "x" , lst[ 0 ] );
            mapping.Add( "y" , lst[ 1 ] );
            mapping.Add( "z" , lst[ 2 ] );   

    }

        private void AddMapping( int type , Dictionary<int , ConfigVo> mapping , ConfigVo value) {

            if ( mapping.ContainsKey( type ) )
            {
                mapping[ type ] = value;
            }
            else {

                mapping.Add( type , value );

            }

        }

        public T GetValueByType<T>(int type) where T : ConfigVo {

            T t=null;

            ConfigVo vo=null;
            
            if ( rideMapping.TryGetValue( type , out vo ) && vo is T )
            {

                return t = vo as T;
            }

            if ( levelMapping.TryGetValue( type , out vo ) && vo is T )
            {

                return t = vo as T;

            }

            if ( rideGiftMapping.TryGetValue( type , out vo ) && vo is T )
            {

                return t = vo as T;
            }

            //if ( rideMaxGiftMapping.TryGetValue( type , out vo ) && vo is T )
            //{

            //    return t = vo as T;
            //}

            return t;
        }

        public void SetRideInfoS2cData( Variant s2cData ) {

            if ( rideInfo == null )
            {
                rideInfo= new RideInfoData();
                rideInfo.giftMapping= new Dictionary<uint, RideGiftData>();
                rideInfo.ridedressMapiping= new Dictionary<uint , RideDressments>();
            }

            rideInfo.lvl = s2cData.getValue( "lvl" )._uint;
            rideInfo.exp = s2cData.getValue( "exp" )._uint;
            rideInfo.dress = s2cData.getValue( "dress" )._uint;
            rideInfo.mount = s2cData.getValue( "mount" )._uint;
            //rideInfo.gift_point = s2cData.getValue( "gift_point" )._uint;
            rideInfo.combpt = s2cData.getValue( "combpt" )._uint;

            rideInfo.configVo =GetValueByType<RideConfigVo>( (int) rideInfo.dress );
            rideInfo.lvlconfigVo=GetValueByType<LevelConfigVo>( ( int ) rideInfo.lvl );

            List<Variant> giftLst = s2cData.getValue( "gift" )._arr;

            for ( int i = 0 ; i < giftLst.Count ; i++ )
            {
                RideGiftData gift = new RideGiftData();
                gift.type = giftLst[ i ].getValue( "type" )._uint;
                gift.lvl = giftLst[ i ].getValue( "lvl" )._uint;

                if ( rideInfo.giftMapping.ContainsKey( gift.type ) )
                {
                    rideInfo.giftMapping[ gift.type ] = gift;
                }
                else {

                    rideInfo.giftMapping.Add( gift.type , gift );
                }
                
            }  //灵力

            List<Variant> dressmentsLst = s2cData.getValue( "dressments" )._arr;

            for ( int i = 0 ; i < dressmentsLst.Count ; i++ )
            {
                RideDressments dress = new RideDressments();

                dress.dress = dressmentsLst[ i ].getValue( "dress" )._uint;

                dress.limit = dressmentsLst[ i ].getValue( "limit" )._uint;

                dress.isforever = dressmentsLst[ i ].getValue( "forever" )._bool;

                if ( rideInfo.ridedressMapiping.ContainsKey( dress.dress ) )
                {
                    rideInfo.ridedressMapiping[ dress.dress ] = dress;
                }
                else
                {
                    rideInfo.ridedressMapiping.Add( dress.dress , dress );
                }

            }   //外观

            if ( GRMap.curSvrConf != null && GRMap.curSvrConf.ContainsKey( "maptype" ) && GRMap.curSvrConf[ "maptype" ]._int > 0 )
            {
                //副本中不携带坐骑
            }
            else
            {
                if ( SelfRole._inst != null  && rideInfo.mount == ( uint ) RIDESTATE.UP )
                    SelfRole._inst.set_Ride( ( int ) A3_RideModel.getInstance().GetRideInfo().dress );
            }

        }

        public void ChangeRideLvlS2cData( Variant s2cData )
        {
            if ( s2cData.ContainsKey( "lvl" ) )
            {
                rideInfo.lvl = s2cData.getValue( "lvl" )._uint;

                rideInfo.lvlconfigVo=GetValueByType<LevelConfigVo>( ( int ) rideInfo.lvl );
            }

            if ( s2cData.ContainsKey( "combpt" ) )
            {
                rideInfo.combpt = s2cData.getValue( "combpt" )._uint;
            }

            rideInfo.exp = s2cData.getValue( "exp" )._uint;

        }

        public void ChangRideGiftLvlS2cData( Variant s2cData ) {

            uint type = s2cData.getValue("type")._uint;

            if ( rideInfo.giftMapping.ContainsKey( type ) )
            {
                rideInfo.giftMapping[ type ].type = type;
                rideInfo.giftMapping[ type ].lvl = s2cData.getValue( "lvl" )._uint;
            }
            else {
                RideGiftData  vo = new  RideGiftData();
                vo.type = type;
                vo.lvl = s2cData.getValue( "lvl" )._uint;
                rideInfo.giftMapping.Add( type , vo );
            }

            rideInfo.combpt = s2cData.getValue( "combpt" )._uint;
            //rideInfo.gift_point = s2cData.getValue( "gift_point" )._uint;

        }

        public void ChangeRideStateS2cData( Variant s2cData )
        {
            rideInfo.mount = s2cData.getValue( "mount" )._uint;

            rideInfo.dress = s2cData.getValue("dress")._uint;

            if ( SelfRole._inst != null  && rideInfo.mount == ( uint ) RIDESTATE.UP )
            {

                SelfRole._inst.set_Ride( ( int ) A3_RideModel.getInstance().GetRideInfo().dress );
            }

            else if ( SelfRole._inst  != null && rideInfo.mount == ( uint ) RIDESTATE.Down )
            {

                SelfRole._inst.Remove_Ride();

            }
        }

        public void ChangeRideDressS2cData( Variant s2cData )
        {
            rideInfo.dress = s2cData.getValue( "dress" )._uint;

            if (s2cData.ContainsKey("lock_dress"))
            {
                var lockdress = s2cData.getValue("lock_dress")._uint;

                rideInfo.ridedressMapiping.Remove(lockdress);  // 限时坐骑 移除

                Globle.err_output(-5704);

            }

            rideInfo.configVo = GetValueByType<RideConfigVo>( (int) rideInfo.dress );

            if ( SelfRole._inst != null  && rideInfo.mount == ( uint ) RIDESTATE.UP  &&  A3_RideProxy.IsCanChangeRide( SelfRole._inst.dianjiTime ) )
            {
                if ( SelfRole._inst.invisible )
                {
                    return; //隐身状态
                }

                SelfRole._inst.set_Ride( ( int ) rideInfo.dress , true );
            }

        }

        public void AddRideDressS2cData( Variant s2cData )
        {
            RideDressments vo = new RideDressments();
            vo.dress = s2cData.getValue( "dress" )._uint;
            vo.limit = s2cData.getValue( "limit" )._uint;
            vo.isforever = s2cData.getValue( "forever" )._bool;

            if ( rideInfo.ridedressMapiping.ContainsKey( vo.dress ) )
            {
                rideInfo.ridedressMapiping[ vo.dress ] = vo;
            }else
            {
                rideInfo.ridedressMapiping.Add( vo.dress , vo );
            }

        }

        public RideInfoData GetRideInfo() {

            return rideInfo;
        }

        public uint GetUpGradeRideItemId() {

            if ( upGradeRideItemId == 0 )
            {
                upGradeRideItemId = XMLMgr.instance.GetSXMLList( "ride.upgrade_ride" )[0].getUint( "need_item" );
            }

            return upGradeRideItemId;

        }

        public uint GetUpGradeGiftItemId()
        {

            if ( upGradeGiftItemId == 0 )
            {
                upGradeGiftItemId = XMLMgr.instance.GetSXMLList( "ride.upgrade_gift" )[ 0 ].getUint( "need_item" );
            }

            return upGradeGiftItemId;

        }

        public Dictionary<int , ConfigVo> GetAllRideMapping() {

            return rideMapping;
        }

        private float [] GetConfigLst(string data) {

            string [] _lst = data.Split(',');

            return new float[] { float.Parse( _lst[ 0 ] ) , float.Parse( _lst[ 1 ] ) , float.Parse( _lst[ 2 ] ) };
        }

        public WeaponVo GetWeaponVoByCarrer(int carrer) {

            return weaponTransform[ carrer ];
        }

        public int GetRideId()
        {
            if ( rideInfo == null )
            {
                return -1;
            }

            return (int)rideInfo.dress;
        }

        public int GetRideState()
        {
            if ( rideInfo == null )
            {
                return 0;
            }

            return ( int ) rideInfo.mount;
        }

        public int GetRideLvl()
        {
            if ( rideInfo == null )
            {
                return -1;
            }

            return ( int ) rideInfo.lvl;
        }


 }

    public class RideConfigVo : ConfigVo
    {
        public int id;
        public string desc;
        public string avatar;
        public string icon;
        public int condition;
        public string name;
        public List<ConditionVO> conditionLst;
        public Dictionary<int,TransformConfigVo> transformMapping;

    }

    public class ConditionVO {
        public uint type;
        public uint lvl;
        public uint need_item;
        public uint need_num;
    }  

    public class LevelConfigVo : ConfigVo
    {
        public int lvl;
        public int exp;
        public float speed;
        public int dressment;
        //public List<AttConfigVo> attLst;
        public Dictionary<int,List<AttConfigVo>> attMappingLst;

    }

    public class AttConfigVo : ConfigVo
    {
        public int att_type;
        public int add;
    }

    public class RideGiftConfigVo : ConfigVo
    {
        public int lvl;
        public int num;
        public Dictionary<int, List<AttConfigVo>> attMaping;
    }

    public class RideMaxGiftConfigVo : ConfigVo
    {
        public int type;
        public int max;
        public string desc;
        public List<AttConfigVo> attLst;
    }

    public class ConfigVo
    {
      
    }

    public class RideInfoData {

        public uint lvl;
        public uint exp;
        public uint dress;
        public uint mount;         // <!-- 0:下坐骑     1:上坐骑   -->
        public uint gift_point;    // <!-- 天赋点 -->
        public uint combpt;
        public Dictionary<uint,RideGiftData> giftMapping;         //灵性
        public Dictionary<uint,RideDressments> ridedressMapiping;  // 幻化
        public RideConfigVo configVo;
        public LevelConfigVo lvlconfigVo;

    }

    public class RideGiftData
    {
        public uint type;
        public uint lvl;   
    }

    public class RideDressments
    {
        public uint dress;
        public uint limit;       //限制时间  
        public bool isforever;  //是否永久
    }

    public class TransformConfigVo{

        public int carrer;  //角色职业
        public Dictionary<string,float> postionMapping;  // x y z
        public Dictionary<string,float> rotationMapping; // x y z
        public Dictionary<string,float> scaleMapping; // x y z
        public Dictionary<string,float> yinzipostionMapping; // x y z
        public Dictionary<string,float> yinziscaleMapping; // x y z
        public float baseoffest = 0f;
        public Dictionary<string,float> runpostionMapping; // x y z
    }

    public class WeaponVo {

        public int carrer;

        public WeaponTransform rightVo;

        public WeaponTransform leftVo;

    }

    public class WeaponTransform {

        public Dictionary<string,float> postionMapping;  // x y z
        public Dictionary<string,float> rotationMapping; // x y z

    }



}
