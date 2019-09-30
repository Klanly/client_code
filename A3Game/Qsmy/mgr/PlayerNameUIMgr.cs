using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using GameFramework;
using Cross;
using System.Collections;
using UnityEngine.EventSystems;
using DG.Tweening;

namespace MuGame
{
    public class PlayerNameUIMgr
    {
        private static PlayerNameUIMgr instacne;
        public static PlayerNameUIMgr getInstance()
        {
            if ( instacne == null )
                instacne = new PlayerNameUIMgr();
            return instacne;
        }


        TickItem process;

        private List<PlayerNameItem> lPool;
        private Dictionary<INameObj, PlayerNameItem> dItem;
        private List<PlayerNameItem> lItem;
        private Transform playerNameLayer;

        private Dictionary<uint,GameObject> allOtherTitle;

        public GameObject selfTitleGo;
        public PlayerNameUIMgr()
        {
            lItem = new List<PlayerNameItem>();
            dItem = new Dictionary<INameObj , PlayerNameItem>();
            lPool = new List<PlayerNameItem>();
            playerNameLayer = GameObject.Find( "playername" ).transform;
            process = new TickItem( onUpdate );

            lActiveItem = new List<ActiveItem>();
            lActiveItemPool = new List<ActiveItem>();

            allOtherTitle = new Dictionary<uint , GameObject>();

            TickMgr.instance.addTick( process );
        }

        PlayerNameItem carItem;
        INameObj carObj;
        public void show( INameObj avatar )
        {

            if ( dItem.ContainsKey( avatar ) )
                return;

            PlayerNameItem item;
            if ( lPool.Count == 0 )
            {
                GameObject temp = GAMEAPI.ABLayer_LoadNow_GameObject("uilayer_name_user");
                GameObject go = GameObject.Instantiate(temp) as GameObject;
                go.transform.SetParent( playerNameLayer , false );
                item = new PlayerNameItem( go.transform );
            }
            else
            {
                item = lPool[ 0 ];
                item.visiable = true;
                lPool.RemoveAt( 0 );
            }


            if ( avatar is ProfessionRole )
            {
                if ( !( avatar as ProfessionRole ).m_isMain )
                {
                    allOtherTitle[ ( avatar as ProfessionRole ).m_unCID ] = item.heroTitleIcon.gameObject;
                }
                else
                {
                    selfTitleGo =  item.heroTitleIcon.gameObject;
                }
            }

            item.refresh( avatar );
            lItem.Add( item );
            dItem[ avatar ] = item;

            if ( avatar is MonsterRole )
            {
                if ( A3_ActiveModel.getInstance() != null )
                {
                    if ( A3_ActiveModel.getInstance().mwlr_target_monId == 0 || ( avatar as MonsterRole ).monsterid != A3_ActiveModel.getInstance().mwlr_target_monId )
                        item.mhPlayerName.transform.parent.gameObject.SetActive( false );
                    else
                        item.mhPlayerName.transform.parent.gameObject.SetActive( true );
                }
                else
                    item.mhPlayerName.transform.parent.gameObject.SetActive( false );
            }
            else
                item.mhPlayerName.transform.parent.gameObject.SetActive( false );
            if ( avatar is MDC000 )
            {
                carObj = avatar;
                item.setHP( ( ( float ) ( avatar.curhp / avatar.maxHp ) * 100 ).ToString() );
                carItem = item;
                item.refreshHp( avatar.curhp , avatar.maxHp , avatar );
            }
            else
            {
                item.cartxt.gameObject.SetActive( false );
                item.refreshHp( avatar.curhp , avatar.maxHp );
            }

            //if (avatar is self)
            //item.refreshVipLv(avatar.lgAvatar.viewInfo["vip"]);
            //else
            item.refreicon();
            item.refreshVipLv( 0 );
            if ( avatar is ProfessionRole )
            {
                if ( ( avatar as ProfessionRole ).m_isMain )
                {
                    if ( PlayerModel.getInstance().istitleActive )
                        item.refreshTitle( a3_RankModel.now_id );
                    item.refresNameColor( ( int ) PlayerModel.getInstance().now_nameState );

                    if (PlayerModel.getInstance().inSpost && PlayerModel.getInstance().lvlsideid != 0)
                    {
                        if (PlayerModel.getInstance().lvlsideid == 1)
                            item.refresNameColor_spost(SPOSTNAME_TYPE.RED);
                        else if (PlayerModel.getInstance().lvlsideid == 2)
                            item.refresNameColor_spost(SPOSTNAME_TYPE.BULE);
                    } else if (PlayerModel.getInstance().inCityWar && PlayerModel.getInstance().lvlsideid != 0)
                    {
                        item.refresNameColor_spost(SPOSTNAME_TYPE.BULE);
                    }
                  
                    item.refresHitback( ( int ) PlayerModel.getInstance().hitBack );
                    item.refreshVipLv( ( uint ) A3_VipModel.getInstance().Level );
                }
                else
                {
                    item.refreshTitle( avatar.title_id );
                    item.refresNameColor( avatar.rednm );

                    if (PlayerModel.getInstance().inSpost && avatar.spost_lvlsideid != 0)
                    {
                        if (1 == avatar.spost_lvlsideid)
                        {
                            item.refresNameColor_spost(SPOSTNAME_TYPE.RED);
                        }
                        else if (2 == avatar.spost_lvlsideid)
                        {
                            item.refresNameColor_spost(SPOSTNAME_TYPE.BULE);
                        }
                    }

                    else if(PlayerModel.getInstance().inCityWar && avatar.spost_lvlsideid != 0) {

                        if (avatar.spost_lvlsideid == PlayerModel.getInstance().lvlsideid)
                        {
                            item.refresNameColor_spost(SPOSTNAME_TYPE.BULE);
                        }
                        else {
                            item.refresNameColor_spost(SPOSTNAME_TYPE.RED);
                        }
                    }
                    item.refresHitback( ( int ) avatar.hidbacktime );
                }
            }
            else
                item.refreshTitle( avatar.title_id );


        }
        int hp_per = 100;
        public void carinfo( GameEvent e )
        {
            carItem.refreshHp( carObj.curhp - 1 , carObj.maxHp );
            hp_per = e.data[ "hp_per" ];
            carItem.hp.localScale = new Vector3( ( float ) hp_per / 100 , 1 , 1 );
            if ( e.data[ "hp_per" ] <= 20 )
            {
                carItem.setHP( hp_per.ToString() , true );
            }
            else
                carItem.setHP( hp_per.ToString() );
        }
        public void refreshTitlelv( INameObj role , int title_id )
        {
            if ( !dItem.ContainsKey( role ) )
                return;
            PlayerNameItem item = dItem[role];
            item.refreshTitle( title_id );

        }
        public void refreshmapCount( INameObj role , int count , bool ismine )
        {
            if ( !dItem.ContainsKey( role ) )
                return;
            if ( !( role is ProfessionRole ) )
                return;
            PlayerNameItem item = dItem[role];
            item.refreshMapcount( count , ismine );
        }
        public void refreserialCount( INameObj role , int count )
        {
            if ( !dItem.ContainsKey( role ) )
                return;
            if ( !( role is ProfessionRole ) )
                return;
            PlayerNameItem item = dItem[role];
            item.refreshserial( count );
        }
        public void refreshNameColor( INameObj role , int rednmstate )
        {
            if ( !dItem.ContainsKey( role ) )
                return;
            PlayerNameItem item = dItem[role];
            item.refresNameColor( rednmstate );
        }

