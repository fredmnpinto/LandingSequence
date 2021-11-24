using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIController : MonoBehaviour
{
    private Pod _pod;
    private Rigidbody2D _podRb;
    
    // Start is called before the first frame update
    void Start()
    {
        _pod = Pod.Instance;
        _podRb = _pod.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnGUI()
    {   
        GUI.Label(new Rect(0, 0, 100, 200), $"POD\n\trotation: {_pod.transform.rotation.z * 180}\n" +
                                            $"velocity: {_podRb.velocity}\n" +
                                            $"angular velocity: {_podRb.angularVelocity}");
    }
}
