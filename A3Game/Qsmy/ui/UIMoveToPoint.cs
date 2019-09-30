using DG.Tweening;
using GameFramework;
using System;
using UnityEngine;

namespace MuGame
{
    class UIMoveToPoint : MonoBehaviour
    {

        public Transform targetUI;
        Baselayer uiLayer;
        GameObject clone;
        Tweener tw;
        Tweener ss;
        [SerializeField]
        public Ease ease;
        [SerializeField]
        public float dutime;
        Action onend;
        public Vector3 endscale = Vector3.one;

        static public UIMoveToPoint Get(GameObject go)
        {
            if (go == null)
            {
                Debug.LogError("EventTriggerListener_go_is_NULL");
                return null;
            }
            else
            {
                UIMoveToPoint listener = go.GetComponent<UIMoveToPoint>();
                if (listener == null) listener = go.AddComponent<UIMoveToPoint>();
                return listener;
            }
        }

        public void Move(Transform target,Action endevent = null)
        {
            targetUI = target;
            onend = endevent;
            Move();
        }

        [ContextMenu("Move")]
        public void Move()
        {
            if (targetUI == null) return;
            Kill();
            clone = GameObject.Instantiate(gameObject);
            clone.transform.SetParent(uiLayer.transform);
            clone.transform.position = transform.position;
            clone.transform.localScale = Vector3.one;
            clone.transform.SetAsLastSibling();
            tw = clone.transform.DOMove(targetUI.position, dutime);
            if (endscale != Vector3.one)
            {
               ss = clone.transform.DOScale(endscale, dutime);
               ss.SetEase(Ease.OutCubic);
            }
            tw.SetEase(ease);
            tw.OnKill<Tweener>(() =>
            {
                if (clone != null)
                    Destroy(clone);
            });
           
            tw.OnComplete<Tweener>(() =>
            {
                if (onend != null)
                    onend();
            });

        }

        public void Kill()
        {
            if (tw != null) tw.Kill(true);
            if (ss != null) ss.Kill();
        }

            void Awake()
        {
            uiLayer = transform.GetComponentInParent<Baselayer>();
        }

        void OnDisable()
        {
            Kill();
        }

    }
}
