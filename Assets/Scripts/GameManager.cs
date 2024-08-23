using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject FinalJobCard;         //최종 직업카드
    public GameObject FinalToolCard;        //최종 도구카드
    public GameObject FinalPurposeCard;     //최종 동기카드

    public bool PlaceWords;             //초기 단어카드 배치
    public int FirstAdjectiveNumbers;   //초기 배치한 형용사카드 숫자
    public int FirstNounNumbers;        //초기 배치한 명사카드 숫자

    public int SecondWordNumber;        //Second Round 단어카드 숫자 카운트
    public int LastWordNumber;          //Last Round 단어카드 숫자 카운트

    public int CardNumbers;        //카드 숫자(제외토큰와 트러거된 카드)
    public GameObject[] GuessedCards = new GameObject[6];       //제외할 카드 오브젝트

    public int GuessCount = 0;      //추리실패 카운트 변수

    public bool FirstRoundStart;     //첫번째 추리시작
    public bool SecondRoundStart;    //두번째 추리시작
    public bool LastRoundStart;      //마지막 추리시작
    public bool FinalGuessStart;     //최종 추리시작 

    public bool FirstGuessComplete;     //첫번째 추리성공
    public bool SecondGuessComplete;    //두번째 추리성공
    public bool LastGuessComplete;      //마지막 추리성공
    public bool GameComplete;     //추리성공, 게임완료

    public GameObject JobCardsViewer;     //직업뷰어카드
    public GameObject ToolCardsViewer;     //도구뷰어카드
    public GameObject PurposeCardsViewer;     //동기뷰어카드

    public GameObject ExemptionTokensPrevention;     //제외토큰 이동제한 
    public GameObject WordsPrevention;     //명사&형용사 이동제한

    UIManager _UIManager;

    public static GameManager instance;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }

        _UIManager = GameObject.Find("UIManager").GetComponent<UIManager>();        
    }

    void Update()
    {
        CountWordCards();
        CountExemptionCards();
        PreventDragging();

        if((FirstRoundStart || SecondRoundStart || LastRoundStart))
        {
            JobCardsViewer.SetActive(true);
            ToolCardsViewer.SetActive(true);
            PurposeCardsViewer.SetActive(true);
        }
        else if((!FirstRoundStart || !SecondRoundStart || !LastRoundStart))
        {
            JobCardsViewer.SetActive(false);
            ToolCardsViewer.SetActive(false);
            PurposeCardsViewer.SetActive(false);
        }

        if(GuessCount == 4)
        {
            _UIManager.SelectJobCard.gameObject.SetActive(true);
            _UIManager.SelectToolCard.gameObject.SetActive(true);
            _UIManager.SelectPurposeCard.gameObject.SetActive(true);

            _UIManager.GameCompleteText.gameObject.SetActive(true);
            _UIManager.GameCompleteText.text = "Investigation Failed";
            _UIManager.GameCompleteRegameButton.gameObject.SetActive(true);
        }

        if ((FirstRoundStart || SecondRoundStart || LastRoundStart))
        {
            _UIManager.HelpButton.gameObject.SetActive(true);
            _UIManager.PauseButton.gameObject.SetActive(true);
        }
        else if ((!FirstRoundStart || !SecondRoundStart || !LastRoundStart))
        {
            _UIManager.HelpButton.gameObject.SetActive(false);
            _UIManager.PauseButton.gameObject.SetActive(false);
        }
    }

    //제외토큰 & 단어카드 이동방지
    void PreventDragging()
    {
        if ((FirstRoundStart || SecondRoundStart || LastRoundStart) || FinalGuessStart)
        {
            WordsPrevention.SetActive(true);
            ExemptionTokensPrevention.SetActive(false);
        }
        else if ((!FirstRoundStart || !SecondRoundStart || !LastRoundStart))
        {
            WordsPrevention.SetActive(false);
            ExemptionTokensPrevention.SetActive(true);
        }
    }

    //올린 단어카드 카운트
    void CountWordCards()
    {
        if (FirstAdjectiveNumbers == 3 && FirstNounNumbers == 3)
        {
            for(int i = 0; i < _UIManager.WordTrigger.Length; i++)
            {
                if (_UIManager.WordTrigger[i].TriggeredAdjective != null)                    
                {
                    _UIManager.SelectedWordsButton.gameObject.SetActive(false);
                }

                if (_UIManager.WordTrigger[i].TriggeredNoun != null)
                {
                    _UIManager.SelectedWordsButton.gameObject.SetActive(false);
                }
            }

            _UIManager.SelectedWordsButton.gameObject.SetActive(true);
        }
        else if (FirstAdjectiveNumbers != 3 || FirstNounNumbers != 3)
        {
            _UIManager.SelectedWordsButton.gameObject.SetActive(false);
        }

        if(FirstGuessComplete && SecondWordNumber == 1)
        {
            _UIManager.SelectedWordsButton.gameObject.SetActive(true);
        }
        else if(FirstGuessComplete && SecondWordNumber != 1)
        {
            _UIManager.SelectedWordsButton.gameObject.SetActive(false);
        }

        if(SecondGuessComplete && LastWordNumber == 1)
        {
            for(int i = 0; i < _UIManager.WordTrigger.Length; i++)
            {
                if (_UIManager.WordTrigger[i].TriggeredAdjective != null)
                {
                    _UIManager.SelectedWordsButton.gameObject.SetActive(false);
                }
                
                if (_UIManager.WordTrigger[i].TriggeredNoun != null)                    
                {
                    _UIManager.SelectedWordsButton.gameObject.SetActive(false);
                }                
            }

            _UIManager.SelectedWordsButton.gameObject.SetActive(true);
        }
        else if(SecondGuessComplete && LastWordNumber != 1)
        {            
            _UIManager.SelectedWordsButton.gameObject.SetActive(false);
        }
    }

    //모든 제외카드 사용여부 확인
    void CountExemptionCards()
    {
        if(CardNumbers == 6)
        {
            _UIManager.GuessButton.gameObject.SetActive(true);
        }
        else
        {
            _UIManager.GuessButton.gameObject.SetActive(false);
        }
    }    
}
