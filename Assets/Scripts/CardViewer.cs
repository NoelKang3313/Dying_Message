using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CardViewer : MonoBehaviour, IPointerClickHandler
{
    AudioManager _audioManager;
    public Image CardViewerImage;     //선택한 카드 이미지 확대용

    void Awake()
    {
        _audioManager = GameObject.Find("AudioManager").GetComponent<AudioManager>();
    }

    //범인, 도구, 동기 카드 클릭 시, 클릭한 카드 확대
    public void OnPointerClick(PointerEventData eventData)
    {
        if ((GameManager.instance.FirstRoundStart || GameManager.instance.SecondRoundStart || GameManager.instance.LastRoundStart))
        {
            _audioManager.ButtonClickAudioSource.Play();

            CardViewerImage.gameObject.SetActive(true);
            CardViewerImage.sprite = GetComponent<Image>().sprite;
        }
        else
            return;
    }       
}
