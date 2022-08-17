using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrameManager : MonoBehaviour
{
    LineRenderer lineRenderer;
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < 5; i++)
        {
            for (int j = 0; j < 5; j++)
            {
                GameObject gameObject = GameObject.Find(NameDefinition.BLOCK + i.ToString("0") + j.ToString("0"));
                lineRenderer = gameObject.transform.GetChild(0).gameObject.GetComponent<LineRenderer>();
                Vector3[] positions = new Vector3[]
                {
                    new Vector3(-0.49f, 0.49f, 0f),
                    new Vector3(0.49f, 0.49f, 0f),
                    new Vector3(0.49f, -0.49f, 0f),
                    new Vector3(-0.49f, -0.49f, 0f)
                };
                lineRenderer.loop = true;
                lineRenderer.positionCount = positions.Length;
                lineRenderer.SetPositions(positions);

                lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
                lineRenderer.startColor = Color.black;
                lineRenderer.endColor = Color.black;

                lineRenderer.startWidth = 0.02f;
                lineRenderer.endWidth = 0.02f;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // ˜g‚ÌF‚ğ‰Šúó‘Ô‚É–ß‚·
    public void InitColor(GamePlayer player)
    {
        for (int i = 0; i < 5; i++)
        {
            for (int j = 0; j < 5; j++)
            {
                GameObject gameObject = GameObject.Find(NameDefinition.BLOCK + i.ToString("0") + j.ToString("0"));
                lineRenderer = gameObject.transform.GetChild(0).gameObject.GetComponent<LineRenderer>();
                lineRenderer.startColor = Color.black;
                lineRenderer.endColor = Color.black;
                lineRenderer.sortingOrder = 2;

                lineRenderer.startWidth = 0.02f;
                lineRenderer.endWidth = 0.02f;

                lineRenderer.numCornerVertices = 0;
            }
        }
    }

    // UŒ‚”ÍˆÍ‚Ì˜g‚ÌF‚ğ•ÏX‚·‚é
    public void SetColorAttackArea(GamePlayer player)
    {
        for (int i = 0; i < 5; i++)
        {
            for (int j = 0; j < 5; j++)
            {
                GameObject gameObject = GameObject.Find(NameDefinition.BLOCK + i.ToString("0") + j.ToString("0"));
                lineRenderer = gameObject.transform.GetChild(0).gameObject.GetComponent<LineRenderer>();
                for (int k = 0; k < player.attackAreaBlockNameList.Length; k++)
                {
                    if (player.attackAreaBlockNameList[k] == gameObject.name)
                    {
                        lineRenderer.startColor = Color.red;
                        lineRenderer.endColor = Color.red;
                        lineRenderer.sortingOrder = 3;

                        lineRenderer.startWidth = 0.04f;
                        lineRenderer.endWidth = 0.04f;

                        lineRenderer.numCornerVertices = 10;
                    }
                }
            }
        }
    }

    // ˆÚ“®”ÍˆÍ‚Ì˜g‚ÌF‚ğ•ÏX‚·‚é
    public void SetColorMoveArea(GamePlayer player)
    {
        for (int i = 0; i < 5; i++)
        {
            for (int j = 0; j < 5; j++)
            {
                GameObject gameObject = GameObject.Find(NameDefinition.BLOCK + i.ToString("0") + j.ToString("0"));
                lineRenderer = gameObject.transform.GetChild(0).gameObject.GetComponent<LineRenderer>();
                for (int k = 0; k < player.moveAreaBlockNameList.Length; k++)
                {
                    if (player.moveAreaBlockNameList[k] == gameObject.name)
                    {
                        lineRenderer.startColor = Color.green;
                        lineRenderer.endColor = Color.green;
                        lineRenderer.sortingOrder = 3;

                        lineRenderer.startWidth = 0.04f;
                        lineRenderer.endWidth = 0.04f;

                        lineRenderer.numCornerVertices = 10;
                    }
                }
            }
        }
    }
}
