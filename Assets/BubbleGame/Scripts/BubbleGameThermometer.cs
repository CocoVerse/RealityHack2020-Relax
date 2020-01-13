using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class BubbleGameThermometer : MonoBehaviour
{
    [SerializeField] Transform yAdjuster;
    [SerializeField] BubbleGameScoreTracker score;
    [SerializeField] Renderer target;
    private MaterialPropertyBlock propertyBlock;

    // Start is called before the first frame update
    void Start() {
        propertyBlock = new MaterialPropertyBlock();
        target.GetPropertyBlock(propertyBlock);
        score.Heat.Subscribe(ApplyHeat).AddTo(this);
    }
    void ApplyHeat(float heat) {
        propertyBlock.SetFloat("_DisplayValue", heat);
        target.SetPropertyBlock(propertyBlock);
    }
    
}
