using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

static public class EnumAni
{
    static public int ANI_RUN = Animator.StringToHash("run");
    static public int ANI_HURT_FRONT = Animator.StringToHash("hurt_front");
    static public int ANI_HURT_BACK = Animator.StringToHash("hurt_back");
    static public int ANI_I_SKILL = Animator.StringToHash("i_skill");
    static public int ANI_ATTACK = Animator.StringToHash("attack");
    //static public int ANI_TURN_L = Animator.StringToHash("turn_l");
    //static public int ANI_TURN_R = Animator.StringToHash("turn_r");
    static public int ANI_FALL_FRONT = Animator.StringToHash("fall_front");
    static public int ANI_FALL_BACK = Animator.StringToHash("fall_back");
    static public int ANI_JUMP = Animator.StringToHash("jump");
    static public int ANI_B_DIE = Animator.StringToHash("b_die");
    static public int ANI_F_FLY = Animator.StringToHash("f_fly");

    static public int ANI_B_BORNED = Animator.StringToHash("borned");
    

    //sbt 自身的子弹
    static public int ANI_T_SBT_ATK = Animator.StringToHash("t_sbt_atk");

    //FX
    static public int ANI_T_FXDEAD = Animator.StringToHash("t_fxdead");
    static public int ANI_T_FXDEAD1 = Animator.StringToHash("t_fxdead1");

    //ride
    static public int ANI_T_RIDERUN = Animator.StringToHash("isRun");
    static public int ANI_T_RIDERANDOM = Animator.StringToHash("random");
    static public int ANI_T_RANDOMVA = Animator.StringToHash("randomValue");
}
