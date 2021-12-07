using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PodEntrance : MonoBehaviour
{
    private BoxCollider2D _entranceArea;
    [SerializeField] private LayerMask astronautMask;
    
    // Start is called before the first frame update
    void Start()
    {
        _entranceArea = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public bool IsAstronautTouchingDoor()
    {
        return _entranceArea.IsTouchingLayers(astronautMask);
    }
}
