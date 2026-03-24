using System.Runtime.InteropServices;
using UnityEngine;

public class PlayerRotate : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    public float rotSpeed = 200f;

    float mx = 0;



    // Update is called once per frame
    void Update()
    {
        float mouse_X = Input.GetAxis("Mouse X");
        float mouse_Y = Input.GetAxis("Mouse Y");

        //Vector3 dir = new Vector3(-mouse_Y, mouse_X, 0);

        //transform.eulerAngles += dir * rotSpeed * Time.deltaTime;

        //Vector3 rot = transform.eulerAngles;
        //rot.x = Mathf.Clamp(rot.x, -90f, 90f);

        mx += mouse_X * rotSpeed * Time.deltaTime;


        transform.eulerAngles = new Vector3(0, mx, 0);

    }
}
