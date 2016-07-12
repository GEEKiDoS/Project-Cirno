using System;
using InfinityScript;

namespace INF3
{
    /// <summary>
    /// 允许实体可以使用玩家触发行为
    /// </summary>
    public interface IUsable
    {
        /// <summary>
        /// 当玩家靠近实体时执行
        /// </summary>
        event Func<Entity,string> UsableText;
        /// <summary>
        /// 当玩家使用实体行为时执行
        /// </summary>
        event Action<Entity> UsableThink;
        
        /// <summary>
        /// 位置
        /// </summary>
        Vector3 Origin { get; }
        /// <summary>
        /// 触发距离
        /// </summary>
        int Range { get; }

        string GetUsableText(Entity player);
        void DoUsableFunc(Entity player);
    }
}
