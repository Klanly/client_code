
using System;

namespace GameFramework
{
    public class PKG_NAME
    {
        //type cmd
        public const uint TYPG_SELECT_CID = 3;
		

		// C2S
		public const uint C2S_ATTACK = 3;
		public const uint C2S_MOVE = 9;
		public const uint C2S_STOP = 10;


		public const uint C2S_FCM_NOTIFY = 1;
        //public const uint C2S_PK_STATE_CHANGE = 2;
        public const uint C2S_PK_V_CHANGE = 3;
        public const uint C2S_CAST_TARGET_SKILL = 4;

        public const uint C2S_CAST_GROUND_SKILL = 5;

        public const uint C2S_FETCH_ITM_CARD = 22;//��ȡ���߿��е���
        public const uint C2S_ITEM_CARD_RES = 23;//��ȡ������Ϣ

        public const uint C2S_CAST_SELF_SKILL = 6;
        public const uint C2S_CUR_ARCHIVE_CHANGE = 34;
        public const uint C2S_GET_ACHIVES_RES = 35;
        public const uint C2S_LINE_INFO_RES = 49;
        public const uint C2S_JOIN_WORLD_RES = 50; //������Ϸ ��ʼ������
        public const uint C2S_MONSTER_ENTER_ZONE = 50;
        public const uint C2S_PLAYER_DETAIL_INFO = 52;
        public const uint C2S_ON_CLIENT_CONFIG = 151;
        public const uint C2S_ON_ERR_MSG = 252;
        public const uint C2S_ON_MAP_ACTIVE = 150;
        public const uint C2S_ON_PING = 183;
        public const uint S2C_ON_PING = 183;

        public const uint C2S_FORGE_OPT = 28;
        public const uint S2C_FORGE_OPT = 45;
        
        //public const uint C2S_ON_STOP_ATK = 11;
        //public const uint C2S_ON_ATTACK = 12;
        //public const uint C2S_ON_CAST_TARGET_SKILL = 13;
        //public const uint C2S_ON_CAST_GROUND_SKILL = 14;
        public const uint C2S_ON_CAST_SELF_SKILL = 15;
		//public const uint C2S_ON_SINGLE_DAMAGE = 18;
		//public const uint C2S_ON_BSTATE_CHANGE = 19;
		//public const uint C2S_ON_SINGLE_SKILL_RES = 22;
		//public const uint C2S_ON_ADD_STATE = 24;
		//public const uint C2S_ON_DIE = 25;
		//public const uint C2S_ON_CAST_SKILL_RES = 27;
		//public const uint C2S_ON_CASTING_SKILL_RES = 28;
		//public const uint C2S_ON_CANCEL_CASTING_SKILL_RES = 29;
		//public const uint C2S_ON_RMV_STATE = 31;

        public const uint C2S_GET_LVLAWARD = 24;//�����ȡ�������
        public const uint C2S_GET_OLWAD = 36;//��ȡ���߽���

        public const uint C2S_ACTIVE = 47;

        public const uint C2S_GET_ITEMS = 65;
        public const uint C2S_USE_ITEM = 68;

        public const uint C2S_BUY_ITEM = 62;
        public const uint C2S_SELL_ITEM = 63;
        public const uint C2S_DELETE_ITEM = 71;
        public const uint C2S_PLY_FUN_MSG = 255;
        public const uint C2S_GET_RANK_INFO = 254;

        public const uint C2S_SPORTS = 191;

        //�������
        public const uint C2S_GMIS_INFO = 42;//Ŀ��������Ϣ
        public const uint C2S_GMIS_AWD = 43;//��ȡ����Ŀ�꽱��
        public const uint C2S_GET_OL_TM = 44;//��ȡ���ߡ�����ʱ�䣬��ȡ���߽��������߲��� 
        public const uint C2S_GET_RECT = 45;//��ȡ���л��Ϣ������
        public const uint C2S_CHANGE_LINE = 49;//����
        public const uint C2S_PICK_DPITEM_RES = 77; //����ʰȡ��� 
        public const uint C2S_RMIS_INFO = 108;//�ƹ�������Ϣ
        //public const uint C2S_ACCEPT_MIS = 110;//��������
        public const uint C2S_COMMIT_MIS = 111;//�ύ����
        public const uint C2S_CHANGE_MIS = 112;//���
        public const uint C2S_AUTO_COMMIT_MIS = 113;//ί���������
        public const uint C2S_MIS_LINE_STATE = 114;//��ȡ�����߽�չ�����Ϣ
        public const uint C2S_ABORD_MIS = 115;//��������
        public const uint C2S_LVLMIS_PRIZE = 116;//��û�����Ϣ

