using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum ModoDeDisparo{
    SemiAuto, FullAuto
}

public class Logica_Arma : MonoBehaviour
{

    [Header("Atributos de Arma")]
    
    protected Animator animator;
    protected AudioSource audioSource;
    public bool tiempoNoDisparo = false;
    public bool puedeDisparar = false;
    public bool recargando = false;

    [Header("Referencia de Objetos")]
    public ParticleSystem fuegoDeArma;

    [Header("Referencia de Sonidos")]
    public AudioClip SonDisparo;
    public AudioClip SonSinBalas;
    public AudioClip SonCartuchoEntra;
    public AudioClip SonCartuchoSale;
    public AudioClip SonVacio;
    public AudioClip SonDesenfundar;

    [Header("Atributos de Arma")]
    public ModoDeDisparo modoDeDisparo = ModoDeDisparo.FullAuto;
    public float daño = 20f;
    public float ritmoDeDisparo = 0.3f;
    public int balasRestantes;
    public int balasEnCartucho;
    public int tamañoDeCartcho = 12;
    public Text textoBalas;
    public int maximoDeBalas = 100;

    // Use this for initialization
    void Start() {
        //audioSource = GetComponent<AudioSource>();
        animator = GetComponent<Animator>();

        balasEnCartucho = tamañoDeCartcho;
        balasRestantes = maximoDeBalas;

        Invoke("HabilitarArmar", 0.5f);
    }

    // Update is called once per frame
    void Update() {
        // if (modoDeDisparo == ModoDeDisparo.FullAuto && Input.GetButton("Fire1"))
        // {
        //     RevisarDisparo();
        // }
        // else if (modoDeDisparo == ModoDeDisparo.SemiAuto && Input.GetButtonDown("Fire1"))
        // {
        //     RevisarDisparo();
        // }

        if (Input.GetButtonDown("Reload"))
        {
            RevisarRecargar();
        }
    }

    void HabilitarArmar()
    {
        puedeDisparar = true;
    }

    public void RevisarDisparo()
    {
        if (!puedeDisparar) return;
        if (tiempoNoDisparo) return;
        if (recargando) return;
        if (balasEnCartucho > 0)
        {
            Disparar();
        }
        else
        {
            SinBalas();
        }
    }

    void Disparar()
    {
        //audioSource.PlayOneShot(SonDisparo);
        tiempoNoDisparo = true;
        // fuegoDeArma.Stop();
        // fuegoDeArma.Play();
        ReproducirAnimacionDisparo();
        balasEnCartucho--;
        textoBalas.text = balasEnCartucho.ToString();
        StartCoroutine(ReiniciarTiempoNoDisparo());
    }

    public virtual void ReproducirAnimacionDisparo()
    {
        if (gameObject.name == "Police9mm") {
            if (balasEnCartucho > 1)
            {
                animator.CrossFadeInFixedTime("Fire", 0.1f);
            }
            else
            {
                animator.CrossFadeInFixedTime("FireLast", 0.1f);
            }
        }
        else
        {
            animator.CrossFadeInFixedTime("Fire", 0.1f);
        }

    }

    void SinBalas()
    {
        //audioSource.PlayOneShot(SonSinBalas);
        tiempoNoDisparo = true;
        StartCoroutine(ReiniciarTiempoNoDisparo());
    }

    IEnumerator ReiniciarTiempoNoDisparo()
    {
        yield return new WaitForSeconds(ritmoDeDisparo);
        tiempoNoDisparo = false;
    }

    public void RevisarRecargar()
    {
        if (balasRestantes > 0 && balasEnCartucho < tamañoDeCartcho)
        {
            Recargar();
        }
    }

    void Recargar()
    {
        if (recargando) return;
        recargando = true;
        animator.CrossFadeInFixedTime("Reload", 0.1f);
        textoBalas.text = "12";
    }

    void RecargarMuniciones()
    {
        int balasParaRecargar = tamañoDeCartcho - balasEnCartucho;
        int restarBalas = (balasRestantes >= balasParaRecargar) ? balasParaRecargar : balasRestantes;

        balasRestantes -= restarBalas;
        balasEnCartucho += balasParaRecargar;
    }

    public void DesenfundarOn()
    {
        //audioSource.PlayOneShot(SonDesenfundar);
    }

    public void CartuchoEntraOn()
    {
        //audioSource.PlayOneShot(SonCartuchoEntra);
        RecargarMuniciones();
    }

    public void CartuchoSaleOn()
    {
        //audioSource.PlayOneShot(SonCartuchoSale);
        RecargarMuniciones();
    }

    public void VacioOn()
    {
        //audioSource.PlayOneShot(SonVacio);
        Invoke("ReiniciarRecargar", 0.1f);
    }
    void ReiniciarRecargar()
    {
        recargando = false;
    }

} 