        public void refresNameColor_spost(INameObj role, SPOSTNAME_TYPE  rednmstate)
        {
            if (!dItem.ContainsKey(role))
                return;
            PlayerNameItem item = dItem[role];
            item.refresNameColor_spost(rednmstate);
        }

        public void refresName(INameObj role)
        {
            if ( role != null && !dItem.ContainsKey(role))
                return;

            if ( role is ProfessionRole )
            {
                PlayerNameItem item = dItem[role];

                item.utext.text = (role as ProfessionRole).roleName;

                uint monId = (role as BaseRole).m_isMain ?  PlayerModel.getInstance().cid : (role as BaseRole).m_unCID;

                if (MonsterMgr._inst.roleSummonMapping.ContainsKey(monId))
                {
                    var monster = MonsterMgr._inst.m_mapMonster[MonsterMgr._inst.roleSummonMapping[monId]];

                    if (dItem.ContainsKey(monster))
                    {
                        item = dItem[monster];

                        item.setName(monster.tempXMl.getString("name"), (role as ProfessionRole).roleName + ContMgr.getCont("MonsterMgr"));

                    }  // 召唤兽的名字

                }
              
            }

        }


        public void refresHitback( INameObj role , int time , bool ismyself = false )
        {
            if ( !dItem.ContainsKey( role ) )
                return;
            PlayerNameItem item = dItem[role];
            if ( ismyself )
                item.refresHitback( time , true );
            else
                item.refresHitback( time , false );
        }
        public void hideAll()
        {
            playerNameLayer.gameObject.SetActive( false );
        }
        public void showAll()
        {
            playerNameLayer.gameObject.SetActive( true );
        }

        public void refreshVipLv( INameObj role , uint viplv )
        {
            if ( !dItem.ContainsKey( role ) )
                return;

            PlayerNameItem item = dItem[role];
            item.refreshVipLv( viplv );
        }

