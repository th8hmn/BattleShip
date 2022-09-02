using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisplaySetting : MonoBehaviour
{
    public GameObject mainCameraObj;
    private Camera mainCamera;
    public GameObject canvasObj;
    public GameObject canvasObj2;
    public GameObject canvasObj3;
    public GameObject canvasObj4;
    private Vector3 scale;

    private float offset = -0.25f;

    // �����̉�ʃT�C�Y��height
    private float defaultHeight = 800f;

    // ���ۂ̉�ʃT�C�Y��height
    private float height;

    // ���ۂ̉�ʃT�C�Y��width
    private float width;

    // Start is called before the first frame update
    void Start()
    {
        
        //scale = gameWorld.transform.localScale;
        Vector3 cameraPos = mainCameraObj.transform.position;
        cameraPos.x += offset;
        mainCameraObj.transform.position = cameraPos;

        mainCamera = mainCameraObj.GetComponent<Camera>();

        // ��ʃT�C�Y
        height = (float)Screen.height;
        width = (float)Screen.width;
        Debug.Log(height);
        Debug.Log(width);

        //���ۂ̉�ʂ̃A�X�y�N�g��
        float actualAspect = (float)Screen.width / (float)Screen.height;

        float ratio = height / defaultHeight;

        //gameWorld.transform.localScale = scale * ratio;

        //���@��unity��ʂ̔䗦
        //float ratio = actualAspect / defaultAspect;

        //�T�C�Y����
        //mainCamera.orthographicSize *= ratio;

        // Canvas�𒲐�
        RectTransform textRect = canvasObj.GetComponent<RectTransform>();
        textRect.sizeDelta = new Vector2(width / ratio, height/ ratio);
        Vector3 canvasPos = canvasObj.transform.position;
        canvasPos.x += offset;
        canvasObj.transform.position = canvasPos;

        RectTransform textRect2 = canvasObj2.GetComponent<RectTransform>();
        textRect2.sizeDelta = new Vector2(width / ratio, textRect2.sizeDelta.y);

        RectTransform textRect3 = canvasObj3.GetComponent<RectTransform>();
        textRect3.sizeDelta = new Vector2(width / ratio, textRect3.sizeDelta.y);
        Vector3 canvasPos3 = canvasObj3.transform.position;
        canvasPos3.x += offset;
        canvasObj3.transform.position = canvasPos3;

        RectTransform textRect4 = canvasObj4.GetComponent<RectTransform>();
        textRect4.sizeDelta = new Vector2(width / ratio, textRect4.sizeDelta.y);
        Vector3 canvasPos4 = canvasObj4.transform.position;
        canvasPos4.x += offset;
        canvasObj4.transform.position = canvasPos4;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
