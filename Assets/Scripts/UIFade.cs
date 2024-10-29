using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UIFade : MonoBehaviour
{
    public static UIFade instance;
    public Image fadeScreen;
    public float fadeSpeed; 

    public bool shouldFadetoBlack;
    public bool shouldFadeFromBlack;
    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        if(shouldFadetoBlack)
        {
            GameManager.instance.fadingBetweenAreas =true;
            fadeScreen.color = new Color(fadeScreen.color.r,fadeScreen.color.g,fadeScreen.color.b, Mathf.MoveTowards(fadeScreen.color.a, 1f,fadeSpeed * Time.deltaTime));

            if(fadeScreen.color.a == 1f)
            {
                shouldFadetoBlack = false;
            }
        }

        if(shouldFadeFromBlack)
        {
            fadeScreen.color = new Color(fadeScreen.color.r,fadeScreen.color.g,fadeScreen.color.b, Mathf.MoveTowards(fadeScreen.color.a, 0f,fadeSpeed * Time.deltaTime));
            
            if(fadeScreen.color.a == 0f)
            {
                shouldFadeFromBlack = false;
                GameManager.instance.fadingBetweenAreas = false;
            }
            
        }
    
    }
    public void FadeToBlack()
    {
        shouldFadetoBlack = true;
        shouldFadeFromBlack = false;
    }

    public void fadeFromBlack()
    {
        shouldFadetoBlack = false;
        shouldFadeFromBlack = true;
    }
}
