using Cross;
using UnityEngine;

namespace MuGame
{
    public class AnyPlotformSDK
    {
        static private bool s_LoadABRes_OK = false;
        static public void LoadAB_Res()
        {
            if (s_LoadABRes_OK) return;
            s_LoadABRes_OK = true;

            GameObject obj_la = new GameObject("LoadAsync");
            SceneFXMgr loading_script = obj_la.AddComponent<SceneFXMgr>();


            //ConnectionImpl.s_bShowNetLog = true;

            BaseRole.TEMP_SHADOW = GAMEAPI.ABFight_LoadPrefab("FX_monsterSFX_com_monster_fx_com_shader");

            ProfessionRole.ROLE_LVUP_FX = GAMEAPI.ABFight_LoadPrefab("FX_comFX_FX_common_shengji");

            P2Warrior_Event.WARRIOR_B1 = GAMEAPI.ABFight_LoadPrefab("bullet_warrior_SFX_9_1");
            P3Mage_Event.MAGE_B1 = GAMEAPI.ABFight_LoadPrefab("bullet_mage_bt1_b1");
            P3Mage_Event.MAGE_B2 = GAMEAPI.ABFight_LoadPrefab("bullet_mage_bt1_b2");
            P3Mage_Event.MAGE_B3 = GAMEAPI.ABFight_LoadPrefab("bullet_mage_bt1_b3");
            P3Mage_Event.MAGE_B4_1 = GAMEAPI.ABFight_LoadPrefab("bullet_mage_bt1_b4_1");
            P3Mage_Event.MAGE_B4_2 = GAMEAPI.ABFight_LoadPrefab("bullet_mage_bt1_b4_2");
            P3Mage_Event.MAGE_B6 = GAMEAPI.ABFight_LoadPrefab("bullet_mage_bt1_b6");
            P3Mage_Event.MAGE_S3002 = GAMEAPI.ABFight_LoadPrefab("FX_mage_SFX_31");
            P3Mage_Event.MAGE_S3011 = GAMEAPI.ABFight_LoadPrefab("FX_mage_SFX_3011_heiqiu");
            P3Mage_Event.MAGE_S3011_1 = GAMEAPI.ABFight_LoadPrefab("FX_mage_SFX_3011_baodian");
            P5Assassin_Event.ASSASSIN_S1 = GAMEAPI.ABFight_LoadPrefab("FX_assa_SFX_11_4");
            P5Assassin_Event.ASSASSIN_S2 = GAMEAPI.ABFight_LoadPrefab("FX_assa_SFX_11_2");

            M0x000_Role_Event.MAGE_B1 = P3Mage_Event.MAGE_B1;
            M0x000_Role_Event.MAGE_B2 = P3Mage_Event.MAGE_B2;
            M0x000_Role_Event.MAGE_B3 = P3Mage_Event.MAGE_B3;
            M0x000_Role_Event.MAGE_B4_1 = P3Mage_Event.MAGE_B4_1;
            M0x000_Role_Event.MAGE_B4_2 = P3Mage_Event.MAGE_B4_2;
            M0x000_Role_Event.MAGE_B6 = P3Mage_Event.MAGE_B6;
            M0x000_Role_Event.MAGE_S3002 = P3Mage_Event.MAGE_S3002;
            M0x000_Role_Event.MAGE_S3011 = P3Mage_Event.MAGE_S3011;
            M0x000_Role_Event.MAGE_S3011_1 = P3Mage_Event.MAGE_S3011_1;
            M0x000_Role_Event.WARRIOR_B1 = P2Warrior_Event.WARRIOR_B1;
            M0x000_Role_Event.ASSASSIN_S1 = P5Assassin_Event.ASSASSIN_S1;
            M0x000_Role_Event.ASSASSIN_S2 = P5Assassin_Event.ASSASSIN_S2;


            P2Warrior.WARRIOR_SFX1 = GAMEAPI.ABFight_LoadPrefab("FX_warrior_SFX_4");
            P2Warrior.WARRIOR_SFX2 = GAMEAPI.ABFight_LoadPrefab("FX_warrior_SFX_101");
            P2Warrior.WARRIOR_SFX3 = GAMEAPI.ABFight_LoadPrefab("FX_warrior_SFX_9");
            P2Warrior.WARRIOR_SFX4 = GAMEAPI.ABFight_LoadPrefab("FX_warrior_SFX_9_1");
            P2Warrior.WARRIOR_SFX5 = GAMEAPI.ABFight_LoadPrefab("FX_warrior_SFX_12");
            ohterP2Warrior.WARRIOR_SFX1 = P2Warrior.WARRIOR_SFX1;
            ohterP2Warrior.WARRIOR_SFX2 = P2Warrior.WARRIOR_SFX2;
            ohterP2Warrior.WARRIOR_SFX3 = P2Warrior.WARRIOR_SFX3;
            ohterP2Warrior.WARRIOR_SFX4 = P2Warrior.WARRIOR_SFX4;
            ohterP2Warrior.WARRIOR_SFX5 = P2Warrior.WARRIOR_SFX5;
            P3Mage.P3MAGE_SFX1 = GAMEAPI.ABFight_LoadPrefab("FX_mage_FX_mage_buff_dun");
            P3Mage.P3MAGE_SFX2 = GAMEAPI.ABFight_LoadPrefab("bullet_mage_bt1_s3003");
            P3Mage.P3MAGE_SFX3 = GAMEAPI.ABFight_LoadPrefab("FX_mage_SFX_30081");
            ohterP3Mage.P3MAGE_SFX1 = P3Mage.P3MAGE_SFX1;
            ohterP3Mage.P3MAGE_SFX2 = P3Mage.P3MAGE_SFX2;
            ohterP3Mage.P3MAGE_SFX3 = P3Mage.P3MAGE_SFX3;
            P5Assassin.ASSASSIN_SFX1 = GAMEAPI.ABFight_LoadPrefab("FX_assa_SFX_5");
            P5Assassin.ASSASSIN_SFX2 = GAMEAPI.ABFight_LoadPrefab("FX_assa_SFX_9_1");
            P5Assassin.ASSASSIN_SFX3 = GAMEAPI.ABFight_LoadPrefab("FX_assa_SFX_12_1");
            ohterP5Assassin.ASSASSIN_SFX1 = P5Assassin.ASSASSIN_SFX1;
            ohterP5Assassin.ASSASSIN_SFX2 = P5Assassin.ASSASSIN_SFX2;
            ohterP5Assassin.ASSASSIN_SFX3 = P5Assassin.ASSASSIN_SFX3;

            worldmap.EFFECT_CHUANSONG1 = GAMEAPI.ABFight_LoadPrefab("FX_comFX_fx_player_cs_FX_com_player_cszhong");
            worldmap.EFFECT_CHUANSONG2 = GAMEAPI.ABFight_LoadPrefab("FX_comFX_fx_player_cs_FX_com_player_chuxian");
        }


        static private IPlotformSDK _inst = new PlotformBaseSDK();
        static public void InitSDK()
        {
            if (Application.platform == RuntimePlatform.IPhonePlayer)
            {
                _inst = new IOSPlatformSDK();
            }
            else if (Application.platform == RuntimePlatform.Android)
            {
                debug.Log("InitSDK");
                _inst = new AndroidPlotformSDK();
            }
        }

        static public int isInited
        {
            get
            {
                return _inst.isinited;
            }
        }

        static public void FrameMove()
        {
            _inst.FrameMove();
        }

        public static void Add_moreCmdInfo(string info, string jstr)
        {
            _inst.Add_moreCmdInfo(info, jstr);
        }

        public static void Call_Cmd(string cmd, string info = null, string jstr = null, bool waiting = true)
        {
            _inst.Call_Cmd(cmd, info, jstr, waiting);
        }

        public static void Cmd_CallBack(Variant v)
        {
            _inst.Cmd_CallBack(v);
        }
    }
}