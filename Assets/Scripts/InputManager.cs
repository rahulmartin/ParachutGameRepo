using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    Vector2 inputPosition;
    Camera mainCamera;
    float inputDelay = 0.2f;
    float timeSinceLastTouch = 0f;
    // Start is called before the first frame update
    void Start()
    {
        mainCamera = Camera.main;
    }
    // Update is called once per frame
    void Update()
    {
        if (timeSinceLastTouch < inputDelay)
        {
            timeSinceLastTouch += Time.deltaTime;
            return;
        }

        if (Input.touchCount > 0 && Input.touches[0].phase == TouchPhase.Began)
        {
            timeSinceLastTouch = 0;
            inputPosition = Input.touches[0].position;
            CheckInput(inputPosition);
        } 
        else if(Input.GetMouseButton(0) && Input.GetMouseButtonDown(0))
        {
            timeSinceLastTouch = 0;
            inputPosition = Input.mousePosition;
            CheckInput(inputPosition);
        }
    }

    void CheckInput(Vector2 input)
    {
        Vector3 inputToWorld = Camera.main.ScreenToWorldPoint(input);
        RaycastHit2D hitInfo = Physics2D.Raycast(inputToWorld, Vector2.zero);
        if(hitInfo)
        if(hitInfo.collider.tag == "Drops")
        {
            GameManager.instance.OnObjectTouched(hitInfo.collider.gameObject);
        } 
    }
}
