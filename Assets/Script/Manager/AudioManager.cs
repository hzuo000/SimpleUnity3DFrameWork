using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : GameInterface
{
    public override void StartUp()
    {
        ManagerType = GameManagerType.Audio;
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
