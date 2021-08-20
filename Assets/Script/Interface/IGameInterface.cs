using UnityEngine;

/// <summary>
/// 管理器状态
/// </summary>
public enum ManagerStatus
{
    Shutdown,//关闭
    Initializing,//正在加载中
    Started//已开启
}
/// <summary>
/// 管理器类型
/// </summary>
public enum GameManagerType
{
    NULL = -1,

    Record,//存档
    UI,//UI
    MessageCenter,//消息中心
    Scene,//场景管理
    Audio,//声音管理

}
 public class GameInterface : MonoBehaviour
{
    public GameManagerType ManagerType { get; protected set; }
    public ManagerStatus Status { get; protected set; }
    /// <summary>
    /// 初始化【base在最后调用】
    /// </summary>
    public virtual void StartUp()
    {
        Status = ManagerStatus.Started;
    }
    /// <summary>
    /// 更新【base在开始时调用】
    /// </summary>
    public virtual void UpdateData()
    {
        if (Status!= ManagerStatus.Started)
        {
            Debug.LogWarning("管理器未启用！");
        }
    }
    /// <summary>
    /// 管理器关闭【base在最后调用】[这个不知道有没有用]
    /// </summary>
    public virtual void Close()
    {
        Status = ManagerStatus.Shutdown;
    }
}
