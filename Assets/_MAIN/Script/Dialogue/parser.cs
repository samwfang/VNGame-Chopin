using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.Assertions;

/*
TO PARSE CORRECTLY YOUR LINES MUST BE OF THIS FORMAT, I hereby name SamScriptTM:

Name: "Dialogue" Command1(args1) Command2(arg2)

All are optional, but name or dialogue being empty will throw a warning due to the unusuality of the script
just in case you forgot something

Do NOT use backslashes apart from as an escape sequence for quotation marks

*/
namespace DIALOGUE {

public class Parser
{
    private const string FULLSTRING_PATTERN_W_COMMAND = @"^(?:(?<name>(?:[^:""\\]|\\.)*):\s*)?(?:""(?<dialogue>(?:[^""\\]|\\.)*?)""\s*)?(?<commands>(?:\s*(?:[^""\s]+\([^()]*\)|[^""\s]+)\s*)*)$";
    private const string COMMAND_PATTERN = "\\w*[^\\s]\\()";
    //parsing wrapper
   public static DialogueLine Parse(string rawline){
        Debug.Log("Parsing Line" + rawline);
        (string, string, string) tuple = SplitLine(rawline);
        DialogueLine dialogue_line = new DialogueLine(tuple.Item1, tuple.Item2, tuple.Item3);
        return dialogue_line;
   }

    //split line into the three main parts
   private static (string, string, string) SplitLine(string rawline){
        string name = "";
        string dialogue = "";
        string commands = "";
        //I heckin' love regex xD

        //Comment logic
        if (rawline.StartsWith("//")){
            return (name, dialogue, commands);
        }                 
        Match match = Regex.Match(rawline, FULLSTRING_PATTERN_W_COMMAND);
        if (match.Success){
                name = match.Groups["name"].Success ? match.Groups["name"].Value : "";
                name = name.Replace("\\", "");
                dialogue = match.Groups["dialogue"].Success ? match.Groups["dialogue"].Value : "";
                dialogue = dialogue.Replace("\\", "");
                commands = match.Groups["commands"].Success ? match.Groups["commands"].Value.Trim() : "";
                
        }
        if (!match.Groups["name"].Success){
            Debug.Log("Warning! Line: " + rawline + " Did not find match for speaker");
        }
        if (!match.Groups["dialogue"].Success){
            Debug.Log("Warning! Line: " + rawline + " Did not find match for dialogue");
        }
        return (name, dialogue, commands);
   }
}

}