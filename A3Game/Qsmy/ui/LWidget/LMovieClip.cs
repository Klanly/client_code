using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;


    /// <summary>
    /// 序列帧动画
    /// </summary>
	public class LMovieClip : MonoBehaviour
    {
        
        public float fps = 15f;
		public bool isPlayOnwake = false;
		public string path;

        protected Image _comImage;
        protected float _time;
        protected int _frameLenght;
        protected bool _isPlaying = false;
		protected int _currentIndex = 0;
        protected Sprite[] _spriteArr;

        // Use this for initialization
        void Start()
        {
            _comImage = gameObject.GetComponent<Image>();

			if (isPlayOnwake) {
				loadTexture ();
				play ();
			}
        }

		public void loadTexture()
		{
            //load textures
            //这个序列帧的聊天内容暂时没用到，先改成这个
            for (int i = 0; i < 1; i++)
            {
                Sprite one = GAMEAPI.ABUI_LoadSprite(path + "/" + i);
                _spriteArr[i] = one;
            }
            _frameLenght = _spriteArr.Length;
		}

        void OnGUI()
        {
            if (_isPlaying)
            {
                drawAnimation();
            }
        }

        // Update is called once per frame
        protected void drawAnimation()
        {
            _comImage.sprite = _spriteArr[_currentIndex];

            if (_currentIndex < _frameLenght)
            {
                _time += Time.deltaTime;
                if (_time >= 1.0f / fps)
                {
					_currentIndex++;
                    _time = 0;
                    if (_currentIndex == _frameLenght)
                    {
                        _currentIndex = 0;
                    }
                }
            }
        }

        public void play()
        {
            _isPlaying = true;
        }

        public void stop()
        {
            _isPlaying = false;
            _currentIndex = 0;
            _comImage.sprite = _spriteArr[0];
        }

        public void pause()
        {
            _isPlaying = false;
        }
    }

