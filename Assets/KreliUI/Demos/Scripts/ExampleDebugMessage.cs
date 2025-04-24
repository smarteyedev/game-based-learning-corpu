using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExampleDebugMessage : MonoBehaviour {

	
    public void DebugMessage()
    {
        Debug.Log("Message from ExampleDebugMessage Class. It works!");
    }

    public void DebugMessage(int i)
    {
        if (i == 1)
        Debug.Log("Button ONCE Click.");
        else
        if (i == 2)
            Debug.Log("Button DOUBLE Click.");
        else
        if (i == 3)
            Debug.Log("Button LONG Click.");
    }
}
