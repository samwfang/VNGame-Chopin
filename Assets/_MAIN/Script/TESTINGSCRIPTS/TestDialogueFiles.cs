using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DIALOGUE;

public class TestDialogueFiles : MonoBehaviour
{
    [SerializeField] private TextAsset fileToRead = null;
    // Start is called before the first frame update
    void Start()
    {
        List<string> textlines = FileManager.ReadTxtAsset(fileToRead, false);

        foreach (string line in textlines){
            Debug.Log("Segmenting line " + line);
            DialogueLine dline = Parser.Parse(line);
            int i = 0;
            foreach(DL_DialogueData.DIALOGUE_SEGMENT segement in dline.dialogue_data.segments){
                Debug.Log("Segment" + (i++) + ": " + segement.dialogue + "|" + segement.startSignal.ToString() + "|" + segement.signalDelay);
            }
        }
        DialogueSystem.instance.Speak(textlines);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