        public const uint C2S_WORSHIP = 166;//Ĥ�������Ϣ

        //������ص�
        public const uint C2S_TRADE_REQ_RES = 140;
        public const uint C2S_TRADE_REQ = 141;
        public const uint C2S_TRADE_REQ_CONFIRM_RES = 142;
        public const uint C2S_CANCEL_TRADE_RES = 143;
        public const uint C2S_TRADE_ADD_ITM = 144;
        public const uint C2S_TRADE_LOCK_STATE = 145;
        public const uint C2S_TRADE_ERR_MSG = 146;
        public const uint C2S_TRADE_DONE = 147;
		


		public const uint C2S_GET_LVLMIS_RES = 117;
        public const uint C2S_CLANTER_RES = 232;
        public const uint C2S_LVL_RES = 233;
        public const uint C2S_CARRCHIEF = 234;
        public const uint C2S_LVL_BROADCAST = 236;
        public const uint C2S_ON_ARENA = 235;
        public const uint C2S_LVL_PVPINFO_BOARD_RES = 237;
        public const uint C2S_MOD_LVL_SELFPVPINFO = 238;
        public const uint C2S_LVL_ERR_MSG = 239;
        public const uint C2S_CHECK_IN_LVL_RES = 240;
        public const uint C2S_CREATE_LVL_RES = 241;
        public const uint C2S_ENTER_LVL_RES = 242;
        public const uint C2S_GET_ASSOCIATE_LVLS_RES = 243;
        public const uint C2S_GET_LVL_INFO_RES = 244;
        public const uint C2S_LVL_FIN = 245;
        public const uint C2S_LVL_GET_PRIZE_RES = 246;
        public const uint C2S_LVL_SIDE_INFO = 247;
        public const uint C2S_CLOSE_LVL_RES = 248;
        public const uint C2S_LVL_KM = 249;
        public const uint C2S_LEAVE_LVL_RES = 250;
        public const uint C2S_ON_BATTLE_DO_RES = 230;
                

        public const uint C2S_ADD_NPCS = 6;
        public const uint C2S_TRIG_EFF = 7;
        public const uint C2S_MONSTER_SPAWN = 20;
        public const uint C2S_PLAYER_RESPAWN = 21;
        public const uint C2S_SPRITE_INVISIBLE = 30;
        public const uint C2S_ON_SPRITE_HP_INFO_RES = 53;
        public const uint C2S_ON_PLAYER_ENTER_ZONE = 54;
        public const uint C2S_ON_SHOW_MAP_OBJ = 55;
        public const uint C2S_ON_SPRITE_LEAVE_ZONE = 56;
        public const uint C2S_ON_BEGIN_CHANGE_MAP_RES = 57;
        public const uint C2S_ONMAPCHANGE = 58;
        public const uint C2S_ON_OTHER_EQP_CHANGE = 70;

        public const uint C2S_GET_VIP_RES = 46;
        public const uint C2S_ITEM_MSG_RES = 95;
        public const uint C2S_CREATE_CLAN_RES = 210;
        public const uint C2S_JOIN_CLAN_REQ = 211;
        public const uint C2S_PL_JOIN_CLAN = 212;
        public const uint C2S_JOIN_CLAN = 213;
        public const uint C2S_PL_LEAVE_CLAN = 214;
        public const uint C2S_CLAN_DONATE_RES = 215;
        public const uint C2S_CLAN_INFO_CHANGE = 216;
        public const uint C2S_CHANGE_PLY_CLANC = 217;
        public const uint C2S_CLAN_DISMISS = 218;
        public const uint C2S_GET_CLAN_INFO_RES = 219;
        public const uint C2S_GET_CLANS_LIST_RES = 220;
        public const uint C2S_QUERY_CLAN_ID_RES = 221;
        public const uint C2S_GET_CLAN_SELFINFO_RES = 222;
        public const uint C2S_GET_CLANPLS_RES = 223;
        public const uint C2S_QUERY_CLINFO_RES = 224;
        public const uint C2S_JOIN_CLAN_REQ_RES = 225;
        public const uint C2S_CLAN_MSG = 226;

