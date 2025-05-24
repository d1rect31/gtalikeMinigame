using UnityEngine;

public class Slide : MonoBehaviour
{
    // get player speed

    private float speed;
    // Update is called once per frame
    void Start()
    {
        speed = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>().speed;
    }
    void Update()
    {
        transform.Translate(speed * Time.deltaTime * Vector2.up);
    }
}
