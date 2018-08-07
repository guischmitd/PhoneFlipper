using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhoneBehaviour : MonoBehaviour {

    public Texture smile01;
    public Texture smile02;
    public Texture smileFlying;

    public bool isFlying = false;
    public bool isCracked = false;
    public bool onGround = false;
    public bool inspecting = false;
    bool armed = true;

    Rigidbody rb;
    public float snapSpeed = 12;
    public float velDamping = .8f;
    Vector3 onGroundPosition;
    Quaternion onGroundRotation;
    Vector3 inspectOffset = (Vector3.up);

    Vector3 startPos;
    Quaternion startRot;
    Vector3 camStartPos;
    Quaternion camStartRot;

    public Material crackedMat;
    Material defaultMat;
    public GameObject phoneGlass;
    Renderer rend;

    Camera cam;
    Collider col;

    float t;
    public float highest;
    public float hiScore;


    // Use this for initialization
    void Start() {
        cam = GameObject.FindObjectOfType<Camera>();
        col = GetComponent<BoxCollider>();

        rb = GetComponent<Rigidbody>();
        rb.useGravity = false;

        rend = phoneGlass.GetComponent<Renderer>();
        defaultMat = rend.material;

        highest = 0;

        startPos = transform.position;
        startRot = transform.rotation;

        camStartPos = cam.transform.position;
        camStartRot = cam.transform.rotation;
        
    }

    // Update is called once per frame
    void Update() {

        
        if (!isFlying && !onGround && !inspecting)
        {
            
            RbFloat(1, 0.003f);

            if (Time.frameCount % 60 == 0)
            {
                print(Time.frameCount);

                if (rend.material.mainTexture == smile01)
                {
                    rend.material.mainTexture = smile02;
                } else
                {
                    rend.material.mainTexture = smile01;
                }
            }

            if (Input.GetMouseButton(0))
            {
                Ray ray = cam.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                Physics.Raycast(ray, out hit);

                Vector3 desired = (new Vector3(hit.point.x, hit.point.y, 0) - new Vector3(rb.position.x, rb.position.y, 0));
                rb.velocity = snapSpeed * desired;
            }
            if (transform.position.y < 6.5f)
            {
                rb.velocity *= velDamping;
            }
            
        }
        
        if(rb.position.y >= 7f) // REWORK
        {            
            isFlying = true;

            if (armed)
            {
                rb.AddForce(500 * Vector3.forward);
                rb.AddTorque(Random.insideUnitSphere.normalized * 80);
                armed = false;
            }

            rb.useGravity = true;
        }

        if (isFlying)
        {
            rend.material.mainTexture = smileFlying;
            rend.material.mainTextureScale = new Vector2(4.608f, 2.4f);
            rend.material.mainTextureOffset = new Vector2(.25f, -.41f);

            cam.transform.LookAt(rb.position);
        }

        if(onGround)
        {
            t += Time.deltaTime / 5;
            float dist = (Vector3.Distance(cam.transform.position, rb.position));
            if (dist > 2.6f)
            {
                cam.transform.position = Vector3.Lerp(cam.transform.position, rb.position + 2.5f * Vector3.up, t);
                cam.transform.LookAt(rb.position);
            }

            if (Input.GetMouseButtonDown(0))
            {
                Ray ray = cam.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                Physics.Raycast(ray, out hit);

                if (hit.collider == col)
                {
                    onGround = false;
                    inspecting = true;

                    rb.useGravity = false;
                    t = 0;

                    onGroundPosition = rb.position;
                    onGroundRotation = rb.rotation;
                }
            }
        }

        if(inspecting)
        {
            if (t <= 1)
            {
                t += Time.deltaTime * 3;

                rb.position = Vector3.Lerp(onGroundPosition, onGroundPosition + inspectOffset, t);
                rb.rotation = Quaternion.Lerp(onGroundRotation, Quaternion.Euler(0, 0, 180), t);
            } else
            {
                RbFloat(1f, 0.002f);
            }

            cam.transform.position = new Vector3(transform.position.x, 4f, transform.position.z);
            cam.transform.LookAt(transform);

            rb.angularVelocity = Vector3.zero;
            rb.velocity = Vector3.zero;

            if (t >= 1 && Input.GetMouseButtonDown(0))
            {

                if (highest > hiScore)
                {
                    hiScore = highest;
                }

                if (isCracked)
                {
                    hiScore = 0;
                }                

                SetupThrow();
            }
        }

        if (rb.position.y > highest)
        {
            highest = rb.position.y;

            if (highest > hiScore)
            {
                hiScore = highest;
            }
        }
        
    }

    void SetupThrow()
    {

        inspecting = false;
        isFlying = false;
        onGround = false;
        isCracked = false;
        rend.material = defaultMat;

        highest = 0;

        rb.position = startPos;
        rb.rotation = startRot;

        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;

        cam.transform.position = camStartPos;
        cam.transform.rotation = camStartRot;

        armed = true;
        isCracked = false;

        rend.material.mainTexture = smile01;

        rend.material.mainTextureScale = new Vector2(3.8f, 2f);
        rend.material.mainTextureOffset = new Vector2(.77f, -.07f);

    }

    void RbFloat(float f, float amp) {
        float h = (amp * Mathf.Sin(Time.time * f * Mathf.PI));
        rb.position -= Vector3.up * h;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (Random.value > 0.6f && !onGround)
        {
            isCracked = true;
            rend.material = crackedMat;
        }

        if(!inspecting)
        {
            isFlying = false;
            onGround = true;
        }
    }

}