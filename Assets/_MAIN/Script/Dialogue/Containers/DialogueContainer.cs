using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

/*
Defines Dialogue Container as an object with root layer, name tag and dialogue text in-game
*/

namespace DIALOGUE{
[System.Serializable] public class DialogueContainer{
    public GameObject root;
    public NameContainer nameContainer;
    public TextMeshProUGUI dialogueText;
}
}