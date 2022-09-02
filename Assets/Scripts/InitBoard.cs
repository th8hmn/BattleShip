using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitBoard : MonoBehaviour
{
    public GameObject blockObj;
    public GameObject blockNoteObj;
    public GameObject noteBoardObj;
    public GameObject oceanObj;
    private const int num_x = 5;
    private const int num_y = 5;

    private GameObject noteBoardObj1;
    private GameObject noteBoardObj2;
    private bool isSet = false;

    private Vector3 blockScale;
    private Vector3 blockNoteScale;
    
    void Awake()
    {
        // ブロックを作成する
        blockScale = blockObj.transform.localScale;
        for (int i = 0; i < num_x; i++)
        {
            for (int j = 0; j< num_y; j++)
            {
                int y_ = num_y - (j + 1);
                GameObject g = Instantiate(blockObj, new Vector3((i -2) * blockScale.x, (y_ - 2) * blockScale.y, 0f), Quaternion.identity);
                g.name = NameDefinition.BLOCK + i.ToString("0") + j.ToString("0");
                g.transform.parent = this.transform;
            }
        }

        // 海を表示
        oceanObj.SetActive(true);


        // メモ用ブロックを作成する
        blockNoteScale = blockNoteObj.transform.localScale;
        for (int i = 0; i < num_x; i++)
        {
            for (int j = 0; j < num_y; j++)
            {
                int y_ = num_y - (j + 1);
                GameObject g1 = Instantiate(blockNoteObj, new Vector3((i - 2) * blockNoteScale.x, (y_ - 2) * blockNoteScale.y, 0f), Quaternion.identity);
                g1.name = NameDefinition.BLOCK + "_NOTE_" + i.ToString("0") + j.ToString("0");
                g1.transform.parent = noteBoardObj.transform;
            }
        }
        noteBoardObj1 = Instantiate(noteBoardObj, new Vector3(6f, 0f, 0f), Quaternion.identity);
        noteBoardObj2 = Instantiate(noteBoardObj, new Vector3(-6f, 0f, 0f), Quaternion.identity);
        noteBoardObj1.SetActive(false);
        noteBoardObj2.SetActive(false);

        Destroy(noteBoardObj);
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (GameSettings.isNote & !isSet)
        {
            noteBoardObj1.SetActive(true);
            noteBoardObj2.SetActive(true);
            isSet = true;
        }
    }
}
