using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;
/// <summary>
/// 当前游戏有的场景
/// </summary>
public enum SceneName
{
    StartUp,
    MainUI,
    Stage,
}
public class SceneLoder : GameInterface
{

    private AsyncOperation async;//异步引用
    /// <summary>
    /// 当前场景
    /// </summary>
    public Scene CurrentScene
    {
        get => SceneManager.GetActiveScene();
    }
    public override void StartUp()
    {
        ManagerType = GameManagerType.Scene;
        async = null;
        SceneManager.sceneLoaded += OnSceneLoaded;
        base.StartUp();
    }

    public override void Close()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
        base.Close();
    }
    public void LoadScene(SceneName _scene)
    {
        StartCoroutine(StartLoadScene(_scene));
    }
    /// <summary>
    /// 异步载入场景
    /// </summary>
    /// <param name="sceneName"></param>
    /// <returns></returns>
    private IEnumerator StartLoadScene(SceneName sceneName)
    {
        //SceneManager.LoadScene((int)SceneName.Loading);
        //IsReadLoadScene = false;
        //yield return new WaitUntil(() => IsReadLoadScene);
        async = SceneManager.LoadSceneAsync((int)sceneName);
        async.allowSceneActivation = false;
        while (async.progress < .9f)
        {
            float curProgress = async.progress;
            //Action_LoadingSceneProcess?.Invoke(curProgress);
            yield return new WaitForEndOfFrame();
        }
        float target = 1f;
        float tempPross = async.progress;
        while (tempPross < target)//异步加载只能到0.9，后面的自己作假
        {
            tempPross += .1f;
            //Action_LoadingSceneProcess?.Invoke(tempPross);
            yield return new WaitForEndOfFrame();
        }
        async.allowSceneActivation = true;
    }
    /// <summary>
    /// 场景载入完成后回调
    /// </summary>
    /// <param name="scene"></param>
    /// <param name="mode"></param>
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        //Action_OnLoadedScene?.Invoke(scene);
        //if (scene.buildIndex == (int)SceneName.Loading)
        //{
        //    IsReadLoadScene = true;
        //}
    }
}
