using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.Events;

namespace BuildABrawl.Events
{

    public class brawlButton : MonoBehaviour, ISubmitHandler, ICancelHandler
    {
        UnityEvent buttonBrawl;
        public bool buttonChk;
        [SerializeField] private IntEvent locationNum;

        [SerializeField] private int locationNumber = 1;

        void Start()
        {
            if (buttonBrawl == null)
            {
                buttonBrawl = new UnityEvent();
            }

        }

        public void OnSubmit(BaseEventData eventData)
        {
            buttonChk = true;
            //Debug.Log("OK IT IS TRUE" + locationNumber);
            locationNum.Raise(locationNumber);

        }

        public void OnCancel(BaseEventData eventData)
        {
            //Debug.Log("It is FALsE");
        }


    }
}