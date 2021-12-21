using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
PlayerProfile is what the players use to sign in and have their own saved/loaded creatures.
It will also keep track of their statistics, and the loading/saving of them.
*/
[System.Serializable]
public class PlayerProfile
{
    private const string DEFAULT_NAME = "noname";
    private string playerName = DEFAULT_NAME;
    //creature data

    public PlayerProfile(string name)
    {
        playerName = name;
    }

    public PlayerProfile()
    {

    }

}
