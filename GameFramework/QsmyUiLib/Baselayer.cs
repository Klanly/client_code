using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;
using Cross;
using DG.Tweening;
namespace GameFramework
{

    public class Baselayer : MonoBehaviour
    {
        public const int LAYER_TYPE_WINDOW = 0;
        public const int LAYER_TYPE_FLOATUI = 1;
        public const int LAYER_TYPE_LOADING = 2;
        public const int LAYER_TYPE_STORY = 3;
        public const int LAYER_TYPE_FIGHT_TEXT = 4;
        public const int LAYER_TYPE_WINDOW_3D = 5;
        public const int LAYER_TYPE_GAMEJOY = 6; //游戏中用户输入的UI

        public string uiName;
        public ArrayList uiData;

        public bool isFunctionBar = false;

        public static RectTransform cemaraRectTran;

        protected GameObject goBg;

        static private float _uiwidth = -1;
        static public float uiWidth
        {
            get
            {
                if (_uiwidth < 0)
                {
                    if (cemaraRectTran == null)
                    {
                        cemaraRectTran = GameObject.Find("Canvas_overlay").GetComponent<RectTransform>();
                        _uiwidth = cemaraRectTran.rect.width;
                    }
                }
                return _uiwidth;
            }
        }
        static private float _uiHeight = -1;
        static public float uiHeight
        {
            get
            {
                if (_uiHeight < 0)
                {
                    if (cemaraRectTran == null)
                        cemaraRectTran = GameObject.Find("Canvas_overlay").GetComponent<RectTransform>();
                    _uiHeight = cemaraRectTran.rect.height;
                }
                return _uiHeight;
            }
        }


//        public static int scaleWidth = 0;
//        public static int scaleHeight = 0;
//        public static void setDesignContentScale(int designWidth = 1136, int designHeight = 640)
//        {
//#if UNITY_ANDROID
//      if (scaleWidth == 0 && scaleHeight == 0)
//        {
//            int width = Screen.currentResolution.width;
//            int height = Screen.currentResolution.height;

//            float s1 = (float)designWidth / (float)designHeight;
//            float s2 = (float)width / (float)height;
//            if (s1 < s2)
//            {
//                designWidth = (int)Mathf.FloorToInt(designHeight * s2);
//            }
//            else if (s1 > s2)
//            {
//                designHeight = (int)Mathf.FloorToInt(designWidth / s2);
//            }
//            float contentScale = (float)designWidth / (float)width;
//            if (contentScale < 1.0f)
//            {
//                scaleWidth = designWidth;
//                scaleHeight = designHeight;
//            }
//        }
//        if (scaleWidth > 0 && scaleHeight > 0)
//        {
//            if (scaleWidth % 2 == 0)
//            {
//                scaleWidth += 1;
//            }
//            else
//            {
//                scaleWidth -= 1;
//            }
//            Screen.SetResolution(scaleWidth, scaleHeight, true);
//                g_debug.Log("::: Screen.SetResolution:" + scaleWidth+"  "+ scaleHeight);
//        }
//#endif
//        }


        static private float _halfuiWidth = -1;
        static private float _halfuiHeight = -1;
        public static float halfuiWidth
        {
            get
            {
                if (_halfuiWidth < 0)
                    _halfuiWidth = uiWidth / 2f;
                return _halfuiWidth;
            }

        }
        public static float halfuiHeight
        {
            get
            {
                if (_halfuiHeight < 0)
                    _halfuiHeight = uiHeight / 2f;
                return _halfuiHeight;
            }
        }


        private static float _uiRatio = 0;
        static public float uiRatio
        {
            get
            {
                if (_uiRatio == 0)
                {
                    RectTransform rectTrans = GameObject.Find("canvas_main").GetComponent<RectTransform>();

                    _uiRatio = rectTrans.localScale.x; ;
                }
                return _uiRatio;
            }
        }


        public void removeAllChild(Transform trans)
        {
            for (int i = 0; i < trans.childCount; i++)
            {
                Destroy(trans.GetChild(i).gameObject);
            }
        }

