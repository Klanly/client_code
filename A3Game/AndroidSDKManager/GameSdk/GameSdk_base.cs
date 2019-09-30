using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cross;
using UnityEngine;
using GameFramework;
namespace MuGame
{
    public class GameSdk_base
    {
        public virtual void Pay(rechargeData data)
        {
           
        }

        public virtual void record_createRole(Variant data)
        {

        }

        public virtual void record_login()
        {
            
        }


        public virtual void record_LvlUp()
        {

        }

        public virtual void record_quit()
        {

        }

        public virtual void sharemsg(string sharemsg, string sharetype, string shareappid, string shareappkey)
        {
            Variant v = new Variant();
            v["sharemsg"] = sharemsg;
            v["sharetype"] = sharetype;
            v["shareappid"] = shareappid;
            v["shareappkey"] = shareappkey;

            string LanPayInfoJsonString = JsonManager.VariantToString(v);
            AnyPlotformSDK.Call_Cmd("gameShare", "lanShare", LanPayInfoJsonString, false);
        }

        public virtual void record_selectionSever()
        {
            if (Globle.Lan != "zh_cn")
                return;

            Variant v = new Variant();
            v["serverId"] = Globle.curServerD.sid;
            v["serverIds"] = Globle.curServerD.sids;


            string serverInfoJsonString = JsonManager.VariantToString(v);
            AnyPlotformSDK.Call_Cmd("selectServer", "lanServer", serverInfoJsonString, false);

            debug.Log("[record]selectionSever:" + serverInfoJsonString);
        }


        public string m_voice_url = "http://10.1.8.66/upload";
        bool recordingVoice = false;
        public virtual bool beginVoiceRecord()
        {
            if (recordingVoice)
                return false;

            AnyPlotformSDK.Call_Cmd("startRecord", null, null, false);
            recordingVoice = true;
            return true;
        }
        public virtual void showbalance()
        {
            AnyPlotformSDK.Call_Cmd("showbalance", null, null, false);
        }

        public Action<string, string, int> voiceRecordHanlde = (string str, string path, int sec) =>
        {
            debug.Log(".....endVoiceRecord sendHanlde:" + str);
            //if (str == "end")
            //{
            //    GameSdkMgr.playVoice(path);
            //}
        };




        public Action<string> voicePlayedHanlde = (string state) =>
        {
            debug.Log(".....endVoiceRecord loadHanlde:" + state);

        };

        public virtual void cancelVoiceRecord()
        {

            if (!recordingVoice)
                return;

            recordingVoice = false;
            connInfo info = NetClient.instance.getObject(OBJECT_NAME.DATA_CONN) as connInfo;
            Variant v = new Variant();
            v["sid"] = Globle.curServerD.sid;

            v["platid"] = Globle.YR_srvlists__platuid;
            v["uid"] = PlayerModel.getInstance().uid;
            v["token"] = info.token == "" ? "76b03211848f7db9b922a39fbe1d1978_2015-09-26 15:11:20-100000503" : info.token;
            v["url"] = "";
            string voicejsonString = JsonManager.VariantToString(v);
            AnyPlotformSDK.Call_Cmd("finishRecord", "lanVoice", voicejsonString, false);

        }

        public virtual void endVoiceRecord()
        {
            if (!recordingVoice)
                return;

            recordingVoice = false;
            connInfo info = NetClient.instance.getObject(OBJECT_NAME.DATA_CONN) as connInfo;
            Variant v = new Variant();
            v["sid"] = Globle.curServerD.sid;

            v["platid"] = Globle.YR_srvlists__platuid;
            v["uid"] = PlayerModel.getInstance().uid;
            v["token"] = info.token == "" ? "76b03211848f7db9b922a39fbe1d1978_2015-09-26 15:11:20-100000503" : info.token;
            v["url"] = m_voice_url;
            string voicejsonString = JsonManager.VariantToString(v);
            AnyPlotformSDK.Call_Cmd("finishRecord", "lanVoice", voicejsonString, false);

            // recordingVoice = false;
        }

        public virtual void playVoice(string path)
        {
            connInfo info = NetClient.instance.getObject(OBJECT_NAME.DATA_CONN) as connInfo;
            Variant v = new Variant();
            v["sid"] = Globle.curServerD.sid;

            v["platid"] = Globle.YR_srvlists__platuid;
            v["uid"] = PlayerModel.getInstance().uid;
            v["token"] = info.token == "" ? "76b03211848f7db9b922a39fbe1d1978_2015-09-26 15:11:20-100000503" : info.token;
            v["url"] = path;
            string voicejsonString = JsonManager.VariantToString(v);
            AnyPlotformSDK.Call_Cmd("playVoice", "lanVoice", voicejsonString, false);
        }

        public virtual void stopVoice(string path)
        {
            connInfo info = NetClient.instance.getObject(OBJECT_NAME.DATA_CONN) as connInfo;
            Variant v = new Variant();
            v["sid"] = Globle.curServerD.sid;

            v["platid"] = Globle.YR_srvlists__platuid;
            v["uid"] = PlayerModel.getInstance().uid;
            v["token"] = info.token == "" ? "76b03211848f7db9b922a39fbe1d1978_2015-09-26 15:11:20-100000503" : info.token;
            v["url"] = path;
            string voicejsonString = JsonManager.VariantToString(v);
            AnyPlotformSDK.Call_Cmd("stopVoice", "lanVoice", voicejsonString, false);
        }

        public virtual void clearVoices()
        {
            AnyPlotformSDK.Call_Cmd("deleteAllVoice", null, null, false);
        }
    }
}
