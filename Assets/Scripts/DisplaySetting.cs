using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisplaySetting : MonoBehaviour
{
    public GameObject mainCameraObj;
    private Camera mainCamera;
    public GameObject canvasObj;
    public GameObject canvasObj2;
    private Vector3 scale;

    private float offset = -0.25f;

    // 初期の画面サイズのheight
    private float defaultHeight = 800f;

    // 実際の画面サイズのheight
    private float height;

    // 実際の画面サイズのwidth
    private float width;

    // Start is called before the first frame update
    void Start()
    {
        
        //scale = gameWorld.transform.localScale;
        Vector3 cameraPos = mainCameraObj.transform.position;
        cameraPos.x += offset;
        mainCameraObj.transform.position = cameraPos;

        mainCamera = mainCameraObj.GetComponent<Camera>();

        // 画面サイズ
        height = (float)Screen.height;
        width = (float)Screen.width;
        Debug.Log(height);
        Debug.Log(width);

        //実際の画面のアスペクト比
        float actualAspect = (float)Screen.width / (float)Screen.height;

        float ratio = height / defaultHeight;

        //gameWorld.transform.localScale = scale * ratio;

        //実機とunity画面の比率
        //float ratio = actualAspect / defaultAspect;

        //サイズ調整
        //mainCamera.orthographicSize *= ratio;

        // Canvasを調整
        RectTransform textRect = canvasObj.GetComponent<RectTransform>();
        textRect.sizeDelta = new Vector2(width / ratio, height/ ratio);
        Vector3 canvasPos = canvasObj.transform.position;
        canvasPos.x += offset;
        canvasObj.transform.position = canvasPos;

        RectTransform textRect2 = canvasObj2.GetComponent<RectTransform>();
        textRect2.sizeDelta = new Vector2(width / ratio, textRect2.sizeDelta.y);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
