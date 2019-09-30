using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cross;
using GameFramework;
using MuGame;
using System.Collections;
using UnityEngine;


namespace MuGame
{
    class a3_counterpartModel : ModelBase<a3_counterpartModel>
    {
        private Dictionary<uint, FbInfo> dicFBEnterPos=new Dictionary<uint, FbInfo>();
        public a3_counterpartModel()
        {
            //SvrMapConfig.instance._mapConfs;
            Dictionary<uint, Variant> mapInfo = SvrMapConfig.instance._mapConfs;
            List<uint> idx = mapInfo.Keys.ToList();
            for (int _i = 0; _i < idx.Count; _i++)
            {
                uint i = idx[_i];
                if (mapInfo[i].ContainsKey("fb"))
                {
                    List<Variant> lstFb = mapInfo[i]["fb"]._arr;
                    for (int j = 0; j < lstFb.Count; j++)
                        if (lstFb[j].ContainsKey("fb_id"))
                        {
                            Vector3 entPos = new Vector3(lstFb[j]["ux"], 0, lstFb[j]["uz"]);
                            dicFBEnterPos[lstFb[j]["fb_id"]._uint] = new FbInfo { pos = entPos, mapId = mapInfo[i]["id"] };
                        }
                }
            }
        }
        public Vector3 GetPosByLevelId(uint id)
        {
            if (dicFBEnterPos.ContainsKey(id))
                return dicFBEnterPos[id].pos;
            else return Vector3.zero;
        }
        public int GetMapIdByLevelId(uint id)
        {
            if (dicFBEnterPos.ContainsKey(id))
                return dicFBEnterPos[id].mapId;
            else return 0;
        }

        public int GetLevelLimitZhuanByLevelId(uint levelId)
        {
            Variant levelData = SvrLevelConfig.instacne.get_level_data(levelId);
            if (levelData.ContainsKey("limit_zhuan"))
                return levelData["limit_zhuan"];
            return 0;
        }
        
        public int GetLevelLimitLevelByLevelId(uint levelId)
        {
            Variant levelData = SvrLevelConfig.instacne.get_level_data(levelId);
            if (levelData.ContainsKey("limit_lvl"))
                return levelData["limit_lvl"];
            return 0;
        }
    }
    public class FbInfo
    {
        public Vector3 pos;
        public int mapId;
    }

}
