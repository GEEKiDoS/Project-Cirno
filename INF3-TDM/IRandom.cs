namespace INF3
{
    /// <summary>
    /// 定义一个接口，用于进行基于权重值的随机
    /// </summary>
    public interface IRandom
    {
        /// <summary>
        /// 权重值
        /// </summary>
        int Weight { get; }
    }
}
