using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Cross;
namespace MuGame
{

    class FightAniUserTempSC : FightAniTempSC
    {
        public Action<GameObject, int> onAttackBeginHanle;
        public Action<GameObject, int> onAttackPointHandle;
        public Action<float, int, float> onAttackShakeHandle;

        // public static FightAniUserTempSC instance;
        public static GameObject goUser;
        public uint iid;
        void Start()
        {
            AttackPointMgr.init();
            //onAttackPointHandle = AttackPointMgr.instacne.onAttackHanle;
            //onAttackBeginHanle = AttackPointMgr.instacne.onAttackBeginHanle;
            onAttackShakeHandle = AttackPointMgr.instacne.onAttackShake;
            if (gameObject.name == "player(Clone)")
                goUser = this.gameObject;
            Object3DBehaviour obj = goUser.transform.parent.GetComponent<Object3DBehaviour>();
        }

        public Vector3 getUserPos()
        {
            return this.transform.position;
        }

        public void onAttackBegin(int num)
        {
            if (onAttackBeginHanle != null)
                onAttackBeginHanle(gameObject, num);
        }

        public void onAttackPoint(int skillid)
        {
            if (onAttackPointHandle != null)
                onAttackPointHandle(gameObject, skillid);

        }

        public void onAttackShake_time_num_str(string pram)
        {
            if (onAttackShakeHandle != null)
            {

                string[] arr = pram.Split(new char[] { ',' });
                onAttackShakeHandle(float.Parse(arr[0]), int.Parse(arr[1]), float.Parse(arr[2]));
            }

        }



        public void onAttack_sound(int id)
        {
            MediaClient.instance.PlaySoundUrl("media/skill/" + id, false, null);
        }
    }
}
