using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FairyGUI;

public class StartUpView : UIViewBase
{
    GTextField textField;
    protected override void OnInitialized()
    {
        InitAction();
        textField = Gcom.GetChild("Percent").asTextField;
    }
    public override void OnShow(object data)
    {
        
    }
    private void InitAction()
    {
        GameManager.GameInitNumAction += UpdateLoadPercent;
    }
    public override void OnDestroy()
    {
        base.OnDestroy();
        GameManager.GameInitNumAction -= UpdateLoadPercent;
    }
    private void UpdateLoadPercent(int curCount,int allCount)
    {
        float percent = (float)curCount / allCount;
        textField.text = ((int)(percent * 100)).ToString()+"%";
    }

}
