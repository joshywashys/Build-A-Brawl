using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using TMPro;

public class scoreBoard : MonoBehaviour
{
    // Start is called before the first frame update

    public RoundManager rm;
    public TextMeshProUGUI firstName;
    public TextMeshProUGUI firstScore;

    public TextMeshProUGUI secondName;
    public TextMeshProUGUI secondScore;

    public TextMeshProUGUI thirdName;
    public TextMeshProUGUI thirdScore;

    public TextMeshProUGUI fourthName;
    public TextMeshProUGUI fourthScore;

    public GameObject play1Spawn;
    public GameObject play2Spawn;
    public GameObject play3Spawn;
    public GameObject play4Spawn;
    
    public GameObject p1SpawnLoc;
    public GameObject p2SpawnLoc;
    public GameObject p3SpawnLoc;
    public GameObject p4SpawnLoc;

    public string firstNameVal;
    public decimal firstScoreVal;

    public string secondNameVal;
    public decimal secondScoreVal;

    public string thirdNameVal;
    public decimal thirdScoreVal;

    public string fourthNameVal;
    public decimal fourthScoreVal;

    public int playerCount;

    public List<RigidbodyController> rigidbodyControllers;

    void Start()
    {
        rm = GameObject.FindObjectOfType<RoundManager>();
        firstScoreVal = rm.stats[0].score;
        secondScoreVal = rm.stats[1].score;
        thirdScoreVal = rm.stats[2].score;
        fourthScoreVal = rm.stats[3].score;

        // firstScoreVal = 2;
        // secondScoreVal = 1;
        // thirdScoreVal = 2;
        // fourthScoreVal = 5;

        // playerCount = 4;

        firstNameVal = "Player1";
        secondNameVal = "Player2";
        thirdNameVal = "Player3";
        fourthNameVal = "Player4";
        playerCount = rm.players.Count;

        if (firstScoreVal == secondScoreVal || firstScoreVal == thirdScoreVal || firstScoreVal == fourthScoreVal){
            firstScoreVal += 0.3M;
        }
        if (secondScoreVal == thirdScoreVal || secondScoreVal == fourthScoreVal){
            secondScoreVal += 0.2M;
        }
        if (thirdScoreVal == fourthScoreVal){
            thirdScoreVal += 0.1M;
        }
        
        if (playerCount == 1){
            firstName.text = firstNameVal;
            firstScore.text = Mathf.Floor((float)firstScoreVal).ToString();
            play1Spawn.transform.position = p1SpawnLoc.transform.position;
        }
        else if (playerCount == 2){
            if (firstScoreVal > secondScoreVal){
                firstName.text = firstNameVal;
                firstScore.text = Mathf.Floor((float)firstScoreVal).ToString();
                play1Spawn.transform.position = p1SpawnLoc.transform.position;

                secondName.text = secondNameVal;
                secondScore.text = Mathf.Floor((float)secondScoreVal).ToString();
                play2Spawn.transform.position = p2SpawnLoc.transform.position;
            }
            else if (secondScoreVal > firstScoreVal){
                firstName.text = secondNameVal;
                firstScore.text = Mathf.Floor((float)secondScoreVal).ToString();
                play2Spawn.transform.position = p2SpawnLoc.transform.position;

                secondName.text = firstNameVal;
                secondScore.text = Mathf.Floor((float)firstScoreVal).ToString();
                play1Spawn.transform.position = p1SpawnLoc.transform.position;
            }

        }
        else if (playerCount == 3){
            if (firstScoreVal > secondScoreVal && firstScoreVal > thirdScoreVal){
                firstName.text = firstNameVal;
                firstScore.text = Mathf.Floor((float)firstScoreVal).ToString();
                play1Spawn.transform.position = p1SpawnLoc.transform.position;
                if (secondScoreVal>thirdScoreVal){
                    secondName.text = secondNameVal;
                    secondScore.text = Mathf.Floor((float)secondScoreVal).ToString();
                    play2Spawn.transform.position = p2SpawnLoc.transform.position;
                    
                    thirdName.text = thirdNameVal;
                    thirdScore.text = Mathf.Floor((float)thirdScoreVal).ToString();
                    play3Spawn.transform.position = p3SpawnLoc.transform.position;
                }
                else if (thirdScoreVal > secondScoreVal){
                    secondName.text = thirdNameVal;
                    secondScore.text = Mathf.Floor((float)thirdScoreVal).ToString();
                    play3Spawn.transform.position = p3SpawnLoc.transform.position;
                    
                    thirdName.text = secondNameVal;
                    thirdScore.text = Mathf.Floor((float)secondScoreVal).ToString();
                    play2Spawn.transform.position = p2SpawnLoc.transform.position;
                }
                
            }
            else if (secondScoreVal > firstScoreVal && secondScoreVal > thirdScoreVal){
                firstName.text = secondNameVal;
                firstScore.text = Mathf.Floor((float)secondScoreVal).ToString();
                play2Spawn.transform.position = p2SpawnLoc.transform.position;
                if (firstScoreVal>thirdScoreVal){
                    secondName.text = firstNameVal;
                    secondScore.text = Mathf.Floor((float)firstScoreVal).ToString();
                    play1Spawn.transform.position = p1SpawnLoc.transform.position;
                    
                    thirdName.text = thirdNameVal;
                    thirdScore.text = Mathf.Floor((float)thirdScoreVal).ToString();
                    play3Spawn.transform.position = p3SpawnLoc.transform.position;
                }
                else if (thirdScoreVal > firstScoreVal){
                    secondName.text = thirdNameVal;
                    secondScore.text = Mathf.Floor((float)thirdScoreVal).ToString();
                    play3Spawn.transform.position = p3SpawnLoc.transform.position;
                    
                    thirdName.text = firstNameVal;
                    thirdScore.text = Mathf.Floor((float)firstScoreVal).ToString();
                    play1Spawn.transform.position = p1SpawnLoc.transform.position;
                }
            }
            else if (thirdScoreVal > firstScoreVal && thirdScoreVal > secondScoreVal){
                firstName.text = thirdNameVal;
                firstScore.text = Mathf.Floor((float)thirdScoreVal).ToString();
                play3Spawn.transform.position = p3SpawnLoc.transform.position;
                if (firstScoreVal>secondScoreVal){
                    secondName.text = firstNameVal;
                    secondScore.text = Mathf.Floor((float)firstScoreVal).ToString();
                    play1Spawn.transform.position = p1SpawnLoc.transform.position;
                    
                    thirdName.text = secondNameVal;
                    thirdScore.text = Mathf.Floor((float)secondScoreVal).ToString();
                    play2Spawn.transform.position = p2SpawnLoc.transform.position;
                }
                else if (secondScoreVal > firstScoreVal){
                    secondName.text = secondNameVal;
                    secondScore.text = Mathf.Floor((float)secondScoreVal).ToString();
                    play2Spawn.transform.position = p2SpawnLoc.transform.position;
                    
                    thirdName.text = firstNameVal;
                    thirdScore.text = Mathf.Floor((float)firstScoreVal).ToString();
                    play1Spawn.transform.position = p1SpawnLoc.transform.position;
                }
            }
        }
        else if (playerCount == 4){
            if (firstScoreVal > secondScoreVal && firstScoreVal > thirdScoreVal && firstScoreVal > fourthScoreVal){
                firstName.text = firstNameVal;
                firstScore.text = Mathf.Floor((float)firstScoreVal).ToString();
                play1Spawn.transform.position = p1SpawnLoc.transform.position;
                if (secondScoreVal>thirdScoreVal && secondScoreVal > fourthScoreVal){
                    secondName.text = secondNameVal;
                    secondScore.text = Mathf.Floor((float)secondScoreVal).ToString();
                    play2Spawn.transform.position = p2SpawnLoc.transform.position;
                    if (thirdScoreVal > fourthScoreVal){
                        thirdName.text = thirdNameVal;
                        thirdScore.text = Mathf.Floor((float)thirdScoreVal).ToString();
                        play3Spawn.transform.position = p3SpawnLoc.transform.position;

                        fourthName.text = fourthNameVal;
                        fourthScore.text = Mathf.Floor((float)fourthScoreVal).ToString();
                        play4Spawn.transform.position = p4SpawnLoc.transform.position;
                    }
                    else if (fourthScoreVal > thirdScoreVal){
                        thirdName.text = fourthNameVal;
                        thirdScore.text = Mathf.Floor((float)fourthScoreVal).ToString();
                        play4Spawn.transform.position = p4SpawnLoc.transform.position;

                        fourthName.text = thirdNameVal;
                        fourthScore.text = Mathf.Floor((float)thirdScoreVal).ToString();
                        play3Spawn.transform.position = p3SpawnLoc.transform.position;
                    }
                }
                else if (thirdScoreVal>secondScoreVal && thirdScoreVal > fourthScoreVal){
                    secondName.text = thirdNameVal;
                    secondScore.text = Mathf.Floor((float)thirdScoreVal).ToString();
                    play2Spawn.transform.position = p2SpawnLoc.transform.position;
                    if (secondScoreVal > fourthScoreVal){
                        thirdName.text = secondNameVal;
                        thirdScore.text = Mathf.Floor((float)secondScoreVal).ToString();
                        play2Spawn.transform.position = p2SpawnLoc.transform.position;

                        fourthName.text = fourthNameVal;
                        fourthScore.text = Mathf.Floor((float)fourthScoreVal).ToString();
                        play4Spawn.transform.position = p4SpawnLoc.transform.position;
                    }
                    else if (fourthScoreVal > secondScoreVal){
                        thirdName.text = fourthNameVal;
                        thirdScore.text = Mathf.Floor((float)fourthScoreVal).ToString();
                        play4Spawn.transform.position = p4SpawnLoc.transform.position;

                        fourthName.text = secondNameVal;
                        fourthScore.text = Mathf.Floor((float)secondScoreVal).ToString();
                        play2Spawn.transform.position = p2SpawnLoc.transform.position;
                    }
                }
                else if (fourthScoreVal>secondScoreVal && fourthScoreVal > thirdScoreVal){
                    secondName.text = fourthNameVal;
                    secondScore.text = Mathf.Floor((float)fourthScoreVal).ToString();
                    play4Spawn.transform.position = p4SpawnLoc.transform.position;
                    if (secondScoreVal > thirdScoreVal){
                        thirdName.text = secondNameVal;
                        thirdScore.text = Mathf.Floor((float)secondScoreVal).ToString();
                        play2Spawn.transform.position = p2SpawnLoc.transform.position;

                        fourthName.text = thirdNameVal;
                        fourthScore.text = Mathf.Floor((float)thirdScoreVal).ToString();
                        play3Spawn.transform.position = p3SpawnLoc.transform.position;
                    }
                    else if (thirdScoreVal > secondScoreVal){
                        thirdName.text = thirdNameVal;
                        thirdScore.text = Mathf.Floor((float)thirdScoreVal).ToString();
                        play3Spawn.transform.position = p3SpawnLoc.transform.position;

                        fourthName.text = secondNameVal;
                        fourthScore.text = Mathf.Floor((float)secondScoreVal).ToString();
                        play2Spawn.transform.position = p2SpawnLoc.transform.position;
                    }
                }
            }
            else if (secondScoreVal > firstScoreVal && secondScoreVal > thirdScoreVal && secondScoreVal > fourthScoreVal){
                firstName.text = secondNameVal;
                firstScore.text = Mathf.Floor((float)secondScoreVal).ToString();
                play2Spawn.transform.position = p2SpawnLoc.transform.position;
                if (firstScoreVal>thirdScoreVal && firstScoreVal > fourthScoreVal){
                    secondName.text = firstNameVal;
                    secondScore.text = Mathf.Floor((float)firstScoreVal).ToString();
                    play1Spawn.transform.position = p1SpawnLoc.transform.position;
                    if (thirdScoreVal > fourthScoreVal){
                        thirdName.text = thirdNameVal;
                        thirdScore.text = Mathf.Floor((float)thirdScoreVal).ToString();
                        play3Spawn.transform.position = p3SpawnLoc.transform.position;

                        fourthName.text = fourthNameVal;
                        fourthScore.text = Mathf.Floor((float)fourthScoreVal).ToString();
                        play4Spawn.transform.position = p4SpawnLoc.transform.position;
                    }
                    else if (fourthScoreVal > thirdScoreVal){
                        thirdName.text = fourthNameVal;
                        thirdScore.text = Mathf.Floor((float)fourthScoreVal).ToString();
                        play4Spawn.transform.position = p4SpawnLoc.transform.position;

                        fourthName.text = thirdNameVal;
                        fourthScore.text = Mathf.Floor((float)thirdScoreVal).ToString();
                        play3Spawn.transform.position = p3SpawnLoc.transform.position;
                    }
                }
                else if (thirdScoreVal>firstScoreVal && thirdScoreVal > fourthScoreVal){
                    secondName.text = thirdNameVal;
                    secondScore.text = Mathf.Floor((float)thirdScoreVal).ToString();
                    play3Spawn.transform.position = p3SpawnLoc.transform.position;
                    if (firstScoreVal > fourthScoreVal){
                        thirdName.text = firstNameVal;
                        thirdScore.text = Mathf.Floor((float)firstScoreVal).ToString();
                        play1Spawn.transform.position = p1SpawnLoc.transform.position;

                        fourthName.text = fourthNameVal;
                        fourthScore.text = Mathf.Floor((float)fourthScoreVal).ToString();
                        play4Spawn.transform.position = p4SpawnLoc.transform.position;
                    }
                    else if (fourthScoreVal > firstScoreVal){
                        thirdName.text = fourthNameVal;
                        thirdScore.text = Mathf.Floor((float)fourthScoreVal).ToString();
                        play4Spawn.transform.position = p4SpawnLoc.transform.position;

                        fourthName.text = firstNameVal;
                        fourthScore.text = Mathf.Floor((float)firstScoreVal).ToString();
                        play1Spawn.transform.position = p1SpawnLoc.transform.position;
                    }
                }
                else if (fourthScoreVal>firstScoreVal && fourthScoreVal > thirdScoreVal){
                    secondName.text = fourthNameVal;
                    secondScore.text = Mathf.Floor((float)fourthScoreVal).ToString();
                    play4Spawn.transform.position = p4SpawnLoc.transform.position;
                    if (firstScoreVal > thirdScoreVal){
                        thirdName.text = firstNameVal;
                        thirdScore.text = Mathf.Floor((float)firstScoreVal).ToString();
                        play1Spawn.transform.position = p1SpawnLoc.transform.position;

                        fourthName.text = thirdNameVal;
                        fourthScore.text = Mathf.Floor((float)thirdScoreVal).ToString();
                        play3Spawn.transform.position = p3SpawnLoc.transform.position;
                    }
                    else if (thirdScoreVal > firstScoreVal){
                        thirdName.text = thirdNameVal;
                        thirdScore.text = Mathf.Floor((float)thirdScoreVal).ToString();
                        play3Spawn.transform.position = p3SpawnLoc.transform.position;

                        fourthName.text = firstNameVal;
                        fourthScore.text = Mathf.Floor((float)firstScoreVal).ToString();
                        play1Spawn.transform.position = p1SpawnLoc.transform.position;
                    }
                }
            }
            else if (thirdScoreVal > firstScoreVal && thirdScoreVal > secondScoreVal && thirdScoreVal > fourthScoreVal){
                firstName.text = thirdNameVal;
                firstScore.text = Mathf.Floor((float)thirdScoreVal).ToString();
                play3Spawn.transform.position = p3SpawnLoc.transform.position;
                if (firstScoreVal>secondScoreVal && firstScoreVal > fourthScoreVal){
                    secondName.text = firstNameVal;
                    secondScore.text = Mathf.Floor((float)firstScoreVal).ToString();
                    play1Spawn.transform.position = p1SpawnLoc.transform.position;
                    if (secondScoreVal > fourthScoreVal){
                        thirdName.text = secondNameVal;
                        thirdScore.text = Mathf.Floor((float)secondScoreVal).ToString();
                        play2Spawn.transform.position = p2SpawnLoc.transform.position;

                        fourthName.text = fourthNameVal;
                        fourthScore.text = Mathf.Floor((float)fourthScoreVal).ToString();
                        play4Spawn.transform.position = p4SpawnLoc.transform.position;
                    }
                    else if (fourthScoreVal > secondScoreVal){
                        thirdName.text = fourthNameVal;
                        thirdScore.text = Mathf.Floor((float)fourthScoreVal).ToString();
                        play4Spawn.transform.position = p4SpawnLoc.transform.position;

                        fourthName.text = secondNameVal;
                        fourthScore.text = Mathf.Floor((float)secondScoreVal).ToString();
                        play2Spawn.transform.position = p2SpawnLoc.transform.position;
                    }
                }
                else if (secondScoreVal>firstScoreVal && secondScoreVal > fourthScoreVal){
                    secondName.text = secondNameVal;
                    secondScore.text = Mathf.Floor((float)secondScoreVal).ToString();
                    play2Spawn.transform.position = p2SpawnLoc.transform.position;
                    if (firstScoreVal > fourthScoreVal){
                        thirdName.text = firstNameVal;
                        thirdScore.text = Mathf.Floor((float)firstScoreVal).ToString();
                        play1Spawn.transform.position = p1SpawnLoc.transform.position;

                        fourthName.text = fourthNameVal;
                        fourthScore.text = Mathf.Floor((float)fourthScoreVal).ToString();
                        play4Spawn.transform.position = p4SpawnLoc.transform.position;
                    }
                    else if (fourthScoreVal > firstScoreVal){
                        thirdName.text = fourthNameVal;
                        thirdScore.text = Mathf.Floor((float)fourthScoreVal).ToString();
                        play4Spawn.transform.position = p4SpawnLoc.transform.position;

                        fourthName.text = firstNameVal;
                        fourthScore.text = Mathf.Floor((float)firstScoreVal).ToString();
                        play1Spawn.transform.position = p1SpawnLoc.transform.position;
                    }
                }
                else if (fourthScoreVal>firstScoreVal && fourthScoreVal > secondScoreVal){
                    secondName.text = fourthNameVal;
                    secondScore.text = Mathf.Floor((float)fourthScoreVal).ToString();
                    play4Spawn.transform.position = p4SpawnLoc.transform.position;
                    if (firstScoreVal > secondScoreVal){
                        thirdName.text = firstNameVal;
                        thirdScore.text = Mathf.Floor((float)firstScoreVal).ToString();
                        play1Spawn.transform.position = p1SpawnLoc.transform.position;

                        fourthName.text = secondNameVal;
                        fourthScore.text = Mathf.Floor((float)secondScoreVal).ToString();
                        play2Spawn.transform.position = p2SpawnLoc.transform.position;
                    }
                    else if (secondScoreVal > firstScoreVal){
                        thirdName.text = secondNameVal;
                        thirdScore.text = Mathf.Floor((float)secondScoreVal).ToString();
                        play2Spawn.transform.position = p2SpawnLoc.transform.position;

                        fourthName.text = firstNameVal;
                        fourthScore.text = Mathf.Floor((float)firstScoreVal).ToString();
                        play1Spawn.transform.position = p1SpawnLoc.transform.position;
                    }
                }
            }
            else if (fourthScoreVal > firstScoreVal && fourthScoreVal > secondScoreVal && fourthScoreVal > thirdScoreVal){
                firstName.text = fourthNameVal;
                firstScore.text = Mathf.Floor((float)fourthScoreVal).ToString();
                play4Spawn.transform.position = p4SpawnLoc.transform.position;
                if (firstScoreVal>secondScoreVal && firstScoreVal > thirdScoreVal){
                    secondName.text = firstNameVal;
                    secondScore.text = Mathf.Floor((float)firstScoreVal).ToString();
                    play1Spawn.transform.position = p1SpawnLoc.transform.position;
                    if (secondScoreVal > thirdScoreVal){
                        thirdName.text = secondNameVal;
                        thirdScore.text = Mathf.Floor((float)secondScoreVal).ToString();
                        play2Spawn.transform.position = p2SpawnLoc.transform.position;

                        fourthName.text = thirdNameVal;
                        fourthScore.text = Mathf.Floor((float)thirdScoreVal).ToString();
                        play3Spawn.transform.position = p3SpawnLoc.transform.position;
                    }
                    else if (thirdScoreVal > secondScoreVal){
                        thirdName.text = thirdNameVal;
                        thirdScore.text = Mathf.Floor((float)thirdScoreVal).ToString();
                        play3Spawn.transform.position = p3SpawnLoc.transform.position;

                        fourthName.text = secondNameVal;
                        fourthScore.text = Mathf.Floor((float)secondScoreVal).ToString();
                        play2Spawn.transform.position = p2SpawnLoc.transform.position;
                    }
                }
                else if (secondScoreVal>firstScoreVal && secondScoreVal > thirdScoreVal){
                    secondName.text = secondNameVal;
                    secondScore.text = Mathf.Floor((float)secondScoreVal).ToString();
                    play2Spawn.transform.position = p2SpawnLoc.transform.position;
                    if (firstScoreVal > thirdScoreVal){
                        thirdName.text = firstNameVal;
                        thirdScore.text = Mathf.Floor((float)firstScoreVal).ToString();
                        play1Spawn.transform.position = p1SpawnLoc.transform.position;

                        fourthName.text = thirdNameVal;
                        fourthScore.text = Mathf.Floor((float)thirdScoreVal).ToString();
                        play3Spawn.transform.position = p3SpawnLoc.transform.position;
                    }
                    else if (thirdScoreVal > firstScoreVal){
                        thirdName.text = thirdNameVal;
                        thirdScore.text = Mathf.Floor((float)thirdScoreVal).ToString();
                        play3Spawn.transform.position = p3SpawnLoc.transform.position;

                        fourthName.text = firstNameVal;
                        fourthScore.text = Mathf.Floor((float)firstScoreVal).ToString();
                        play1Spawn.transform.position = p1SpawnLoc.transform.position;
                    }
                }
                else if (thirdScoreVal>firstScoreVal && thirdScoreVal > secondScoreVal){
                    secondName.text = thirdNameVal;
                    secondScore.text = Mathf.Floor((float)thirdScoreVal).ToString();
                    play3Spawn.transform.position = p3SpawnLoc.transform.position;
                    if (firstScoreVal > secondScoreVal){
                        thirdName.text = firstNameVal;
                        thirdScore.text = Mathf.Floor((float)firstScoreVal).ToString();
                        play1Spawn.transform.position = p1SpawnLoc.transform.position;

                        fourthName.text = secondNameVal;
                        fourthScore.text = Mathf.Floor((float)secondScoreVal).ToString();
                        play2Spawn.transform.position = p2SpawnLoc.transform.position;
                    }
                    else if (secondScoreVal > firstScoreVal){
                        thirdName.text = secondNameVal;
                        thirdScore.text = Mathf.Floor((float)secondScoreVal).ToString();
                        play2Spawn.transform.position = p2SpawnLoc.transform.position;

                        fourthName.text = firstNameVal;
                        fourthScore.text = Mathf.Floor((float)firstScoreVal).ToString();
                        play1Spawn.transform.position = p1SpawnLoc.transform.position;
                    }
                }
            }
            
        }
        Debug.Log(firstNameVal + " " + firstScoreVal);
        Debug.Log(secondNameVal + " " + secondScoreVal);
        Debug.Log(thirdNameVal + " " + thirdScoreVal);
        Debug.Log(fourthNameVal + " " + fourthScoreVal);
        Debug.Log(playerCount);

        rigidbodyControllers = new List<RigidbodyController>();
        RigidbodyController[] searchList = FindObjectsOfType<RigidbodyController>();
        for (int i = 0; i < searchList.Length; i++)
        {
            rigidbodyControllers.Add(searchList[i]);
        }

        foreach (RigidbodyController rbc in rigidbodyControllers)
        {
            int playerNum = rbc.GetComponentInChildren<CreatureStats>().GetPlayerNum();
            if (playerNum == rm.players[0].GetPlayerNum())
            {
                rbc.gameObject.transform.position = play1Spawn.transform.position;
            }
            if (playerNum == rm.players[1].GetPlayerNum())
            {
                rbc.gameObject.transform.position = play2Spawn.transform.position;
            }
            if (playerNum == rm.players[2].GetPlayerNum())
            {
                rbc.gameObject.transform.position = play3Spawn.transform.position;
            }
            if (playerNum == rm.players[3].GetPlayerNum())
            {
                rbc.gameObject.transform.position = play4Spawn.transform.position;
            }
        }

    }
}
