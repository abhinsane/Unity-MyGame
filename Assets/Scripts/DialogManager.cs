using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class DialogManager : MonoBehaviour
{

    public TextMeshProUGUI dialogText;
    public TextMeshProUGUI nameText;
    public GameObject dialogBox;
    public GameObject nameBox;
    public string[] dialogLines;
    public int currentLine;
    public static DialogManager instance;

    

    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        //dialogText.text = dialogLines[currentLine]; 
    }

    // Update is called once per frame
    void Update()
    {
        if(dialogBox.activeInHierarchy)
        {
            if(Input.GetButtonUp("Fire1"))
            {
                currentLine++;

                if(currentLine >=dialogLines.Length)
                {
                    dialogBox.SetActive(false);
                    
                }
                else
                {
                    dialogText.text = dialogLines[currentLine];
                }

            }
        }
    }
    public void ShowDialog(string[] newLines)
    {
        dialogLines = newLines;

        currentLine = 0;

        dialogText.text = dialogLines[0];
        dialogBox.SetActive(true);
    }
}
