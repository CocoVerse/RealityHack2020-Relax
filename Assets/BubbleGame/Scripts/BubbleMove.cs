using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class BubbleMove : MonoBehaviour
{
    [SerializeField] BubbleColorCode colorCode;
    [SerializeField] GameObject spawnOnPop;

    public BubbleColorCode ColorCode => colorCode;

    [SerializeField] float lifespan = 2f;
    [SerializeField] float speed = 0.5f;
    private float startTime;

    // Start is called before the first frame update
    void Start()
    {
        startTime = Time.time;
    }

    internal void Pop(bool good) {
        if (spawnOnPop != null) {
            var obj = Instantiate(spawnOnPop, transform.position, transform.rotation);
            obj.transform.localScale = transform.localScale;
        }
        Destroy(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += transform.forward * Time.deltaTime * speed;
        if (Time.time > startTime + lifespan) Pop(false);
    }
}
