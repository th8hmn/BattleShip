using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitBoard : MonoBehaviour
{
    public GameObject blockObj;
    public GameObject oceanObj;
    private const int num_x = 5;
    private const int num_y = 5;

    private Vector3 blockScale;
    
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
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
