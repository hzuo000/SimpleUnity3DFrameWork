using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MessageCenter : GameInterface
{
    public override void StartUp()
    {
        ManagerType = GameManagerType.MessageCenter;
        base.StartUp();
    }
    public override void UpdateData()
    {
        base.UpdateData();
    }
    public override void Close()
    {
        base.Close();
    }
}
