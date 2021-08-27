using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FairyGUI;

public class FadingMessage : UIViewBase
{
    GTextField tipsText;
    Transition animation;
    protected override void OnInitialized()
    {
        tipsText = Gcom.GetChild("n0").asCom.GetChild("Tips").asTextField;
        animation = Gcom.GetTransition("t0");
    }
    public override void OnShow(object data)
    {
        string tips = data as string;
        if (tips!=null)
        {
            tipsText.text = tips;
            animation.Play(()=> {
                GameManager.UI.ClosePanel(panelName);
            });
        }
    }
}
