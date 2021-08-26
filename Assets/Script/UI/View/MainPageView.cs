using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FairyGUI;

public class MainPageView : UIViewBase
{
    private GButton StartBtn;
    protected override void OnInitialized()
    {
        base.OnInitialized();
        StartBtn = Gcom.GetChild("StartBtn").asButton;
        StartBtn.onClick.Add(()=> { GameManager.Scene.LoadScene(SceneName.Stage); });
    }
}
