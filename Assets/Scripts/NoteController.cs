using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteController : MonoBehaviour
{
    public GameObject player;
    Vector3 MoveThingy;
    float calculatedZ;
    public float Rotation;
    public Transform Note;
    public float Multiplier;
    //GameObject.Find("MyObject").GetComponent<MyScript>().MyVariable;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Main Camera");
        Multiplier = GameObject.Find("Global Behaviours").GetComponent<GlobalBehaviours>().SpeedMultipliers;
        MoveThingy = (transform.position - player.transform.position);
        calculatedZ = MoveThingy.z/Mathf.Abs(MoveThingy.z);
        Rotation = Note.eulerAngles.z;

    }

    // Update is called once per frame
    void Update()
    {

        transform.Translate(new Vector3(0,0,-calculatedZ) *Time.deltaTime* Multiplier,Space.World);

    }
}
