using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FairyGUI;
public class StageHUDView : UIViewBase
{
    private GButton BackBtn;
    protected override void OnInitialized()
    {
        base.OnInitialized();
        BackBtn = Gcom.GetChild("BackBtn").asButton;
        BackBtn.title = "结束游戏";
        BackBtn.onClick.Add(()=>GameManager.Scene.LoadScene(SceneName.MainUI));
    }
}
