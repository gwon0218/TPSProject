using UnityEngine;

public class BilBoard : MonoBehaviour
{

    public Transform target;


    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.forward = target.transform.forward;
    }
}
