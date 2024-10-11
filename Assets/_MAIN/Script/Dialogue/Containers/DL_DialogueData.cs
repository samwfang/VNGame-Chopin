using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using System.Text.RegularExpressions;
using UnityEngine;

public class DL_DialogueData
{
    //dialogue consists of a collection of segments
    //EXAMPLE: "Hello{c}World{wa}HiThere!" splits into 3 segments
    public List<DIALOGUE_SEGMENT> segments; 
    //{c}, {a}, {wc <float>}, or {wa <float>} followed by DialogueLine
    private const string SEGMENT_PATTERN = @"(?:\{(wc|wa|a|c)?\s*([\d\.]+)?\})?([^{]+)";
    public bool hasDialogue => segments.Count > 0;

    public struct DIALOGUE_SEGMENT{
        public string dialogue;
        public StartSignal startSignal;
        public float signalDelay;

        public bool append_mode => startSignal == StartSignal.A || startSignal == StartSignal.WA;
        public enum StartSignal{NONE, C, A, WA, WC}
    }

    public DL_DialogueData(string rawdialogue){
        segments = SplitIntoSegments(rawdialogue);
    }

    public List<DIALOGUE_SEGMENT> SplitIntoSegments(string rawdialogue){
        List<DIALOGUE_SEGMENT> cur_segments = new();

        MatchCollection matches = Regex.Matches(rawdialogue, SEGMENT_PATTERN);
        foreach (Match match in matches){
            // First group is the start signal (if present)
            string startSignalStr = match.Groups[1].Success ? match.Groups[1].Value : "";
            //Parse Start Signal to Enum
            DIALOGUE_SEGMENT.StartSignal signal;
            switch (startSignalStr){
                case "c":
                    signal = DIALOGUE_SEGMENT.StartSignal.C;
                    break;
                case "a":
                    signal = DIALOGUE_SEGMENT.StartSignal.A;
                    break;
                case "wc":
                    signal = DIALOGUE_SEGMENT.StartSignal.WC;
                    break;
                case "wa":
                    signal = DIALOGUE_SEGMENT.StartSignal.WA;
                    break;
                default:
                    signal = DIALOGUE_SEGMENT.StartSignal.NONE;
                    break;
            }
            // Second group is the delay (if present)
            float signalDelay = match.Groups[2].Success ? float.Parse(match.Groups[2].Value) : 0;

            // Third group is the dialogue text
            string dialogueText = match.Groups[3].Value.Trim();

            cur_segments.Add(new DIALOGUE_SEGMENT{
                dialogue = dialogueText,
                startSignal = signal,
                signalDelay = signalDelay
            });
        }
        return cur_segments;
        
    }

 
}
