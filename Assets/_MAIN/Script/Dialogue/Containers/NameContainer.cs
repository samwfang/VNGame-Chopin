using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
namespace DIALOGUE{
[System.Serializable]
public class NameContainer
{
    [SerializeField] private GameObject box;
    [SerializeField] private TextMeshProUGUI nametext;
    public void Show(string name_to_show = ""){
        box.SetActive(true);

        if (name_to_show != string.Empty){
            nametext.text = name_to_show;
        }
    }

    public void Hide(){
        box.SetActive(false);
    }
}
}