        public const uint C2S_TEAM_SETTING = 119;
        public const uint C2S_CREATE_TEAM_RES = 120;
        public const uint C2S_JOIN_TEAM_REQ = 121;
        public const uint C2S_CANCEL_JOIN_TEAM_REQ = 122;
        public const uint C2S_REFUSE_JOIN_TEAM_REQ = 123;
        public const uint C2S_LEAVE_TEAM = 124;
        public const uint C2S_INVITE_JOIN_TEAM = 125;
        public const uint C2S_CANCEL_INVITE_JOIN_TEAM = 126;
        public const uint C2S_REFUSE_INVITE_JOIN_TEAM = 127;
        public const uint C2S_PUBLISH_TEAM_RES = 128;
        public const uint C2S_DROP_TEAM_DPITM = 129;
        public const uint C2S_CHANGE_TEAM_LEADER = 130;
        public const uint C2S_GET_PUBED_TEAM_RES = 131;
        public const uint C2S_TEAM_ROLL_DPITM = 132;
        public const uint C2S_JOIN_TEAM_RES = 133;
        public const uint C2S_PLAYER_JOIN_TEAM = 134;
        public const uint C2S_ROOM_OPERATE_RES = 135;
        public const uint C2S_TEAM_CHANGE = 136;
        public const uint C2S_MEMBER_ONLINE_CHANGE = 137;
        public const uint C2S_MEMBER_ATT_CHANGE = 138;
        public const uint C2S_TEAM_PICK_DPITM = 139;

        public const uint C2S_LVL_ZHSLY = 184;//�ٻ�����԰

        public const uint C2S_ON_LOAD_WEAPON = 205; //���
        public const uint C2S_ON_SEND_MAIL = 155;//�����ʼ�
        //����
        public const uint C2S_ON_FRIEND_LIST= 168;
        public const uint C2S_ON_FRIEND_APPLY_LIST=169;
        public const uint C2S_ON_ADDFRIEND_BUTTON = 170;
        public const uint C2S_ON_FRIEND_REMOVE = 171;
        public const uint C2S_ON_LOAD_SRARCH_FRIEND= 172;
        public const uint C2S_ONAPPLYFRIEND = 173;
        public const uint C2S_ON_ADD_OR_REFUSE_FRIEND = 174;
        public const uint C2S_ON_LOAD_RECOMMENT_FRIEND= 175;
        public const uint C2S_ON_FRIEND_INFO = 176;
        //�齱
        public const uint C2S_LOTTERY = 98;
        //a3�̳�
        public const uint C2S_A3_SHOP = 62;
        //a3ǩ��
        public const uint C2S_A3_SIGN = 207;
        //a3����
        public const uint C2S_A3_SKILL = 86;
        //a3����
        public const uint C2S_A3_RUNE = 42;
        //a3�ɾ�
        public const uint C2S_A3_ACHIEVEMENT = 34;
        //PKģʽ
        public const uint C2S_A3_PKMODEL = 2;
        //ϴ����
        public const uint C2S_A3_WASHREDNAME = 32;
		//������
		public const uint C2S_A3_AUCTION = 117;
		//ħ�����Ը���
		public const uint C2S_A3_ACTIVE_MWSL = 165;
		//����Boss����
		public const uint C2S_A3_ACTIVE_BOSS = 166;
        //�����丱��
        public const uint C2S_A3_ACTIVE_GETCHEST = 199;
        //public const uint S2C_A3_ACTIVE_GETCHEST = 40;
        //����ף��
        public const uint C2S_A3_FB_BLESSING = 232;
        //a3��ʯ
        public const uint C2S_A3_RUNESTONE = 182;

        public const uint C2S_A3_ACTIVE_SWEEP = 193;

        public const uint C2S_ACTIVEDEGREE_RES = 27;//��Ծ��
        //ʥ��
        public const uint C2S_A3_HALLOWS = 185;

        //�����
        public const uint C2S_A3_AWARD = 187;

        //ͷ��
        public const uint C2S_A3_HEROTITLE = 190;

        //******** 	 	        ********
        //********   S2C TPKG   ********
        //********   		    ********

        public const uint S2C_TPKG_ON_DELET_CHAR = 4; // 
		public const uint S2C_TPKG_ON_CREATE_CHAR = 5; // 
		public const uint S2C_TPKG_GET_WORLD_COUNT = 6; // 
		public const uint S2C_TPKG_ERR = 7; // err
		public const uint S2C_TPKG_LOGIN_LINE = 20; //  
        public const uint S2C_TPKG_OUT_SVR = 51;
		//******** 	 			********
		//********   S2C RPC	********
		//********   			********
		public const uint S2C_FCM_NOTIFY 			= 1;
        ///public const uint S2C_PK_STATE_CHANGE		= 2;
        public const uint S2C_PK_V_CHANGE			= 3;
        public const uint S2C_LINE_CHANGE			= 4;
        public const uint S2C_GAIN_ACHIVE			= 5;
        public const uint S2C_USE_SELF_SKILL 				= 6;
        public const uint S2C_TRIG_EFF 				= 7;
		public const uint S2C_POS_CORRECT			= 8; //λ�ý���
		public const uint S2C_MOVE 					= 9; //�ƶ�
		public const uint S2C_STOP 					= 10; //ֹͣ�ƶ�
		