        public void setName( INameObj role , string sumname , string mastername )
        {
            if ( !dItem.ContainsKey( role ) )
                return;

            PlayerNameItem item = dItem[role];
            item.setName( sumname , mastername );

            if ( role is MS0000 && ( role as MS0000 ).masterid == PlayerModel.getInstance().cid )
            {
                item.hp.gameObject.GetComponent<Image>().color =new Color( 128f/255f , 223f/255f , 120f/255f , 255f/255f );
            }
            else if ( TeamProxy.getInstance().MyTeamData != null && TeamProxy.getInstance().MyTeamData.IsInMyTeam( ( role as MS0000 ).masterid ) )
            {
                item.hp.gameObject.GetComponent<Image>().color =new Color( 128f/255f , 223f/255f , 120f/255f , 255f/255f ); // 队友召唤兽
            }
            else
            {
                item.hp.gameObject.GetComponent<Image>().color = new Color( 229f/255f , 49f/255f , 49f/255f , 255f/255f );
            }
        }

        public void setDartName( INameObj role , string legionName )
        {
            if ( !dItem.ContainsKey( role ) )
                return;

            PlayerNameItem item = dItem[role];
            item.setDartName( legionName );
        }
        public void seticon_forDaobao( INameObj role , int num )
        {
            if ( !dItem.ContainsKey( role ) )
                return;
            PlayerNameItem item = dItem[role];
            item.seticon_forDaobao( num );
        }

        public void seticon_forMonsterHunter( INameObj role , bool hide = false )
        {
            if ( !dItem.ContainsKey( role ) )
                return;
            PlayerNameItem item = dItem[role];
            item.show_mhMark( role , hide );
        }

        public void hide( INameObj role )
        {
            if ( !dItem.ContainsKey( role ) )
                return;
            PlayerNameItem item = dItem[role];
            item.clear();
            item.visiable = false;
            dItem.Remove( role );
            lItem.Remove( item );
            lPool.Add( item );
        }

        public void monsterTitleInvisible( INameObj role , bool invisible )
        {
            if ( !dItem.ContainsKey( role ) )
                return;
            PlayerNameItem item = dItem[role];
            item.bg.gameObject.SetActive( !invisible );
            item.hp.gameObject.SetActive( !invisible );
            item.sumtext.gameObject.SetActive( !invisible );
        }

        public void refreshHp( INameObj role , Variant d )
        {
            if ( role != null  && dItem.ContainsKey( role ) )
                dItem[ role ].refreshHp( d[ "cur" ] , d[ "max" ] );
        }

        public void refeshHpColor( INameObj role ) {

            if ( dItem.ContainsKey( role ) )
            {   

                if ( role is ProfessionRole && ( role as BaseRole ).m_unTeamID != 0 &&  ( role as BaseRole ).m_unTeamID ==  PlayerModel.getInstance().teamid )
                {
                    dItem[ role ].hp.gameObject.GetComponent<Image>().color =new Color( 128f/255f , 223f/255f , 120f/255f , 255f/255f );
                }
                else
                {
                    dItem[ role ].hp.gameObject.GetComponent<Image>().color = new Color( 229f/255f , 49f/255f , 49f/255f , 255f/255f );
                }
            }

            if ( role != null && role is ProfessionRole && MonsterMgr._inst.roleSummonMapping.ContainsKey( ( role as BaseRole ).m_unCID )  )
            {
                if ( MonsterMgr._inst.m_mapMonster.ContainsKey( MonsterMgr._inst.roleSummonMapping[ ( role as BaseRole ).m_unCID ] ))
                {
                    if ( dItem.ContainsKey( MonsterMgr._inst.m_mapMonster[ MonsterMgr._inst.roleSummonMapping[ ( role as BaseRole ).m_unCID ] ] ) )
                    {
                        var monster =  MonsterMgr._inst.m_mapMonster[ MonsterMgr._inst.roleSummonMapping[ ( role as BaseRole ).m_unCID ] ] ;

                        dItem [ monster ].hp.gameObject.GetComponent<Image>().color = dItem[ role ].hp.gameObject.GetComponent<Image>().color;

                    }
                }
            }

            if ( role is ProfessionRole && (role as ProfessionRole ).m_isMain )
            {
                dItem[ role ].hp.gameObject.GetComponent<Image>().color =new Color( 128f/255f , 223f/255f , 120f/255f , 255f/255f );
            }
      
        }

