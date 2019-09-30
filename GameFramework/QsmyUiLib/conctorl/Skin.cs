using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
namespace GameFramework
{
    public class Skin
    {
        public object data;
        public Transform __mainTrans;
        public RectTransform recTransform;
        public Skin(Transform trans)
        {
            __mainTrans = trans;
            recTransform = trans.GetComponent<RectTransform>();
        }


        public Vector3 pos
        {
            get { return recTransform.position; }
            set { recTransform.position = value; }
        }

        public Transform getTransformByPath(string path)
        {
            string[] arr = path.Split(new char[] { '.' });
            Transform trans = this.__mainTrans;
            for (int i = 0; i < arr.Length; i++)
            {
                trans = trans.FindChild(arr[i]);
            }
            return trans;
        }

        public GameObject getGameObjectByPath(string path)
        {
            return getTransformByPath(path).gameObject;
        }

        public Button getButtonByPath(string path)
        {
            return getComponentByPath<Button>(path);
        }

        public GameObject gameObject
        {
            get { return __mainTrans.gameObject; }
        }

        public Transform transform
        {
            get { return __mainTrans.transform; }
        }

        public void setPerent(Transform p)
        {
            __mainTrans.transform.SetParent(p,false);
        }


        protected bool __visiable = true;
        virtual public bool visiable
        {
            get { return __visiable; }
            set { if (__mainTrans.gameObject.active == value)return; __visiable = value; __mainTrans.gameObject.SetActive(value); }
        }

        public T getComponentByPath<T>(string path) where T : Component
        {
            Transform trans = this.__mainTrans;
          
                string[] arr = path.Split(new char[] { '.' });
                for (int i = 0; i < arr.Length; i++)
                {
                    trans = trans.Find(arr[i]);
                }
          
           
            return trans.GetComponent<T>();

        }

        public EventTriggerListener getEventTrigerByPath(string path = "")
        {
            if (path == "")
                return EventTriggerListener.Get(__mainTrans.gameObject);
            else
                return EventTriggerListener.Get(getGameObjectByPath(path));
        }



        public void clearListenersPath(string path = "")
        {
            GameObject go;
            if (path == "")
                go = __mainTrans.gameObject;
            else
                go = getGameObjectByPath(path);
            EventTriggerListener.Get(go).clearAllListener();
        }

        public void destoryGo()
        {
            GameObject.Destroy(__mainTrans.gameObject);
            __mainTrans = null;
            recTransform = null; 
        }

    }
}
