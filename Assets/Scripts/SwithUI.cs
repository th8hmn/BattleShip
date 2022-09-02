using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwithUI : MonoBehaviour
{
    public GameObject matchmakingObj;
    public GameObject roomCreateObj;

    private Canvas matchmakingCanvas;
    private Canvas roomCreateCanvas;

    private CanvasGroup matchmakingCanvasGroup;
    private CanvasGroup roomCreateCanvasGroup;

    // Start is called before the first frame update
    void Start()
    {
        matchmakingCanvas = matchmakingObj.GetComponent<Canvas>();
        roomCreateCanvas = roomCreateObj.GetComponent<Canvas>();

        matchmakingCanvas.sortingOrder = 5;
        roomCreateCanvas.sortingOrder = 5;

        GameObject tmp1 = matchmakingObj.transform.GetChild(0).transform.gameObject;
        GameObject tmp2 = roomCreateObj.transform.GetChild(0).transform.gameObject;
        matchmakingCanvasGroup = tmp1.GetComponent<CanvasGroup>();
        roomCreateCanvasGroup = tmp2.GetComponent<CanvasGroup>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SwitchToJoinRoom()
    {
        matchmakingCanvas.sortingOrder = 7;
        roomCreateObj.SetActive(false);
    }

    public void SwitchToCreateRoom()
    {
        roomCreateCanvas.sortingOrder = 7;
        matchmakingObj.SetActive(false);
    }

    public void BackToSwitchDisplay()
    {
        matchmakingCanvas.sortingOrder = 5;
        matchmakingObj.SetActive(true);
        matchmakingCanvasGroup.interactable = true;

        roomCreateCanvas.sortingOrder = 5;
        roomCreateObj.SetActive(true);
        roomCreateCanvasGroup.interactable = true;
    }
}
