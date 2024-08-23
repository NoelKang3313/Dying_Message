using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardManager : MonoBehaviour
{
    UIManager _UIManager;

    public GameObject[] JobCards = new GameObject[9];       //직업카드
    public GameObject[] ToolCards = new GameObject[9];      //도구카드
    public GameObject[] PurposeCards = new GameObject[9];   //동기카드

    public GameObject[] AdjectiveCards = new GameObject[8]; //형용사카드
    public GameObject[] NounCards = new GameObject[8];      //명사카드

    public List<Material> JobMaterials = new List<Material>();      //모든 직업카드 이미지
    public List<Material> ToolMaterials = new List<Material>();     //모든 도구카드 이미지
    public List<Material> PurposeMaterials = new List<Material>();  //모든 동기카드 이미지

    public List<Material> AdjectiveMaterials = new List<Material>();    //모든 형용사카드 이미지
    public List<Material> NounMaterials = new List<Material>();         //모든 명사카드 이미지    

    public GameObject[] FirstAdjectiveCards = new GameObject[6];    //형용사카드 첫 위치 게임오브젝트
    public GameObject[] FirstNounCards = new GameObject[6];         //명사카드 첫 위치 게임오브젝트

    public Vector3[] FirstAdjectivePositions = new Vector3[6];  //형용사카드 첫 위치
    public Vector3[] FirstNounPositions = new Vector3[6];       //명사카드 첫 위치

    public GameObject[] ExemptionTokens = new GameObject[6];    //제외토큰와 트리거된 카드 오브젝트
    public Vector3[] FirstExemptionTokenPositions = new Vector3[6]; //초기 제외토큰 위치

    public GameObject[] GuessTokens = new GameObject[4];        //추리토큰 오브젝트    

    void Awake()
    {
        _UIManager = GameObject.Find("UIManager").GetComponent<UIManager>();

        for(int i = 0; i < 6; i++)
        {
            FirstAdjectiveCards[i] = AdjectiveCards[i];
            FirstNounCards[i] = NounCards[i];

            FirstAdjectivePositions[i] = FirstAdjectiveCards[i].transform.position;
            FirstNounPositions[i] = FirstNounCards[i].transform.position;

            FirstExemptionTokenPositions[i] = ExemptionTokens[i].transform.position;
        }
    }

    void Start()
    {
        for (int i = 0; i < JobCards.Length; i++)
        {
            int jobRandom = Random.Range(0, JobMaterials.Count);
            JobCards[i].transform.GetChild(0).GetComponent<MeshRenderer>().material = JobMaterials[jobRandom];

            Texture2D jobTexture = JobMaterials[jobRandom].mainTexture as Texture2D;
            _UIManager.JobCardViewers[i].GetComponent<Image>().sprite = ConvertToSprite(jobTexture);

            JobMaterials.RemoveAt(jobRandom);
        }

        for(int i = 0; i < ToolCards.Length; i++)
        {
            int toolRandom = Random.Range(0, ToolMaterials.Count);
            ToolCards[i].transform.GetChild(0).GetComponent<MeshRenderer>().material = ToolMaterials[toolRandom];

            Texture2D toolTexture = ToolMaterials[toolRandom].mainTexture as Texture2D;
            _UIManager.ToolCardViewers[i].GetComponent<Image>().sprite = ConvertToSprite(toolTexture);

            ToolMaterials.RemoveAt(toolRandom);
        }

        for(int i = 0; i < PurposeCards.Length; i++)
        {
            int purposeRandom = Random.Range(0, PurposeMaterials.Count);
            PurposeCards[i].transform.GetChild(0).GetComponent<MeshRenderer>().material = PurposeMaterials[purposeRandom];

            Texture2D purposeTexture = PurposeMaterials[purposeRandom].mainTexture as Texture2D;
            _UIManager.PurposeCardViewers[i].GetComponent<Image>().sprite = ConvertToSprite(purposeTexture);

            PurposeMaterials.RemoveAt(purposeRandom);
        }

        for (int i = 0; i < AdjectiveCards.Length; i++)
        {
            int adjectiveRandom = Random.Range(0, AdjectiveMaterials.Count);
            AdjectiveCards[i].transform.GetChild(0).GetComponent<MeshRenderer>().material = AdjectiveMaterials[adjectiveRandom];
            AdjectiveMaterials.RemoveAt(adjectiveRandom);
        }

        for(int i = 0; i < NounCards.Length; i++)
        {
            int nounRandom = Random.Range(0, NounMaterials.Count);
            NounCards[i].transform.GetChild(0).GetComponent<MeshRenderer>().material = NounMaterials[nounRandom];
            NounMaterials.RemoveAt(nounRandom);
        }
    }

    void Update()
    {
        CountGuessToken();
    }

    //선택한 범인, 도구, 동기카드 머티리얼의 텍스처를 스프라이트로 변경 메소드
    public Sprite ConvertToSprite(Texture2D texture)
    {
        return Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.zero);
    }

    //범인, 도구, 동기, 형용사, 명사 카드 모두 뒤집기
    public void TurnAllCards()
    {
        for(int i = 0; i < 9; i++)
        {
            JobCards[i].transform.rotation = Quaternion.Euler(-90, 180, 0);
            ToolCards[i].transform.rotation = Quaternion.Euler(-90, 180, 0);
            PurposeCards[i].transform.rotation = Quaternion.Euler(-90, 180, 0);
        }        

        for(int i = 0; i < 8; i++)
        {
            AdjectiveCards[i].transform.rotation = Quaternion.Euler(Vector3.zero);
            NounCards[i].transform.rotation = Quaternion.Euler(Vector3.zero);
        }
    }

    //추리실패 시, 추리토큰 180도 회전
    void CountGuessToken()
    {
        switch(GameManager.instance.GuessCount)
        {
            case 1:
                GuessTokens[0].transform.rotation = Quaternion.Euler(0, 0, 180);
                break;

            case 2:
                GuessTokens[1].transform.rotation = Quaternion.Euler(0, 0, 180);
                break;

            case 3:
                GuessTokens[2].transform.rotation = Quaternion.Euler(0, 0, 180);
                break;

            case 4:
                GuessTokens[3].transform.rotation = Quaternion.Euler(0, 0, 180);
                break;
        }
    }

    //추리 성공 후 새로운 명사와 형용사 카드 배치
    public void SetWords()
    {
        if(GameManager.instance.FirstGuessComplete && GameManager.instance.FirstRoundStart)
        {
            GameManager.instance.FirstRoundStart = false;

            FirstAdjectiveCards[3] = AdjectiveCards[6];
            FirstNounCards[3] = NounCards[6];

            AdjectiveCards[6].transform.position = FirstAdjectivePositions[3];
            NounCards[6].transform.position = FirstNounPositions[3];
        }

        if(GameManager.instance.SecondGuessComplete && GameManager.instance.SecondRoundStart)
        {
            GameManager.instance.SecondRoundStart = false;

            for(int i = 0; i < FirstAdjectiveCards.Length; i++)
            {                
                if (FirstAdjectiveCards[i] == null)
                {
                    FirstAdjectiveCards[i] = AdjectiveCards[7];
                    AdjectiveCards[7].transform.position = FirstAdjectivePositions[i];
                    break;
                }
            }

            for(int i = 0; i < FirstNounCards.Length; i++)
            {
                if (FirstNounCards[i] == null)
                {
                    FirstNounCards[i] = NounCards[7];
                    NounCards[7].transform.position = FirstNounPositions[i];
                    break;
                }
            }
        }
    }
}
