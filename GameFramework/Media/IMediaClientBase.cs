using System;
using System.Collections.Generic;
using Cross;

namespace GameFramework
{
    public interface IMediaClientBase
    {
        void PlayMusicUrl(String url, Action finFun = null, bool force = false);

        void PlaySoundUrl(String url, Boolean loop, Action finFun = null);
		
		void StopSoundUrls(Variant urlArr);
		
		void setSoundVolume(float v=1);
		
		void setMusicVolume(float v=1);
		
		Boolean isPlaySound{get;}
		
		void StopMusic();
    }

}