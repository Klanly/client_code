using System;
using System.Collections;
using System.Collections.Generic;
using Cross;
using UnityEngine;

namespace MuGame
{
    abstract public class MediaClientBase
    {

        public String _curMusicUrl = "";

        private int m_SoundAS_index = 0;
        private AudioSource[] m_SoundAS_array = new AudioSource[8]; //8个声道

        protected float _musicVolume = 1;
        protected float _soundVolume = 1;

        protected Boolean _isPlaySound = true;
        protected Boolean _isPlayMusic = true;

        private Boolean _enableLoopPlayMusic = true;

        protected GameObject musicgo;
        protected AudioSource musicAudioSource;


        public MediaClientBase()
        {
            musicgo = new GameObject();
            musicgo.name = "music";
            musicAudioSource = musicgo.AddComponent<AudioSource>();

            for( int i = 0; i < m_SoundAS_array.Length; ++i )
            {
                m_SoundAS_array[i] = musicgo.AddComponent<AudioSource>();
            }
        }


        public bool _pause = false;
        public void pause(bool b)
        {
            _pause = b;

        }

        /**
         *音乐是否循环播放 
         */
        public Boolean enableLoopPlayMusic
        {
            get
            {
                return _enableLoopPlayMusic;
            }
            set
            {
                _enableLoopPlayMusic = value;
            }
        }

        public Boolean isPlaySound
        {
            get
            {
                return _isPlaySound;
            }
            set
            {
                bool b = _isPlaySound != value;
                _isPlaySound = value;

                if (b)
                    return;

                if (!_isPlaySound)
                    StopSounds();
            }
        }

        public Boolean isPlayMusic
        {
            get
            {
                return _isPlayMusic;
            }
            set
            {
                bool b = _isPlayMusic == value;
                _isPlayMusic = value;
                if (b)
                    return;

                if (!_isPlayMusic)
                    StopMusic();
                else
                    PlayMusicUrl(_curMusicUrl, null, true);
            }
        }

        public void StopMusic()
        {
            musicAudioSource.Stop();
        }

        public void PlayMusicUrl(String url, Action finFun = null, bool force = false)
        {
            if (url == "")
            {
                musicAudioSource.Stop();
                return;
            }


            if (_curMusicUrl == url && force == false)
            {
                return;
            }
            _curMusicUrl = url;
            if (!_isPlayMusic)
            {
                return;
            }
            musicAudioSource.Stop();
            //musicAudioSource.clip = U3DAPI.loadAssetBundle<AudioClip>("media/music", url, false);
            GAMEAPI.ABAUDIO_LoadAudioClip(url, MusicAC_Loaded, url);

            //musicAudioSource.volume = _musicVolume;
            //musicAudioSource.loop = true;
            ////_curMusic.SetFinFun(finFun);
            //musicAudioSource.Play();
        }

        private void MusicAC_Loaded(UnityEngine.Object ac, System.Object data)
        {
            musicAudioSource.clip = ac as AudioClip;
            musicAudioSource.volume = _musicVolume;
            musicAudioSource.loop = true;
            //_curMusic.SetFinFun(finFun);
            musicAudioSource.Play();
        }

        public void PlaySoundUrl(String url, Boolean loop, Action finFun, float volumeRate = 1)
        {
            if (_pause)
                return;

            if (url == null || url == "")
            {
                return;
            }
            if (!_isPlaySound)
            {
                return;
            }

            GAMEAPI.ABAUDIO_LoadAudioClip(url, SoundAC_Loaded, volumeRate);
        }

        private void SoundAC_Loaded(UnityEngine.Object ac, System.Object data)
        {
            if (m_SoundAS_index >= m_SoundAS_array.Length) m_SoundAS_index = 0;

            AudioSource audio = m_SoundAS_array[m_SoundAS_index];
            ++m_SoundAS_index;

            if (audio.isPlaying)
            {
                return;
            }

            audio.clip = ac as AudioClip;
            audio.volume = _soundVolume * (float)data;
            audio.Play();
        }


        public void setSoundVolume(float v = 1)
        {
            if (v > 1) v = 1;
            if (v < 0) v = 0;
            _soundVolume = v;

            for (int i = 0; i < m_SoundAS_array.Length; ++i)
            {
                m_SoundAS_array[i].volume = _soundVolume;
            }

            if (_soundVolume <= 0)
            {
                _isPlaySound = false;
            }
            else
            {
                _isPlaySound = true;
            }
        }
        public void setMusicVolume(float v = 1)
        {
            if (v > 1) v = 1;
            if (v < 0) v = 0;
            _musicVolume = v;


            musicAudioSource.volume = _musicVolume;


            if (_musicVolume <= 0)
            {
                _isPlayMusic = false;
            }
            else
            {
                _isPlayMusic = true;
            }
        }

        public void StopSounds()
        {
            //停止全部音效
            for (int i = 0; i < m_SoundAS_array.Length; ++i)
            {
                m_SoundAS_array[i].Stop();
                m_SoundAS_array[i].clip = null;
            }
        }
    }
}