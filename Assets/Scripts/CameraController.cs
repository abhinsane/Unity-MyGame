using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class CameraController : MonoBehaviour
{
    public Transform target;

    public Tilemap theMap;
    private Vector3 bottomLeftLimit;
    private Vector3 topRightLimit;

    private float halfhHeight;
    private float halfWidth;

    // Start is called before the first frame update
    void Start()
    {
        target = PlayerController.instance.transform;
        
        halfhHeight = Camera.main.orthographicSize;
        halfWidth = halfhHeight * Camera.main.aspect; 

        bottomLeftLimit = theMap.localBounds.min + new Vector3(halfWidth,halfhHeight,0f);
        topRightLimit = theMap.localBounds.max + new Vector3(-halfWidth,-halfhHeight,0f);

        PlayerController.instance.Setbounds(theMap.localBounds.min, theMap.localBounds.max);
    }

    // Late Update is called once per frame after update
    void LateUpdate()
    {
        transform.position = new Vector3(target.position.x,target.position.y,transform.position.z);

        //keep the camera inside the bounds
        transform.position = new Vector3(Mathf.Clamp(transform.position.x, bottomLeftLimit.x, topRightLimit.x),Mathf.Clamp(transform.position.y, bottomLeftLimit.y, topRightLimit.y), transform.position.z);
    }
}
