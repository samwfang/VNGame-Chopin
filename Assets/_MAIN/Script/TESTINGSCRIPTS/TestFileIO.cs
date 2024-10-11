using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DIALOGUE;

public class TestFileIO : MonoBehaviour
{
    //private string filename = "testFile.txt";
    //private string filename = "testFile";
    [SerializeField] private TextAsset filename;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Run());
    }

    IEnumerator Run(){
        List<string> lines = FileManager.ReadTxtAsset(filename, false);
        //List<string> lines = FileManager.ReadTxtFile(filename, false);

        foreach(string line in lines){
            Debug.Log(line);
        }
        yield return null;
    }
}
