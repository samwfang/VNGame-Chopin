using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

namespace DIALOGUE{
public class FileManager
{
    public static List<string> ReadTxtFile(string path, bool include_blank_lines = true){
        bool absolute_path = path.StartsWith('/');
        //force path to be root
        if (!absolute_path){
            path = FilePaths.root + path;
        }

        //return lines here
        List<string> lines = new List<string>();

        //open file and read everything in it, skipping lines if necessary
        try{
            using (StreamReader sr = new(path)){
                while (!sr.EndOfStream){
                    string line = sr.ReadLine();
                    if (include_blank_lines || !string.IsNullOrWhiteSpace(line)){
                        lines.Add(line);
                    }
                }
            }

        } catch (FileNotFoundException e){
            Debug.LogError("File not found: " + e.FileName);
        }
        return lines;
    }

    public static List<string> ReadTxtAsset(string path, bool include_blank_lines = true){
        TextAsset asset = Resources.Load<TextAsset>(path);
        if (asset == null){
            Debug.LogError("Asset Not Found" + path);
            return null;
        }

        return ReadTxtAsset(asset, include_blank_lines);
    }

    public static List<string> ReadTxtAsset(TextAsset asset, bool include_blank_lines = true){
        //return lines here
        List<string> lines = new List<string>();

        using (StringReader sr = new(asset.text)){
            while (sr.Peek() != -1){
                string line = sr.ReadLine();
                if (include_blank_lines || !string.IsNullOrWhiteSpace(line)){
                    lines.Add(line);
                }
            }
        }

        return lines;
    }

}
}