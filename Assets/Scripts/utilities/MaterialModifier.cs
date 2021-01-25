using System;
using UnityEngine;
using UnityEngine.UI;
public class MaterialModifier : MonoBehaviour
{


    private Material material;
    private Color mateialTintColor;
    private Color materialColor;

    private float tintFadeSpeed;
    private float fadeSpeed;
    void Awake()
    {
        SetMaterial(GetComponent<SpriteRenderer>() ? GetComponent<SpriteRenderer>().material : GetComponent<Image>().material);
        mateialTintColor = new Color(1, 0, 0, 0);
        materialColor = new Color(1, 1, 1, 1);


        SetTintFadeSpeed(6f);

    }
    private bool IsFadingIn = false;

    private bool IsFadingOut = false;

    void Update()
    {
        //Decrease transparancy of the tint color when alpha is set to 1
        if (mateialTintColor.a > 0)
        {
            mateialTintColor.a = Mathf.Clamp01(mateialTintColor.a - tintFadeSpeed * Time.deltaTime);
            material.SetColor("_Tint", mateialTintColor);
        }


        //Decrease transparancy of the main color when alpha is set to 1
        if (IsFadingOut)
        {
            if (materialColor.a > 0)
            {

                materialColor.a = Mathf.Clamp01(materialColor.a - fadeSpeed * Time.deltaTime);
                material.SetColor("_Color", materialColor);
            }
            else if (materialColor.a == 0) IsFadingOut = false;



        }
        if (IsFadingIn)
        {

            //Increase transparancy of the main color when alpha is set to 0
            if (materialColor.a < 1)
            {

                materialColor.a = Mathf.Clamp01(materialColor.a + fadeSpeed * Time.deltaTime);
                material.SetColor("_Color", materialColor);
            }
            else if (materialColor.a == 1) IsFadingIn = false;


        }

    }

    void SetMaterial(Material material)
    {
        this.material = material;
    }



    public void SetTintColor(Color color, float tintFadeSpeed = 6f)
    {
        SetTintFadeSpeed(tintFadeSpeed);
        mateialTintColor = color;
        material.SetColor("_Tint", mateialTintColor);
    }

    public void SetMainColor(Color color, float fadeSpeed = 3f)
    {
        SetMainColorFadeSpeed(fadeSpeed);
        materialColor = color;
        material.SetColor("_Color", materialColor);
        IsFadingIn = color.a == 0 ? true : false;
        IsFadingOut = color.a == 1 ? true : false;
    }

    public void SetTintFadeSpeed(float tintFadeSpeed)
    {
        this.tintFadeSpeed = tintFadeSpeed;
    }

    public void SetMainColorFadeSpeed(float fadeSpeed)
    {

        this.fadeSpeed = fadeSpeed;
    }
}