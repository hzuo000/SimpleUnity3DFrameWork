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
/// 管理器接口
/// </summary>
public interface IManager
{
    void StartUp();
    void UpdateData();
    void Close();
}

 public abstract class GameInterface : MonoBehaviour,IManager
{
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
