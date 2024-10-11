using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DIALOGUE;
using System;

public class TestCommandParse : MonoBehaviour
{
    [SerializeField] private TextAsset fileToRead = null;
    // Start is called before the first frame update
    void Start()
    {
        List<string> textlines = FileManager.ReadTxtAsset(fileToRead, false);

        foreach (string line in textlines){
            Debug.Log("Parsing line " + line);
            DialogueLine dline = Parser.Parse(line);
            if (dline.has_commands)
            {
                foreach (var command in dline.command_data.commandlist)
                {
                    Debug.Log("Command: " + command.name + " Args: ");
                    foreach (var arg in command.args)
                    {
                        Debug.Log(arg);
                    }
                }
            }
        }
        DialogueSystem.instance.Speak(textlines);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
