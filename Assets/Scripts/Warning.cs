using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Warning : MonoBehaviour
{
    public GameManager myGM;
    public GameObject warning;
    public bool isColliding = false;
    public List<GameObject> Colliders;
    // Start is called before the first frame update
    void Start()
    {
        myGM = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if(myGM.GameOver || myGM.pause)
        {
            warning.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Object")
        {
            Colliders.Add(other.gameObject);
            warning.SetActive(true);
            Invoke("StopWarning", 1f);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        Colliders.Remove(other.gameObject);
    }

    private void StopWarning()
    {
        if(Colliders.Count <= 0)
        {
            warning.SetActive(false);
        }       
    }
}
