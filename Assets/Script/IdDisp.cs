using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IdDisp : MonoBehaviour
{
    private GameObject parent;
    private Text text;
    // Start is called before the first frame update
    void Start()
    {
        parent = transform.parent.gameObject;
        this.text = this.GetComponent<Text>();
        this.text.text = parent.name;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
