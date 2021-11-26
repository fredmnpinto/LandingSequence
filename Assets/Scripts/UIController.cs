using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    private Transform _podTransform;
    private Rigidbody2D _podRb;
    
    private Text _velocityLabel;
    public string velocityLabelDefaultText = "Velocity: ";

    // Start is called before the first frame update
    void Start()
    {
        _velocityLabel = transform.Find("VelocityLabel").gameObject.GetComponent<Text>();
        
        Pod pod = Pod.Instance;
        
        _podTransform = pod.transform;
        _podRb = pod.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        _velocityLabel.text = velocityLabelDefaultText + Math.Abs(Math.Round(_podRb.velocity.y, 1));
    }
}
