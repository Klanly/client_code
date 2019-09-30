using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameFramework;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections;
using Cross;
using DG.Tweening;

namespace MuGame
{
    public class flytxt : LoadingUI
    {
        static public int COMMON_TYPE = 0;
        public static flytxt instance;

        protected int[] m_num = {0,0,0,0,0,0,0,0,0};      //缓存的数量
        protected float[] m_time = {0.2f,0.2f,0.2f,0.2f,0.4f,0.2f,0.2f,0.6f,0.2f};  //间隔时间
        protected Dictionary<int, List<string>> m_txtmap;  //缓存的txt
        protected Dictionary<int, List<GameObject>> m_gameobject;  //缓存的道具

        public override void init()
        {
            instance = this;
            transform.SetAsFirstSibling();
            m_txtmap = new Dictionary<int, List<string>>();
            m_gameobject = new Dictionary<int, List<GameObject>>();
            for (int i = 0; i < m_num.Length; i++)
            {
                List<string> txtlist = new List<string>();
                m_txtmap.Add(i, txtlist);
            }
            for (int i = 0; i < m_num.Length; i++)
            {
                List<GameObject> glist = new List<GameObject>();
                m_gameobject.Add(i, glist);
            }
        }
        public void fly(string txt,int tag = 0, Color m_color = default(Color),GameObject showIcon=null)
        {
			if (m_num[tag] > 0 && m_time[tag] > 0) {
				m_txtmap[tag].Add(txt);
                m_gameobject[tag].Add(showIcon);

            }
            m_num[tag]++;
            switch (tag) {
				case 0:
					fly0(txt);
					break;
				case 1:
					fly1(txt);
					break;
				case 2:
					fly2(txt);
					break;
				case 3://经验飘字
					fly3(txt);
					break;
				case 4://道具飘字
					fly4(txt, m_color);
					break;
                case 5://更新任务进度飘字
                    fly5(txt);
                    break;
                case 6:
                    fly6(showIcon);
                    break;
                case 7:
                    fly7(txt);
                    break;
                case 8:
                    flyTaskNotice(txt);
                    break;

			}
           
        }

        Queue<Transform> qRewardFlyTxt = new Queue<Transform>();
        public void FlyQueue(List<KeyValuePair<string,string>> showPrefab)
        {
            Queue<Transform> doMoveQueue = new Queue<Transform>();
            for (int i = 0; i < showPrefab.Count; i++)
            {
                Transform _prefab = GameObject.Instantiate(transform.FindChild(showPrefab[i].Key));
                _prefab.GetComponentInChildren<Text>().text = showPrefab[i].Value;
                qRewardFlyTxt.Enqueue(_prefab);
            }                        
            if (!IsInvoking("FlyNext"))
                InvokeRepeating("FlyNext", 0f, 0.2f);
        }

        public void FlyQueue(Transform tfPrefab)
        {
            qRewardFlyTxt.Enqueue(tfPrefab);
            if(IsInvoking("FlyNext"))
                CancelInvoke("FlyNext");
            InvokeRepeating("FlyNext", 0f, 0.2f);
        }
        //public void FlyYOneByOne(Queue<Transform> reward,float y,float time = 1.5f)
        //{
        //    Transform _prefab = reward.Dequeue();
        //    if (_prefab == null)
        //        return;

        //    _prefab.gameObject.SetActive(true);
        //    _prefab.SetParent(transform,false);
        //    _prefab.DOLocalMoveY(y, time).OnComplete(delegate() {
        //        Destroy(_prefab,0.5f);
        //        instance.FlyNext(reward, y, time);
        //    });
        //}

        //[SerializeField]
        //float _time = 1.5f;
        //public float maxYLength = 25;
        public void FlyNext()
        {
            if (qRewardFlyTxt.Count == 0)
            {
                CancelInvoke("FlyNext");
                return;
            }         
            Transform tf = qRewardFlyTxt.Dequeue();
            if (tf == null)
                return;
            tf.gameObject?.SetActive(true);
            tf.SetParent(transform,false);                
            Destroy(tf.gameObject, 0.8f);
        }

        public static void flyUseContId(string id, List<string> contPram = null, int tag = 0)
        {
            instance.fly(ContMgr.getCont(id,contPram),tag);
        }

        public override void onShowed()
        {
        }
        public override void onClosed()
        {
        }

        void timeGo_0()
        {
            if (m_txtmap[0].Count > 0)
            {
                fly0(m_txtmap[0][0]);
                m_txtmap[0].RemoveAt(0);
                m_num[0]--;
            }
        }
        void timeGo_1()
        {
            if (m_txtmap[1].Count > 0)
            {
                fly1(m_txtmap[1][0]);
                m_txtmap[1].RemoveAt(0);
                m_num[1]--;
            }
        }
        void timeGo_2()
        {
            if (m_txtmap[2].Count > 0)
            {
                fly2(m_txtmap[2][0]);
                m_txtmap[2].RemoveAt(0);
                m_num[2]--;
            }
        }
        void timeGo_3()
        {
            if (m_txtmap[3].Count > 0)
            {
                fly3(m_txtmap[3][0]);
                m_txtmap[3].RemoveAt(0);
                m_num[3]--;
            }
        }

