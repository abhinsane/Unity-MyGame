using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EVE : MonoBehaviour
{
    public string transitionName;
    // Start is called before the first frame update
    void Start()
    {
        if(transitionName == PlayerController.instance.areaTransitionName)
        {
            PlayerController.instance.transform.position = transform.position;
        }
        UIFade.instance.fadeFromBlack();
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