        public void refeshHpColor()
        { 
            foreach ( KeyValuePair<INameObj , PlayerNameItem> item in dItem )
            {
                if ( item.Key is ProfessionRole )
                {
                    item.Value.hp.gameObject.GetComponent<Image>().color =  ( ( item.Key as BaseRole ).m_unTeamID != 0 &&
                        ( item.Key as BaseRole ).m_unTeamID ==  PlayerModel.getInstance().teamid ) ?
                        new Color( 128f/255f , 223f/255f , 120f/255f , 255f/255f ) :
                        new Color( 229f/255f , 49f/255f , 49f/255f , 255f/255f );
                }

                else if ( item.Key is MS0000 ) {

                    var  v = item.Key as MS0000;

                    var  a = TeamProxy.getInstance().MyTeamData != null && TeamProxy.getInstance().MyTeamData.IsInMyTeam( ( item.Key as MS0000 ).masterid );

                    if ( ( TeamProxy.getInstance().MyTeamData != null && TeamProxy.getInstance().MyTeamData.IsInMyTeam( ( item.Key as MS0000 ).masterid ) ) 

                        || ( item.Key as MS0000 ).masterid == PlayerModel.getInstance().cid ) {

                        item.Value.hp.gameObject.GetComponent<Image>().color =new Color( 128f/255f , 223f/255f , 120f/255f , 255f/255f );

                    }

                    else {

                        item.Value.hp.gameObject.GetComponent<Image>().color =new Color( 229f/255f , 49f/255f , 49f/255f , 255f/255f );

                    }

                }
            }
        }

        public void refreshHp( INameObj role , int cur , int max )
        {
            if ( dItem.ContainsKey( role ) )
            {

                if ( role is MDC000 )
                {
                    dItem[ role ].refreshHp( cur , max , role );
                }
                else
                    dItem[ role ].refreshHp( cur , max  );
            }

        }

        private int tick = 0;
        private void onUpdate( float s )
        {

            if ( lItem.Count > 0 )
            {
                foreach ( PlayerNameItem item in lItem )
                {
                    item.update();
                }
            }

            if ( lActiveItem.Count > 0 )
            {
                List<ActiveItem> l = new List<ActiveItem>();
                foreach ( ActiveItem activeItem in lActiveItem )
                {
                    if ( activeItem.update() )
                    {
                        l.Add( activeItem );
                    }
                }

                foreach ( ActiveItem activeItem in l )
                {
                    clearActive( activeItem );
                }

            }

        }

        public void SetOtherTitle( uint cid , int titleId , bool isShow )
        {
            if ( allOtherTitle.ContainsKey( cid ) )
            {
                if (titleId != 0)
                {
                    allOtherTitle[cid].GetComponent<Image>().sprite = GAMEAPI.ABUI_LoadSprite(a3_HeroTitleServer.getInstance().roleTitleData_Dic[titleId].title_img);

                    GameObject othermapCount = allOtherTitle[cid].transform.parent.parent.FindChild("othermapcount").gameObject;

                    if (!othermapCount.activeSelf)
                    {
                        allOtherTitle[cid].transform.parent.gameObject.SetActive(isShow);
                    }

                }
                else {


                    allOtherTitle[cid].GetComponent<Image>().sprite = null;

                    allOtherTitle[cid].transform.parent.gameObject.SetActive(false);

                }
                
            }

        }

        private List<ActiveItem> lActiveItem;
        private List<ActiveItem> lActiveItemPool;
        public void clearActive( ActiveItem item )
        {
            item.clear();
            lActiveItem.Remove( item );
            lActiveItemPool.Add( item );
        }
    }

    public class ActiveItem : Skin
    {
        private Animator ani;
        private GRAvatar _avatar;

        public ActiveItem( Transform trans )
            : base( trans )
        {
            initUI();
        }

        void initUI()
        {
            ani = __mainTrans.GetComponent<Animator>();
        }

        //public void show(GRAvatar av)
        //{
        //    _avatar = av;
        //    this.visiable = true;
        //    ani.SetTrigger("showit");
        //}

        public void clear()
        {
            _avatar = null;
        }

        public bool update()
        {
            pos = _avatar.getHeadPos();

            if ( ani.GetCurrentAnimatorStateInfo( 0 ).normalizedTime >= 1f )
            {
                this.visiable = false;
                return true;
            }
            return false;
        }
    }


    class PlayerNameItem : Skin
    {
        public INameObj _avatar;
        private Text ptxt;
        private Text mtxt;
        public Text utext;
        private Text htxt;
        public Text sumtext;
        private Text mastertext;
        public RectTransform hp;
        public Image bg;

        private Image vipIcon;
        private RectTransform rectVipIcon;
        private GameObject serial;
        private bool hpshowed = false;
        private float wVip;
        private Text curName;
        private Image titleIcon;
        private Text baotu;
        //private Text mapCount;
        private GameObject mapCount;
        private GameObject othermapCount;
        private GameObject hitback_state;
        private GameObject monsterHunter;
        public Text mhPlayerName;
        public Text cartxt;
        public Text legionName;

