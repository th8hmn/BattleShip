using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEnd : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // すべて戦艦のHPが0となった場合、ゲーム終了
    public bool SetFinishGameFlag(int[] hp)
    {
        bool isAllDestroyed = false;

        for (int i = 0; i < hp.Length; i++)
        {
            if (hp[i] != 0)
            {
                isAllDestroyed = false;
                break;
            }
            else
            {
                isAllDestroyed = true;
            }
        }

        return isAllDestroyed;
    }
}
