using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Gestor_Intro_menu : MonoBehaviour
{
    public Animator Transicion;
    // public string url = "https://www.youtube.com/watch?v=cyR6-7WG5QU&t=0s" https://zmdev.itch.io/ https://cjgg.itch.io/;


    public void QuitGame()
    {
        Application.Quit();
    }
    public void IrAYoutube(string url)
    {
   
            Application.OpenURL(url);
        
    }
    public void PresionarBoton()
    {
        MusicMixed.index.PresionarBoton();

    }

    public void ElegirPlayers(int a) 
    {
        PlayerPrefs.SetInt("NPlayer", a);
        Transiciones.Index.TFinal("");
    }
  
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