        //延迟显示，解决手机上会闪屏的bug
        public int showAfter = 10;

        public Image heroTitleIcon;
        public PlayerNameItem( Transform trans )
            : base( trans )
        {
            initUI();
        }

        public void clear()
        {
            _avatar = null;

            hp.gameObject.GetComponent<Image>().color = new Color( 229f/255f , 49f/255f , 49f/255f , 255f/255f );
        }

        void initUI()
        {
            vipIcon = getComponentByPath<Image>( "uname/vipicon" );
            bg = this.getComponentByPath<Image>( "bg" );
            ptxt = getComponentByPath<Text>( "pname" );
            mtxt = getComponentByPath<Text>( "mname" );
            utext = getComponentByPath<Text>( "uname" );
            htxt = getComponentByPath<Text>( "hname" );
            sumtext = getComponentByPath<Text>( "sumname" );
            mastertext = getComponentByPath<Text>( "sumname/mastername" );
            mhPlayerName = getComponentByPath<Text>( "mhname/name" );
            titleIcon = getComponentByPath<Image>( "uname/title" );
            //mapCount = getComponentByPath<Text>("mapcount");
            //othermapCount = getGameObjectByPath("othermapcount");
            mapCount = getGameObjectByPath( "TitleLst/mapcount" );
            othermapCount = getGameObjectByPath( "TitleLst/othermapcount" );

            baotu = getComponentByPath<Text>( "baotu_guai" );
            monsterHunter = getGameObjectByPath( "mh_mark" );
            hitback_state = getGameObjectByPath( "hitback_state" );
            serial = getGameObjectByPath( "uname/serial" );
            cartxt = getComponentByPath<Text>( "carHP" );
            legionName = getComponentByPath<Text>( "carHP/legionname" );

            heroTitleIcon = getComponentByPath<Image>( "TitleLst/TitleIcon/Icon" );

            hp = getComponentByPath<RectTransform>( "hp" );
            hp.localScale = new Vector3( 0.1f , 1 , 1 );
            bg.gameObject.SetActive( false );
            hp.gameObject.SetActive( false );
            titleIcon.gameObject.SetActive( false );
            vipIcon.gameObject.SetActive( false );
            hitback_state.gameObject.SetActive( false );
            baotu.gameObject.SetActive( false );
            mapCount.gameObject.SetActive( false );
            othermapCount.SetActive( false );
            serial.SetActive( false );
            cartxt.gameObject.SetActive( false );

            rectVipIcon = vipIcon.GetComponent<RectTransform>();
            wVip = rectVipIcon.sizeDelta.x;
        }

        public void setName( string sum_name , string master_name )
        {
            sumtext.text = sum_name;
            mastertext.text = master_name;
            //   debug.Log("::::::::::::::aaaaaaaa:::::" + __mainTrans.name + " " + name);
        }
        public void setDartName( string dart_name )
        {
            legionName.text = dart_name;
        }
        public void setHP( string hp_per , bool show = false )
        {
            cartxt.gameObject.SetActive( true );
            if ( show )
            {
                cartxt.text = hp_per + "%" + ContMgr.getCont( "gameroom_wudi" );
            }
            else
                cartxt.text = hp_per + "%";
        }

