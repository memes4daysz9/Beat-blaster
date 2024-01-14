using System.Net;
using System.Reflection;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
//spawns notes during runtime
class NoteSpawner
{
        int NoteMS;
        int NoteX;
        int NoteY;
        int NoteDegreeOffset;
        int NoteDirection;
        bool GetNextNote;
        bool SongStart = false;
        public float Timerf = 0f;
        decimal dec;
        double TimerD;
        public double simplifiedTimer;
        public double SecondsTimer;
        public double milliseconds;
        public float FPS;
        float additiveFrameDelay;
        float FrameDelayCalc;
        int SetNoteType;
        bool NoteType1(int msValue,int x,int y,int DO){ //1
            GetNextNote = false;
            SetNoteType = 1;
            Debug.Log("Instantiating a normal block at " + msValue);
            additiveFrameDelay = msValue/FPS;
            return (GetNextNote);}
        bool NoteType2(int msValue,int x,int y,int DO){  //2
            GetNextNote = false;
            SetNoteType = 2;
            Debug.Log("Instantiating a bomb at " + msValue);
            additiveFrameDelay = msValue/FPS;
            return (GetNextNote);}
        bool NoteType3(int msValue,int x,int y,int DO,int dir){ //3
            GetNextNote = false;
            SetNoteType = 3;
            Debug.Log("Instantiating a directional block at " + msValue);
            additiveFrameDelay = msValue/FPS;
            return (GetNextNote);}
        bool NoteType4(int msValue,int x,int y,int DO){ //4
            GetNextNote = false;
            SetNoteType = 4;
            Debug.Log("Instantiating a Beat block at " + msValue);
            additiveFrameDelay = msValue/FPS;
            return (GetNextNote);}
        bool NoteType5(int msValue,int x,int y,int z,int DO, int x2,int y2){ //5
            GetNextNote = false;
            SetNoteType = 5;
            Debug.Log("Instantiating a block at " + msValue);
            additiveFrameDelay = msValue/FPS;
            return (GetNextNote);}
        void Start(){
            Application.targetFrameRate = -1; // make game Run as Fast as possible
        }
        void Update(){
            FPS = 1.0f / Time.deltaTime;
            Timerf = Time.time;
            dec = new decimal(Timerf);
            TimerD = (double)dec;
            FrameDelayCalc = additiveFrameDelay + NoteMS + (float)milliseconds + 5;// 5 added for sloppy calculations
            simplifiedTimer = Math.Round(TimerD,3);

            milliseconds = Math.Round(Timerf*1000,0);
            SecondsTimer = Math.Round(milliseconds*1000,0);// standard time calculations
            
            if (SongStart == true){
                if (FrameDelayCalc <= NoteMS){ // note spawning area
                    if(SetNoteType == 1){
                        // set normal Note
                    }else if (SetNoteType == 2){
                        // set bomb
                    }else if (SetNoteType == 3){
                        // set slash Note
                    }else if (SetNoteType == 4){
                        // set Blast Note
                    }else if (SetNoteType == 5){
                        // set Box
                    }else {
                        //set Blast Note.
                        // its a blast note as it has the least amount of requirements, meaning that it'll always have everything needed to spawn the note, otherwise it was improperly made
                    }
                }
            }

    }
    
}
    class SongFileGrabber{ // Folder Buisiness
        void SongFilesGrabber(){
        //scan folders For songs
        DirectoryInfo SongDir = new DirectoryInfo("C:/Program Files (x86)/Beat Blaster/Songs");
        DirectoryInfo[] SongDirArr = SongDir.GetDirectories();
        }
    }
