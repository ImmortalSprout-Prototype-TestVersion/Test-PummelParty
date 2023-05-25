using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    public List<Transform> transforms = new List<Transform>();
    
    void Start()
    {
        for (int i = 0; i < transforms.Count ;++i)
        {
            Vector3 direction = (transform.position - transforms[i].position).normalized;
            Debug.Log($"{i}번째 큐브와의 방향 = {direction}");
        }
        Debug.Log(Vector3.right);
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