        public const uint S2C_ON_STOP_ATK			= 11;        
		public const uint S2C_MONSTER_HATED			= 12; //���
      
        public const uint S2C_ON_CAST_TARGET_SKILL			= 13;        
        public const uint S2C_ON_CAST_GROUND_SKILL			= 14;        
        public const uint S2C_ON_CAST_SELF_SKILL			= 15;
        public const uint S2C_ON_CAST_SKILL_ACT             = 38; //���ܲ���
		public const uint S2C_ON_SINGLE_DAMAGE 				= 18; //��ͨ�������ν��		 
        public const uint S2C_ON_BSTATE_CHANGE				= 19;        
        public const uint S2C_MONSTER_SPAWN					= 20; 
        public const uint S2C_PLAYER_RESPAWN 				= 21;  //��ɫ����
        public const uint S2C_ON_SINGLE_SKILL_RES			= 22;        
        public const uint S2C_ON_ADD_STATE					= 24;        
        public const uint S2C_ON_DIE						= 25;// ����   
       
        public const uint S2C_ON_CAST_SKILL_RES				= 27;        
        public const uint S2C_ON_CASTING_SKILL_RES			= 28;        
        public const uint S2C_ON_CANCEL_CASTING_SKILL_RES	= 29;        
        public const uint S2C_SPRITE_INVISIBLE				= 30;
        public const uint S2C_ON_RMV_STATE					= 31;
        public const uint S2C_CUR_ARCHIVE_CHANGE			= 34;
        public const uint S2C_GET_ACHIVES_RES				= 35;
        public const uint S2C_LINE_INFO_RES					= 49;
        public const uint S2C_JOIN_WORLD_RES				= 50; //������Ϸ ��ʼ������
        public const uint S2C_ON_SPRITE_HP_INFO_RES			= 53;
        public const uint S2C_PLAYER_ENTER_ZONE				= 54; // ��ҽ�����Ұ
        public const uint S2C_MONSTER_ENTER_ZONE			= 55; // ���������Ұ
        public const uint S2C_SPRITE_LEAVE_ZONE				= 56; //ʵ���뿪��Ұ
        public const uint S2C_BEGIN_CHANGE_MAP_RES			= 57; //��ͼ��ʼ�л�
        public const uint S2C_MAP_CHANGE					= 58; // ��ͼ�л����
        public const uint S2C_ON_OTHER_EQP_CHANGE			= 70;
        public const uint S2C_ON_CLIENT_CONFIG				= 151 ;
        public const uint S2C_ON_ERR_MSG					= 252;
        public const uint S2C_MAP_STATES_ACT                = 196;
        public const uint S2C_ON_SOMETHING_TODO = 99; //�����Ӱ˵ĸ���ϵͳ��Ϣ

        public const uint S2C_ACTIVEDEGREE_RES = 167;//��Ծ��

        public const uint S2C_ACTIVEONLINE = 189;//��Ծ�


        public const uint S2C_TRANS_REPO_ITM_RES = 33; //�ֿ��뱳��֮��ת�����߽��
        public const uint S2C_GET_OLWAD_RES = 36;//��ȡ���߽������
        public const uint S2C_MOD_PKG_SPC_RES = 37; //���������ֿ���ӽ��
        //public const uint S2C_WPN_BREAK_CHANGE = 38; //
        public const uint S2C_EQP_DURA_CHANGE = 39; //�����;ñ仯 
        public const uint S2C_BAGITEM_CDTIME = 48; //��Ʒ��ʹ��ʱ��cd


        public const uint S2C_GET_OL_TM_RES = 44;//��ȡ���ߡ�����ʱ���������ߡ�����ʱ�䷢���仯 
        public const uint S2C_GET_RECT_RES = 45;//��ȡ���л��Ϣ���������



        public const uint S2C_ACTIVE = 47;



        public const uint S2C_EQP_FRG_TRANS_RES = 59; //��ȡװ������ȼ����



