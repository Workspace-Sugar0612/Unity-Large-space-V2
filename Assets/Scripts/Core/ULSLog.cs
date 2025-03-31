using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Log
{
    public static void input(string strlog)
    {
        Debug.Log(strlog);
    }

    public static void cinput(string color, string log)
    {
        Debug.Log($"<color={color}>{log}</color>");
    }
}
