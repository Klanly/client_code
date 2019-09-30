//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using UnityEngine;
//using MuGame;

//namespace MuGame.role.monster
//{
//    class M10005 : MonsterRole
//    {
//        public override void Init(string prefab_path, int layer, Vector3 pos, float roatate = 0)
//        {
//            m_fNavStoppingDis = 2f;

//            base.Init(prefab_path, layer, pos, roatate);

//            maxHp = curhp = 1000;
//        }
//    }

//    override protected void Model_Loaded_Over()
//    {
//        M00000_Default_Event mde = m_curModel.gameObject.AddComponent<M00000_Default_Event>();
//        mde.m_monRole = this;
//    }
//}