        public void refreicon()
        {
            baotu.gameObject.SetActive( false );
        }
        public void seticon_forDaobao( int num )
        {
            if ( num > 0 )
            {
                baotu.gameObject.SetActive( true );
                baotu.text = num.ToString();
            }
            else
            {
                baotu.gameObject.SetActive( false );
            }
        }
        public void show_mhMark( INameObj role , bool show )
        {
            mhPlayerName.text = string.Format( "{0}\n{1}" , ( role as MonsterRole ).roleName , PlayerModel.getInstance().name );
            mhPlayerName.transform.parent.gameObject.SetActive( !show );
            monsterHunter.SetActive( !show );
        }
        public void refresh( INameObj avatar )
        {
            _avatar = avatar;
            showAfter = 10;
            this.visiable = false;
            ptxt.text = mtxt.text = utext.text = htxt.text = sumtext.text = mastertext.text = "";

            heroTitleIcon.transform.parent.gameObject.SetActive( false );

            mapCount.gameObject.SetActive( false );
            othermapCount.SetActive( false );
            serial.SetActive( false );
            this.transform.SetAsFirstSibling();
            if ( avatar is ProfessionRole )
            {
                this.transform.SetAsLastSibling();
                curName = utext;
                // mapCount.gameObject.SetActive(true);
                if ( ( avatar as ProfessionRole ).m_isMain )
                {
                    debug.Log( "roleName:--------------------"+avatar.roleName );
                  
                    if ( a3_HeroTitleServer.getInstance().isShowTitle &&  a3_HeroTitleServer.getInstance().eqpTitleId !=0 && !mapCount.activeSelf )
                    {
                        heroTitleIcon.sprite =  GAMEAPI.ABUI_LoadSprite( a3_HeroTitleServer.getInstance().roleTitleData_Dic[ a3_HeroTitleServer.getInstance().eqpTitleId ].title_img );
                        heroTitleIcon.SetNativeSize();
                        heroTitleIcon.transform.parent.gameObject.SetActive( true );
                    }
                    a3_HeroTitleServer.getInstance().addEventListener( a3_HeroTitleServer.SET_UI_ICON , ( GameEvent e ) =>
                    {
                        if (a3_HeroTitleServer.getInstance().eqpTitleId != 0)
                        {
                            heroTitleIcon.sprite = GAMEAPI.ABUI_LoadSprite(a3_HeroTitleServer.getInstance().roleTitleData_Dic[a3_HeroTitleServer.getInstance().eqpTitleId].title_img);
                            heroTitleIcon.SetNativeSize();

                        }
                        else {

                            heroTitleIcon.sprite = null;
                        }

                        heroTitleIcon.transform.parent.gameObject.SetActive(a3_HeroTitleServer.getInstance().isShowTitle && heroTitleIcon.sprite != null);

                    } );
                }
                else
                {
                    int titleID = (avatar as ProfessionRole).heroTitleID;
                    if ( titleID != 0 )
                    {

                        heroTitleIcon.sprite =  GAMEAPI.ABUI_LoadSprite( a3_HeroTitleServer.getInstance().roleTitleData_Dic[ titleID ].title_img);
                        heroTitleIcon.SetNativeSize();
                        if ( ( avatar as ProfessionRole ).heroTitle_isShow && !othermapCount.activeSelf )
                        {
                            heroTitleIcon.transform.parent.gameObject.SetActive( true );
                        }
                        else
                        {
                            heroTitleIcon.transform.parent.gameObject.SetActive( false );
                        }

                    }

                    debug.Log( "OtherRoleName:--------------------"+avatar.roleName );
                }

            }
            else if ( avatar is MonsterRole )
            {
                heroTitleIcon.transform.parent.gameObject.SetActive( false );
                if ( avatar is MonsterPlayer )
                {
                    curName = utext;
                    refresNameColor( ( int ) REDNAME_TYPE.RNT_NORMAL );
                }
                else
                    return;
                //curName = mtxt;

            }
            else if ( avatar is NpcRole )
            {
                curName = ptxt;
                heroTitleIcon.transform.parent.gameObject.SetActive( false );
            }
            else
            {
                curName = null;
                heroTitleIcon.transform.parent.gameObject.SetActive( false );
            }
            if ( curName )
                curName.text = avatar.roleName;

            //    debug.Log("::::::::::::::aaaaaaaa:::::" + __mainTrans.name + " " + avatar.roleName);
        }
        //红名类型更改名字的颜色
        public void refresNameColor( int rednmstate )
        {
            if (PlayerModel.getInstance().inSpost || PlayerModel.getInstance().inCityWar) return; // 战场中
            if ( utext != null )
            {
                switch ( rednmstate )
                {
                    case ( int ) REDNAME_TYPE.RNT_NORMAL:
                    utext.color = new Color( 1 , 1 , 1 , 1 );
                    break;
                    case ( int ) REDNAME_TYPE.RNT_RASCAL:
                    utext.color = new Color( 116.0f / 255.0f , 125.0f / 255.0f , 61.0f / 255.0f , 1 );
                    break;
                    case ( int ) REDNAME_TYPE.RNT_EVIL:
                    utext.color = new Color( 1 , 1f , 0f , 1 );
                    break;
                    case ( int ) REDNAME_TYPE.RNT_DEVIL:
                    utext.color = new Color( 1f , 0f , 0f , 1 );
                    break;

                };
            }

        }

        public void refresNameColor_spost(SPOSTNAME_TYPE state)
        {
            if (!PlayerModel.getInstance().inSpost && !PlayerModel.getInstance().inCityWar) return; // 不在战场中或城战
            if (utext != null)
            {
                switch (state)
                {
                    case SPOSTNAME_TYPE.BULE:
                        utext.color = new Color(0, 1, 1, 1);
                        break;
                    case SPOSTNAME_TYPE.RED :
                        utext.color = new Color(1f, 0.2f, 0.4f, 1);
                        break;
                    //case SPOSTNAME_TYPE.TEAMMATE:
                    //    utext.color = new Color(0, 1, 0, 1);
                    //    break;
                }
            }
        }

