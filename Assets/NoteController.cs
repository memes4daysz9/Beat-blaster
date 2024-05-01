using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteController : MonoBehaviour
{
    public GameObject player;
    Vector3 MoveThingy;
    float calculatedZ;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Main Camera");
        MoveThingy = (transform.position - player.transform.position);
        calculatedZ = MoveThingy.z/Mathf.Abs(MoveThingy.z);
    }

    // Update is called once per frame
    void Update()
    {
                transform.Translate(new Vector3(0,calculatedZ,0)* Time.deltaTime);
    }
}
