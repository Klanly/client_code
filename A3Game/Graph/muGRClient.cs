using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using GameFramework;
using Cross;
namespace MuGame
{

   public  class muGRClient : GRClient
    {

        public muGRClient(gameMain m)
            : base(m)
        {

        }

        override protected void onInit()
        {
            regCreator(OBJECT_NAME_DEF.SCENE_MAP_CTRL, LGGRMap.create);
            regCreator(OBJECT_NAME_DEF.SCENE_MAP_DRAW, GRMap.create);

            regCreator(OBJECT_NAME_DEF.SCENE_CAMERA_CTRL, LGGRCamera.create);
            regCreator(OBJECT_NAME_DEF.SCENE_CAMERA_DRAW, GRCamera.create);


            regCreator(OBJECT_NAME_DEF.SCENE_MAIN_PLAY_CTRL, LGGRAvatarMain.create);
            regCreator(OBJECT_NAME_DEF.SCENE_MAIN_PLAY_DRAW, GRUsrplayerAvatar.create);

            regCreator(OBJECT_NAME_DEF.SCENE_NPC_CTRL, sceneCtrlAvatarNpc.create);
            regCreator(OBJECT_NAME_DEF.SCENE_NPC_DRAW, GRAvatar.create);

            regCreator(OBJECT_NAME_DEF.SCENE_OTHER_PLAY_CTRL, LGGRAvatarOtherPlayer.create);
            regCreator(OBJECT_NAME_DEF.SCENE_OTHER_PLAY_DRAW, GrPlayerAvatar.create);


            regCreator(OBJECT_NAME_DEF.SCENE_MONSTER_CTRL, LGGRAvatarMonster.create);
            regCreator(OBJECT_NAME_DEF.SCENE_MONSTER_DRAW, GrMonsterAvatar.create);

            regCreator(OBJECT_NAME_DEF.SCENE_HERO_CTRL, LGGRAvatarHero.create);
            regCreator(OBJECT_NAME_DEF.SCENE_HERO_DRAW, GRHero.create);

            addEventListener(GAME_EVENT.SCENE_CREATE_MAIN_CHAR, onCreateMainPlay);
            addEventListener(GAME_EVENT.SCENE_CREATE_CAMERA, onCreateMainCamera);
            addEventListener(GAME_EVENT.SCENE_CREATE_MAP, onCreateMap);
            addEventListener(GAME_EVENT.SCENE_CREATE_NPC, onCreateNPC);
            addEventListener(GAME_EVENT.SCENE_CREATE_MONSTER, onCreateMonster);
            addEventListener(GAME_EVENT.SCENE_CREATE_HERO, onCreateHero);
            addEventListener(GAME_EVENT.SCENE_CREATE_OTHER_CHAR, onCreateOtherPlayer);


        }

		public IGREffectParticles createEffect( string effID, float x, float y, bool loop=false ) 
		{ 
			IGREffectParticles eff = createEffect( effID );
			if( eff == null ) return null;
			eff.loop = loop;
			eff.x = (float)GameTools.inst.pixelToUnit( (double)x );
			eff.z = (float)GameTools.inst.pixelToUnit( (double)y );
			eff.y = (float)getZ( x, y );
			return eff;
		}

        private void onCreateMainPlay(GameEvent e)
        {
            createMainAvatarObject(e.target as lgGDBase);
        }

        private void onCreateMainCamera(GameEvent e)
        {
            createCamera(e.target as lgGDBase);
        }
        private void onCreateMap(GameEvent e)
        {
            createMapObject(e.target as lgGDBase);
        }



        private void onCreateNPC(GameEvent e)
        {
            createNPC(e.target as lgGDBase);
        }

        private void onCreateMonster(GameEvent e)
        {
            createMonster(e.target as lgGDBase);
        }

        private void onCreateHero(GameEvent e)
        {
            createHero(e.target as lgGDBase);
        }

        private void onCreateOtherPlayer(GameEvent e)
        {
            createOtherPlayer(e.target as lgGDBase);
        }


