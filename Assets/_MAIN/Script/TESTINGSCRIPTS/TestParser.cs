using System.Collections;
using System.Collections.Generic;
using DIALOGUE;
using UnityEngine;

public class TestParser : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        List<string> textlines = FileManager.ReadTxtAsset("testFile", false);
        string[] lines = {
        "Sam, the \\\"False Angel\\\": \"Hello world and the \\\"false\\\" world as well\" print(\"hello world\")",
        "Chopin \\\"The One and Only\\\": ",
        "Fred(\\\"???\\\"): PlaySong(\"Happy\")",
        "PlaySong(\"Happy World\") FadeOut() SwordAnimation(\"Blue\")",
        "\"<i>Music Plays</i>\" PlaySong(\"Happy\")"
        };
        foreach (string line in textlines){
            DialogueLine parsed = Parser.Parse(line);
            Debug.Log(parsed.speaker_data + " | " + parsed.dialogue_data + " | " + parsed.command_data);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
