using System.Collections;
using System;
using UnityEngine;
using System.IO;
using System.Media;
using System.Threading;
public class GameManagement : MonoBehaviour
{
    [Header("Random Info")]
     public AudioSource audioData;
        public bool StartSong;
        public bool playingSong;
        public int currentSong;
        public bool TellNoteToSpawn;
        
        [Header("File System")]
        public string[] SFXList; //List Of Sound Effect and their respected "ID"

        public int[][] parsedArrays;
        public string[] lines;
        public string[] values;
        
        public string[] folderNames;//would also be the song names per each song
                string[] SongList; //List Of Songs And Their Respected "ID"
                public string songsFolderPath = Application.dataPath + "/Songs";
                
            [Header("Note Calculations")]
    public int NoteType;
    public int calculatedmillisecondoffset;
    public int millisecondValue;
    public int currentTime;
    public int TimeAtStartOfLevel;
    public int FrameRate;
    public int NoteOffset = 2000;//Note Offset for the note coming at the user

    public bool NoteSpawned = false;

    [Header("In-Game Info")]
    public float MissMultiplier;
    public int Health = 100;
    public int Combo = 0;
    public int Score = 0;
    float Multiplier = 1;
    [Header("Notes")]
    public GameObject Bomb; // Tag "Bomb"
    public GameObject SlashNote; // Tag "Slash"
    public GameObject NormalNote; //Tag "Normal"
    public GameObject BlasterNote; //Tag "Blaster"

    const string BombTag = "Bomb";
    const string SlashTag = "Slash";
    const string NormalTag = "Normal";// tags for easy use
    const string BlasterTag = "Blaster";

    public GameObject LeftBlaster;
    public GameObject RightBlaster;

    RaycastHit LeftBlasterhit;
    RaycastHit RightBlasterhit;

    public int X;
    public int Y;
    public int DegreeOffset;
    public float calculatedDistanceOffset;

    public GameObject NoteToSpawn;
    System.Media.SoundPlayer player;


        public int[][] LoadNotesFromFile(string fileName)
    {
        // Check if the file exists
        if (!File.Exists(fileName))
        {
            Debug.LogError("File " + fileName + " not found!");
            return null;
        }

        // Read all lines from the file
        
        lines = File.ReadAllLines(fileName);

        // Initialize arrays to store parsed values
        parsedArrays = new int[lines.Length][];

        // Loop through each line
        for (int lineNumber = 0; lineNumber < lines.Length; lineNumber++)
        {
            // Split the line by commas
            values = lines[lineNumber].Split(',');

            // Ensure the line has the correct number of values
            if (values.Length != 6)
            {
                Debug.LogWarning("Invalid line at line number " + lineNumber + ": " + lines[lineNumber]);
                continue;
            }

            // Initialize array to store parsed values for this line
            parsedArrays[lineNumber] = new int[6];

            // Parse each value from string to integer and store in the array
            for (int j = 0; j < 6; j++)
            {
                if (!int.TryParse(values[j], out parsedArrays[lineNumber][j]))
                {
                    Debug.LogWarning("Error parsing line at line number " + lineNumber + ": " + lines[lineNumber]);
                    continue;
                }
            }
        }

        // Return the parsed arrays
        return parsedArrays;
    }

    void GetSongList(int PreSongCount){
        Debug.Log("Getting Songs");
    

        if (Directory.Exists(songsFolderPath))
        {
            folderNames = Directory.GetDirectories(songsFolderPath);
            Debug.Log("Folders inside 'Songs':");
            foreach (string folderName in folderNames)
            {
                string fullPath = Path.Combine(songsFolderPath, folderName);
                Debug.Log(Path.GetFileName(fullPath));
                // Add the full path to the SongList array
                AddToSongList(fullPath);
            }
        }
        else
        {
            Debug.LogError("The 'Songs' folder does not exist.");
        }
    }
    
        bool AddToSongList(string song)
    {
        // Create a new array with a size one greater than the current SongList array
        string[] newSongList = new string[SongList.Length + 1];
        
        // Copy existing items to the new array
        for (int i = 0; i < SongList.Length; i++)
        {
            newSongList[i] = SongList[i];
        }
        
        // Add the new folder path to the end of the new array
        newSongList[newSongList.Length] = song;
        
        // Replace the old SongList array with the new one
        SongList = newSongList;
        return true; // made it into a returnable function to make sure it actually goes through the entire thing
    }
    void LoadSongMenu(){
        /*for (int i; i < folderNames.Length;i++){
            //print the folder name and go to the next line in a loop
        }*/
    }
    void Start(){//pre Song Compiler
    Application.targetFrameRate = 120;
    audioData = GetComponent<AudioSource>();
    Debug.Log("Starting!");
        int SongCount = 5;
        //SFX Audio
        SFXList = new string[]{
            Application.dataPath + "/BuiltIn/SFX/LevelFail",
            Application.dataPath + "/BuiltIn/SFX/LevelComplete",
            Application.dataPath + "/BuiltIn/SFX/MenuMusic",
            Application.dataPath + "/BuiltIn/SFX/LevelEditor",
            Application.dataPath + "/BuiltIn/SFX/BombHit"
        };


        SongList = new string[]{
            "C:/Users/benj0/Downloads/TestPath/Beat Blaster",
            Application.dataPath + "/BuiltIn/Songs/Factory",
            Application.dataPath + "/BuiltIn/Songs/LavenderTown",
            Application.dataPath + "/BuiltIn/Songs/BreakThrough",
            Application.dataPath + "/BuiltIn/Songs/Quest"
        };
        Debug.Log("Songs");
        int PreSongCount = SongCount;//as the get songlist hasnt been called yet, the length is how many songs are added above
        //UserSongs
        Debug.Log("Calling GetSongList");
        GetSongList(PreSongCount);
        Debug.Log("loading song menu");
        LoadSongMenu();

    }
   
