using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DIALOGUE;

public class TestSpeakerParse : MonoBehaviour
{
    [SerializeField] private TextAsset fileToRead = null;
    // Start is called before the first frame update
    void Start()
    {
        List<string> textlines = FileManager.ReadTxtAsset(fileToRead, false);

        foreach (string line in textlines){
            Debug.Log("Parsing line " + line);
            DialogueLine dline = Parser.Parse(line);
            if (dline.has_speaker)
            {
                Debug.Log(dline.speaker_data.name + " as " + dline.speaker_data.display_name + " at " + dline.speaker_data.cast_position);
                var expr = dline.speaker_data.CastExpressions;
                for (int c = 0; c < expr.Count; c++)
                {
                    Debug.Log("[Layer " + expr[c].layer + "] = " + expr[c].expression + "]");
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
