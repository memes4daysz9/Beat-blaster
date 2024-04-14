using System;
using UnityEngine;
using System.IO;
using System.Media;
public class GameManagement : MonoBehaviour
{
        bool StartSong;
        int currentSong;
        string[] SongList; //List Of Songs And Their Respected "ID"
        string[] SFXList; //List Of Sound Effect and their respected "ID"

        int[][] parsedArrays;
        string[] lines;
        string[] folderNames;//would also be the song names per each song
    int NoteType;
    int calculatedmillisecondoffset;
    int millisecondValue;
    int currentTime;
    int TimeAtStartOfLevel;
    int FrameRate;
    int NoteOffset = 2000;//Note Offset for the note coming at the user

    
    float MissMultiplier;
    int Health = 100;
    int Combo = 0;
    int Score = 0;
    float Multiplier = 1;

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

    int X;
    int Y;
    int DegreeOffset;


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
            string[] values = lines[lineNumber].Split(',');

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
    string songsFolderPath = Application.dataPath + "/Songs";

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
        const int PreSongCount = 3;
        int SongCount;
        const string BIS = "BuiltIn/Songs"; //constant variables W
        const string BISFX = "BuiltIn/SFX";
        //SFX Audio
        SFXList[0] = BISFX + "LevelFail";
        SFXList[1] = BISFX + "LevelComplete";
        SFXList[2] = BISFX + "MenuMusic";
        SFXList[3] = BISFX + "LevelEditor";
        SFXList[4] = BISFX + "BombHit";
        
        //Song Audio
        SongList[0] = BIS + "Beat Blaster";
        SongList[1] = BIS + "Factory";
        SongList[2] = BIS + "LavenderTown";
        SongList[3] = BIS + "BreakThrough";

        //UserSongs
        GetSongList(PreSongCount);
        LoadSongMenu();

    }

    void playSFX(int SFXID){
            string filepath = SFXList[SFXID];
            string songPath = "/SFX.mp3";
            SoundPlayer player = new SoundPlayer();
            player.SoundLocation = songPath;
            player.Play();

    }
    void PlaySong(int SongID){
            string filepath = SongList[SongID];
            string songPath = "/Song.mp3";
            SoundPlayer player = new SoundPlayer();
            player.SoundLocation = songPath;
            player.Play();
    }

    void SpawnCalculations(int CurrentNote){
        //Instantiate(prefab, new Vector3(i * 2.0f, 0, 0), Quaternion.identity);
        NoteType = parsedArrays[CurrentNote][4];//prefab
        NoteOffset = parsedArrays[CurrentNote][0];
        X = parsedArrays[CurrentNote][1];//(x,y,0)
        Y = parsedArrays[CurrentNote][2];//(x,y,0)
        DegreeOffset = parsedArrays[CurrentNote][3];
        calculatedmillisecondoffset = millisecondValue + ((1000/FrameRate)*2) + NoteOffset;

    }
    bool SpawnNote(){
        bool NoteSpawned = false;
        while ((calculatedmillisecondoffset <= millisecondValue) && NoteSpawned != true){
            Instantiate(NormalNote, new Vector3(X,Y,0), Quaternion.Euler(0, DegreeOffset, 0));
            NoteSpawned = true;
        }
        return true;
    }
    void GamePlayMech(int SongID){
            LoadNotesFromFile(SongList[SongID] + "Notes.chart");//gets the note data for later use
            TimeAtStartOfLevel = currentTime;
            Health = 50;
            MissMultiplier = 1;
            for (int i = 0; i < lines.Length; i++){//gets all the nessisary items needed for a note spawn
                SpawnCalculations(i);

            }//start note thing
        while (StartSong){

            //the longest if statements ever jesus 
            if ((Physics.Raycast(LeftBlaster.transform.position, transform.TransformDirection(Vector3.forward), out LeftBlasterhit, 10))||(Physics.Raycast(RightBlaster.transform.position, transform.TransformDirection(Vector3.forward), out RightBlasterhit, 10))){//blaster detection lmao
                if ((LeftBlasterhit.collider.CompareTag(BlasterTag)||LeftBlasterhit.collider.CompareTag(NormalTag))||(RightBlasterhit.collider.CompareTag(BlasterTag)||RightBlasterhit.collider.CompareTag(NormalTag))){//makes sure the blasters got the correct notes
                    DestroyNote();//calls the function to destroy the note that got hit
                    NoteHit();
                }else if (LeftBlasterhit.collider.CompareTag(BombTag)||RightBlasterhit.collider.CompareTag(BombTag)){

                }
            }
        } 
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
        Multiplier = combo/10;
        Score += (100 * Multiplier);
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
        FrameRate = Time.captureFramerate;
        millisecondValue = currentTime;
        const int PlaceHolderSongID = 1;
        if (StartSong){
            GamePlayMech(PlaceHolderSongID);
        }

        

    }
}
    // Function to load notes from the file and return parsed arrays
