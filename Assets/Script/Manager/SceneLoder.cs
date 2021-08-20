using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneLoder : GameInterface
{
    public override void StartUp()
    {
        ManagerType = GameManagerType.Scene;
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
