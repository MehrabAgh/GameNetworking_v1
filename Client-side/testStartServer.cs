using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testStartServer : MonoBehaviour
{  
    void Start()
    {
        client.instance.ConnectedToServer();
    }
 
}