    void playSFX(int SFXID){
            string filepath = SFXList[SFXID];
            string songPath = filepath + "/SFX.mp3";
            SoundPlayer player = new SoundPlayer();
            player.SoundLocation = songPath;
            player.Play();

    }
    void PlaySong(int SongID){
            string filepath = SongList[SongID];
            string fs = filepath + "/Song.wav";
            //FileStream fs = new FileStream(filepath + "/Song.mp3",FileMode.Open, FileAccess.Read);
            player = new System.Media.SoundPlayer(fs);
            player.Play();
    }

    void SpawnCalculations(int CurrentNote){
        Debug.Log("Calculating");
        //Instantiate(prefab, new Vector3(i * 2.0f, 0, 0), Quaternion.identity);
        NoteType = parsedArrays[CurrentNote][4];//prefab
        NoteOffset = parsedArrays[CurrentNote][0];
        X = parsedArrays[CurrentNote][1];//(x,y,0)
        Y = parsedArrays[CurrentNote][2];//(x,y,0)
        DegreeOffset = parsedArrays[CurrentNote][3];
        calculatedmillisecondoffset = millisecondValue + ((1000/FrameRate)*2) + NoteOffset;

    }
    /*
    gonna use a quadrant cordinate plane to calculate X and Y
    if the blocks are 1 meter, then i just need to offset by that amount which unity already does so i dont (hopefully) need to do anything!
    1 unity unit = 1 meter
    */
    
    void SpawnNote(int NoteType){
        int RotateAmt = 0;
        Debug.Log("Spawning Note!");
        if (NoteType == 1){
        NoteToSpawn = NormalNote;
        }else if(NoteType == 2){
        NoteToSpawn = SlashNote;
        RotateAmt = 180;
        }else if (NoteType == 3){
        NoteToSpawn = BlasterNote;
        }else if(NoteType == 4){
        NoteToSpawn = Bomb;
        }
        //if a note is travling at 1meter/sec, then divide the offset value 
        calculatedDistanceOffset = (calculatedmillisecondoffset / 1000) + 1.5f;
            Instantiate(NoteToSpawn, new Vector3(X,Y,calculatedDistanceOffset), Quaternion.Euler(-90, DegreeOffset, RotateAmt));
            Debug.Log("Note Spawned! Note Type:" + NoteType);
        
        
    }
    IEnumerator GamePlayMech(int SongID){
            LoadNotesFromFile(SongList[SongID] + "/Notes.chart");//gets the note data for later use
            
            TimeAtStartOfLevel = currentTime;
            Health = 50;
            MissMultiplier = 1;
            
            for (int i = 0; i < lines.Length; i++){//gets all the nessisary items needed for a note spawn
                SpawnCalculations(i);
                Debug.Log("Done calculating Note!");
                SpawnNote(NoteType);

            }//start note thing
            
        while (playingSong){

            //the longest if statements ever jesus 
            if ((Physics.Raycast(LeftBlaster.transform.position, transform.TransformDirection(Vector3.forward), out LeftBlasterhit, 10))||(Physics.Raycast(RightBlaster.transform.position, transform.TransformDirection(Vector3.forward), out RightBlasterhit, 10))){//blaster detection lmao
                if ((LeftBlasterhit.collider.CompareTag(BlasterTag)||LeftBlasterhit.collider.CompareTag(NormalTag))||(RightBlasterhit.collider.CompareTag(BlasterTag)||RightBlasterhit.collider.CompareTag(NormalTag))){//makes sure the blasters got the correct notes
                    DestroyNote();//calls the function to destroy the note that got hit
                    NoteHit();
                }else if (LeftBlasterhit.collider.CompareTag(BombTag)||RightBlasterhit.collider.CompareTag(BombTag)){
                    NoteMiss();
                }
            }
        } 
        yield return new WaitForSeconds(0.01f);
    }
    void OnTriggerEnter(Collider NoteDetector)//saber note detection
    {
        if ((NoteDetector.CompareTag(SlashTag))||(NoteDetector.CompareTag(NormalTag)))
        {
            Destroy(NoteDetector.gameObject);
        }
    }
    void NoteHit(){
        if (Health != 100){
            Health += 5;
        }else if (Health > 100){
            Health = 100;
        }
        Combo += 1;
        Multiplier = Combo/10;
        Score += (100 * (int)Multiplier);
    }
    void NoteMiss(){
        Health -= 10 * (int)MissMultiplier;
        MissMultiplier += (float)0.5;
        Combo = 0;
        
    }
    void DestroyNote(){
        Destroy(LeftBlasterhit.collider.gameObject);
        // it doesnt matter if it tries to destory the note from the other blaster, you cant destroy nothing so itll hopefully skip the other destroy function. otherwise ill have to change that in the future
        Destroy(RightBlasterhit.collider.gameObject);
    }
    void Update(){
        currentTime = (int)Time.fixedTime;
        FrameRate =  (int)(1.0f / Time.deltaTime);
        millisecondValue = (int)Math.Round(Time.time * 1000,0);
        const int PlaceHolderSongID = 0;
        if (StartSong){
            //PlaySong(PlaceHolderSongID);
            StartSong = false;
            StartCoroutine(GamePlayMech(PlaceHolderSongID));
            playingSong = true;
            
        }

        

    }
}
    // Function to load notes from the file and return parsed arrays
