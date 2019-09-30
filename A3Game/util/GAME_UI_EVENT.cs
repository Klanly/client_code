namespace MuGame
{
    //1 - 1000Ϊ��Ϣ�����¼�
    //1001-4000 Ϊ��Ϸ�¼�
    //4001-6000 Ϊ���洦���¼�
    public class UI_EVENT
    {
        //����㲥�¼�
        /// <summary>
        /// ��ָ������
        /// </summary>
        public const uint UI_OPEN = 4001;
        /// <summary>
        ///�ر�ָ������
        /// </summary>
        public const uint UI_CLOSE = 4002;
        /// <summary>
        ///���򿪹ؽ���
        /// </summary>
        public const uint UI_OPEN_SWITCH = 4003;
        /// <summary>
        /// ���Ĵ�������ĳ���
        /// </summary>
        public const uint UI_DIPOSE_CREAT = 4004;
        /// <summary>
        /// ѡ�д�����ɫ
        /// </summary>
        public const uint UI_SELECT_CHAR = 4005;

		/// <summary>
        ///ѡ������
        /// </summary>
        public const uint UI_ON_SELECT_SID = 4015;

        /// <summary>
        ///׼��������Ϸ
        /// </summary>
        public const uint UI_ON_TRY_ENTER_GAME = 4020;
        /// <summary>
        ///������ɫ
        /// </summary>
        public const uint UI_ACT_CREATE_CHAR = 4031;
        /// <summary>
        /// ɾ����ɫ
        /// </summary>
        public const uint UI_ACT_DELETE_CHAR = 4032;
        /// <summary>
        ///ѡ�н�ɫ
        /// </summary>
        public const uint UI_ACT_SELECT_CHAR = 4033;
        /// <summary>
        ///������Ϸ
        /// </summary>
        public const uint UI_ACT_ENTER_GAME = 4034;
        /// <summary>
        ///�ƶ�ҡ�� 
        /// </summary>
        public const uint UI_ACT_PLAY_MOV = 4040;
        /// <summary>
        ///ֹͣҡ��
        /// </summary>
        public const uint UI_ACT_PLAY_STOP = 4041;
        /// <summary>
        ///��������
        /// </summary>
        public const uint UI_ACT_MISSION = 4042;
        /// <summary>
        ///�ύ����
        /// </summary>
        public const uint UI_SUBMIT_MISSION = 4043;
        /// <summary>
        ///������ָ��
        /// </summary>
        public const uint UI_OPNE_MISSION_GUIDE = 4044;
        /// <summary>
        ///��ɫ���� 
        /// </summary>
        public const uint UI_HERO_ATTACK = 4045;
        /// <summary>
        ///��ɫ����
        /// </summary>
        public const uint UI_HERO_SKILL = 4046;
        /// <summary>
        ///��ɫ��Ծ 
        /// </summary>
        public const uint UI_HERO_JUMP = 4047;
        /// <summary>
        /// ɾ����ɫ�Ժ����UI
        /// </summary>
        public const uint UI_DELETE_CHARUI = 4048;
        /// <summary>
        /// �������ɫ���ֵȼ�
        /// </summary>
        public const uint UI_MAIN_TEXT = 4049;


        /// <summary>
        /// ��ȡ�̳��޹���Ϣ
        /// </summary>
        public const uint UI_SHOP_GET_HOT_ITM = 4500;
        /// <summary>
        /// �л��̳Ƿ�ҳ
        /// </summary>
        public const uint UI_SHOP_CHANGESHOP = 4501;
        /// <summary>
        /// �����ʾ������ϸ����
        /// </summary>
        public const uint UI_SHOP_SHOW = 4502;
        /// <summary>
        /// �������
        /// </summary>
        public const uint UI_SHOP_BUY = 4503;           // �����ֵ
        public const uint UI_SHOP_CHARGE = 4504;
        public const uint UI_CHAT_TALK = 4505;          // ���콲��

        //���������Ϣ
        public const uint UI_OPEN_ROLE = 4600;           //�򿪽�ɫ
        //public const uint UI_OPEN_PKPANEL = 4601;        //�򿪱���
        public const uint UI_OPEN_PACKAGE = 4601;       //�򿪱���
        public const uint UI_OPEN_SHOP = 4602;           //���̳�
        public const uint UI_OPEN_PK = 4603;             //��PK
        public const uint UI_OPEN_SMITHY = 4603;         //�򿪶��� 
        public const uint UI_OPEN_CHAT = 4604;           //������
        public const uint UI_OPEN_SET = 4605;            //������
        public const uint UI_OPEN_WORLDMAP = 4606;       //�������ͼ
        public const uint UI_OPEN_NPCFUN = 4607;       //��NPC����
        public const uint UI_OPEN_NPCMIS = 4608;       //��NPC����
        public const uint UI_OPEN_LEVELHALL = 4609;       //�򿪸�������
        public const uint UI_OPEN_CLAN = 4610;      //�򿪰���
        public const uint UI_OPEN_DONATE = 4611;    //�򿪾���
        public const uint UI_OPEN_MARKET = 4610;//������
        public const uint UI_OPEN_USEITEM_PROMPT = 4611; //��ʹ����Ʒ

        //��ɫ
        public const uint UI_SETCHARACTER_VALUE = 5001;         //��ɫLG�㵽UI����ʾ����
        public const uint UI_SETCHARACTER_CHAGEEQP = 5002;      //��ɫLG�㵽UI,�л�װ��
        public const uint UI_SETCHARACTER_RMVEQP = 5003;        //��ɫLG�㵽UI,�Ƴ�װ��
        public const uint UI_SETCHARACTER_SETW = 5004;          //��ɫLG�㵽UI,���ó������
        public const uint UI_SETCHARACTER_SETFW = 5005;          //��ɫLG�㵽UI,����ս�����
        public const uint UI_SETCHARACTER_CLEARW = 5005;          //��ɫLG�㵽UI,������
        public const uint UI_SETCHARACTER_SETWINFO = 5007;       //��ɫLG�㵽UI,����ս�����info
        public const uint UI_SETCHARACTER_SETWEQUIP = 5008;       //��ɫLG�㵽UI,����ս�����epuip
        public const uint UI_SETCHARACTER_CHGEQP = 5009;         //��ɫLG�㵽UI,װ�����
        public const uint UI_SET_EPHF = 5010;                     //SetEquipTileHandeFun
        public const uint UI_SET_OP_CK = 5010;                     //SetOpenCallBack
        public const uint UI_SETCK = 5011;                        //SetCallBack
        public const uint UI_SET_OP_CB = 5012;                    //SetOperateCB
        public const uint UI_SET_RE_CB = 5013;                    //SetRideEventCB
        public const uint UI_SET_NO_CB = 5014;                     //SetNobOpenCB
        public const uint UI_SET_NCB = 5015;                       //SetNobCallback
        public const uint UI_INIT_CF = 5016;                      //InitChainfo
        public const uint UI_ROM_3DEQP = 5017;                      //Remove3DEqps
        public const uint UI_SET_P3DSH = 5018;                      //SetPlayer3DShow
        public const uint UI_REF_SKILLLIST = 5019;               //RefreshSkillList
        public const uint UI_SET_RADIO = 5020;                   //SetRadio
        public const uint UI_SET_ONCLKIDX = 5021;               //SetOnClkIdx
        public const uint UI_SET_TILENAME = 5022;               //setTileName
        public const uint UI_ONCHAGE_EQP = 5023;                 //OnEquipChange
        public const uint UI_SET_LVL = 5024;                    //SetChainfoLvl
        public const uint UI_SET_PROFESS = 5025;               //SetChainfoProfess
        public const uint UI_SET_DINFO_CHANGE = 5026;          //SelfDetailInfoChange
        public const uint UI_REFESH_EQP = 5027;                 //RefreshEquips
        public const uint UI_CHAGE_SHOP_SS = 5028;             //changeShopSwitchStatus
        public const uint UI_SET_NAME = 5029;                  //SetChainfoName
        public const uint UI_SET_INFO = 5030;
        public const uint SET_SHOP_ITEM = 5031;
        public const uint SET_SHOP_RADIO = 5032;
        public const uint PREPARE_SHOP = 5033;
        public const uint UI_LEVELHALLINFO = 5034;
        public const uint UI_LEVELHALL = 5035;
        public const uint TILE_TRANSCRIPT = 5036;
        public const uint TILE_TRANSCRIPTINFO = 5037;
        public const uint INITTRANSCRIPTTILES = 5038;
        public const uint LGUI_LEVEL = 5039;
        public const uint LGGD_LEVEL = 5040;
        public const uint TILE_TAGRADIOBTN = 5036;

        //����㽻���¼�
        public const uint TEST_BUTTON1 = 7000;              //		 
        public const uint TEST_BUTTON2 = 7001;              //		 
        public const uint SELECT_CHAR_INIT = 7002;          //	
        /// <summary>
        ///ѡ�н�����Ϸ 
        /// </summary>
        public const uint SELECT_CHAR_ENTER_GAME = 7003;
        /// <summary>
        ///ѡ�н�ɫ 
        /// </summary>
        public const uint CHAR_SELECT = 7004;
        /// <summary>
        ///������Ϣ 
        /// </summary>
        public const uint SET_TILE_INFO = 7005;
        /// <summary>
        ///  //�����Ϣ
        /// </summary>
        public const uint CLEAR_TILE_INFO = 7006;
        /// <summary>
        /// //�޸���Ϣ
        /// </summary>
        public const uint MODIFY_TILE_INFO = 7007;
        /// <summary>
        ///��tile ��ӵ������� 
        /// </summary>
        public const uint ON_CHAR_TILE_INIT = 7008;

        /// <summary>
        ///��ʼ�б�tile ��ӵ�����
        /// </summary>
        public const uint ON_TILES_INIT = 7010;
        /// <summary>
        ///����б�tile ��ӵ����� 
        /// </summary>
        public const uint ON_TILES_ADDED = 7011;
        /// <summary>
        ///�б��¼����� 
        /// </summary>
        public const uint ON_LIST_ADD = 7012;
        /// <summary>
        ///�б�ɾ������ 
        /// </summary>
        public const uint ON_LIST_RMV = 7013;
        /// <summary>
        ///�б��޸�����
        /// </summary>
        public const uint ON_LIST_MOD = 7014;
        /// <summary>
        ///�������� 
        /// </summary>
        public const uint ON_LIST_SET_DATA = 7015;
        /// <summary>
        ///�б�tileѡ�� 
        /// </summary>
        public const uint ITEM_SELECT = 7020;

        public const uint ITEM_GET = 7021;

        public const uint CREAT_PROMPT = 7022;

        /// <summary>
        ///UIˢ����Ϣ
        /// </summary>
        public const uint ON_REFRESH_INFO = 7030;
        /// <summary>
        ///UI��ť����ı�
        /// </summary>
        public const uint ON_BUTTON_CHANGE = 7031;
        /// <summary>
        ///ѡ�б�ǩ��ť
        /// </summary>
        public const uint ON_RADIO_SELECT = 7032;
        /// <summary>
        /// ���� 
        /// </summary>
        public const uint CHAINFO_INIT = 7033;              //��ɫ�����ʼ��
        public const uint CHAINFO_TILE_INIT = 7033;         //��ɫ����������Ϣ
        public const uint TILE_SET_SHOW_DATA = 7034;         //���õ�������
        /// <summary>
        /// NPC���� 
        /// </summary>
        public const uint ON_NPCDIALOG_FIN = 7035;
        public const uint ON_NPCDIALOG_ACP = 7036;
        public const uint ON_NPCDIALOG_DOUBLE_FIN = 7037;
        public const uint ON_NPCDIALOG_CLKTILE = 7038;
        public const uint ON_NPCDIALOG_NEXT = 7039;
        public const uint TESTBASEOK = 8000;
        public const uint DIANJIBASE = 8001;

        //public const uint TESTBASEOK = 8000;
        //public const uint DIANJIBASE = 8001;
        // ��������
        public const uint ON_LOAD_ITEMS = 9001;
        public const uint ON_BUY_ITEMS = 9002;
        public const uint ON_SELL_ITEMS = 9003;
        public const uint ON_USE_ITEMS = 9004;
        public const uint ON_MONEY_CHANGE = 9005;
        public const uint ON_LV_CHANGE = 9006;
        public const uint ON_VIP_CHANGE = 9007;
        public const uint ON_EXP_CHANGE = 9008;
        public const uint ON_LOAD_GIFT_SHOP = 9009;
        public const uint ON_CHANGE_ITEM = 9010;

        //ʱװ����
        public const uint ON_LOAD_DRESS = 10001;
        public const uint ON_DRESS_UP = 10002;
        public const uint ON_FINISH_TASK = 10003;
        public const uint ON_ACTIVE = 10004;
        public const uint ON_UPGRADE = 10005;
        public const uint ON_REFESH_PROPERTY = 10006;
        //��������
        public const uint ON_LOAD_WEAPON = 12001;
        public const uint ON_CAILIAOJB_WEAPON = 12002;
        public const uint ON_CAILIAOYB_WEAPON = 12003;
        public const uint ON_WPUPGRADE_WEAPON = 12004;
        //ǩ��
        public const uint ON_LOAD_SIGN = 13001;
        public const uint ON_SIGN = 13002;
        public const uint ON_FILL_SIGN = 13003;
        //����
        public const uint GO_PUBLISH = 14001;
      //  public const uint GET_PUBLISH = 14002;
        //�ʼ�
        public const uint ON_LOAD_MAIL = 15001;
        public const uint ON_NEW_MAIL = 15002;
        public const uint ON_SEND_MAIL = 15003;
        public const uint ON_DELETE_MAIL = 15004;

        //��Ʊ
        public const uint ON_TAELS_INIT = 16001;
        public const uint ON_TAELS_CRIT = 16002;

        //���а�
        public const uint ON_GET_RANKING = 18001;

        //�齱
        public const uint ON_GETLOTTERYDRAWINFO = 19001;
        public const uint ON_STOPLOTTERYAWARD = 1902;//ֹͣ������Ʒ

		//��������
		public const uint ON_FUBENENTER = 20001;	//��������
		public const uint ON_FUBENEXIT = 20002;		//�����뿪
        
        //���ѽ���
        public const uint ON_OPENFRIENDPANEL = 17001;//�򿪺���UI
    }
}