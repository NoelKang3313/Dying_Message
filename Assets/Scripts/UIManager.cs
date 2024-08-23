using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class UIManager : MonoBehaviour
{
    CardManager _cardManager;
    AudioManager _audioManager;
    public WordTrigger[] WordTrigger = new WordTrigger[3];
    
    public string[] TextArr = new string[6];     //텍스트 저장 배열
    Dictionary<int, string> GameText = new Dictionary<int, string>();     //저장된 배열 호출용
    int currentIndex;     //현재 텍스트 인덱스
    public TextMeshProUGUI ProgressText;     //텍스트

    public TextMeshProUGUI GameCompleteText;     //게임종료 텍스트

    public Button TextButton;       //텍스트 넘기기용 버튼
    public Button HelpButton;     //도움말 버튼
    public Button ExitHelpButton;     //도움말창 닫기 버튼
    public Button PauseButton;      //일시정지 버튼
    public Button RegameButton;     //재시작 버튼
    public Button SettingButton;    //설정 버튼
    public Button ExitSettingButton;     //설정 나가기 버튼
    public Button ReturnButton;     //돌아가기 버튼
    public Button SelectedCardsButton;      //최종카드 확인 후 진행버튼
    public Button SelectedWordsButton;      //최종 제시어 확정버튼
    public Button GuessButton;      //제외카드 배치완료 버튼
    public Button GameCompleteRegameButton;     //게임종료, 게임 재시작버튼

    public GameObject JobCardsButtons;      //모든 직업카드
    public GameObject ToolCardsButtons;     //모든 도구카드
    public GameObject PurposeCardsButtons; //모든 동기카드

    public Image SelectJobCard;     //최종선택 직업카드
    public Image SelectToolCard;     //최종선택 도구카드
    public Image SelectPurposeCard;     //최종선택 동기카드

    public Button[] JobButtons = new Button[9];     //직업버튼
    public Button[] ToolButtons = new Button[9];    //도구버튼
    public Button[] PurposeButtons = new Button[9]; //동기버튼

    public Image[] JobCardViewers = new Image[9];     //직업 확대용(IPointer 인터페이스용)
    public Image[] ToolCardViewers = new Image[9];     //도구 확대용(IPointer 인터페이스용)
    public Image[] PurposeCardViewers = new Image[9];     //동기 확대용(IPointer 인터페이스용)

    public int JobNumber;       //직업카드 인덱스
    public int ToolNumber;      //도구카드 인덱스
    public int PurposeNumber;   //동기카드 인덱스

    public GameObject HelpPanel;     //도움말창
    public GameObject TextPanel;     //텍스트창

    public GameObject SettingPanel;      //설정창

    public Slider VolumeSlider;     //볼륨 슬라이더
    private float volumeValue;     //볼륨 크기
    public Sprite FullVolumeImage;     //볼륨 최대
    public Sprite MediumVolumeImage;   //볼륨 중간
    public Sprite NoVolumeImage;       //볼륨 무음
    public Sprite VolumeMuteImage;     //볼륨 음소거
    public Button VolumeButton;     //볼륨 버튼
    private bool isVolumeMute;     //볼륨 음소거 확인

    private int _finalJobIndex;     //최종 직업카드 인덱스
    private int _finalToolIndex;    //최종 도구카드 인덱스
    private int _finalPurposeIndex; //최종 동기카드 인덱스

    public Button CardSelectedButton;       //카드 선택완료 버튼

    public Button ConfirmButton;    //재시작 확인버튼
    public Button CancelButton;     //재시작 취소버튼

    public GameObject PausePanel;   //일시정지창
    public GameObject RegamePanel;  //재시작창

    //직업, 도구, 동기버튼 선택확인
    [SerializeField] private bool _jobCardSelected;
    [SerializeField] private bool _toolCardSelected;
    [SerializeField] private bool _purposeCardSelected;    

    void Awake()
    {
        _cardManager = GameObject.Find("CardManager").GetComponent<CardManager>();
        _audioManager = GameObject.Find("AudioManager").GetComponent<AudioManager>();

        TextButton.onClick.AddListener(OnTextButtonClicked);
        HelpButton.onClick.AddListener(OnHelpButtonClicked);
        ExitHelpButton.onClick.AddListener(OnExitHelpButtonClicked);
        PauseButton.onClick.AddListener(OnPauseButtonClicked);
        SettingButton.onClick.AddListener(OnSettingButtonClicked);
        ExitSettingButton.onClick.AddListener(OnExitSettingButtonClicked);
        RegameButton.onClick.AddListener(OnRegameButtonClicked);
        ReturnButton.onClick.AddListener(OnReturnButtonClicked);
        SelectedCardsButton.onClick.AddListener(OnSelectedCardsButtonClicked);
        SelectedWordsButton.onClick.AddListener(OnSelectedWordsButtonClicked);
        GuessButton.onClick.AddListener(OnGuessButtonClicked);
        GameCompleteRegameButton.onClick.AddListener(OnGameCompleteRegameButtonClicked);

        CardSelectedButton.onClick.AddListener(OnCardSelectedButtonClicked);

        ConfirmButton.onClick.AddListener(OnConfirmButtonClicked);
        CancelButton.onClick.AddListener(OnCancelButtonClicked);

        VolumeButton.onClick.AddListener(OnVolumeButtonClicked);

        //직업카드 버튼선택
        for(int i = 0; i < JobButtons.Length; i++)
        {
            int number = i;
            JobButtons[i].onClick.AddListener(() => OnJobButtonClicked(number));
        }

        //도구카드 버튼선택
        for(int i = 0; i < ToolButtons.Length; i++)
        {
            int number = i;
            ToolButtons[i].onClick.AddListener(() => OnToolButtonClicked(number));
        }

        //동기카드 버튼선택
        for(int i = 0; i < PurposeButtons.Length; i++)
        {
            int number = i;
            PurposeButtons[i].onClick.AddListener(() => OnPurposeButtonClicked(number));
        }

        //텍스트 초기화
        for(int i = 0; i < TextArr.Length; i++)
        {
            GameText[i] = TextArr[i];
        }

        ChangeText(0);
    }
    
    void Update()
    {
        if(_jobCardSelected && _toolCardSelected && _purposeCardSelected)
        {
            CardSelectedButton.gameObject.SetActive(true);
        }

        ChangeVolume();
    }

    //텍스트 변경
    int ChangeText(int num)
    {
        ProgressText.text = GameText[num];
        currentIndex = num;

        return currentIndex;
    }

    //게임볼륨 변경
    void ChangeVolume()
    {
        volumeValue = VolumeSlider.value;
        _audioManager.BGMAudioSource.volume = volumeValue;

        if (!isVolumeMute)
        {
            if (0.5f <= volumeValue && volumeValue <= 1.0f)
            {
                VolumeButton.GetComponent<Image>().sprite = FullVolumeImage;
            }
            else if (0f < volumeValue && volumeValue < 0.5f)
            {
                VolumeButton.GetComponent<Image>().sprite = MediumVolumeImage;
            }
            else
            {
                VolumeButton.GetComponent<Image>().sprite = NoVolumeImage;
            }
        }
        else
        {
            VolumeButton.GetComponent<Image>().sprite = VolumeMuteImage;
            _audioManager.BGMAudioSource.volume = 0;
        }
    }

    //볼륨 음소거 버튼
    void OnVolumeButtonClicked()
    {
        isVolumeMute = !isVolumeMute;
        _audioManager.ButtonClickAudioSource.Play();
    }

    //범인직업 선택
    void OnJobButtonClicked(int number)
    {        
        for(int i = 0; i < JobButtons.Length; i++)
        {
            _audioManager.ButtonClickAudioSource.Play();

            if (JobButtons[i].transform.GetChild(0).gameObject.activeSelf)
            {
                JobButtons[i].transform.GetChild(0).gameObject.SetActive(false);
                JobButtons[number].transform.GetChild(0).gameObject.SetActive(true);

                _jobCardSelected = true;
                JobNumber = number;
            }
            else
            {
                JobButtons[number].transform.GetChild(0).gameObject.SetActive(true);

                _jobCardSelected = true;
                JobNumber = number;
            }
        }
    }

    //범행도구 선택
    void OnToolButtonClicked(int number)
    {
        _audioManager.ButtonClickAudioSource.Play();

        for (int i = 0; i < ToolButtons.Length; i++)
        {
            if (ToolButtons[i].transform.GetChild(0).gameObject.activeSelf)
            {
                ToolButtons[i].transform.GetChild(0).gameObject.SetActive(false);
                ToolButtons[number].transform.GetChild(0).gameObject.SetActive(true);

                _toolCardSelected = true;
                ToolNumber = number;
            }
            else
            {
                ToolButtons[number].transform.GetChild(0).gameObject.SetActive(true);

                _toolCardSelected = true;
                ToolNumber = number;
            }
        }
    }

    //범행동기 선택
    void OnPurposeButtonClicked(int number)
    {
        _audioManager.ButtonClickAudioSource.Play();

        for (int i = 0; i < PurposeButtons.Length; i++)
        {
            if (PurposeButtons[i].transform.GetChild(0).gameObject.activeSelf)
            {
                PurposeButtons[i].transform.GetChild(0).gameObject.SetActive(false);
                PurposeButtons[number].transform.GetChild(0).gameObject.SetActive(true);

                _purposeCardSelected = true;
                PurposeNumber = number;
            }
            else
            {
                PurposeButtons[number].transform.GetChild(0).gameObject.SetActive(true);

                _purposeCardSelected = true;
                PurposeNumber = number;
            }
        }
    }

    //범인, 도구, 동기 모두 선택 후, 확정버튼
    void OnCardSelectedButtonClicked()
    {
        _audioManager.ButtonClickAudioSource.Play();

        _jobCardSelected = false;
        _toolCardSelected = false;
        _purposeCardSelected = false;

        CardSelectedButton.gameObject.SetActive(false);

        _finalJobIndex = JobNumber;
        _finalToolIndex = ToolNumber;
        _finalPurposeIndex = PurposeNumber;

        GameManager.instance.FinalJobCard = _cardManager.JobCards[_finalJobIndex];
        GameManager.instance.FinalToolCard = _cardManager.ToolCards[_finalToolIndex];
        GameManager.instance.FinalPurposeCard = _cardManager.PurposeCards[_finalPurposeIndex];

        for(int i = 0; i < 9; i++)
        {
            JobButtons[i].gameObject.SetActive(false);
            ToolButtons[i].gameObject.SetActive(false);
            PurposeButtons[i].gameObject.SetActive(false);
        }

        SelectJobCard.gameObject.SetActive(true);
        SelectToolCard.gameObject.SetActive(true);
        SelectPurposeCard.gameObject.SetActive(true);

        Texture2D finalJobCardTexture = GameManager.instance.FinalJobCard.transform.GetChild(0).GetComponent<MeshRenderer>().material.mainTexture as Texture2D;
        Texture2D finalToolCardTexture = GameManager.instance.FinalToolCard.transform.GetChild(0).GetComponent<MeshRenderer>().material.mainTexture as Texture2D;
        Texture2D finalPurposeCardTexture = GameManager.instance.FinalPurposeCard.transform.GetChild(0).GetComponent<MeshRenderer>().material.mainTexture as Texture2D;

        SelectJobCard.sprite = _cardManager.ConvertToSprite(finalJobCardTexture);
        SelectToolCard.sprite = _cardManager.ConvertToSprite(finalToolCardTexture);
        SelectPurposeCard.sprite = _cardManager.ConvertToSprite(finalPurposeCardTexture);

        SelectedCardsButton.gameObject.SetActive(true);
    }

    //선택한 카드 확대 후, 비활성화용 버튼
    void OnSelectedCardsButtonClicked()
    {
        if(SelectJobCard.gameObject.activeSelf && SelectToolCard.gameObject.activeSelf && SelectPurposeCard.gameObject.activeSelf)
        {
            _audioManager.ButtonClickAudioSource.Play();

            SelectJobCard.gameObject.SetActive(false);
            SelectToolCard.gameObject.SetActive(false);
            SelectPurposeCard.gameObject.SetActive(false);

            TextPanel.SetActive(true);

            SelectedCardsButton.gameObject.SetActive(false);
        }
    }

    //단어 카드 선택 후, 확정버튼 
    void OnSelectedWordsButtonClicked()
    {
        _audioManager.ButtonClickAudioSource.Play();        

        TextPanel.SetActive(true);        

        if (GameManager.instance.PlaceWords)
        {
            GameManager.instance.PlaceWords = false;
            GameManager.instance.FirstRoundStart = true;

            GameManager.instance.FirstAdjectiveNumbers = 0;
            GameManager.instance.FirstNounNumbers = 0;

            for (int i = 0; i < WordTrigger.Length; i++)
            {
                WordTrigger[i].PlacedAdjectiveWords[0] = WordTrigger[i].TriggeredAdjective;
                WordTrigger[i].PlacedNounWords[0] = WordTrigger[i].TriggeredNoun;

                WordTrigger[i].TriggeredAdjective.transform.position = WordTrigger[i].AdjectivePositions[0];
                WordTrigger[i].TriggeredNoun.transform.position = WordTrigger[i].NounPositions[0];

                WordTrigger[i].TriggeredAdjective.transform.gameObject.tag = "PlacedWord";
                WordTrigger[i].TriggeredNoun.transform.gameObject.tag = "PlacedWord";

                Destroy(WordTrigger[i].TriggeredAdjective.GetComponent<DragAndDrop>());
                Destroy(WordTrigger[i].TriggeredNoun.GetComponent<DragAndDrop>());

                for (int j = 0; j < 6; j++)
                {
                    if (_cardManager.FirstAdjectiveCards[j] == WordTrigger[i].TriggeredAdjective)
                    {
                        _cardManager.FirstAdjectiveCards[j] = null;
                    }

                    if (_cardManager.FirstNounCards[j] == WordTrigger[i].TriggeredNoun)
                    {
                        _cardManager.FirstNounCards[j] = null;
                    }
                }
            }

            ArrangeWords();
        }

        if (GameManager.instance.FirstGuessComplete)
        {
            ChangeText(3);

            GameManager.instance.FirstGuessComplete = false;
            GameManager.instance.SecondWordNumber = 0;

            GameManager.instance.SecondRoundStart = true;

            for (int i = 0; i < WordTrigger.Length; i++)
            {                
                if (WordTrigger[i].SecondTriggeredAdjective != null)
                {
                    WordTrigger[i].PlacedAdjectiveWords[1] = WordTrigger[i].SecondTriggeredAdjective;
                    WordTrigger[i].SecondTriggeredAdjective.transform.position = WordTrigger[i].AdjectivePositions[1];

                    WordTrigger[i].SecondTriggeredAdjective.transform.gameObject.tag = "PlacedWord";
                    Destroy(WordTrigger[i].SecondTriggeredAdjective.GetComponent<DragAndDrop>());
                }
                else if (WordTrigger[i].SecondTriggeredNoun != null)
                {
                    WordTrigger[i].PlacedNounWords[1] = WordTrigger[i].SecondTriggeredNoun;
                    WordTrigger[i].SecondTriggeredNoun.transform.position = WordTrigger[i].NounPositions[1];

                    WordTrigger[i].SecondTriggeredNoun.transform.gameObject.tag = "PlacedWord";
                    Destroy(WordTrigger[i].SecondTriggeredNoun.GetComponent<DragAndDrop>());
                }

                for(int j = 0; j < 6; j++)
                {
                    if (_cardManager.FirstAdjectiveCards[j] == WordTrigger[i].SecondTriggeredAdjective)
                    {
                        _cardManager.FirstAdjectiveCards[j] = null;
                    }

                    if (_cardManager.FirstNounCards[j] == WordTrigger[i].SecondTriggeredNoun)
                    {
                        _cardManager.FirstNounCards[j] = null;
                    }
                }
            }

            ArrangeWords();
        }

        if(GameManager.instance.SecondGuessComplete)
        {
            ChangeText(3);

            GameManager.instance.SecondGuessComplete = false;
            GameManager.instance.LastWordNumber = 0;

            GameManager.instance.LastRoundStart = true;

            for(int i = 0; i < WordTrigger.Length; i++)
            {                
                if (WordTrigger[i].SecondTriggeredAdjective != null)
                {
                    WordTrigger[i].SecondTriggeredAdjective.transform.position = WordTrigger[i].AdjectivePositions[1];
                }
                
                if (WordTrigger[i].SecondTriggeredNoun != null)
                {
                    WordTrigger[i].SecondTriggeredNoun.transform.position = WordTrigger[i].NounPositions[1];
                }
            }

            ArrangeWords();
        }
    }

    //형용사 & 명사 카드 정렬
    void ArrangeWords()
    {
        for (int i = 0; i < 6; i++)
        {
            if (_cardManager.FirstAdjectiveCards[i] == null)
            {
                for (int j = i; j < 6; j++)
                {
                    if (_cardManager.FirstAdjectiveCards[j] != null)
                    {
                        _cardManager.FirstAdjectiveCards[i] = _cardManager.FirstAdjectiveCards[j];
                        _cardManager.FirstAdjectiveCards[j].transform.position = _cardManager.FirstAdjectivePositions[i];
                        _cardManager.FirstAdjectiveCards[j] = null;
                        break;
                    }
                }
            }
        }

        for (int i = 0; i < 6; i++)
        {
            if (_cardManager.FirstNounCards[i] == null)
            {
                for (int j = i; j < 6; j++)
                {
                    if (_cardManager.FirstNounCards[j] != null)
                    {
                        _cardManager.FirstNounCards[i] = _cardManager.FirstNounCards[j];
                        _cardManager.FirstNounCards[j].transform.position = _cardManager.FirstNounPositions[i];
                        _cardManager.FirstNounCards[j] = null;
                        break;
                    }
                }
            }
        }
    }

    //추리확인 버튼 (모든 제외토큰이 카드 위에 올려져있을 시 버튼 활성화)
    void OnGuessButtonClicked()
    {        
        int _index;

        for (int i = 0; i < 6; i++)
        {            
            GameManager.instance.GuessedCards[i] = _cardManager.ExemptionTokens[i].GetComponent<ExemptionTrigger>().TriggeredCard;
        }

        for (_index = 0; _index < GameManager.instance.GuessedCards.Length; _index++)
        {
            if (GameManager.instance.GuessedCards[_index] == GameManager.instance.FinalJobCard ||
                    GameManager.instance.GuessedCards[_index] == GameManager.instance.FinalToolCard ||
                    GameManager.instance.GuessedCards[_index] == GameManager.instance.FinalPurposeCard)
            {
                _audioManager.WrongAudioSource.Play();

                GameManager.instance.GuessCount++;
                    
                for (int j = 0; j < 6; j++)
                {
                    _cardManager.ExemptionTokens[j].transform.position = _cardManager.FirstExemptionTokenPositions[j];
                }
                break;
            }
            else if (GameManager.instance.GuessedCards[_index] != GameManager.instance.FinalJobCard &&
                GameManager.instance.GuessedCards[_index] != GameManager.instance.FinalToolCard &&
                GameManager.instance.GuessedCards[_index] != GameManager.instance.FinalPurposeCard)
            {
                continue;
            }
        }

        if (_index == 6)
        {            
            _audioManager.CorrectAudioSource.Play();

            if (GameManager.instance.FirstRoundStart)
            {
                GameManager.instance.FirstGuessComplete = true;

                TextPanel.SetActive(true);                
                ChangeText(4);
            }
            else if(GameManager.instance.SecondRoundStart)
            {
                GameManager.instance.SecondGuessComplete = true;

                TextPanel.SetActive(true);
                ChangeText(4);
            }
            else if(GameManager.instance.LastRoundStart)
            {
                GameManager.instance.LastGuessComplete = true;

                TextPanel.SetActive(true);
                ChangeText(5);
            }
            else if(GameManager.instance.FinalGuessStart)
            {
                GameManager.instance.GameComplete = true;

                SelectJobCard.gameObject.SetActive(true);
                SelectToolCard.gameObject.SetActive(true);
                SelectPurposeCard.gameObject.SetActive(true);

                GameCompleteText.gameObject.SetActive(true);
                GameCompleteText.text = "Investigation Complete";
                GameCompleteRegameButton.gameObject.SetActive(true);
            }

            for (int i = 0; i < GameManager.instance.GuessedCards.Length; i++)
            {
                GameManager.instance.GuessedCards[i].transform.rotation = Quaternion.Euler(90, 0, 0);
                GameManager.instance.GuessedCards[i].GetComponent<BoxCollider>().center =
                    new Vector3(GameManager.instance.GuessedCards[i].GetComponent<BoxCollider>().center.x,
                    GameManager.instance.GuessedCards[i].GetComponent<BoxCollider>().center.y,
                    0.1f);
            }

            for (int i = 0; i < _cardManager.ExemptionTokens.Length; i++)
            {
                _cardManager.ExemptionTokens[i].transform.position = _cardManager.FirstExemptionTokenPositions[i];
            }

            _cardManager.SetWords();
        }
    }

    //다음 텍스트 및 텍스트 창 비활성화용 버튼
    void OnTextButtonClicked()
    {
        _audioManager.ButtonClickAudioSource.Play();

        switch(currentIndex)
        {
            case 0:
                ChangeText(1);
                break;
            case 1:
                TextPanel.SetActive(false);
                ChangeText(2);

                JobCardsButtons.SetActive(true);
                ToolCardsButtons.SetActive(true);
                PurposeCardsButtons.SetActive(true);
                break;
            case 2:
                TextPanel.SetActive(false);
                ChangeText(3);

                _cardManager.TurnAllCards();
                GameManager.instance.PlaceWords = true;
                break;
            case 3:
                TextPanel.SetActive(false);                
                break;
            case 4:
                TextPanel.SetActive(false);
                break;
            case 5:
                TextPanel.SetActive(false);

                GameManager.instance.LastRoundStart = false;
                GameManager.instance.LastGuessComplete = true;
                GameManager.instance.FinalGuessStart = true;

                break;
        }
    }

    //도움말창 활성화
    void OnHelpButtonClicked()
    {
        _audioManager.ButtonClickAudioSource.Play();

        HelpPanel.SetActive(true);
    }

    //도움말창 비활성화
    void OnExitHelpButtonClicked()
    {
        _audioManager.ButtonClickAudioSource.Play();

        HelpPanel.SetActive(false);
    }

    //일시정지 버튼
    void OnPauseButtonClicked()
    {
        _audioManager.ButtonClickAudioSource.Play();

        PausePanel.SetActive(true);
    }

    //재시작창 활성화
    void OnRegameButtonClicked()
    {
        _audioManager.ButtonClickAudioSource.Play();

        RegamePanel.SetActive(true);
    }

    //재시작 창 -> 확인버튼
    void OnConfirmButtonClicked()
    {
        _audioManager.ButtonClickAudioSource.Play();

        SceneManager.LoadScene("MainScene");
    }

    //재시작 창 -> 취소버튼
    void OnCancelButtonClicked()
    {
        _audioManager.ButtonClickAudioSource.Play();

        RegamePanel.SetActive(false);
    }

    //설정창 활성화
    void OnSettingButtonClicked()
    {
        _audioManager.ButtonClickAudioSource.Play();

        SettingPanel.SetActive(true);
    }

    //설정창 비활성화
    void OnExitSettingButtonClicked()
    {
        _audioManager.ButtonClickAudioSource.Play();

        SettingPanel.SetActive(false);
    }

    //돌아가기 버튼 (일시정지창 비활성화)
    void OnReturnButtonClicked()
    {
        _audioManager.ButtonClickAudioSource.Play();

        PausePanel.SetActive(false);
    }

    //게임종료 시, 새로운 게임 시작버튼
    void OnGameCompleteRegameButtonClicked()
    {
        SceneManager.LoadScene("MainScene");
    }
}
