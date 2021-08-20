using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecordManager : GameInterface
{
    public override void StartUp()
    {
        ManagerType = GameManagerType.Record;
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
