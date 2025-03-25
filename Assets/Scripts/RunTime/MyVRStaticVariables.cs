using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyVRStaticVariables
{
    public static string playerName = "";

    /// <summary> Join in PlayGame Player Number. </summary>
    [SyncVar]
    public static int personCount = 0;  
}
