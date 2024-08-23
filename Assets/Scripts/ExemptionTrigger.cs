using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExemptionTrigger : MonoBehaviour
{
    public GameObject TriggeredCard;        //트리거된 카드

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Card"))
        {
            GameManager.instance.CardNumbers++;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if(other.CompareTag("Card"))
        {
            TriggeredCard = other.gameObject;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.CompareTag("Card"))
        {
            GameManager.instance.CardNumbers--;
            TriggeredCard = null;
        }
    }
}
