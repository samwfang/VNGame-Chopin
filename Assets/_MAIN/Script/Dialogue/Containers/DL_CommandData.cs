using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using UnityEngine;

public class DL_CommandData
{
    public List<Command> commandlist;
    private const string COMMAND_SPLIT = @"(\w+)\(([^)]*)\)";
    private const char LEFT_PAREN = '(';

    //Command Struct
    // Format: Command(Arg1, Arg2 = val, Arg3 = val)
   public struct Command
    {
        public string name;
        public string[] args;
    }

    
    public DL_CommandData(string rawcommands)
    {
        //Split Data by Regex
        List<Command> cur_commands = new List<Command>();

        var matches = Regex.Matches(rawcommands, COMMAND_SPLIT);
        if (matches.Count == 0){
            Debug.Log("Warning: No Matches Found For Non-Null Command Line: " + rawcommands + ". Please Check for Malformed Commands");
        }

        //Add each command string to commandlist
        foreach (Match match in matches){
            Command cmd = new();
            cmd.name = match.Groups[1].Value;
            string rawargs = match.Groups[2].Value;
            cmd.args = rawargs.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            for (int i = 0; i < cmd.args.Length; i++)
            {
                cmd.args[i] = cmd.args[i].Trim();
            }
            cur_commands.Add(cmd);
        }
        commandlist = cur_commands;

    }
}
