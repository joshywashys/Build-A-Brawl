using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using TMPro;

public class scoreBoard : MonoBehaviour
{
    // Start is called before the first frame update
    public TextMeshProUGUI firstName;
    public TextMeshProUGUI firstScore;

    public TextMeshProUGUI secondName;
    public TextMeshProUGUI secondScore;

    public TextMeshProUGUI thirdName;
    public TextMeshProUGUI thirdScore;

    public TextMeshProUGUI fourthName;
    public TextMeshProUGUI fourthScore;

    public string firstNameVal;
    public int firstScoreVal;

    public string secondNameVal;
    public int secondScoreVal;

    public string thirdNameVal;
    public int thirdScoreVal;

    public string fourthNameVal;
    public int fourthScoreVal;

    public int playerCount;

    void Start()
    {
        if (playerCount == 1){
            firstName.text = firstNameVal;
            firstScore.text = firstScoreVal.ToString();
        }
        else if (playerCount == 2){
            if (firstScoreVal > secondScoreVal){
                firstName.text = firstNameVal;
                firstScore.text = firstScoreVal.ToString();

                secondName.text = secondNameVal;
                secondScore.text = secondScoreVal.ToString();
            }
            else if (secondScoreVal > firstScoreVal){
                firstName.text = secondNameVal;
                firstScore.text = secondScoreVal.ToString();

                secondName.text = firstNameVal;
                secondScore.text = firstScoreVal.ToString();
            }

        }
        else if (playerCount == 3){
            if (firstScoreVal > secondScoreVal && firstScoreVal > thirdScoreVal){
                firstName.text = firstNameVal;
                firstScore.text = firstScoreVal.ToString();
                if (secondScoreVal>thirdScoreVal){
                    secondName.text = secondNameVal;
                    secondScore.text = secondScoreVal.ToString();
                    
                    thirdName.text = thirdNameVal;
                    thirdScore.text = thirdScoreVal.ToString();
                }
                else if (thirdScoreVal > secondScoreVal){
                    secondName.text = thirdNameVal;
                    secondScore.text = thirdScoreVal.ToString();
                    
                    thirdName.text = secondNameVal;
                    thirdScore.text = secondScoreVal.ToString();
                }
                
            }
            else if (secondScoreVal > firstScoreVal && secondScoreVal > thirdScoreVal){
                firstName.text = secondNameVal;
                firstScore.text = secondScoreVal.ToString();
                if (firstScoreVal>thirdScoreVal){
                    secondName.text = firstNameVal;
                    secondScore.text = firstScoreVal.ToString();
                    
                    thirdName.text = thirdNameVal;
                    thirdScore.text = thirdScoreVal.ToString();
                }
                else if (thirdScoreVal > secondScoreVal){
                    secondName.text = thirdNameVal;
                    secondScore.text = thirdScoreVal.ToString();
                    
                    thirdName.text = firstNameVal;
                    thirdScore.text = firstScoreVal.ToString();
                }
            }
            else if (thirdScoreVal > firstScoreVal && thirdScoreVal > secondScoreVal){
                firstName.text = thirdNameVal;
                firstScore.text = thirdScoreVal.ToString();
                if (firstScoreVal>secondScoreVal){
                    secondName.text = firstNameVal;
                    secondScore.text = firstScoreVal.ToString();
                    
                    thirdName.text = secondNameVal;
                    thirdScore.text = secondScoreVal.ToString();
                }
                else if (secondScoreVal > firstScoreVal){
                    secondName.text = secondNameVal;
                    secondScore.text = secondScore.ToString();
                    
                    thirdName.text = firstNameVal;
                    thirdScore.text = firstScoreVal.ToString();
                }
            }
        }
        else if (playerCount == 4){
            if (firstScoreVal > secondScoreVal && firstScoreVal > thirdScoreVal && firstScoreVal > fourthScoreVal){
                firstName.text = firstNameVal;
                firstScore.text = firstScoreVal.ToString();
                if (secondScoreVal>thirdScoreVal && secondScoreVal > fourthScoreVal){
                    secondName.text = secondNameVal;
                    secondScore.text = secondScoreVal.ToString();
                    if (thirdScoreVal > fourthScoreVal){
                        thirdName.text = thirdNameVal;
                        thirdScore.text = thirdScoreVal.ToString();

                        fourthName.text = fourthNameVal;
                        fourthScore.text = fourthScoreVal.ToString();
                    }
                    else if (fourthScoreVal > thirdScoreVal){
                        thirdName.text = fourthNameVal;
                        thirdScore.text = fourthScoreVal.ToString();

                        fourthName.text = thirdNameVal;
                        fourthScore.text = thirdScoreVal.ToString();
                    }
                }
                else if (thirdScoreVal>secondScoreVal && thirdScoreVal > fourthScoreVal){
                    secondName.text = thirdNameVal;
                    secondScore.text = thirdScoreVal.ToString();
                    if (secondScoreVal > fourthScoreVal){
                        thirdName.text = secondNameVal;
                        thirdScore.text = secondScoreVal.ToString();

                        fourthName.text = fourthNameVal;
                        fourthScore.text = fourthScoreVal.ToString();
                    }
                    else if (fourthScoreVal > secondScoreVal){
                        thirdName.text = fourthNameVal;
                        thirdScore.text = fourthScoreVal.ToString();

                        fourthName.text = secondNameVal;
                        fourthScore.text = secondScoreVal.ToString();
                    }
                }
                else if (fourthScoreVal>secondScoreVal && fourthScoreVal > thirdScoreVal){
                    secondName.text = fourthNameVal;
                    secondScore.text = fourthScoreVal.ToString();
                    if (secondScoreVal > thirdScoreVal){
                        thirdName.text = secondNameVal;
                        thirdScore.text = secondScoreVal.ToString();

                        fourthName.text = thirdNameVal;
                        fourthScore.text = thirdScoreVal.ToString();
                    }
                    else if (thirdScoreVal > secondScoreVal){
                        thirdName.text = thirdNameVal;
                        thirdScore.text = thirdScoreVal.ToString();

                        fourthName.text = secondNameVal;
                        fourthScore.text = secondScoreVal.ToString();
                    }
                }
            }
            else if (secondScoreVal > firstScoreVal && secondScoreVal > thirdScoreVal && secondScoreVal > fourthScoreVal){
                firstName.text = secondNameVal;
                firstScore.text = secondScoreVal.ToString();
                if (firstScoreVal>thirdScoreVal && firstScoreVal > fourthScoreVal){
                    secondName.text = firstNameVal;
                    secondScore.text = firstScoreVal.ToString();
                    if (thirdScoreVal > fourthScoreVal){
                        thirdName.text = thirdNameVal;
                        thirdScore.text = thirdScoreVal.ToString();

                        fourthName.text = fourthNameVal;
                        fourthScore.text = fourthScoreVal.ToString();
                    }
                    else if (fourthScoreVal > thirdScoreVal){
                        thirdName.text = fourthNameVal;
                        thirdScore.text = fourthScoreVal.ToString();

                        fourthName.text = thirdNameVal;
                        fourthScore.text = thirdScoreVal.ToString();
                    }
                }
                else if (thirdScoreVal>firstScoreVal && thirdScoreVal > fourthScoreVal){
                    secondName.text = thirdNameVal;
                    secondScore.text = thirdScoreVal.ToString();
                    if (firstScoreVal > fourthScoreVal){
                        thirdName.text = firstNameVal;
                        thirdScore.text = firstScoreVal.ToString();

                        fourthName.text = fourthNameVal;
                        fourthScore.text = fourthScoreVal.ToString();
                    }
                    else if (fourthScoreVal > firstScoreVal){
                        thirdName.text = fourthNameVal;
                        thirdScore.text = fourthScoreVal.ToString();

                        fourthName.text = firstNameVal;
                        fourthScore.text = firstScoreVal.ToString();
                    }
                }
                else if (fourthScoreVal>firstScoreVal && fourthScoreVal > thirdScoreVal){
                    secondName.text = fourthNameVal;
                    secondScore.text = fourthScoreVal.ToString();
                    if (firstScoreVal > thirdScoreVal){
                        thirdName.text = firstNameVal;
                        thirdScore.text = firstScoreVal.ToString();

                        fourthName.text = thirdNameVal;
                        fourthScore.text = thirdScoreVal.ToString();
                    }
                    else if (thirdScoreVal > firstScoreVal){
                        thirdName.text = thirdNameVal;
                        thirdScore.text = thirdScoreVal.ToString();

                        fourthName.text = firstNameVal;
                        fourthScore.text = firstScoreVal.ToString();
                    }
                }
            }
            else if (thirdScoreVal > firstScoreVal && secondScoreVal > secondScoreVal && thirdScoreVal > fourthScoreVal){
                firstName.text = thirdNameVal;
                firstScore.text = thirdScoreVal.ToString();
                if (firstScoreVal>secondScoreVal && firstScoreVal > fourthScoreVal){
                    secondName.text = firstNameVal;
                    secondScore.text = firstScoreVal.ToString();
                    if (secondScoreVal > fourthScoreVal){
                        thirdName.text = secondNameVal;
                        thirdScore.text = secondScoreVal.ToString();

                        fourthName.text = fourthNameVal;
                        fourthScore.text = fourthScoreVal.ToString();
                    }
                    else if (fourthScoreVal > secondScoreVal){
                        thirdName.text = fourthNameVal;
                        thirdScore.text = fourthScoreVal.ToString();

                        fourthName.text = secondNameVal;
                        fourthScore.text = secondScoreVal.ToString();
                    }
                }
                else if (secondScoreVal>firstScoreVal && secondScoreVal > fourthScoreVal){
                    secondName.text = secondNameVal;
                    secondScore.text = secondScoreVal.ToString();
                    if (firstScoreVal > fourthScoreVal){
                        thirdName.text = firstNameVal;
                        thirdScore.text = firstScoreVal.ToString();

                        fourthName.text = fourthNameVal;
                        fourthScore.text = fourthScoreVal.ToString();
                    }
                    else if (fourthScoreVal > firstScoreVal){
                        thirdName.text = fourthNameVal;
                        thirdScore.text = fourthScoreVal.ToString();

                        fourthName.text = firstNameVal;
                        fourthScore.text = firstScoreVal.ToString();
                    }
                }
                else if (fourthScoreVal>firstScoreVal && fourthScoreVal > secondScoreVal){
                    secondName.text = fourthNameVal;
                    secondScore.text = fourthScoreVal.ToString();
                    if (firstScoreVal > secondScoreVal){
                        thirdName.text = firstNameVal;
                        thirdScore.text = firstScoreVal.ToString();

                        fourthName.text = thirdNameVal;
                        fourthScore.text = thirdScoreVal.ToString();
                    }
                    else if (secondScoreVal > firstScoreVal){
                        thirdName.text = secondNameVal;
                        thirdScore.text = secondScoreVal.ToString();

                        fourthName.text = firstNameVal;
                        fourthScore.text = firstScoreVal.ToString();
                    }
                }
            }
            else if (fourthScoreVal > firstScoreVal && fourthScoreVal > secondScoreVal && fourthScoreVal > thirdScoreVal){
                firstName.text = fourthNameVal;
                firstScore.text = fourthScoreVal.ToString();
                if (firstScoreVal>secondScoreVal && firstScoreVal > thirdScoreVal){
                    secondName.text = firstNameVal;
                    secondScore.text = firstScoreVal.ToString();
                    if (secondScoreVal > thirdScoreVal){
                        thirdName.text = secondNameVal;
                        thirdScore.text = secondScoreVal.ToString();

                        fourthName.text = thirdNameVal;
                        fourthScore.text = thirdScoreVal.ToString();
                    }
                    else if (thirdScoreVal > secondScoreVal){
                        thirdName.text = thirdNameVal;
                        thirdScore.text = thirdScoreVal.ToString();

                        fourthName.text = secondNameVal;
                        fourthScore.text = secondScoreVal.ToString();
                    }
                }
                else if (secondScoreVal>firstScoreVal && secondScoreVal > thirdScoreVal){
                    secondName.text = secondNameVal;
                    secondScore.text = secondScoreVal.ToString();
                    if (firstScoreVal > thirdScoreVal){
                        thirdName.text = firstNameVal;
                        thirdScore.text = firstScoreVal.ToString();

                        fourthName.text = thirdNameVal;
                        fourthScore.text = thirdScoreVal.ToString();
                    }
                    else if (thirdScoreVal > firstScoreVal){
                        thirdName.text = thirdNameVal;
                        thirdScore.text = thirdScoreVal.ToString();

                        fourthName.text = firstNameVal;
                        fourthScore.text = firstScoreVal.ToString();
                    }
                }
                else if (thirdScoreVal>firstScoreVal && thirdScoreVal > secondScoreVal){
                    secondName.text = thirdNameVal;
                    secondScore.text = thirdScoreVal.ToString();
                    if (firstScoreVal > secondScoreVal){
                        thirdName.text = firstNameVal;
                        thirdScore.text = firstScoreVal.ToString();

                        fourthName.text = thirdNameVal;
                        fourthScore.text = thirdScoreVal.ToString();
                    }
                    else if (secondScoreVal > firstScoreVal){
                        thirdName.text = secondNameVal;
                        thirdScore.text = secondScoreVal.ToString();

                        fourthName.text = firstNameVal;
                        fourthScore.text = firstScoreVal.ToString();
                    }
                }
            }
            
        }
    }
}
