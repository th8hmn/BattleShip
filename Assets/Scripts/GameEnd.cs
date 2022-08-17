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

    // ‚·‚×‚ÄíŠÍ‚ÌHP‚ª0‚Æ‚È‚Á‚½ê‡AƒQ[ƒ€I—¹
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
