using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WordTrigger : MonoBehaviour
{
    public GameObject[] PlacedAdjectiveWords = new GameObject[2];
    public GameObject[] PlacedNounWords = new GameObject[2];

    public Vector3[] AdjectivePositions = new Vector3[2];       //형용사카드 배치위치
    public Vector3[] NounPositions = new Vector3[2];            //명사카드 배치위치

    public GameObject TriggeredAdjective;       //트리거된 첫번째 형용사카드
    public GameObject TriggeredNoun;            //트리거된 첫번째 명사카드

    public GameObject SecondTriggeredAdjective;  //트리거된 두번째 형용사카드
    public GameObject SecondTriggeredNoun;    //트리거된 두번째 명사카드

    private void OnTriggerEnter(Collider other)
    {
        if(GameManager.instance.PlaceWords && other.CompareTag("Adjective"))
        {
            GameManager.instance.FirstAdjectiveNumbers++;
        }

        if(GameManager.instance.PlaceWords && other.CompareTag("Noun"))
        {
            GameManager.instance.FirstNounNumbers++;
        }

        if(other.CompareTag("Adjective") || other.CompareTag("Noun"))
        {
            if(GameManager.instance.FirstGuessComplete)
            {
                GameManager.instance.SecondWordNumber++;
            }
            else if(GameManager.instance.SecondGuessComplete)
            {
                GameManager.instance.LastWordNumber++;
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if(GameManager.instance.PlaceWords && other.CompareTag("Adjective"))
        {
            TriggeredAdjective = other.gameObject;
        }

        if(GameManager.instance.PlaceWords && other.CompareTag("Noun"))
        {
            TriggeredNoun = other.gameObject;
        }

        if(GameManager.instance.FirstGuessComplete)
        {
            if(other.CompareTag("Adjective"))
            {
                SecondTriggeredAdjective = other.gameObject;
            }
            else if(other.CompareTag("Noun"))
            {
                SecondTriggeredNoun = other.gameObject;
            }
        }

        if(GameManager.instance.SecondGuessComplete)
        {
            if (other.CompareTag("Adjective") && SecondTriggeredAdjective != null)
                return;
            else if (other.CompareTag("Adjective") && SecondTriggeredAdjective == null)
            {
                SecondTriggeredAdjective = other.gameObject;
            }             

            if (other.CompareTag("Noun") && SecondTriggeredNoun != null)
                return;
            else if (other.CompareTag("Noun") && SecondTriggeredNoun == null)
            {
                SecondTriggeredNoun = other.gameObject;
            }            
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(GameManager.instance.PlaceWords && other.CompareTag("Adjective"))
        {
            GameManager.instance.FirstAdjectiveNumbers--;
            TriggeredAdjective = null;
        }

        if(GameManager.instance.PlaceWords && other.CompareTag("Noun"))
        {
            GameManager.instance.FirstNounNumbers--;
            TriggeredNoun = null;
        }

        if(other.CompareTag("Adjective"))
        {
            if(GameManager.instance.FirstGuessComplete)
            {
                GameManager.instance.SecondWordNumber--;
               
                SecondTriggeredAdjective = null;
            }
            else if(GameManager.instance.SecondGuessComplete)
            {
                GameManager.instance.LastWordNumber--;                

                SecondTriggeredAdjective = null;
            }
        }

        if (other.CompareTag("Noun"))
        {
            if (GameManager.instance.FirstGuessComplete)
            {
                GameManager.instance.SecondWordNumber--;
                SecondTriggeredNoun = null;
            }
            else if (GameManager.instance.SecondGuessComplete)
            {
                GameManager.instance.LastWordNumber--;
                
                SecondTriggeredNoun = null;
            }
        }
    }
}
