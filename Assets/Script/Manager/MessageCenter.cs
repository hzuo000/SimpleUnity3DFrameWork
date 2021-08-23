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

}
