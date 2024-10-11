using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEditor.Rendering.Universal;
using UnityEngine;

namespace DIALOGUE{
public class DL_SpeakerData
{
    //name of character speaking
    public string name;
    //name to display
    public string display_name;
    //position of speaker
    public Vector2 cast_position;
    //expression to use
    public List<(int layer, string expression)> CastExpressions{get; set; }

    public const string SPEAKER_PATTERN = @"(?<name>\w+)(?:\s+as\s+(?<display_name>\w+))?(?:\s+at\s+(?<coords>\d+;\d+))?(?:\s+\[(?<expressions>[^\]]+)\])?";

    public DL_SpeakerData(string rawspeaker){
        Regex speaker_regex = new Regex(SPEAKER_PATTERN);
        Match match = speaker_regex.Match(rawspeaker);
        CastExpressions = new();

        if (!match.Success){
            name = rawspeaker;
            display_name = "";
            cast_position = Vector2.zero;
        }
        else {
            name = match.Groups["name"].Value;

            display_name = match.Groups["display_name"].Success ?
                            match.Groups["display_name"].Value : name;
            
            cast_position = Vector2.zero;
            if (match.Groups["coords"].Success){
                string[] coordParts = match.Groups["coords"].Value.Split(";");
                cast_position = new(float.Parse(coordParts[0]), float.Parse(coordParts[1]));
            }

            if (match.Groups["expressions"].Success){
                string[] expr_parts = match.Groups["expressions"].Value.Split(",");
                foreach (var part in expr_parts){
                    string[] layer_expression = part.Trim().Split(";");
                    CastExpressions.Add(((int.Parse(layer_expression[0])),layer_expression[1]));
                }
            }
        }
    }
}
}