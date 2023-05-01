using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] public float speed = 20f;
    [SerializeField] private float rotateSpeed = 1f;
    [SerializeField] private Vector3 rotationDirection = new Vector3();
    public Rigidbody2D rb;
    private Camera mainCam;
    private Vector3 mousePos;
    // Start is called before the first frame update
    void Start()
    {
        mainCam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        mousePos = mainCam.ScreenToWorldPoint(Input.mousePosition);
        Vector3 direction = mousePos - transform.position;
        rb.velocity = new Vector2(direction.x, direction.y).normalized * speed;
    }

    void Update(){
        transform.Rotate(rotateSpeed * rotationDirection * Time.deltaTime);
        StartCoroutine(SelfDestruct());
    }

    IEnumerator SelfDestruct(){
        yield return new WaitForSeconds(3f);
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D hitInfo)
    {
        if(hitInfo.name != "Player"){
            Destroy(gameObject);
        }
    }
}
