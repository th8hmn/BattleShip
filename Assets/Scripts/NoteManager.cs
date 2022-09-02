using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NoteManager : MonoBehaviour
{
    public Toggle battleshipToggle;
    public Toggle destroyerToggle;
    public Toggle submarineToggle;
    public Toggle nothingToggle;
    public Text nothingText;
    public LineRenderer lineRenderer;

    private float rVal;
    private float gVal;
    private float bVal;
    private const float baseVal = 255;

    private Material mat;
    private Color objColor;

    public SpriteRenderer battleshipMaterial;
    public SpriteRenderer destoryerMaterial;
    public SpriteRenderer submarineMaterial;
    private float defaultColorAlpha = 1.0f;
    private float changedColorAlpha = 0.3f;

    // Start is called before the first frame update
    void Start()
    {
        rVal = 255 / baseVal;
        gVal = 255 / baseVal;
        bVal = 255 / baseVal;

        mat = this.GetComponent<Renderer>().material;
        objColor = new Color(rVal, gVal, bVal, 1.0f);
        mat.color = objColor;

        nothingText.color = new Color(0f, 0f, 0f, changedColorAlpha * 0.5f);

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

    // Update is called once per frame
    void Update()
    {
        if (!battleshipToggle.isOn && !destroyerToggle.isOn && !submarineToggle.isOn)
        {
            nothingToggle.isOn = true;
        }
        else
        {
            nothingToggle.isOn = false;
        }

        if (battleshipToggle.isOn)
        {
            rVal = 255 / baseVal;
            objColor.r = rVal;
        }
        else
        {
            rVal = 50 / baseVal;
            objColor.r = rVal;
        }
        if (destroyerToggle.isOn)
        {
            gVal = 255 / baseVal;
            objColor.g = gVal;
        }
        else
        {
            gVal = 50 / baseVal;
            objColor.g = gVal;
        }
        if (submarineToggle.isOn)
        {
            bVal = 255 / baseVal;
            objColor.b = bVal;
        }
        else
        {
            bVal = 50 / baseVal;
            objColor.b = bVal;
        }

        mat.color = objColor;
    }

    public void ChangebattleshipColor()
    {
        Color changedColor = new Color(1.0f, 1.0f, 1.0f, defaultColorAlpha);

        if (!battleshipToggle.isOn)
        {
            changedColor = new Color(1.0f, 1.0f, 1.0f, changedColorAlpha);
        }

        battleshipMaterial.color = changedColor;
    }

    public void ChangedestoryerColor()
    {
        Color changedColor = new Color(1.0f, 1.0f, 1.0f, defaultColorAlpha);

        if (!destroyerToggle.isOn)
        {
            changedColor = new Color(1.0f, 1.0f, 1.0f, changedColorAlpha);
        }

        destoryerMaterial.color = changedColor;
    }

    public void ChangesubmarineColor()
    {
        Color changedColor = new Color(1.0f, 1.0f, 1.0f, defaultColorAlpha);

        if (!submarineToggle.isOn)
        {
            changedColor = new Color(1.0f, 1.0f, 1.0f, changedColorAlpha * 0.5f);
        }

        submarineMaterial.color = changedColor;
    }

    public void CheckNothing()
    {
        if (nothingToggle.isOn)
        {
            battleshipToggle.isOn = false;
            destroyerToggle.isOn = false;
            submarineToggle.isOn = false;
            nothingText.color = new Color(0f, 0f, 0f, defaultColorAlpha);
        }
        else
        {
            nothingText.color = new Color(0f, 0f, 0f, changedColorAlpha * 0.5f);
        }
    }
}
