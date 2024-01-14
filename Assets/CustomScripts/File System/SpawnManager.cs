using System.Net;
using System.Reflection;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SpawnManager : MonoBehaviour
{

public float Timerf = 0f;
decimal dec;
double TimerD;
public double simplifiedTimer;
public double SecondsTimer;
public double milliseconds;
bool SpawnNow;
public float delayforSpawn;
public GameObject CubePrefab;
public GameObject BombPrefab;
public Vector3 CubePos;
public bool HitOnce;
public bool Loop;

//Song Scriptss
public Script LavenderTownScript;







    // Start is called before the first frame update
    void Start()
    {
        LavenderTownScript = GameObject.Find("SongBlock").GetComponent<Script>();


    }

    // Update is called once per frame
    void LateUpdate()
    {

        Timerf = Time.time;
        dec = new decimal(Timerf);
        TimerD = (double)dec;

        simplifiedTimer = Math.Round(TimerD,3);

        milliseconds = Math.Round(Timerf*1000,0);
        SecondsTimer = Math.Round(milliseconds*1000,0);


        SpawnNow = !LavenderTownScript.ReadTime;
        CubePos = new Vector3(0,LavenderTownScript.YValue,LavenderTownScript.XValue);

        delayforSpawn = LavenderTownScript.milliseconds - (float)milliseconds;



        if (LavenderTownScript.ReadTime == false){
            NoteSpawner();
            Loop = false;
        }
        if (delayforSpawn <= -6){
            Debug.Log("should Be Looping");
            Loop = true;
            LavenderTownScript.ReadTime = true;
        }

       

    }
    void NoteSpawner(){
        if (delayforSpawn >= 1){
            //yield return new WaitForSeconds(delayforSpawn/1000);
            HitOnce = false;
        }else if ((delayforSpawn <= 0)&& (HitOnce == false)) {
            Debug.Log("CubeInstantiating");
            Instantiate(CubePrefab,CubePos,transform.rotation);
            HitOnce = true;

        }

    }
}