        public const uint S2C_BUY_ITEM_RES = 62; //������߽��
        public const uint S2C_SELL_ITEM_RES = 63; //���۵��߽��
        public const uint S2C_BUY_SOLD_ITEM_RES = 64; //�ع����߽�� 
        public const uint S2C_GET_ITEMS_RES = 65; // ��ȡ�Լ��ĵ�����Ϣ���ֿ⡢��ʱ�ֿ�����
        public const uint S2C_DO_EQUIP_RES = 66; //װ������
        public const uint S2C_COMBINE_ITEM_RES = 67; //���ӵ��߽��
        public const uint S2C_USE_UITEM_RES = 68; //ʹ�õ��߽��     
        public const uint S2C_UP_EQUIP_RES = 16; //װ������
        public const uint S2C_ADVANCE_EQUIP_RES = 17; //���Ϻϳ�
        public const uint S2C_STRENGTH_EQUIP_RES = 69; //����װ����Ϣ
        public const uint S2C_DELETE_ITEM_RES = 71; //�������߽��
        public const uint S2C_EQP_FORGE_RES = 72; //װ�������� 
        public const uint S2C_EMBED_STONE_RES = 73; //��ʯ��Ƕ��� 
        public const uint S2C_REMOVE_STONE_RES = 74; //��ʯժ����� 
        public const uint S2C_ITEM_CHANGE = 75; //���߱仯��Ϣ
        public const uint S2C_ITEM_DROPED = 76; //���ߵ��� 
        public const uint S2C_PICK_DPITEM_RES = 77; //����ʰȡ��� 
        public const uint S2C_SUMMON_OPERATION_RES = 79; //�ٻ��޲���Э�� 
        public const uint S2C_GET_DBMKT_ITM = 94; //��ȡ�̳Ǵ��۵��߽�� 
        public const uint S2C_ITM_MERGE_RES = 103; //
        public const uint S2C_ITM_QUERY_RES = 104; //
        public const uint S2C_GOD_LIGHT = 107;
        public const uint S2C_BEGIN_COLLECT = 108;//
        public const uint S2C_A3_ACTIVE_GETCHEST = 199;
        public const uint S2C_STOP_COLLECT = 109;//
        public const uint S2C_GET_MAIL = 153;     //
        public const uint S2C_GOT_NEW_MAIL = 154; //������ʼ�
        public const uint S2C_SEND_MAIL_RES = 155;//�����ʼ����
        public const uint S2C_LOCK_MAIL_RES = 156;//�����ʼ����
        public const uint S2C_GET_MAIL_ITEM_RES = 157;//����ʼ�����
        public const uint S2C_DEL_MAIL_RES = 158; //ɾ���ʼ�
        public const uint S2C_CHAT_MSG = 160;//���졢�㲥
        public const uint S2C_CHAT_MSG_RES = 161;//����������һ���ϵͳ��������Ϣ
        public const uint S2C_WORSHIP_RES = 166; //Ĥ�ݽ��������Ϣ
        public const uint S2C_ADD_AUC_ITM_RES = 200; //���۵���,��ӵ�����������
        public const uint S2C_RMV_AUC_ITM_RES = 201;  //ȡ������
        public const uint S2C_BUY_AUC_ITM_RES = 202;  //�������̵���
        public const uint S2C_GET_PLY_AUC_LIST_RES = 203;
        public const uint S2C_FETCH_AUC_MONEY_RES = 204;
        public const uint S2C_GET_AUC_INFO_RES = 205;

        //�������
        //public const uint S2C_GMIS_INFO_RES = 42;//Ŀ��������Ϣ
        public const uint S2C_GMIS_AWD_RES = 43;//��ȡ����Ŀ�꽱��
      
        public const uint S2C_ACCEPT_MIS_RES = 110;//��������
        public const uint S2C_COMMIT_MIS_RES = 111;//�ύ����
        public const uint S2C_FINED_MIS_STATE_RES = 112;//��ȡ���������״̬���
        public const uint S2C_DATA_MIS_MODIFY_RES = 113;//����仯
        public const uint S2C_MIS_LINE_STATE_RES = 114;//��ȡ�����߽�չ�����Ϣ
        public const uint S2C_ABORD_MIS_RES = 115;//��������
        public const uint S2C_LVLMIS_PRIZE_RES = 116;//��û�����Ϣ
                          
        public const uint S2C_GET_SKILL = 85;//��ȡ�����б�
       // public const uint S2C_LEARN_SKILL = 86;//ѧϰ����
        public const uint S2C_SETUP_SKILL = 87;//skillup
        public const uint S2C_SKEXP_UP_RES = 90;//��������
		


        //�����ɫ��Ϣ
        public const uint S2C_ATT_CHANGE = 26;//Ŀ�����Ա仯
        public const uint S2C_SELF_ATT_CHANGE = 32;//�Լ����Ա仯
        public const uint S2C_DETAIL_INFO_CHANGE = 40;//
        public const uint S2C_SKEXP_CHANGE = 41;
        public const uint S2C_PLAYER_SHOW_INFO = 51;
        public const uint S2C_PLAYER_DETAIL_INFO = 52;
        public const uint S2C_LVL_UP = 60;
        public const uint S2C_MODE_EXP = 61;
        public const uint S2C_VIEW_AVATAR_CHANGE = 78;
        //public const uint S2C_UP_LV_CHANGE = 91;
        public const uint S2C_ADD_POINT = 148;
        
