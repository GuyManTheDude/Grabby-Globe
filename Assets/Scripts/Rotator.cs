using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotator : MonoBehaviour
{
    //[SerializeField] float rotationSpeed = 100f;
    //bool dragging = false;

    public Rigidbody rb;
    public int itemsCaught = 0;
    public Vector3 cachePos;
    public Quaternion cacheRot;
    public Vector3 cacheScale;
    public GameManager gm;
    public List<GameObject> Touchy;

    private bool istouchy = false;
    // Start is called before the first frame update
    void Start()
    {
        cacheScale = new Vector3(1, 1, 1);
    }

    // Update is called once per frame
    //void Update()
    //{
    //        dragging = Input.GetMouseButton(0);
    //}

    //private void OnMouseDown()
    //{
    //    rb.freezeRotation = true;
    //}

    //private void OnMouseDrag()
    //{
    //    dragging = true;
    //}

    //private void FixedUpdate()
    //{
    //    if(dragging)
    //    {
    //        rb.freezeRotation = false;
    //        rb.angularDrag = 0f;
    //        float x = Input.GetAxisRaw("Mouse X") * rotationSpeed * Time.fixedDeltaTime;
    //        float y = Input.GetAxisRaw("Mouse Y") * rotationSpeed * Time.fixedDeltaTime;

    //        rb.AddTorque(Vector3.down * x);
    //        rb.AddTorque(Vector3.right * y);
    //    }
    //    else
    //    {
    //        rb.angularDrag = 2f;
    //    }

    //}
    


    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Object")
        {

            if(!Touchy.Contains(collision.gameObject))
            {                
                itemsCaught++;
                gm.scoreCounter++;
                collision.gameObject.transform.parent = gameObject.transform;


                    MeshFilter[] meshFilters = GetComponentsInChildren<MeshFilter>();
                    CombineInstance[] combine = new CombineInstance[meshFilters.Length];

                    int i = 0;
                    cachePos = transform.position;
                    cacheRot = transform.rotation;
                    while (i < meshFilters.Length)
                    {
                        combine[i].mesh = meshFilters[i].sharedMesh;
                        transform.position = Vector3.zero;
                        transform.rotation = Quaternion.Euler(Vector3.zero);
                        combine[i].transform = meshFilters[i].transform.localToWorldMatrix;
                        meshFilters[i].gameObject.SetActive(false);

                        i++;
                    }
                    transform.position = cachePos;
                    transform.rotation = cacheRot;
                    transform.localScale = cacheScale;

                    transform.GetComponent<MeshFilter>().mesh = new Mesh();
                    transform.GetComponent<MeshFilter>().mesh = null;

                     transform.GetComponent<MeshFilter>().mesh.CombineMeshes(combine);
                     transform.GetComponent<MeshFilter>().mesh.RecalculateBounds();
                     MeshCollider meshCollider = gameObject.GetComponent<MeshCollider>();
                     meshCollider.sharedMesh = transform.GetComponent<MeshFilter>().mesh;

                     transform.gameObject.SetActive(true);


                     Destroy(collision.gameObject);

                    

                if (collision.gameObject.GetComponent<Grabity>())
                {
                    if (Vector3.Distance(Vector3.zero, gm.OOBPoint.position) < Vector3.Distance(Vector3.zero, collision.gameObject.transform.position))
                    {
                        gm.GameOver = true;
                        Debug.Log("Game Over");
                    }
                }
                else if (collision.gameObject.GetComponentInChildren<Grabity>())
                {
                    if (Vector3.Distance(Vector3.zero, gm.OOBPoint.position) < Vector3.Distance(Vector3.zero, collision.gameObject.GetComponentInChildren<Transform>().position))
                    {
                        gm.GameOver = true;
                        Debug.Log("Game Over");
                    }
                }


                if(gm.scoreCounter < gm.scoreThresh)
                {
                    gm.mySM.ScoreSound(0.08f);
                }
                Touchy.Clear();
                Touchy.Add(collision.gameObject);


            }
            

        }
    }

}
