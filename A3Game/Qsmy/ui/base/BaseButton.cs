using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine.UI;
using UnityEngine;
using GameFramework;
using DG.Tweening;
using UnityEngine.EventSystems;
using System.Collections;

namespace MuGame
{
    class BaseButton : Skin
    {
       public BtnEventTriggerListener __listener;
        private int _soundType = 1;
        private int _effectType = 1;
        Button btn;

        private float beginScale;
        private float endScale;

        public BaseButton(Transform trans, int effectType = 1, int soundType = 1)
            : base(trans)
        {
            _soundType = soundType;
            _effectType = effectType;

            beginScale = recTransform.localScale.x;
            endScale = beginScale * 0.92f;
            btn = trans.GetComponent<Button>();
            addEvent();
        }

        public bool interactable
        {

            set { if (btn!=null) btn.interactable = value; }
            get { if (btn == null)return false; return btn.interactable; }
        }

        public bool active
        {
            get { return gameObject.active; }
        }

        
        private static Action<GameObject> _handler;
        public static Action<GameObject> Handler
        {
            set
            {
                if (_handler != null)
                    _handler = value;
                else
                    _handler += value;
            }
        }
        private Action<GameObject> onClickhandle;
        public Action<GameObject> onClick
        {
            set
            {
                if (value == null && onClickhandle != null)
                {
                    if (__listener != null)
                        __listener.onClick = null;
                }

                onClickhandle = value;
                onClickhandle += _handler;
            }
            get
            {
                return onClickhandle;
            }
        }

		private Action<GameObject> onClickFalseHandle;
		public Action<GameObject> onClickFalse {
			set {
				onClickFalseHandle = value;
			}
			get {
				return onClickFalseHandle;
			}
		}

        private void doClick(GameObject go)
        {
            if (go.transform.GetComponent<Button>())
            {
                if (go.transform.GetComponent<Button>().interactable == false)
                {
					if (onClickFalseHandle != null)
						onClickFalseHandle(gameObject);
                    return;
                }
            }
            doClick();
        }

        public void doClick()
        {
            if (_soundType == 1)
            {
                //MediaClient.instance.PlaySoundUrl("media_ui_button", false, null);
                MediaClient.instance.PlaySoundUrl("audio_common_click_button", false, null);
            }
            //   debug.Log("播放音效");

            if (onClickhandle != null)
                onClickhandle(gameObject);
        }


        private void doDown(GameObject go)
        {
            if (go.transform.GetComponent<Button>())
            {
                if (go.transform.GetComponent<Button>().interactable == false)
                {
                    return;
                }
            }

            //if (_effectType == 1)
            //    recTransform.DOScale(endScale, 0.15f);
        }

        private void doUp(GameObject go)
        {
            if (go.transform.GetComponent<Button>())
            {
                if (go.transform.GetComponent<Button>().interactable == false)
                {
                    return;
                }
            }

            //if (_effectType == 1)
            //    recTransform.DOScale(beginScale, 0.2f);
        }

        public void removeAllListener()
        {
            if (__listener != null)
            {
                __listener.onDown = null;
                __listener.onUp = null;
                __listener.onExit = null;
                __listener.onClick = null;
            }
            recTransform.localScale = Vector3.one;
        }

        public void addEvent()
        {
            __listener = BtnEventTriggerListener.Get(gameObject);
            if (_effectType == 1)
            {
                __listener.onDown = doDown;
                __listener.onUp = doUp;
                __listener.onExit = doUp;
            }
            __listener.onClick = doClick;
        }

        public void dispose()
        {
            if (__listener != null)
            {
                __listener.clearAllListener();
                __listener = null;
            }

            GameObject.Destroy(gameObject);
        }        
    }
}
