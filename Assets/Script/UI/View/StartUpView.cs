using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FairyGUI;

public class StartUpView : UIViewBase
{
    GTextField textField;
    protected override void OnInitialized()
    {
        textField = Gcom.GetChild("Percent").asTextField;
    }
    public override void OnShow(object data)
    {
        textField.text = "20p";
    }
}
