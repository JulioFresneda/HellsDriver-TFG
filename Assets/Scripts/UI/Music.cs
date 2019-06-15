using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Music : MonoBehaviour
{
    public List<AudioClip> songs;
    private AudioSource source;
    System.Random rnd;
    private string songName;

    public Text songNameText;

    private static bool started = false;


    // Start is called before the first frame update
    void Start()
    {
        songName = "";
        if (!started)
        {
            started = true;
            DontDestroyOnLoad(gameObject);
            rnd = new System.Random();
            source = GetComponent<AudioSource>();
            source.volume = 0.4f;
            PlaySong();
        }
        else
        {
            foreach(Music m in GameObject.FindObjectsOfType<Music>())
            {
                m.songNameText = GameObject.Find("SongTitle").GetComponent<Text>();
                m.SetSongNameOnScreen();
            }
            Destroy(this);
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!source.isPlaying) PlaySong();


    }

    public void SetSongNameOnScreen()
    {
        if (songNameText != null && songName != "") songNameText.text = "Escuchando:    " + songName;
    }

    private void PlaySong()
    {
        int r = rnd.Next(songs.Count);
        source.PlayOneShot(songs[r]);
        songName = songs[r].name;
        if (songNameText != null) songNameText.text = "Escuchando:    " + songName;
    }

    public string GetSongName() => songName;
}
