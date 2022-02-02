using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
PlayerProfile is what the players use to sign in and have their own saved/loaded creatures.
It will also keep track of their statistics, and the loading/saving of them.

I'm going to use this is store their key/controller bindings if they changed them. Think like smash brothers - Tristan
*/
[System.Serializable]
public class PlayerProfile
{
    // Unregistered Player
    public const string DEFAULT_NAME = "noname";
    public bool IsDefault() => playerName == DEFAULT_NAME;

    // Player Data
    public string playerName = DEFAULT_NAME;
    //creature data

    public PlayerProfile(string name)
    {
        playerName = name;
    }

    public PlayerProfile()
    {
    
    }


    // Key/Controller Bind Profile

    public class KeyBinding
    {

    }

    public class GamepadBinding
    {

    }
}