        void getColor()
        {

        }
        TickItem hitbacktime;
        float times = 0;
        int i;
        bool isself = false;
        public void refresHitback( int time , bool ismyself = false )
        {
            //debug.Log("时间是多少::::::::：" + time);
            if ( time <= 0 )
            {
                hitback_state.SetActive( false );
                return;
            }
            else
                hitback_state.SetActive( true );
            if ( hitbacktime == null )
            {
                hitbacktime = new TickItem( onUpdates );
                TickMgr.instance.addTick( hitbacktime );
            }
            i = time;
            if ( ismyself )
                isself = true;
            else
                isself = false;
        }

        void onUpdates( float s )
        {
            times += s;
            if ( times >= 1 )
            {

                i--;
                if ( isself )
                    PlayerModel.getInstance().hitBack = ( uint ) i;   //服务器发的时间戳，要不停刷新玩家自己的反击buff时间
                //debug.Log(i+"");
                if ( i == 0 )
                {
                    hitback_state.SetActive( false );
                    i = 0;

                    TickMgr.instance.removeTick( hitbacktime );
                    hitbacktime = null;
                }
                times = 0;
            }


        }

        public void refreshVipLv( uint viplv )
        {
            if ( viplv == 0 )
            {
                if ( vipIcon.gameObject.active )
                    vipIcon.gameObject.SetActive( false );
            }
            else
            {
                if ( curName == null )
                    return;

                vipIcon.gameObject.SetActive( true );
                vipIcon.sprite = GAMEAPI.ABUI_LoadSprite( "icon_vip_" + ( viplv ) );
                vipIcon.transform.SetParent( curName.transform , false );
                Vector2 vec = rectVipIcon.anchoredPosition;

                Vector2 size = rectVipIcon.sizeDelta;
                //if (viplv > 9)
                //    size.x = wVip * 1.3f;
                //else
                //    size.x = wVip;
                //rectVipIcon.sizeDelta = size;
            }
        }

        public void refreshTitle( int title_id )
        {
            if ( title_id == 0 )
            {
                titleIcon.gameObject.SetActive( false );
            }
            else
            {
                if ( curName == null )
                    return;

                titleIcon.gameObject.SetActive( true );
                titleIcon.sprite = GAMEAPI.ABUI_LoadSprite( "icon_achievement_title_ui_t" + title_id );
                titleIcon.SetNativeSize();
            }
        }
        public void refreshMapcount( int Count , bool ismine )
        {
            if ( Count <= 0 )
            {
                mapCount.gameObject.SetActive( false );
                othermapCount.SetActive( false );
            }
            else
            {
                if ( curName == null )
                    return;


                if ( ismine )
                {
                    mapCount.gameObject.SetActive( true );
                    mapCount.transform.FindChild( "num" ).GetComponent<Text>().text = Count.ToString();
                    heroTitleIcon.transform.parent.gameObject.SetActive(false);
                }
                else
                {
                    othermapCount.SetActive( true );
                    heroTitleIcon.transform.parent.gameObject.SetActive( false );
                    //string str = "";
                    //if (Count <= 10)
                    //    str = "稀少";
                    //else if (Count <= 20)
                    //    str = "充裕";
                    //else if (Count <= 30)
                    //    str = "较多";
                    //else
                    //    str = "富足";
                    //othermapCount.text = str;
                }
            }
        }
        public void refreshserial( int Count )
        {
            if ( curName == null )
                return;
            if ( Count <= 3 ) { serial.SetActive( false ); }
            else
            {
                if ( Count <= 10 )
                {
                    serial.transform.FindChild( "count" ).GetComponent<RectTransform>().localScale = new Vector2( .8f , .8f );
                }
                else if ( Count <= 20 )
                {
                    serial.transform.FindChild( "count" ).GetComponent<RectTransform>().localScale = new Vector2( 1f , 1f );
                }
                else
                {
                    serial.transform.FindChild( "count" ).GetComponent<RectTransform>().localScale = new Vector2( 1.2f , 1.2f );
                }
                if ( Count >= 9999 )
                {
                    Count = 9999;
                }
                serial.SetActive( true );
                serial.transform.FindChild( "count" ).GetComponent<Text>().text = Count.ToString();
            }

        }

