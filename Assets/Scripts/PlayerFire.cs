using UnityEngine;

public class PlayerFire : MonoBehaviour
{
    public GameObject firePosition;

    public GameObject bombFactory;

    public float throwPower = 15f;

    void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            GameObject bomb = Instantiate(bombFactory);
            bomb.transform.position = firePosition.transform.position;
              
            Rigidbody rb = bomb.GetComponent<Rigidbody>();
            rb.AddForce(Camera.main.transform.forward * throwPower, ForceMode.Impulse);
        }
    }
}