        void timeGo_7()
        {
            if (m_txtmap[7].Count > 0)
            {
                fly3(m_txtmap[7][0]);
                m_txtmap[7].RemoveAt(0);
                m_num[7]--;
            }
        }

        string str;
        public void fly0Delay()
        {
            fly0(str);
            str = "";
        }
        public GameObject fly0(string txt)
        {
            GameObject item = transform.FindChild("txt_1").gameObject;
            GameObject txtclone = ((GameObject)GameObject.Instantiate(item));
            //txtclone.gameObject.SetActive(true);
            txtclone.transform.SetParent(transform, false);

            Image desc = txtclone.transform.GetComponent<Image>();
            Text desc_txt = txtclone.transform.FindChild("txt_1").GetComponent<Text>();
            desc_txt.text = txt;

			Tweener tween1 = desc.transform.DOLocalMoveY(-50, 1.5f).SetDelay(Mathf.Max(0, m_num[0]-1) * m_time[0]).OnStart(() => {
				if (m_txtmap[0].Count > 0) {
					m_txtmap[0].RemoveAt(0);
					m_num[0]--;
				}
				txtclone.gameObject.SetActive(true);
			}); ;
            tween1.OnComplete(delegate()
            {
                this.onEnd(txtclone);
            });

            //Tweener tween0 = desc.material.DOFade(1.0f, 0.2f);
            //Tweener tween1 = desc.transform.DOLocalMoveY(-100, 0.5f);
            //Tweener tween2 = desc.transform.DOLocalMoveY(0, 0.5f);
            //tween2.SetDelay(1.0f);
            //Tweener tween3 = desc.material.DOFade(0.0f, 0.2f);
            //tween3.SetDelay(2.0f);
            //tween3.OnComplete(delegate()
            //{
            //    this.onEnd(txtclone);
            //});
            return txtclone;
        }
        public void fly1(string txt)
        {
            GameObject item = transform.FindChild("txt_1").gameObject;
            GameObject txtclone = ((GameObject)GameObject.Instantiate(item));
            txtclone.transform.SetParent(transform, false);

            Image desc = txtclone.transform.GetComponent<Image>();
            Text desc_txt = txtclone.transform.FindChild("txt_1").GetComponent<Text>();
            desc_txt.text = txt;

			Tweener tween1 = desc.transform.DOLocalMoveY(0, 1.0f).SetDelay(Mathf.Max(0, m_num[1] - 1) * m_time[1]).OnStart(() => {
				if (m_txtmap[1].Count > 0) {
					m_txtmap[1].RemoveAt(0);
					m_num[1]--;
				}
				txtclone.gameObject.SetActive(true);
			}); 
            tween1.OnComplete(delegate()
            {
                this.onEnd(txtclone);
            });
        }
        private void onEnd(GameObject go)
        {
            Destroy(go);
        }
        public void fly2(string txt)
        {
            GameObject item = transform.FindChild("txt_1").gameObject;
            GameObject txtclone = ((GameObject)GameObject.Instantiate(item));
            txtclone.transform.SetParent(transform, false);

            Image desc = txtclone.transform.GetComponent<Image>();
            Text desc_txt = txtclone.transform.FindChild("txt_1").GetComponent<Text>();
            desc_txt.text = txt;
            desc_txt.fontSize = 30;
            desc_txt.color = Globle.COLOR_GREEN;
            txtclone.transform.FindChild("bg").gameObject.SetActive(false);

			Tweener tween1 = desc.transform.DOLocalMoveY(0, 1.0f).SetDelay(Mathf.Max(0, m_num[2] - 1) * m_time[2]).OnStart(() => {
				if (m_txtmap[2].Count > 0) {
					m_txtmap[2].RemoveAt(0);
					m_num[2]--;
				}
				txtclone.gameObject.SetActive(true);
			}); 
            tween1.OnComplete(delegate()
            {
                this.onEnd(txtclone);
            });
        }
        public void fly3(string txt)
        {
            GameObject item = transform.FindChild("txt_2").gameObject;
            GameObject txtclone = ((GameObject)GameObject.Instantiate(item));
            txtclone.transform.FindChild("txt").GetComponent<Text>().text = txt;
            txtclone.transform.SetParent(transform, false);
            //List<KeyValuePair<string,string>> temp = new List<KeyValuePair<string, string>>();
            //temp.Add(new KeyValuePair<string, string>("txt_2",txt));
            //FlyQueue(temp);
            //GameObject.Destroy(txtclone,2.5f);
            //Tweener tween1 = txtclone.transform.FindChild("txt").DOLocalMoveY(100, 2.5f).SetDelay(Mathf.Max(0, m_num[3] - 1) * m_time[3]).OnStart(() => {
            //	if (m_txtmap[3].Count > 0) {
            //		m_txtmap[3].RemoveAt(0);
            //		m_num[3]--;
            //	}
            //	txtclone.gameObject.SetActive(true);
            //}); 
            //tween1.OnComplete(delegate()
            //{
            //    this.onEnd(txtclone);
            //});

            txtclone.gameObject.SetActive(true);
            Destroy(txtclone.gameObject, 1f);
        }
		//获得奖励道具飘字
		public void fly4(string txt, Color m_color, int index = 0) {
			GameObject item = transform.FindChild("txt_3").gameObject;
			if (item == null) return;
			GameObject txtclone = ((GameObject)GameObject.Instantiate(item));
			txtclone.transform.FindChild("txt").GetComponent<Text>().text = txt;
			txtclone.transform.SetParent(transform, false);
			txtclone.transform.FindChild("txt").GetComponent<Text>().color = m_color;
			//GameObject.Destroy(txtclone,2.5f);
			Tweener tween1 = txtclone.transform.FindChild("txt").DOLocalMoveY(100, 2.5f).SetDelay(Mathf.Max(0, m_num[4] - 1) * m_time[4]).OnStart(() => {
				if (m_txtmap[4].Count > 0) {
					m_txtmap[4].RemoveAt(0);
					m_num[4]--;
				}
				txtclone.gameObject.SetActive(true);
			}); 
			tween1.OnComplete(delegate() {
				this.onEnd(txtclone);
			});
		}
        //更新任务进度飘字
        public void fly5(string txt)
        {
            GameObject item = transform.FindChild("txt_4").gameObject;
            GameObject txtclone = ((GameObject)GameObject.Instantiate(item));
            txtclone.transform.SetParent(transform, false);

            Image desc = txtclone.transform.GetComponent<Image>();
            Text desc_txt = txtclone.transform.FindChild("txt_1").GetComponent<Text>();
            desc_txt.text = txt;
            //txtclone.SetActive(true);            
            FlyQueue(txtclone.transform);
            //Tweener tween1 = desc.transform.DOLocalMoveY(0, 1.0f).SetDelay(Mathf.Max(0, m_num[5] - 1) * m_time[5]).OnStart(() => {
            //    if (m_txtmap[5].Count > 0)
            //    {
            //        m_txtmap[5].RemoveAt(0);
            //        m_num[5]--;
            //    }
            //    txtclone.gameObject.SetActive(true);
            //});
            //tween1.OnComplete(delegate ()
            //{
            //    this.onEnd(txtclone);
            //});
            Destroy(txtclone,1f);
        }
        //更新任务奖励飘字
        public void fly6(GameObject go)
        {
            GameObject item = transform.FindChild("txt_5").gameObject;
            Transform itemicon = go.transform;
            itemicon.GetComponent<RectTransform>().sizeDelta = item.GetComponent<RectTransform>().sizeDelta;            
            GameObject txtclone = ((GameObject)GameObject.Instantiate(item));
            txtclone.transform.SetParent(transform, false);
            itemicon.SetParent(txtclone.transform, false);
            //  Debug.LogError(txtclone.transform.GetComponent<RectTransform>().anchoredPosition);
            //if (txtclone.transform.GetComponent<RectTransform>().anchoredPosition.y < 0)
            //{
            //    itemicon.GetComponent<RectTransform>().localScale = new Vector2(1, 1);/* item.GetComponent<RectTransform>().localScale * 0.5f*/;
            //}
            Tweener tween0 = txtclone.transform.DOLocalMoveY(10, 0.2f).SetDelay(Mathf.Max(0, m_num[6] - 1) * m_time[6]).OnStart(() => {
               
                txtclone.gameObject.SetActive(true);
               // itemicon.GetComponent<RectTransform>().sizeDelta = item.GetComponent<RectTransform>().sizeDelta * 0.9f;
                if (m_gameobject[6].Count > 0)
                {
                    m_gameobject[6].RemoveAt(0);
                    m_num[6]--;
                }
            });
            tween0.OnComplete(delegate ()
            {
             
                   Tweener tween1 = txtclone.transform.DOLocalMoveY(-30, 0.3f).SetDelay(Mathf.Max(0, m_num[6] - 1) * m_time[6]).OnStart(() => {
                    itemicon.GetComponent<RectTransform>().sizeDelta = item.GetComponent<RectTransform>().sizeDelta * 0.5f;
                  
                });
                tween1.OnComplete(delegate () {
                    this.onEnd(txtclone);
                });
            });
           


            //Image desc = txtclone.transform.GetComponent<Image>();
           // txtclone.gameObject.SetActive(true);
            //Destroy(txtclone, 0.5f);
        }

