using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
namespace GameFramework
{
   public class ScenceUtils
    {
       public static float getGroundHight(float x,float y)
       {
           NavMeshHit hit;
           if (  NavMesh.SamplePosition(new Vector3(x, 0, y),out hit,999f, 1 << NavMesh.GetNavMeshLayerFromName("Default")))
           {
               Vector3 vec = hit.position;
               return vec.y;
           }
           return 0;
       }

       public static float getParticleSystemLength(Transform transform)
        {
            ParticleSystem[] particleSystems = transform.GetComponentsInChildren<ParticleSystem>();
            float maxDuration = 0;
            foreach (ParticleSystem ps in particleSystems)
            {
                if (ps.enableEmission)
                {
                    if (ps.loop)
                    {
                        return -1f;
                    }
                    float dunration = 0f;
                    if (ps.emissionRate <= 0)
                    {
                        dunration = ps.startDelay + ps.startLifetime;
                    }
                    else
                    {
                        dunration = ps.startDelay + Mathf.Max(ps.duration, ps.startLifetime);
                    }
                    if (dunration > maxDuration)
                    {
                        maxDuration = dunration;
                    }
                }
            }
            return maxDuration;
        }

    }
}