        public const uint S2C_Get_USER_CID_RES = 251;
       // public const uint S2C_QUERY_PLY_INFO_RES = 253;
        public const uint S2C_GET_RANK_INFO = 254;
        public const uint S2C_PLY_FUN_MSG = 255;

		public const uint S2C_GETPLAYERINFO_FROMNAME = 211;

        //������ص�
        public const uint S2C_TRADE_REQ_RES = 140;
        public const uint S2C_TRADE_REQ = 141;
        public const uint S2C_TRADE_REQ_CONFIRM_RES = 142;
        public const uint S2C_CANCEL_TRADE_RES = 143;
        public const uint S2C_TRADE_ADD_ITM = 144;
        public const uint S2C_TRADE_LOCK_STATE = 145;
        public const uint S2C_TRADE_ERR_MSG = 146;
        public const uint S2C_TRADE_DONE = 147;

        //public const uint S2C_GET_MAIL_LIST = 152;//��ȡ�ʼ��б�
        //public const uint S2C_GET_MAIL = 153;
        //public const uint S2C_GOT_NEW_MAIL = 154;
        //public const uint S2C_SEND_MAIL_RES = 155;
        //public const uint S2C_LOCK_MAIL_RES = 156;
        //public const uint S2C_GET_MAIL_ITEM_RES = 157;
        //public const uint S2C_DEL_MAIL_RES = 158;
        public const uint S2C_GET_VIP_RES = 46;
        public const uint S2C_ITEM_MSG_RES = 95;
        public const uint S2C_CREATE_CLAN_RES = 210;
        public const uint S2C_JOIN_CLAN_REQ = 211;
        public const uint S2C_PL_JOIN_CLAN = 212;
        public const uint S2C_JOIN_CLAN = 213;
        public const uint S2C_PL_LEAVE_CLAN = 214;
        public const uint S2C_CLAN_DONATE_RES = 215;
        public const uint S2C_CLAN_INFO_CHANGE = 216;
        public const uint S2C_CHANGE_PLY_CLANC = 217;
        public const uint S2C_CLAN_DISMISS = 218;
        public const uint S2C_GET_CLAN_INFO_RES = 219;
        public const uint S2C_GET_CLANS_LIST_RES = 220;
        public const uint S2C_QUERY_CLAN_ID_RES = 221;
        public const uint S2C_GET_CLAN_SELFINFO_RES = 222;
        public const uint S2C_GET_CLANPLS_RES = 223;
        public const uint S2C_QUERY_CLINFO_RES = 224;
        public const uint S2C_JOIN_CLAN_REQ_RES = 225;
        public const uint S2C_CLAN_MSG = 226;

        public const uint S2C_TEAM_SETTING = 119;
        public const uint S2C_CREATE_TEAM_RES = 120;
        public const uint S2C_JOIN_TEAM_REQ = 121;
        public const uint S2C_CANCEL_JOIN_TEAM_REQ = 122;
        public const uint S2C_REFUSE_JOIN_TEAM_REQ = 123;
        public const uint S2C_LEAVE_TEAM = 124;
        public const uint S2C_INVITE_JOIN_TEAM = 125;
        public const uint S2C_CANCEL_INVITE_JOIN_TEAM = 126;
        public const uint S2C_REFUSE_INVITE_JOIN_TEAM = 127;
        public const uint S2C_PUBLISH_TEAM_RES = 128;
        public const uint S2C_DROP_TEAM_DPITM = 129;
        public const uint S2C_CHANGE_TEAM_LEADER = 130;
        public const uint S2C_GET_PUBED_TEAM_RES = 131;
        public const uint S2C_TEAM_ROLL_DPITM = 132;
        public const uint S2C_JOIN_TEAM_RES = 133;
        public const uint S2C_PLAYER_JOIN_TEAM = 134;
        public const uint S2C_ROOM_OPERATE_RES = 135;
        public const uint S2C_TEAM_CHANGE = 136;
        public const uint S2C_MEMBER_ONLINE_CHANGE = 137;
        public const uint S2C_MEMBER_ATT_CHANGE = 138;
        public const uint S2C_TEAM_PICK_DPITM = 139;

