using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Transiciones : MonoBehaviour
{
    public static Transiciones Index;
    public Animator Transicion;
    public CanvasGroup cnavasconfi;
    public AnimationClip Inicio, Final;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Index = this;
        Iniciar();
    }
   
    // Update is called once per frame
    void Update()
    {
     
    }
    public void Iniciar()
    {
        Transicion.Play(Inicio.name, 0, 0.0f);
        cnavasconfi.alpha = 1.0f;
        cnavasconfi.blocksRaycasts = true;
        StartCoroutine(EsperarFinAnimacion(""));
      
    }
    public void TFinal(string a)
    {
        Transicion.Play(Final.name, 0, 0.0f);
        cnavasconfi.alpha = 1.0f;
        cnavasconfi.blocksRaycasts = true;
        StartCoroutine(EsperarFinAnimacion(a));
    
    }
   
   
    IEnumerator EsperarFinAnimacion(string NuevaScena)
    {

        yield return null;

        AnimatorStateInfo stateInfo = Transicion.GetCurrentAnimatorStateInfo(0);

        while (stateInfo.normalizedTime < 1.0f && !Transicion.IsInTransition(0))
        {
            Debug.Log(stateInfo.normalizedTime);
            stateInfo = Transicion.GetCurrentAnimatorStateInfo(0);
            yield return null;
        }
        if (NuevaScena.Equals(""))
        {
            cnavasconfi.alpha = 0.0f;
            cnavasconfi.blocksRaycasts = false;
        }
        else
        SceneManager.LoadScene(NuevaScena);

    }
}
