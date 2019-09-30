using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
namespace MuGame
{
    class CoroutineMgr : MonoBehaviour
    {
        bool runing = false;
        public delegate IEnumerator objDeleget1(System.Object prama);
        public void addCoroutine(objDeleget1 act, System.Object prama)
        {
            CoroDta dta = new CoroDta();
            dta.obj = act;
            dta.prama = prama;
            cache.Add(dta);
            if (!runing)
                StartCoroutine(main());
        }

        List<CoroDta> cache = new List<CoroDta>();
        IEnumerator main()
        {
            runing = true;

            while (cache.Count > 0)
            {
                Debug.Log(":::::::::::::::runingCoroutineMgr::");
                CoroDta obj = cache[0];
                cache.RemoveAt(0);
                yield return StartCoroutine(obj.obj(obj.prama));

            }
            runing = false;
        }




        static CoroutineMgr _instance;
        public static CoroutineMgr getInstacne()
        {
            if (_instance == null)
            {
                GameObject go = GameObject.Find("GameManager");

                _instance = go.AddComponent<CoroutineMgr>();
            }
            return _instance;
        }


    }

    class CoroDta
    {
        public CoroutineMgr.objDeleget1 obj;
        public System.Object prama;
    }
}