        public void fly7(string txt)
        {
            GameObject item = transform.FindChild("txt_7").gameObject;
            GameObject txtclone = ((GameObject)GameObject.Instantiate(item));
            Text desc_txt = txtclone.transform.FindChild("txt_1").GetComponent<Text>();
            desc_txt.text = txt;
            Image desc = txtclone.transform.GetComponent<Image>();
            txtclone.transform.SetParent(transform, false);
            Tweener tween1 = desc.transform.DOLocalMoveY(200, 1f).SetDelay(Mathf.Max(0, m_num[7] - 1) * m_time[7]).OnStart(() => {
                if (m_txtmap[7].Count > 0)
                {
                    m_txtmap[7].RemoveAt(0);
                    m_num[7]--;
                }
                txtclone.gameObject.SetActive(true);
            }); ;
            tween1.OnComplete(delegate ()
            {
                this.onEnd(txtclone);
            });
        }

        public void flyTaskNotice(string txt)
        {
            GameObject _prefab = transform.FindChild("txt_task").gameObject;
            GameObject txtclone = GameObject.Instantiate(_prefab);
            Text desc_txt = txtclone.transform.FindChild("txt").GetComponent<Text>();
            desc_txt.text = txt;
            txtclone.transform.SetParent(transform, false);
            txtclone.gameObject.SetActive(true);
            Destroy(txtclone, 4f);
        }
        //public void fly4(string txt,int tag)
        //{
        //    GameObject item; 
        //    GameObject txtclone;
        //    Tweener tween1;
        //    switch (tag)
        //    {
        //        case 8: 
        //                item = transform.FindChild("txt_3_equ1").gameObject;
        //                txtclone = ((GameObject)GameObject.Instantiate(item));
        //                txtclone.gameObject.SetActive(true);
        //                txtclone.transform.FindChild("txt").GetComponent<Text>().text = txt;
        //                txtclone.transform.SetParent(transform, false);
        //                //GameObject.Destroy(txtclone,2.5f);
        //                tween1 = txtclone.transform.FindChild("txt").DOLocalMoveY(100, 2.5f);
        //                tween1.OnComplete(delegate()
        //                {
        //                    this.onEnd(txtclone);
        //                });
        //            break;
        //        case 9: 
        //                item = transform.FindChild("txt_3_equ2").gameObject;
        //                txtclone = ((GameObject)GameObject.Instantiate(item));
        //                txtclone.gameObject.SetActive(true);
        //                txtclone.transform.FindChild("txt").GetComponent<Text>().text = txt;
        //                txtclone.transform.SetParent(transform, false);
        //                //GameObject.Destroy(txtclone,2.5f);
        //                 tween1 = txtclone.transform.FindChild("txt").DOLocalMoveY(100, 2.5f);
        //                tween1.OnComplete(delegate()
        //                {
        //                    this.onEnd(txtclone);
        //                });
        //            break;
        //        case 10: 
        //                item = transform.FindChild("txt_3_equ3").gameObject;
        //                txtclone = ((GameObject)GameObject.Instantiate(item));
        //                txtclone.gameObject.SetActive(true);
        //                txtclone.transform.FindChild("txt").GetComponent<Text>().text = txt;
        //                txtclone.transform.SetParent(transform, false);
        //                //GameObject.Destroy(txtclone,2.5f);
        //                 tween1 = txtclone.transform.FindChild("txt").DOLocalMoveY(100, 2.5f);
        //                tween1.OnComplete(delegate()
        //                {
        //                    this.onEnd(txtclone);
        //                });
        //            break;
        //   }

        //}       

        Queue<string> qFlytxt = new Queue<string>();
        GameObject curFlyTxt;
        public void AddDelayFlytxt(string element) => qFlytxt.Enqueue(element);
        public void AddDelayFlytxtList(List<string> element_list)
        {
            for (int i = 0; i < element_list.Count; i++)
                AddDelayFlytxt(element_list[i]);
        }
        public void StartDelayFly(float start = 0f,float timeSpan = 0.2f) => InvokeRepeating("DoDelayFly", start, timeSpan);
        protected void DoDelayFly()
        {
            if (qFlytxt.Count == 0)
            {
                CancelInvoke("DoDelayFly");
                return;
            }
            //if (!curFlyTxt || !curFlyTxt.activeSelf)
            curFlyTxt = fly0(qFlytxt.Dequeue());
        }
        public void StopDelayFly()
        {
            if (!curFlyTxt)
                return;
            DestroyImmediate(curFlyTxt);
            CancelInvoke("DoDelayFly");
            qFlytxt.Clear();
        }
    }
}
