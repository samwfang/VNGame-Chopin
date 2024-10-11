using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DIALOGUE;

public class PlayerInputManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Space) ||
         Input.GetKeyUp(KeyCode.Return)){
            PromptAdvance();
         }
         if (Input.GetKeyDown(KeyCode.R)){
            StartSkip();
         }
    }

    public void PromptAdvance(){
        DialogueSystem.instance.OnUserPrompt_Next();
    }

    public void StartSkip(){
        DialogueSystem.instance.OnUserSkip();
    }
    //TODO: StopSkip() and OffUserSkip()
}
