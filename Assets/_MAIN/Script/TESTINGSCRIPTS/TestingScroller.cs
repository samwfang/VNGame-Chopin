using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using UnityEngine;
using DIALOGUE;

public class TestingScroller : MonoBehaviour
{
    DialogueSystem ds;
    TextScroller scroller;

    string [] lines = new string[5]{
    "<i>Music Playing</i>",
    "...",
    "Ugh! Damn It!",
    "I've been practicing this piece for so long...",
    "and I still can't get the hang of it"
    };

    string longline = "wifjewoifjefefqowqoi iqwjfqiofjqo qiofjqwofiqjofij qwfqjwofiwjfjwoifjowj wiefjoiwejfoijjoij efjioEP EOWFFJIOwe weifjeowif";
    // Start is called before the first frame update
    void Start()
    {
        ds = DialogueSystem.instance;
        scroller = new(ds.dialogueContainer.dialogueText);
        scroller.scroll_method = TextScroller.ScrollMethod.TYPEWRITER;
    }

    // Update is called once per frame
    void Update()
    {
      if (Input.GetKeyDown(KeyCode.Space)){
        if (scroller.is_building){
            if (!scroller.skip_text){
                scroller.skip_text = true;
            }
            else{
                scroller.ForceComplete();
            }
        }
        scroller.Build(lines[Random.Range(0, lines.Length)]);
      }  
      else if (Input.GetKeyDown(KeyCode.A)){
        scroller.Append(longline);
        //scroller.Append(lines[Random.Range(0, lines.Length)]);
      }
    }
}
