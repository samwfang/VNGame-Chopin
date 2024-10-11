using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Unity.VisualScripting;
using UnityEngine;

namespace DIALOGUE{

public class ConversationHandler
{
    private DialogueSystem dialogueSystem => DialogueSystem.instance;
    private Coroutine process = null;
    public bool is_running => (process != null);
    private TextScroller textConstructor;

    private bool userPrompt_next = false;
    private bool skipping_text = false;

    public ConversationHandler(TextScroller textScroller){
        textConstructor = textScroller;
        //subscribe to userprompt event in dialogue system
        //so OnUserPromptNext is called when event is triggered
        dialogueSystem.handleUserPrompt_Next += OnUserPrompt_Next;
        dialogueSystem.handleSkipText += OnUserSkip;
    }

    private void OnUserPrompt_Next(){
        userPrompt_next = true;
    }

    private void OnUserSkip(){
        userPrompt_next = true;
        skipping_text = true;
    }

    //start conversation with list of strings for conversation data as input
    public void StartConversation(List<string> conversation){
        StopConversation();
        process = dialogueSystem.StartCoroutine(RunningConversation(conversation));
    }

    public void StopConversation(){
        if (!is_running){
            return;
        }

        dialogueSystem.StopCoroutine(process);
        process = null;
    }

    //Coroutine to Run Conversation through Parsing each raw line and
    //Displaying Dialogue and Running Commands
    IEnumerator RunningConversation(List<string> conversation){
        //cache last stored speaker
        DL_SpeakerData last_stored_speaker = new("");
        foreach (string line in conversation){
            DialogueLine dline = Parser.Parse(line);

            //skip blank line or line with only spaces
            if (string.IsNullOrWhiteSpace(line)){
                continue;
            }

            //update last stored speaker cache on new speaker declaration
            if (!dline.has_speaker){
                dline.speaker_data = last_stored_speaker;
            }
            else {
                last_stored_speaker = dline.speaker_data;
            }

            //Display Dialogue Text
            if (dline.has_dialogue){
                yield return Run_Dialogue(dline);
            }

            //Run Commands
            if (dline.has_commands){
                yield return Run_Commands(dline);
            }

            yield return new WaitForSeconds(0.1f);
        }
    }

    //Wrapper to Construct Dialogue from Line w/ Speaker Logic
    IEnumerator Run_Dialogue(DialogueLine line){
        if (line.has_speaker){
            dialogueSystem.ShowSpeakerName(line.speaker_data.display_name);
        }
        else{
            dialogueSystem.HideSpeakerName();
        }

        //yield to build dialogue from its segments
        yield return Build_LineSegments(line.dialogue_data);

        //wait for user input
        yield return PatientWaiterForClick();
    }

    //TODO: Execute Commands
    IEnumerator Run_Commands(DialogueLine line){
        Debug.Log(line.command_data);
        yield return null;
    }

    //Work with Line Segments: Trigger Signal Sequence Property, then Build Dialogue using Dialogue Text for Each Segment
    IEnumerator Build_LineSegments(DL_DialogueData line){
        foreach (DL_DialogueData.DIALOGUE_SEGMENT segment in line.segments){
            yield return WaitForSSTrigger(segment);

            yield return Build_Dialogue(segment.dialogue, segment.append_mode);
        }
    }

    //Handle the Behavior Indicated by Signal Sequence 
    IEnumerator WaitForSSTrigger(DL_DialogueData.DIALOGUE_SEGMENT segment){
        switch(segment.startSignal){
            case DL_DialogueData.DIALOGUE_SEGMENT.StartSignal.C:
            case DL_DialogueData.DIALOGUE_SEGMENT.StartSignal.A:
                yield return PatientWaiterForClick();
                break;
            case DL_DialogueData.DIALOGUE_SEGMENT.StartSignal.WA:
            case DL_DialogueData.DIALOGUE_SEGMENT.StartSignal.WC:
                yield return new WaitForSeconds(segment.signalDelay);
                break;
            default:
                break;
        }
    }

    //Use TextScroller to construct dialogue from line
    IEnumerator Build_Dialogue(string dialogue, bool append = false){
        //Append or Build Dialogue depending on mode
        if (append){
            textConstructor.Append(dialogue);
        }
        else {
            textConstructor.Build(dialogue);
        }

        //force complete dialogue when user inputs again during building
        while (textConstructor.is_building){
            if (userPrompt_next){
                if (textConstructor.skip_text){
                     textConstructor.skip_text = false;
                 }
                else{
                    textConstructor.ForceComplete();
                }

                userPrompt_next = false;
            }
            if (skipping_text){
                //TODO: Implement Skip Functionality
            }
            yield return null;
        }
    }

    //wait and reset user prompt to false again
    IEnumerator PatientWaiterForClick(){
        while (!userPrompt_next){
            yield return null;
        }

        userPrompt_next = false;
    }
}

}