        public void refreshHp( int cur , int max , INameObj avatar = null )
        {
            if ( max <= 0 )
            {
                setHpVisible( false );
                return;
            }

            if ( cur >= max )
                setHpVisible( false );
            else
            {
                setHpVisible( true );
                hp.localScale = new Vector3( ( float ) cur / max , 1 , 1 );
            }
            if ( hp.localScale.x <= 0 )
            {
                bg.gameObject.SetActive( false );
                hp.gameObject.SetActive( false );
            }

            if ( avatar is MDC000 )
            {
                //hp.gameObject.SetActive(true);
                setHpVisible( true );
                hp.localScale = new Vector3( ( float ) cur / max , 1 , 1 );
                if ( ( int ) ( ( ( ( float ) cur / ( float ) max ) ) * 100 ) <= 20 )
                {
                    cartxt.text = "20%" + ContMgr.getCont( "gameroom_wudi" );
                }
                else
                    cartxt.text = ( ( int ) ( ( ( ( float ) cur / ( float ) max ) ) * 100 ) ).ToString() + "%";
            }

        }


        public void update()
        {
            if ( _avatar == null)
                return;

            Vector3 vec = _avatar.getHeadPos();

            if ( vec == Vector3.zero )
            {
                this.visiable = false;
                return;
            }
            if (showAfter > 0)
            {
                showAfter--;
                pos = vec;
                this.visiable = false;
                return;
            }
            this.visiable = true;

            pos = vec;
        }

        public void setHpVisible( bool b )
        {
            if ( b == hpshowed )
                return;
            hpshowed = b;
            if ( b )
            {
                bg.gameObject.SetActive( true );
                hp.gameObject.SetActive( true );
            }
            else
            {
                bg.gameObject.SetActive( false );
                hp.gameObject.SetActive( false );
            }
        }
    }

    public class GameTextNode
    {
        public Transform m_tfTxtNode = null;
        public Transform m_tfParent = null;
        public TextMesh m_mainTextM = null;
        public TextMesh m_shadowTextM = null;
        public GameTextNode( GameObject clone )
        {
            m_tfTxtNode = GameObject.Instantiate<GameObject>( clone ).transform;
            //gtn.m_tfTxtNode.SetParent(s_tfTempPool, false);

            m_mainTextM = m_tfTxtNode.gameObject.GetComponent<TextMesh>();
            //gtn.m_shadowTextM = gtn.m_tfTxtNode.FindChild("shadow").gameObject.GetComponent<TextMesh>();
            m_shadowTextM = m_tfTxtNode.GetChild( 0 ).gameObject.GetComponent<TextMesh>();
        }

        public string text
        {
            get
            {
                return m_mainTextM.text;
            }

            set
            {
                m_mainTextM.text = value;
                m_shadowTextM.text = value;
            }
        }
    }

    static public class ShowTextMgr
    {
        static private GameObject s_CloneTemplate = null;
        static float checkTime = 3f;
        static private List<GameTextNode> s_listAttachObj = new List<GameTextNode>();
        static public void Init( GameObject data_obj )
        {
            if ( s_CloneTemplate == null )
                s_CloneTemplate = data_obj.transform.FindChild( "text" ).gameObject;
        }
        static public GameTextNode CreateEmpty( Transform attachObj )
        {
            GameTextNode gtn = new GameTextNode(s_CloneTemplate);
            gtn.m_tfParent = attachObj;
            gtn.m_tfTxtNode.SetParent( attachObj , false );
            return gtn;
        }
        static public GameTextNode CreateText( string text , Vector3 pos )
        {
            GameTextNode gtn = new GameTextNode(s_CloneTemplate);
            gtn.m_tfTxtNode.position = pos;
            gtn.text = text;
            return gtn;
        }

        static public GameTextNode CreateText( string text , Transform attachObj )
        {
            for ( int i = 0 ; i < s_listAttachObj.Count ; i++ )
            {
                if ( s_listAttachObj[ i ] != null && s_listAttachObj[ i ].m_tfParent == attachObj )
                {
                    s_listAttachObj[ i ].text = text;
                    return s_listAttachObj[ i ];
                }
            }
            GameTextNode gtn = new GameTextNode(s_CloneTemplate);
            gtn.m_tfParent = attachObj;
            gtn.m_tfTxtNode.SetParent( attachObj , false );
            gtn.text = text;
            s_listAttachObj.Add( gtn );
            return gtn;
        }

        static void CleanInvalidAttachObj()
        {
            List<int> invalidIndex = new List<int>();
            for ( int i = 0 ; i < s_listAttachObj.Count ; i++ )
                if ( s_listAttachObj[ i ] == null || s_listAttachObj[ i ].m_tfParent == null )
                    invalidIndex.Add( i );
            for ( int i = invalidIndex.Count - 1 ; i >= 0 ; i-- )
                s_listAttachObj.RemoveAt( invalidIndex[ i ] );
        }

        static void FrameMove( float delta )
        {
            if ( checkTime > 0 ) return;
            checkTime -= delta;
            if ( checkTime <= 0 )
            {
                CleanInvalidAttachObj();
            }
        }
    }


}
