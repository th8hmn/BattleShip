using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InitChart : MonoBehaviour
{
    public GameObject chartObj;
    public GameObject blockObj;
    public GameObject noteChartObj;
    private GameObject noteChart1;
    private GameObject noteChart2;
    private bool isSet = false;
    private const int num = 5;

    private Vector3 chartScale;
    private Vector3 blockScale;
    float weight;

    private Vector3[] positions = new Vector3[]
    {
        new Vector3(-0.49f, 0.49f, 0f),
        new Vector3(0.49f, 0.49f, 0f),
        new Vector3(0.49f, -0.49f, 0f),
        new Vector3(-0.49f, -0.49f, 0f)
    };

    void Awake()
    {
        InstantiateChart(this.transform);

        InstantiateChart(noteChartObj.transform);
        noteChart1 = Instantiate(noteChartObj, new Vector3(6f, 0f, 0f), Quaternion.identity);
        noteChart2 = Instantiate(noteChartObj, new Vector3(-6f, 0f, 0f), Quaternion.identity);
        noteChart1.SetActive(false);
        noteChart2.SetActive(false);
        Destroy(noteChartObj);
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
            noteChart1.SetActive(true);
            noteChart2.SetActive(true);
            isSet = true;
        }
    }

    private void InstantiateChart(Transform parentTransform)
    {
        chartScale = chartObj.transform.localScale;
        blockScale = blockObj.transform.localScale;
        weight = 0.5f;
        int j = 2;

        // 角の空白座標を作成
        Vector3 pos1 = new Vector3
            (-j * blockScale.y - (blockScale.y + chartScale.y * weight) / 2,
            j * blockScale.y + (blockScale.y + chartScale.y * weight) / 2,
            0f);
        string name1 = NameDefinition.CHART + "0" + "0";
        CreateChart(pos1, name1, true, true, 0, parentTransform);

        for (int i = 0; i < num; i++)
        {
            // 列の座標を作成
            Vector3 pos2 = new Vector3
                ((i - 2) * chartScale.x,
                j * blockScale.y + (blockScale.y + chartScale.y * weight) / 2,
                0f);
            string name2 = NameDefinition.CHART + i.ToString("0") + "_col";
            CreateChart(pos2, name2, false, true, i, parentTransform);

            // 行の座標を作成
            Vector3 pos3 = new Vector3
                (-j * blockScale.y - (blockScale.y + chartScale.y * weight) / 2,
                (i - 2) * chartScale.x,
                0f);
            string name3 = NameDefinition.CHART + i.ToString("0") + "_row";
            CreateChart(pos3, name3, true, false, i, parentTransform);
        }
    }

    // 座標作成の処理
    private void CreateChart(Vector3 position, string name, bool half_x, bool half_y, int idx, Transform parentTransform)
    {
        GameObject g = Instantiate(chartObj, position, Quaternion.identity);
        g.name = name;
        g.transform.parent = parentTransform;
        GameObject canvasObj = g.transform.GetChild(1).gameObject;
        GameObject textObj = canvasObj.transform.GetChild(0).gameObject;

        Vector3 tmpScale_g = chartScale;
        Vector3 tmpScale_txt = textObj.transform.localScale;
        if (half_x)
        {
            tmpScale_g.x = chartScale.x * weight;
            tmpScale_txt.x = chartScale.x / weight;
        }
        if (half_y)
        {
            tmpScale_g.y = chartScale.y * weight;
            tmpScale_txt.y = chartScale.y / weight;
        }        
        g.transform.localScale = tmpScale_g;
        textObj.transform.localScale = tmpScale_txt;

        LineRenderer lineRenderer = g.transform.GetChild(0).gameObject.GetComponent<LineRenderer>();
        LineSetting(lineRenderer);

        Text text = textObj.GetComponent<Text>();
        if (half_x && half_y)
        {
            text.text = "";
        }
        else if (half_x)
        {
            int idx_ = num - idx;
            text.text = idx_.ToString("0");
        }
        else if (half_y)
        {
            text.text = PositionManager.GetChartAlphabet(idx);
        }
    }

    // 枠の設定
    private void LineSetting(LineRenderer lineRenderer)
    {
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
