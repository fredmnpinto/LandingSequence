using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    private Pod pod;
    
    private Transform _podTransform;
    private Rigidbody2D _podRb;
    
    private Text _velocityLabel;
    private Text _thrustCapLabel;

    public string thrustCapLabelDefaultText = "Thrust Cap: ";
    public string velocityLabelDefaultText = "Velocity: ";

    // Start is called before the first frame update
    void Start()
    {
        _velocityLabel = transform.Find("VelocityLabel").gameObject.GetComponent<Text>();
        _thrustCapLabel = transform.Find("ThrustCapLabel").gameObject.GetComponent<Text>();
        
        pod = Pod.Instance;
        
        _podTransform = pod.transform;
        _podRb = pod.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnGUI()
    {
        _velocityLabel.text = velocityLabelDefaultText + Math.Abs(Math.Round(_podRb.velocity.y, 1));
        _thrustCapLabel.text = thrustCapLabelDefaultText + Math.Abs(Math.Round(pod.currentThrustCapacity, 1));
    }
}
