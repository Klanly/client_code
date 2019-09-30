using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameFramework
{
	public class UiEventCenter:GameEventDispatcher
	{
        public static uint EVENT_WIN_OPEN = 1;
        public static uint EVENT_MAP_CHANGED = 2;
        public static uint EVENT_WIN_CLOSE = 3;
        public static uint EVENT_LOTTERY_DRAW = 4;
        public static uint EVENT_HERO_3D_SKILL_OVER = 4;
        public static uint EVENT_FB_INITED = 5;
        public static uint EVENT_FB_WIPEOUT_OVER = 6;
        public static uint EVENT_ACHIEVE_INITED = 7;
        public static uint EVENT_NEW_SKILL = 8;
        public static uint EVENT_SKILL_DRAWEND = 9;
        public static uint EVENT_START_MOVE = 10;
        public static uint EVENT_ADD_POINT = 11;

        public static string lastClosedWinID;

        public void onWinOpen(string winid)
        {
            dispatchEvent(GameEvent.Create(EVENT_WIN_OPEN, this, winid));
        }

        public void onWinClosed(string winid)
        {
            lastClosedWinID = winid;
            dispatchEvent(GameEvent.Create(EVENT_WIN_CLOSE, this, winid));
        }

        public void onLotteryDrawed()
        {
            dispatchEvent(GameEvent.Create(EVENT_LOTTERY_DRAW, this, null));
        }

        public void onMapChanged()
        {
            dispatchEvent(GameEvent.Create(EVENT_MAP_CHANGED, this, null));
        }

        public void onHeroSkillPlayerOver()
        {
            dispatchEvent(GameEvent.Create(EVENT_HERO_3D_SKILL_OVER, this, null));
        }

        public void onFbInited()
        {
            dispatchEvent(GameEvent.Create(EVENT_FB_INITED, this, null));
        }


        public void onFbWipeoutOver()
        {
            dispatchEvent(GameEvent.Create(EVENT_FB_WIPEOUT_OVER, this, null));
        }

        public void onAchiInited()
        {
            dispatchEvent(GameEvent.Create(EVENT_ACHIEVE_INITED, this, null));
        }

        public void onNewSkill()
        {
            dispatchEvent(GameEvent.Create(EVENT_NEW_SKILL, this, null));
        }
        public void onSkillDrawEnd()
        {
            dispatchEvent(GameEvent.Create(EVENT_SKILL_DRAWEND, this, null));
        }
        public void onStartMove()
        {
            dispatchEvent(GameEvent.Create(EVENT_START_MOVE, this, null));
        }

        public void onAddPoint()
        {
            dispatchEvent(GameEvent.Create(EVENT_ADD_POINT, this, null));
        }

        private static UiEventCenter _instance;
        public static UiEventCenter getInstance()
        {
            if (_instance == null)
                _instance = new UiEventCenter();
            return _instance;
        }

       // public 

	}
}
