using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


[RequireComponent(typeof(AudioManager))]
[RequireComponent(typeof(SceneLoder))]
[RequireComponent(typeof(MessageCenter))]
[RequireComponent(typeof(UIManager))]
[RequireComponent(typeof(RecordManager))]
[RequireComponent(typeof(FactoryManager))]
public class GameManager : MonoBehaviour
{
    public static event Action<int, int> GameInitNumAction;//加载进度事件<已加载，总量>
    public static event Action GameInitReadyAction;//加载完成事件

    private static GameManager _this;
    public static GameManager Inst { get => _this; }

    private List<GameInterface> _startSequence;//manager队列

    public static RecordManager Record { get; private set; }//存档
    public static UIManager UI { get; private set; }//ui管理器
    public static MessageCenter Observer { get; private set; }//消息中心
    public static SceneLoder Scene { get; private set; }//场景管理
    public static AudioManager Audio { get; private set; }//音效管理
    public static FactoryManager Factory { get; private set; }//工厂管理（管理各种读表）

    private const int targetFPS = 60;//游戏目标帧数
    private void Awake()
    {
        _this = this;
        Input.multiTouchEnabled = false;//开启多点触控
        DontDestroyOnLoad(gameObject);
        Application.targetFrameRate = targetFPS;

        Record = GetComponent<RecordManager>();
        UI = GetComponent<UIManager>();
        Observer = GetComponent<MessageCenter>();
        Scene = GetComponent<SceneLoder>();
        Audio = GetComponent<AudioManager>();
        Factory = GetComponent<FactoryManager>();

        _startSequence = new List<GameInterface>
        {//这里顺序是初始化顺序
            UI,
            Factory,
            Record,
            Observer,
            Scene,
            Audio,
        };
        StartCoroutine(StartUpMg());
    }
    private void OnDestroy()
    {
        foreach (var gm in _startSequence)
        {
            gm.Close();
        }
    }
    /// <summary>
    /// 应用退出（ iOS后台挂起后，再kill掉进程不会调用该函数 ）
    /// </summary>
    private void OnApplicationQuit()
    {
        //Debug.Log("退出应用");
    }
    /// <summary>
    /// 应用挂起（iOS启动时不会调用该函数）
    /// </summary>
    /// <param name="pause"></param>
    private void OnApplicationPause(bool pause)
    {
        if (pause)
        {
            //Debug.Log("进入后台");
        }
        else
        {
            //Debug.Log("在前台");
        }
    }
    private void FixedUpdate()
    {
        foreach (var mg in _startSequence)
        {
            if (mg.Status == ManagerStatus.Started)
            {
                mg.UpdateData();
            }
        }
    }
    private IEnumerator StartUpMg()
    {
        foreach (var gameManager in _startSequence)
        {
            gameManager.StartUp();
        }
        yield return null;
        int numModules = _startSequence.Count;
        int numReady = 0;

        while (numReady < numModules)
        {
            int lastReady = numReady;
            numReady = 0;

            foreach (var manager in _startSequence)
            {
                if (manager.Status == ManagerStatus.Started)
                {
                    numReady++;
                }
                if (numReady > lastReady)
                {
                    //Debug.Log("加载进度：" + numReady + "/" + numModules);
                    GameInitNumAction?.Invoke(numReady, numModules);
                }
                yield return new WaitForSeconds(0.3f);
            }
        }
        //Debug.Log("加载完成！");
        GameInitReadyAction?.Invoke();
    }
}
