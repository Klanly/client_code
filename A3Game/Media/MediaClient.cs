using System;
using System.Collections;
using System.Collections.Generic;
using Cross;
using GameFramework;

namespace MuGame
{
    public class MediaClient : MediaClientBase
    {
        public static MediaClient instance = new MediaClient();

        protected Boolean _isPauseMusic = false;

        private string _curSuondUrl = "";

        public MediaClient()
            : base()
        {
            instance = this;
        }

        public static MediaClient getInstance()
        {
            if (instance == null)
                instance = new MediaClient();
            return instance;
        }

        public float getSoundVolume() { return _soundVolume; }

        public float getMusicVolume() { return _musicVolume; }


        //private void onPlayMusic(GameEvent e)
        //{//播放地图声音
        //    Variant data = e.data;
        //    bool loop = false;
        //    if (data.ContainsKey("loop"))
        //    {
        //        loop = data["loop"]._bool;
        //    }
        //    PlaySound(data["sid"]._str, loop);
        //}

        public Boolean isPauseMusic
        {
            get { return _isPauseMusic; }
            set
            {
                _isPauseMusic = value;

                if (!_isPlayMusic)
                    return;

                //if (_isPauseMusic)
                //    musicAudioSource.Pause();
                //else
                //    musicAudioSource.Play();

            }
        }



        public void Play(String url, Boolean loop, Action finFun = null)
        {
            PlaySoundUrl(url, loop, finFun);
        }
    }
}