        virtual public float type
        {
            get
            {
                return LAYER_TYPE_WINDOW;
            }
        }

        void Start()
        {
            init();
            // onShowed();
            __open(false);
        }

        virtual public void init()
        {

        }



        virtual public void onShowed()
        {
            UiEventCenter.getInstance().onWinOpen(uiName);
        }

        virtual public void onClosed()
        {
            UiEventCenter.getInstance().onWinClosed(uiName);
        }

        virtual public void onAfterShow()
        {

        }

        virtual public void dispose()
        {
            Destroy(this.gameObject);
        }

        public Transform getTransformByPath(string path)
        {

            return transform.FindChild(path);
        }

        public GameObject getGameObjectByPath(string path)
        {
            return getTransformByPath(path).gameObject;
        }

        public Button getButtonByPath(string path)
        {
            return getComponentByPath<Button>(path);
        }

        public T getComponentByPath<T>(string path) where T : Component
        {

            return transform.FindChild(path).GetComponent<T>();

        }

        public EventTriggerListener getEventTrigerByPath(string path)
        {
            return EventTriggerListener.Get(getGameObjectByPath(path));
        }

        public void clearListenersPath(string path)
        {
            GameObject go = getGameObjectByPath(path);
            EventTriggerListener.Get(go).clearAllListener();
            Destroy(go);
        }

        virtual public bool showBG
        {
            get { return false; }
        }

        virtual public bool openAni
        {
            get { return false; }
        }

        virtual public void doShowAni()
        {

            transform.localScale = new UnityEngine.Vector3(.5f, .5f, .5f);
            transform.DOScale(1f, .3f).SetEase(Ease.OutBack).OnComplete(onAfterShow);

        }

        public void __open(bool setactive = true)
        {
            if (setactive)
                gameObject.SetActive(true);
            onShowed();
            if (openAni)
            {
                doShowAni();
            }
            else
            {
                onAfterShow();
            }
        }

        public void __close()
        {
            onClosed();
            if (openAni)
            {
                doCloseAni();
            }
            else
            {
                onCLoseAniOver();
            }
        }


        virtual public void doCloseAni()
        {
            transform.DOScale(.7f, .3f).SetEase(Ease.InBack).OnComplete(onCLoseAniOver);
        }


        public void onCLoseAniOver()
        {
            // onClosed();
            gameObject.SetActive(false);
        }

        public void addBg()
        {
            if (!showBG)
                return;

            goBg = new GameObject("ig_bg_bg");
            Image bg = goBg.AddComponent<Image>();

            RectTransform cv = cemaraRectTran;
            goBg.transform.GetComponent<RectTransform>().sizeDelta = new Vector2(cv.rect.width * 2, cv.rect.height * 2);
            bg.color = new Color(0, 0, 0, 0.5f);
            goBg.transform.SetParent(transform, false);
            goBg.transform.SetSiblingIndex(0);
        }

        //private static float anchoredX
        //{

        //}



        protected void alain()
        {
            gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(Baselayer.uiWidth, Baselayer.uiHeight);

            //return;
            //if(cemaraRectTran==null)
            //    cemaraRectTran = GameObject.Find("canvas_main").GetComponent<RectTransform>();

            //RectTransform t = GetComponent<RectTransform>();
            //float x = t.anchoredPosition.x;
            //float y = t.anchoredPosition.y;

            //RectTransform cv = cemaraRectTran;

            //if (h == 0)
            //{
            //    x = (960 - cv.rect.width)/2;
            //}
            //else if (h == 1)
            //{
            //    x = (cv.rect.width - 960)/2;
            //}


            //if (v == 0)
            //{
            //    y = (640 - cv.rect.height) / 2;

            //}
            //else if (v == 1)
            //{
            //    y = (cv.rect.height - 640) / 2;
            //}

            //t.anchoredPosition = new Vector2(x, y);
        }
    }
}
