using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class MusicMixed : MonoBehaviour
{ //public 
    public AudioMixer Sonido;
    //public GameObject MusicOn, MusicOff, SFXOn, SFXOff;
    public AudioSource Roca_choque,Presionar_Boton,premio_sonido;
    public static MusicMixed index;
    public int mA, mB;
    public List<AudioSource> PlayList;
    public enum ListaMelodias { Hero_Raise=0, Ocean_Deep=1 }
    public ListaMelodias MelodiaActual;
    public float volumen_melodia=0.5f;
    public Text Titulos_melodias;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        index = this;
       // Titulos_melodias.text= PlayList_Select( PlayerPrefs.GetInt("Melodia",0));
        if (PlayerPrefs.GetInt("Musica", 1) > 0)
        {
            OnOffMusica(true);
        }
        else
        {
            OnOffMusica(false);
        }
        if (PlayerPrefs.GetInt("SFX", 1) > 0)
        {
            OnOffSFX(true);
        }
        else
        {
            OnOffSFX(false);
        }
        DontDestroyOnLoad(gameObject);

    }
    public void Premio()
    {
        premio_sonido.Play();
    }
    public void CambiarMAB ()
    {
        int a = mA;
        PlayList[mA].time = 0;
        PlayList[mA].volume = 0f;
        mA = mB;
        mB = a;
    }
    public string PlayList_Select(int Tocar)
    {

        PausarMusica();
        switch (Tocar)
        {
            case ((int)ListaMelodias.Hero_Raise):
                NuevaMElodia(0,1);
                PlayerPrefs.SetInt("Melodia", 0);

                return "Hero Rise";
                break;
            case ((int)ListaMelodias.Ocean_Deep):
                PlayerPrefs.SetInt("Melodia", 1);
                NuevaMElodia(2, 3);
                return "Ocean Deep";
                break;
             default:
                
                PlayerPrefs.SetInt("Melodia", 0);
                NuevaMElodia(0, 1);
                return "Hero Rise";
                break;
        }

    }
    public void PausarMusica()
    {
        if (PlayList[mA].isPlaying)
        {
            PlayList[mA].time = 0f;
            PlayList[mA].volume = 0f;
        }
        if (PlayList[mB].isPlaying)
        {
            PlayList[mB].time = 0f;
            PlayList[mB].volume = 0f;
        }
    }

    public void NuevaMElodia(int a, int b)
    {
        mA = a;
        mB = b;
        if (!PlayList[mA].isPlaying)
            PlayList[mA].Play();
        if (!PlayList[mB].isPlaying)
            PlayList[mB].Play();
        PlayList[mA].time = 0;
        PlayList[mA].volume = volumen_melodia;
        PlayList[mB].time = 0;
        PlayList[mB].volume = 0f;
        
    }
    public void TocarSeleccion()
    {
        
    }
    public void PresionarBoton()
    {
        Presionar_Boton.Play();
    }
    public void OnOffSFX(bool a)
    {
        if (a)
        {
            Sonido.SetFloat("SFX", 1);
            PlayerPrefs.SetInt("SFX", 1);
        }
        else
        {
            Sonido.SetFloat("SFX", -80);
            PlayerPrefs.SetInt("SFX", -80);
        }
     //   SFXOn.SetActive(a);
      //  SFXOff.SetActive(!a);
    }
    public void OnOffMusica(bool a)
    {
        if (a)
        {
            Sonido.SetFloat("Music", 1);
            PlayerPrefs.SetInt("Musica", 1);
        }
        else
        { Sonido.SetFloat("Music", -80);
            PlayerPrefs.SetInt("Musica", -80);
        }

      //  MusicOn.SetActive(a);
       // MusicOff.SetActive(!a);
    }
    public void RocaChocando(string tag)
    {
      //  Debug.Log(tag);
        Roca_choque.Play();
         }
    // Update is called once per frame
    void Update()
    {
     
   
        if (!PlayList[mA].isPlaying)
        {
            PlayList[mA].Play();
            PlayList[mA].time = 0;
            PlayList[mA].volume = volumen_melodia;
        }
        if (PlayList[mA].time > PlayList[mA].clip.length * 0.9f)
        {
            if (PlayList[mB].volume == 0.0f)
            {
                PlayList[mB].time = 0.0f;
            }
            if (PlayList[mA].volume > 0.01f)
                PlayList[mA].volume = PlayList[mA].volume - (0.1f * Time.deltaTime);
            else
                PlayList[mA].volume = 0.0f;
            if (PlayList[mB].volume < volumen_melodia)
                PlayList[mB].volume = PlayList[mB].volume + (0.1f * Time.deltaTime);
            else
                PlayList[mB].volume = volumen_melodia;

            if (PlayList[mB].volume == volumen_melodia && PlayList[mA].volume == 0.0f)
            {

                CambiarMAB();
                // PlayList[mB];
            }
        }
        else
        {
            if (PlayList[mB].volume != 0 && PlayList[mA].volume != 0)
            {
                PlayList[mB].volume = 0;
            }
            if (PlayList[mA].volume <= volumen_melodia)
                PlayList[mA].volume = volumen_melodia;
        }


    }
}
