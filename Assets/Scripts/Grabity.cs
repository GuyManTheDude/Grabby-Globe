using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grabity : MonoBehaviour
{
    private Vector3 target = Vector3.zero;
    public float speed = 1f;
    public float spinSpeed = 1f;
    public bool outOfBounds = true;
    private Vector3 spin;
    public Vector3 rotateOffset;

    private bool GO = false;

    public bool isUI = false;
    // Start is called before the first frame update
    void Start()
    {        
        if(!isUI)
        {
            Invoke("SetGO", 0.33f);
            transform.LookAt(Vector3.zero);            
            speed *= Random.Range(1f, 1.1f);           
        }
        transform.Rotate(rotateOffset);
        spin = new Vector3(Random.Range(0f, 30f), Random.Range(0f, 30f), Random.Range(0f, 30f));
    }

    // Update is called once per frame
    void Update()
    {
        if(GO)
        {
            if (!isUI)
            {
                transform.position = Vector3.MoveTowards(transform.position, Vector3.zero, Time.deltaTime * speed * (Vector3.Distance(transform.position, Vector3.zero) / 5));
            }         
            gameObject.transform.Rotate(spin * Time.deltaTime * spinSpeed);
        }
        else if (isUI)
        {
            gameObject.transform.Rotate(spin * Time.deltaTime * spinSpeed);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Out")
        {
            outOfBounds = false;
        }
    }

    void SetGO()
    {
        GO = !GO;
    }
}
