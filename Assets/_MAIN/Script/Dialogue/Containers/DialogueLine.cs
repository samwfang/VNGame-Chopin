using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DIALOGUE {

public class DialogueLine
{
    public DL_SpeakerData speaker_data;
    public bool has_speaker => (speaker_data != null);
    public DL_DialogueData dialogue_data;
    public bool has_dialogue => dialogue_data.hasDialogue;
    public DL_CommandData command_data;
    public bool has_commands => (command_data != null);

    public DialogueLine(string speaker, string dialogue, string commands){
        this.speaker_data = (string.IsNullOrWhiteSpace(speaker)) ? null : new DL_SpeakerData(speaker);
        this.dialogue_data = new DL_DialogueData(dialogue);
        this.command_data = (string.IsNullOrWhiteSpace(commands)) ? null : new DL_CommandData(commands);
    }
}
}
