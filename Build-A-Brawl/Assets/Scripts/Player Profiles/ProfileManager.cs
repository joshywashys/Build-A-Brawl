using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
This script manages player profiles- registering, deleting, etc.
*/
public class ProfileManager : MonoBehaviour
{
    private List<PlayerProfile> profiles = new List<PlayerProfile>();
    private const int MAX_PROFILE_COUNT = 8;


    //delete profile at index
    public void deleteProfile(int indexToRemove)
    {
        if (profiles.Count > indexToRemove)
        {
            profiles.RemoveAt(indexToRemove);
        }
        else
        {
            print("Failed to remove index " + indexToRemove);
        }
    }
    
    //register nameless profile
    public void registerProfile()
    {
        if (profiles.Count < MAX_PROFILE_COUNT)
        {
            PlayerProfile newProfile = new PlayerProfile();
            profiles.Add(newProfile);
        }
        else
        {
            print("List is already at max values.");
        }
    }

    //register profile with name
    public void registerProfile(string name)
    {
        if (profiles.Count < MAX_PROFILE_COUNT)
        {
            PlayerProfile newProfile = new PlayerProfile(name);
            profiles.Add(newProfile);
        }
        else
        {
            print("exceeds max!");
        }
    }

    public PlayerProfile getProfileAt(int indexOfProfile)
    {
        return profiles[indexOfProfile];
    }

    public List<PlayerProfile> getProfileList()
    {
        return profiles;
    }

    public int getNumProfiles()
    {
        return profiles.Count;
    }

    //NOTE: copy tristans stuff for this or find a tutorial that does something similar
    //loads profile list stored on the drive
    public void loadProfiles()
    {

    }

    //writes profile list to the drive - will this be autosaved or called when the game starts?
    public void saveProfiles()
    {

    }

    public void Start()
    {
        registerProfile("Gary");
        registerProfile("Alfred");
        print(profiles.Count);
    }

    //debugging
    public void printProfiles()
    {
        print(profiles[0]);
    }

}