        //override public GRCharacter3D createGraphCharShadow( string avatarid )
        //{		
        //    Variant conf = getEntityConf( avatarid );
        //    if( conf == null )
        //    {
        //        GameTools.PrintCrash( "createGraphCharShadow conf Err!" );
        //        return null;
        //    }

        //    return createGraphChar( conf );
        //}

		//========= avatar conf =======
		override public string getNPCAvatar( uint npcid )
		{
			Variant conf = svrNpcConf.get_npc_data( (int)npcid );			
			if( conf == null ) return GameConstant.DEFAULT_AVATAR;
			return conf["obj"]._str;
		}

		override public string  getAvatarId( uint sex, uint carr )
		{
            if (sex == 0)
                return "0";
            else
                return "1";
		}

		override public string getMonAvatarId( uint mid )
		{
            Variant conf = MonsterConfig.instance.getMonster(mid+"");
				
			if( conf == null || !conf.ContainsKey("obj") ) return GameConstant.DEFAULT_AVATAR;
			 
			return conf["obj"]._str;
		}
        override public string getHeroAvararId(uint hid)
        {
            SXML avatarxml = XMLMgr.instance.GetSXML("hero.hero", "id==" + hid);
            uint id = avatarxml.getUint("obj");
            if (avatarxml == null)
            {
                return "1010";
            }
            return id.ToString();
        }
		override public avatarInfo getEqpAvatarInfo( uint tpid, Variant eqp )
		{
			Variant itm = svrItemConf.get_item_conf( tpid );
			if( itm == null )
			{
				//DebugTrace.print( "getEqpAvatarId conf Err!" );
				return null;
			} 
			avatarInfo x = new avatarInfo();
			x.iid = eqp["id"]._uint;
			x.tpid = tpid;
			x.avatarid = itm["conf"]["avatar"]._str;
			x.pos = itm["conf"]["pos"]._int;
			return x;
		}

        //========				 

        //private int hdtindex;
        //override public double getZ( double x, double y)
        //{
        //	//return 0;
        //	byte[] gray = (this.g_gameConfM as muCLientConfig).localGrd.gray;
        //	float width = (this.g_gameConfM as muCLientConfig).localGrd.wight;
        //	float height = (this.g_gameConfM as muCLientConfig).localGrd.height;
        //	double posy = 0;
        //	if (gray!=null)
        //	{
        //		int index = (int)((y * GameConstant.PIXEL_TRANS_UNIT) * width) + (int)(x * GameConstant.PIXEL_TRANS_UNIT);
        //		if (Math.Abs(index) < gray.Length)
        //		{
        //			hdtindex = gray[(int)((y * GameConstant.PIXEL_TRANS_UNIT) * width) + (int)(x * GameConstant.PIXEL_TRANS_UNIT)];
        //			posy = (hdtindex - GameConstant.MIN_GRAY) * GameConstant.GRAY_TRANS_PIXEL;
        //			if (posy <= 0.5)
        //			{
        //				posy = 0;
        //			}
        //			return posy;
        //		}
        //	}
        //	return posy;

        //	/*
        //	Variant pinfo = this._netM.joinWorldInfoInst.mainPlayerInfo;
        //	uint currMapid = pinfo["mapid"];
        //	todo map conf
        //	Variant local_conf = this.mgr._sceneM.GraphMgr.getMapConf( currMapid.ToString() ); 
        //	Variant conf =  this._gameConfM.svrMapsConf.getSingleMapConf( currMapid );
        //	float tile_size = conf["tile_size"];
        //	float globaMapW = conf["width"] * tile_size;
        //	float globaMapH = conf["height"] * tile_size;*/
        //}