        public const uint S2C_GET_OBJTEAM_INFO = 000001;
        //���͵���Ϣ-hh
        public const uint S2C_FETCH_ITM_CARD = 22;//��ȡ���߿��е���
        public const uint S2C_ITEM_CARD_RES = 23;//��ȡ������Ϣ
        public const uint S2C_GET_MERI_INFO_RES = 79;
        public const uint S2C_MERI_ACTIVATE = 80;
        public const uint S2C_ACUP_ACTIVATE = 81;
        public const uint S2C_BUY_MYSTORY = 82;//���͹�������
        public const uint S2C_GET_MYSTERY_INFO = 83;//��ȡ��������
        public const uint S2C_REFRSH_MYSTORY = 84;//ˢ����������
        public const uint S2C_GET_MERIS_RES = 91;
        public const uint S2C_OPENACUP = 93;
        public const uint S2C_DATA = 95;
        public const uint S2C_ON_AWARD_RES = 100;//���ֽ��������Ϣ
        public const uint S2C_ON_ACHIEVE_RES = 106;
        public const uint S2C_ON_DMIS_RES = 109;//ÿ�ձ������������Ϣ
        public const uint S2C_ON_ADD_SELF_LOTTERY_LOG = 162;//�����Լ��齱��־��¼��Ϣ
        public const uint S2C_ON_OTHER_LOTTERY_LOG = 163;//�������˳齱��־��¼��Ϣ
        public const uint S2C_ON_ADD_BUDDY_RES = 170;//��Ӻ��ѡ������������˽��
        public const uint S2C_ON_RMV_BUDDY_RES = 171;//�Ƴ����ѡ�������������
        public const uint S2C_ON_GET_BUDDY_RES = 172;//��ȡ���ѡ�������������
        public const uint S2C_ON_BECOME_BUDDY = 173;//��������Ӻ���
        public const uint S2C_ON_GET_BUDDY_OL_RES = 174;//��ȡ��������״̬���


        //---------------------------------------InGameLevelMsgs
        public const uint S2C_GET_LVLMIS_RES = 117;
        public const uint S2C_CLANTER_RES = 232;
        public const uint S2C_LVL_RES = 233;
        public const uint S2C_CARRCHIEF = 234;
        public const uint S2C_LVL_BROADCAST = 236;
        public const uint S2C_ON_ARENA = 235;
        public const uint S2C_LVL_PVPINFO_BOARD_RES = 237;
        public const uint S2C_MOD_LVL_SELFPVPINFO = 238;
        public const uint S2C_LVL_ERR_MSG = 239;
        public const uint S2C_CHECK_IN_LVL_RES = 240;
        public const uint S2C_CREATE_LVL_RES = 241;//��������
        public const uint S2C_ENTER_LVL_RES = 242;
        public const uint S2C_GET_ASSOCIATE_LVLS_RES = 243; //��������ͷ
        public const uint S2C_GET_LVL_INFO_RES = 244;
        public const uint S2C_LVL_FIN = 245;
        public const uint S2C_LVL_GET_PRIZE_RES = 246;
        public const uint S2C_LVL_SIDE_INFO = 247;
        public const uint S2C_CLOSE_LVL_RES = 248;
        public const uint S2C_LVL_KM = 249;
        public const uint S2C_LEAVE_LVL_RES = 250;
        public const uint S2C_ON_BATTLE_DO_RES = 230;
        public const uint S2C_ON_LOAD_DRESS = 206;//ʱװ

       // public const uint S2C_SIGN = 207;//ǩ��

        //�ɾ�
        public const uint S2C_GAInN_ACHIEVE_REWARD_RES = 5;
        public const uint S2C_ARCHIEVE_CHANGE= 34;
        public const uint S2C_ACTIVATE_TITLE = 35;

       // public const uint S2C_ACTIVEDEGREE_RES = 000000001;

        //  ��Ʊ
        public const uint S2C_TAELS_EXCHANGE = 105;

