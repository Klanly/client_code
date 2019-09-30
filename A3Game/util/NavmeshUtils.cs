using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
namespace MuGame
{
    public class NavmeshUtils
    {


        public static List<int> listARE = new List<int> {0 ,AgentLayerNameToValue("ARE1"),
        AgentLayerNameToValue("ARE2"),
        AgentLayerNameToValue("ARE3"),
        AgentLayerNameToValue("ARE4"),
        AgentLayerNameToValue("ARE5")};

        public static int allARE = listARE[1] + listARE[2] + listARE[3] + listARE[4] + listARE[5];


        // Nav层名字-->层的值，1、2、4、8、16
        public static int AgentLayerNameToValue(string name)
        {
            int idx = NavMesh.GetNavMeshLayerFromName(name);
            return 0x1 << idx;
        }

        // Nav层名字-->层索引，0、1、2、3、4
        public static int AgentLayerNameToIndex(string name)
        {
            return NavMesh.GetNavMeshLayerFromName(name);
        }

        // 获取角色当前所在的层值，1、2、4、8、16
        public static int GetAgentLayer(NavMeshAgent agent)
        {
            NavMeshHit hit;
            // 不要使用agent.raduis为采样范围，因为当该值为0时，函数将返回0
            bool reach = NavMesh.SamplePosition(agent.transform.position, out hit, 1f, -1);
            return hit.mask;
        }

        public static Vector3 SampleNavMeshPosition(Vector3 logicPosition, out bool reachable)
        {
            NavMeshHit hit;
            reachable = NavMesh.SamplePosition(logicPosition, out hit, 1f, -1);
            return reachable ? hit.position : logicPosition;
        }

        //// 开启导航层
        //public static void EnableNavMeshLayer(NavMeshAgent agent, string layerName)
        //{
        //    if (agent == null)
        //        return;

        //    int layerValue = NavMesh.GetNavMeshLayerFromName(layerName);
        //    if (layerValue == -1)
        //        return;

        //    int mask = agent.walkableMask | 0x1 << layerValue;
        //    WalkArbiter.SetWalkableMask(agent, mask);
        //}

        //// 关闭导航层
        //public static void DisableNavMeshLayer(NavMeshAgent agent, string layerName)
        //{
        //    if (agent == null)
        //        return;

        //    int layerValue = NavMesh.GetNavMeshLayerFromName(layerName);
        //    if (layerValue == -1)
        //        return;

        //    int mask = agent.walkableMask & ~(0x1 << layerValue);

        //    WalkArbiter.SetWalkableMask(agent, mask);
        //}

        // 检查某个层是否为开启
        public static bool IsNavMeshLayerOpen(NavMeshAgent agent, string layerName)
        {
            int layerValue = NavMesh.GetNavMeshLayerFromName(layerName);
            if (layerValue == -1)
                return true;

            int ret = agent.walkableMask & (0x1 << layerValue);
            return ret > 0 ? true : false;
        }
    }
}
