using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class Minimap : MonoBehaviour
{
    [SerializeField] private Tilemap map;

    public int minimapIconsLayer;

    private GameObject _iconsMapObject;

    private Camera _minimapCam;

    // Start is called before the first frame update
    void Start()
    {
        CreateMinimapCam();
        
        ConvertGroundToIcon();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void CreateMinimapCam()
    {
        /* Instantiate the object and Camera */
        GameObject minimapCamGO = new GameObject("Minimap Cam");
        _minimapCam = minimapCamGO.AddComponent<Camera>();

        /* The camera that follows the player */
        Camera playerCam = Camera.main;

        /* Set the minimap cam to mimic the main cam */
        Transform mmcTransform = minimapCamGO.transform;
        Transform mainCamTransform = playerCam.gameObject.transform;
        
        mmcTransform.position = mainCamTransform.position;
        mmcTransform.parent = mainCamTransform;

        /* Set the minimap camera to have a much higher FOV than
         the actual player cam */
        _minimapCam.cullingMask = LayerMask.GetMask(LayerMask.LayerToName(minimapIconsLayer));
        _minimapCam.orthographic = true;
        _minimapCam.orthographicSize = playerCam.orthographicSize * 2f;
        
        /* Set the camera output to the UI */
        RawImage minimapOutput = GetComponent<RawImage>();
        RenderTexture minimapRenderTexture = new RenderTexture(256, 256, 16, RenderTextureFormat.ARGB32);
        minimapRenderTexture.Create();

        _minimapCam.targetTexture = minimapRenderTexture;
        minimapOutput.texture = minimapRenderTexture;
    }

    void ConvertGroundToIcon()
    {
        GameObject mapObj = map.gameObject;
        _iconsMapObject = Instantiate(mapObj, mapObj.transform);
        Tilemap iconsMap = _iconsMapObject.GetComponent<Tilemap>();
        iconsMap.gameObject.layer = minimapIconsLayer;
    }
}