        //  ����
        //  S2C
        public const uint S2C_FAMILY_KICK_RES = 209;            //  ��ȡ���˽��
        public const uint S2C_FAMILY_JION_REQ = 211;            //  ��ȡ������������Ϣ, �᳤�����᳤
        public const uint S2C_FAMILY_JION_RESULT = 213;         //  ��ȡ�������ɹ�
        public const uint S2C_FAMILY_LEAVE_RES = 214;           //  ��ȡ�˳�������Ϣ
        public const uint S2C_FAMILY_DONATE_RES = 215;          //  ��ȡ������Ϣ
        public const uint S2C_FAMILY_CHANGE_POST_RES = 217;     //  ��ȡְλ�����Ϣ
        public const uint S2C_FAMILY_CHANGE_LOGO_RES = 218;     //  ��ȡ��־�����Ϣ
        public const uint S2C_FAMILY_INFO_RES = 219;            //  ��ȡ������Ϣ��Դ
        public const uint S2C_FAMILY_RECOMMEND = 220;           //  ��ȡ�Ƽ��б�
        public const uint S2C_FAMILY_SEACH_RES = 221;           //  ��ȡ��Ѱ���
        public const uint S2C_FAMILY_SKKILL_UP_RES = 222;       //  ��ȡ����������Ϣ
        public const uint S2C_FAMILY_MEMBER_RES = 223;          //  ��ȡ��Ա��Ϣ
        public const uint S2C_FAMILY_JION_REQ_RES = 225;        //  ��ȡ���������
        public const uint S2C_FAMILY_DISSOLVE = 226;            //  ��ȡ��ɢ��Ϣ
        public const uint S2S_FAMILY_UNDISSOLVE = 227;          //  ���ȡ����ɢ��Ϣ
        public const uint S2C_FAMILY_TREASURE = 253;            //  ���屦��
        public const uint S2C_FAMILY_ERROR = 252;               //  ������Ϣ
        //  C2S
        public const uint C2S_FAMILY_CREATE = 210;              //  ��������
        public const uint C2S_FAMILY_JOIN = 211;                //  �������
        public const uint C2S_FAMILY_APROV = 212;               //  ��������
        public const uint C2S_FAMILY_LEAVE = 213;               //  �뿪����
        public const uint C2S_FAMILY_DONATE = 214;              //  �������
        public const uint C2S_FAMILY_DISSOLUTION = 215;         //  �����ɢ
        public const uint C2S_FAMILY_UNDISSOLUTION = 220;       //  ȡ����ɢ
        public const uint C2S_FAMILY_CHANGE_POST = 216;         //  ���ְ��
        public const uint C2S_FAMILY_KICK = 217;                //  �߳�����
        public const uint C2S_FAMILY_CHANGE_LEADER = 218;       //  ����峤
        public const uint C2S_FAMILY_CHANGE_INFO = 219;         //  ���Ĺ��桢���뷽ʽ
        public const uint C2S_FAMILY_GET_INFO = 221;            //  ��ȡ������Ϣ
        public const uint C2S_FAMILY_IMPEACH_LEADER = 222;      //  �����峤
        public const uint C2S_FAMILY_SEACH = 223;               //  ���Ҽ���
        public const uint C2S_FAMILY_CHANGE_LOGO = 224;         //  ����LOGO
        public const uint C2S_FAMILY_GET_MEMBERS = 225;         //  ��Ա�б�
        public const uint C2S_FAMILY_SKILL_UP = 226;            //  ��������
        public const uint C2S_FAMILY_SEND_MAIL = 227;           //  �����ʼ�


        public const uint C2S_BEGIN_COLLECT = 108;//
        public const uint C2S_STOP_COLLECT = 109;//

        //  ���䳡
        //  S2C
        //  ���䳡�ܻ�ȡ��ǩ :
        public const uint S2C_AREAN_TOTAL_RES = 37;

        //  C2S
        //  ���䳡�������ǩ : 
        public const uint C2S_AREAN_TOTAL = 37;

        //117//  PKG_NAME.C2S_GET_LVLMIS_RES
        public const uint C2S_BUY_ENERGY = 248;

        //�޸�����
        //S2C
        public const uint S2C_MODIFY_NAME_RES = 208;
        //C2S
        public const uint C2S_MODIFY_NAME = 208;

        //�������
        public const uint C2S_PET = 88;
        public const uint S2C_PET_RES = 88; //������Ϣ���

        public const uint C2S_WING = 89;
        public const uint S2C_WING_RES = 89;

        //�в�ϵͳ���
        public const uint C2S_EXCHANGE = 105;
        public const uint S2C_EXCHANGE_RES = 105;

        //A3 Mail���
        public const uint C2S_GET_MAIL = 152;
        public const uint S2C_GET_MAIL_RES = 152; //�ʼ��б�

        //A3 Vip���
        public const uint C2S_VIP = 228;
        public const uint S2C_VIP_RES = 228;

        //A3 �������
        public const uint C2S_TASK = 110;
        public const uint S2C_TASK_RES = 110;

        // ������Ӣ��
        public const uint S2C_ELITE_MONSTER = 156;

        public const uint S2C_CLAN_SLAYDRAGON = 180;
        public const uint C2S_CLAN_SLAYDRAGON = 180;

        //�·��
        public const uint C2S_NEW_ACTIVE = 188;

        //�����ڳ�����
        public const uint S2C_CLAN_ESCORT = 181;
        public const uint C2S_CLAN_ESCORT = 181;
        //����Ŀ��
        public const uint SEVENDAYS = 186;

        //����ս
        public const uint S2C_CLAN_CITYWAR = 194;

        //����
        public const uint S2C_RIDE = 192;
        public const uint C2S_RIDE = 192;

        //ʱװ
        public const uint C2S_FASHIONSHOW = 197;
    }
}