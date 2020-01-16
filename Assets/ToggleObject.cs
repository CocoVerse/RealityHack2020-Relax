using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleObject : MonoBehaviour
{
    [SerializeField] bool initialState = false;
    [SerializeField] GameObject target;
    [SerializeField] KeyCode key = KeyCode.F1;


    private void Awake() {
        target.SetActive(initialState);
    }
    
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(key)) target.SetActive(!target.activeSelf);
    }
}
