using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Runtime.CompilerServices;
using Unity.VisualScripting.Dependencies.Sqlite;
using Unity.VisualScripting;

/*
Implements Text Scrolling Functionality for Instant and Typewriter-Style Appearances

Either call Build(text) or Append(text) coroutines to make text visible
*/
public class TextScroller
{
    private TextMeshProUGUI tmpro_ui;
    private TextMeshPro tmpro_world;

    //why use => operator? This is to dynamically evaluate value of tmpro whenever it is called
    public TMP_Text tmpro => (tmpro_ui != null) ? tmpro_ui : tmpro_world;

    public string current_text => tmpro.text;

    public string target_text {get; private set; } = "";
    public string pre_text {get; private set; } = "";
    private int pre_text_length = 0;

    public string full_target_text => pre_text + target_text;

    public enum ScrollMethod {INSTANT, TYPEWRITER, FADING} 
    public ScrollMethod scroll_method = ScrollMethod.TYPEWRITER;

    public Color textColor {get {return tmpro.color;} set {tmpro.color = value;}}

    public float velocity {get {return BASE_VELOCITY * velocity_multiplier;} set {velocity_multiplier = value;}}
    private const float BASE_VELOCITY = 1;
    private float velocity_multiplier = 1;

    public int chars_per_cycle {get {return velocity <= 2f ? char_multiplier : velocity <= 2.5f  ? char_multiplier * 2 : char_multiplier * 3;}}
    private int char_multiplier = 1;
    public bool skip_text = false;


    //instantiate Text Scroller
    public TextScroller(TextMeshProUGUI tmpro_ui){
        this.tmpro_ui = tmpro_ui;
    }
    public TextScroller(TextMeshPro tmpro_world){
        this.tmpro_world = tmpro_world;
    }

    //start a new text sequence by returning coroutine 
    public Coroutine Build(string text){
        pre_text = "";
        target_text = text;

        Stop();

        build_process = tmpro.StartCoroutine(Building());
        return build_process;
    }

    //append to text that is already there by returning coroutine
     public Coroutine Append(string text){
        pre_text = tmpro.text;
        target_text = text;

        Stop();

        build_process = tmpro.StartCoroutine(Building());
        return build_process;
    }

    
    //stuff to stop the text building coroutine
    private Coroutine build_process = null;
    public bool is_building => (build_process != null);
    public void Stop(){
        if (!is_building){
            return;
        }
        tmpro.StopCoroutine(build_process);
        build_process = null;
    }

    //coroutine runs this enumeration
    IEnumerator Building(){
        Prepare();
        switch(scroll_method){
            case ScrollMethod.TYPEWRITER:
            yield return Build_Typewriter();
            break;
            
            case ScrollMethod.FADING:
            yield return Build_Fading();
            break;
        }
        OnComplete();

        yield return null;
    }

    public void ForceComplete(){
        switch(scroll_method){
            case ScrollMethod.TYPEWRITER:
                tmpro.maxVisibleCharacters = tmpro.textInfo.characterCount;
                break;
            case ScrollMethod.FADING:
                break;
        }

        Stop();
        OnComplete();
    }

    private void Prepare(){
        switch(scroll_method){
            case ScrollMethod.INSTANT:
            PrepareInstant();
            break;
            case ScrollMethod.TYPEWRITER:
            PrepareTypewriter();
            break;
            case ScrollMethod.FADING:
            PrepareFading();
            break;
        }
    }

    private void PrepareInstant(){
        //reapply color to vertices
        tmpro.color = tmpro.color;
        tmpro.text = full_target_text;
        tmpro.ForceMeshUpdate();
        //make sure every char is visible onscreen
        tmpro.maxVisibleCharacters = tmpro.textInfo.characterCount;
    }

    private void PrepareTypewriter(){
        //reapply color to vertices
        tmpro.color = tmpro.color;
        //starting with nothing visible
        tmpro.maxVisibleCharacters = 0;

        //if pre-text exists, force update and make it visible
        tmpro.text = pre_text;
        if (pre_text != ""){
            tmpro.ForceMeshUpdate();
            tmpro.maxVisibleCharacters = tmpro.textInfo.characterCount;
        }

        //append target text
        tmpro.text += target_text;
        tmpro.ForceMeshUpdate();

    }

    private void PrepareFading(){
        return;
    }

    private void OnComplete(){
        build_process = null;
    }

    //Code for Typewriter Logic: This Runs Every Time A New Character or Characters
    //Are added to dialogue
    private IEnumerator Build_Typewriter(){
        while(tmpro.maxVisibleCharacters < tmpro.textInfo.characterCount){
            if (skip_text){
                tmpro.maxVisibleCharacters += chars_per_cycle * 5;
            }
            else {
                tmpro.maxVisibleCharacters += chars_per_cycle;
            }
            yield return new WaitForSeconds(0.015f/velocity);
        }
    }

    private IEnumerator Build_Fading(){
        yield return null;
    }

}   

