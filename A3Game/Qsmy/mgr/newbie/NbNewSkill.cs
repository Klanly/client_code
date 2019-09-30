using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameFramework;
using Cross;
using UnityEngine;
namespace MuGame
{
    class NbNewSkill : NewbieTeachItem
    {
        // private int lv = 0;


        public static NbNewSkill create(string[] arr)
        {

            NbNewSkill nbskill = new NbNewSkill();
            return nbskill;
        }

        override public void addListener()
        {
            UiEventCenter.getInstance().addEventListener(UiEventCenter.EVENT_NEW_SKILL, onHanlde);
        }

        override public void removeListener()
        {
            UiEventCenter.getInstance().removeEventListener(UiEventCenter.EVENT_NEW_SKILL, onHanlde);
        }
    }

    class NbSkillOn : NewbieTeachItem
    {
        // private int lv = 0;


        public static NbSkillOn create(string[] arr)
        {

            NbSkillOn nbskillon = new NbSkillOn();
            return nbskillon;
        }

        override public void addListener()
        {
            UiEventCenter.getInstance().addEventListener(UiEventCenter.EVENT_SKILL_DRAWEND, onHanlde);
        }

        override public void removeListener()
        {
            UiEventCenter.getInstance().removeEventListener(UiEventCenter.EVENT_SKILL_DRAWEND, onHanlde);
        }
    }
    class NbJoystick : NewbieTeachItem
    {
        public static NbJoystick create(string[] arr)
        {

            NbJoystick nbJoystick = new NbJoystick();
            return nbJoystick;
        }

        override public void addListener()
        {
            UiEventCenter.getInstance().addEventListener(UiEventCenter.EVENT_START_MOVE, onHanlde);
        }

        override public void removeListener()
        {
            UiEventCenter.getInstance().removeEventListener(UiEventCenter.EVENT_START_MOVE, onHanlde);
        }
    }

    class NbAddPoint : NewbieTeachItem
    {
        public static NbAddPoint create(string[] arr)
        {

            NbAddPoint nbaddPoint = new NbAddPoint();
            return nbaddPoint;
        }

        override public void addListener()
        {
            UiEventCenter.getInstance().addEventListener(UiEventCenter.EVENT_ADD_POINT, onHanlde);
        }

        override public void removeListener()
        {
            UiEventCenter.getInstance().removeEventListener(UiEventCenter.EVENT_ADD_POINT, onHanlde);
        }
    }
}