        override public float getZ(float x, float y)
        {

            return ScenceUtils.getGroundHight(x, y);

            return 0;

            ClientGrdConfig cgc = (this.g_gameConfM as muCLientConfig).localGrd;
            if (cgc.m_hdtdata != null)
            {
                //debug.Log("getZ x = " + x* GameConstantDef.PIXEL_TRANS_HDTP + "   y = " + y* GameConstantDef.PIXEL_TRANS_HDTP); ��ǰ�ĸ߶�ͼ���赲���9�����൱һ���赲3*3���߶�
                uint msk_x = (uint)(x * 0.03125f);
                uint msk_y = (uint)(y * 0.03125f);

                uint idx = msk_y * lgmap.m_unMapWidth + msk_x;

                //debug.Log("getZ x = " + msk_x + "   y = " + msk_y + "   map w =" + lgmap.m_unMapWidth);

                if (idx < cgc.m_hdtdata.Length)
                {
                    return cgc.m_hdtdata[idx];
                }
            }

            //jason ����ͳһ�߶�
            float fhdt_z = (this.g_gameConfM as muCLientConfig).localGrd.m_hdt_z;
            return fhdt_z;

            //������Ҫ�����²�ֵ

			
            //byte[] gray = (this.g_gameConfM as muCLientConfig).localGrd.gray;
            //float width = (this.g_gameConfM as muCLientConfig).localGrd.grayWidth;
            //float height = (this.g_gameConfM as muCLientConfig).localGrd.grayHeight;

            ////todo 
            //float hmin = (this.g_gameConfM as muCLientConfig).localGrd.min * 0.01f;
            //float hmax = (this.g_gameConfM as muCLientConfig).localGrd.max * 0.01f;

            //int hdt = gethdt((int)(x * GameConstantDef.PIXEL_TRANS_HDTP), (int)(y* GameConstantDef.PIXEL_TRANS_HDTP) );

            ////�м�߶�Ϊ  =  ��͵�+(��ߵ�-��͵�)*(��ǰ����-1)/(255-1)   hmin="277" hmax="2503"/
            //double z = hmin + (hmax - hmin) * (hdt - 1.0f) / 254.0f;

            //return z;
        }

        //public int gethdt(int x, int y)
        //{
        //    byte[] gray = (this.g_gameConfM as muCLientConfig).localGrd.gray;
        //    int width = (this.g_gameConfM as muCLientConfig).localGrd.grayWidth;
        //    int height = (this.g_gameConfM as muCLientConfig).localGrd.grayHeight;

        //    if (gray == null)
        //        return 0;

        //    int ix = Math.Max(0, Math.Min(x, width - 1));
        //    int iy = Math.Max(0, Math.Min(y, height - 1));
        //    int idx = iy * width + ix;

        //    return gray[idx];
        //}
		 
		// ===== objs =====

		private SvrItemConfig svrItemConf
		{
			get{
			
				return this.g_gameConfM.getObject( OBJECT_NAME.CONF_SERVER_ITEM ) as SvrItemConfig;
			}	
		}

		private SvrNPCConfig svrNpcConf
		{
			get{
			
				return this.g_gameConfM.getObject( OBJECT_NAME.CONF_SERVER_NPC ) as SvrNPCConfig;
			}	
		}
		private SvrMonsterConfig svrMonConf
		{
			get{
			
				return this.g_gameConfM.getObject( OBJECT_NAME.CONF_SERVER_MONSTER ) as SvrMonsterConfig;
			}	
		}

		private LGMap lgmap
		{
			get{
			
				return this.g_gameM.getObject( OBJECT_NAME.LG_MAP ) as LGMap;
			}	
		}

    }

	 
	 
	 //posavatar:�ֲ���װ����
	 //   a. avt:�ֲ���װ��Ϣ�ڵ�
	 //	   i. stid��״̬id
	 //	   ii. pos����Ҫ��װ�Ĳ�λ��ͬװ����pos.
	 //	   iii. avatar����װavatar��id
	 //	   iv. carr����ְҵ��Ӧ�����⴦����Ϣ
	 //		   i. id��ְҵid
	 //		   ii. eqppos����ѡ����������װ��ָ��λ��
	 //			   1) ="r":װ������
	 //			   2) ="l":װ������
	 //		   iii. double����ѡ�����Ƿ�˫��
	 //		   iv. subtp����ѡ����������subtp�������󡢶�ʱ��������
	 //		   v. holdDouble����ѡ�����Ƿ�ʹ��˫�ֶ���	 
	 
	
}


 