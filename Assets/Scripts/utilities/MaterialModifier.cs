using System;
using UnityEngine;

public class MaterialModifier : MonoBehaviour{


    private Material material;
    private Color mateialTintColor;
    private float tintFadeSpeed;
    private bool fadeUp = false;
    void Awake(){
        mateialTintColor =new Color(1,0,0,0);
        SetMaterial(GetComponent<SpriteRenderer>().material);
        tintFadeSpeed = 6f;

    }

    void Update(){
            if( mateialTintColor.a > 0){
                        mateialTintColor.a = Mathf.Clamp01(mateialTintColor.a - tintFadeSpeed * Time.deltaTime);
                        material.SetColor("_Tint",mateialTintColor);
                    }
          
        
    }

    void SetMaterial(Material material){
        this.material = material;
    }

 

    public void SetTintColor(Color color, float tintFadeSpeed = 6f ){
        SetTintFadeSpeed(tintFadeSpeed);
        mateialTintColor = color;
        material.SetColor("_Tint",mateialTintColor);
    }

    
    public void SetTintFadeSpeed(float tintFadeSpeed){
        this.tintFadeSpeed = tintFadeSpeed;
    }
}