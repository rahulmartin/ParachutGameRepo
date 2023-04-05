using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DropController : MonoBehaviour , Drops
{
    [SerializeField]
    GameObject parent;
    [SerializeField]
    float droppingSpeed;
    [SerializeField]
    Animator animController;
    [SerializeField]
    TextMeshProUGUI tmproText;

    private bool _questionResult;

    public bool isWrongStatement
    {
        get { return _questionResult; }
        set { _questionResult = value; }
    }

    const string createdAnimation = "Created";
    const string idleAnimation = "Idle";
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        parent.transform.position += Vector3.down * Time.deltaTime * droppingSpeed;
    }

    private void OnEnable()
    {
        animController.Play(createdAnimation);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        GameManager.instance.ObjectReachedGround(this.gameObject);
    }

    public void SetText(string s)
    {
        tmproText.text = s;
    }
}
