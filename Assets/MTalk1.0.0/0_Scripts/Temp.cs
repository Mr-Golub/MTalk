using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Temp : MonoBehaviour
{
    void Awake()
    {
        EventManager.Init();
    }

    public void CallTestTalk()
    {
        EventManager.FireEvent("NPC_p001", new object[] { "talk" });
    }
}
