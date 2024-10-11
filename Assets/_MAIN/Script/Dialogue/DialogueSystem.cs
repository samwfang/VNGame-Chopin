using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/*
Implements Overarching Dialogue System to
*/
namespace DIALOGUE{
public class DialogueSystem : MonoBehaviour
{
    public DialogueContainer dialogueContainer = new();
    private ConversationHandler conversationHandler;
    private TextScroller textConstructor;

    public static DialogueSystem instance {get; private set;}

    //Declare a DELEGATE to allow for event subscription
    //that means when onUserPrompt_Next is triggered, all subscribed
    //functions run!
    public delegate void DialogueSystemEvent();
    public event DialogueSystemEvent handleUserPrompt_Next;

    public event DialogueSystemEvent handleSkipText;


    public bool conversation_is_running => conversationHandler.is_running;

    //make sure only one container exists at a time in game
    private void Awake(){
        if (instance == null){
            instance = this;
            Initialize();
        }
        else {
            DestroyImmediate(gameObject);
        }
    }

    bool initialized = false;
    private void Initialize(){
        if (initialized){
            return;
        }

        //initialize text constructor with TMProUI Dialogue Textbox
        textConstructor = new(dialogueContainer.dialogueText);
        conversationHandler = new(textConstructor);

    }

    //Run all functions subscribed to onUserPromptNext event
    public void OnUserPrompt_Next(){
        //invoke functions if they exist
        if (handleUserPrompt_Next != null){
            handleUserPrompt_Next();
        }
    }
    public void OnUserSkip(){
        handleSkipText?.Invoke();
    }

    //wrapper for a single piece of dialogue
    public void Speak(string speaker, string dialogue){
        List<string> conversation = new(){speaker + " \"" + dialogue + "\" "};
        Speak(conversation);
    }

    //take conversation as list of strings
    public void Speak(List<string> conversation){
        conversationHandler.StartConversation(conversation);
    }
    
    //display nametag and box 
    public void ShowSpeakerName(string speaker_name = ""){
        if (speaker_name.ToLower() != "narrator"){
            dialogueContainer.nameContainer.Show(speaker_name);
        }
        else {
            HideSpeakerName();
        }
    }

    //hide nametag and box
    public void HideSpeakerName(){
        dialogueContainer.nameContainer.Hide();
    }
}
}