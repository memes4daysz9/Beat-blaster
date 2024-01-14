
using System.Net;
using System.Text;
using System.Security.AccessControl;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class Script : MonoBehaviour
{
    public int LineCount;
    private AudioSource SongAudio;
    public AudioClip LTSong;
    public bool ReadTime = true; //makes sure the Reader only Reads one at a time
    public int milliseconds;
    public int XValue;
    public int YValue;
    public int NoteType;
    public int DegreeOffset;
    string line;
    string[] noteDataParsed;
    SpawnManager SPScript;
    // Start is called before the first frame update
    void Start()
    {
        SongAudio = GetComponent<AudioSource>();

        SPScript = GameObject.Find("SpawnManager").GetComponent<SpawnManager>();

 
        LineCount = File.ReadAllLines("C:/Users/benj0/Downloads/Create-with-VR_2020LTS/Create-with-VR_2020LTS/VR Room Project/Assets/CustomSongs/LavenderTown TheTrueJJ/Notes.chart").Length;

        SongAudio.PlayOneShot(LTSong,1.0f);



    }
        void Update()
    {
        if (SPScript.Loop == true){
            ReadTime = true;
        }
        ReadChartFile();
    }
    void ReadChartFile(){
    const int BufferSize = 128;
    using (var fileStream = File.OpenRead("C:/Users/benj0/Downloads/Create-with-VR_2020LTS/Create-with-VR_2020LTS/VR Room Project/Assets/CustomSongs/LavenderTown TheTrueJJ/Notes.chart"))
    using (var streamReader = new StreamReader(fileStream, Encoding.UTF8, true, BufferSize)) {

    while (!streamReader.EndOfStream)
    {
        line = streamReader.ReadLine();
        if (ReadTime == true){

        noteDataParsed = line?.Split(" ");

        if (noteDataParsed != null&&noteDataParsed.Length >= 5){
            
            

            Debug.Log("CurrentLine "+line);

            milliseconds = int.Parse(noteDataParsed[0]);
            XValue = int.Parse(noteDataParsed[1]);
            YValue = int.Parse(noteDataParsed[2]);
            NoteType = int.Parse(noteDataParsed[3]);
            DegreeOffset = int.Parse(noteDataParsed[4]);
        
            ReadTime = false;

        }


        }

            if (streamReader.EndOfStream){
            Debug.Log("Existing the loop");}
    }
  }
  
    }
}
