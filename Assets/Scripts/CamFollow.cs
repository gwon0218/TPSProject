using UnityEngine;

public class CamFollow : MonoBehaviour
{
    public Transform target;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    { 
        transform.position = target.position;
        
    }
}
