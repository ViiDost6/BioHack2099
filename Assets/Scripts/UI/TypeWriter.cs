using UnityEngine;
using TMPro;
using System.Collections;

public class TypeWriter : MonoBehaviour
{
    public TextMeshProUGUI textoUI;
    [TextArea]
    public string textoCompleto;
    public float velocidadLetra = 0.05f;
    public CanvasGroup imagenes;
    public CanvasGroup boton1;
    public CanvasGroup boton2;
    public float tiempoBotones = 0.5f;
    public float tiempoFundido = 2f;
    public float tiempoEscritura = 0.5f;

    private Coroutine escritura;
    private Coroutine fundido;
    private Coroutine entradaBoton1;
    private Coroutine entradaBoton2;

    void Start()
    {
        //aseguramos que el texto no se vea al inicio
        //pero si en el editor
        boton1.alpha = 0f;
        boton2.alpha = 0f;
        imagenes.alpha = 0f;


        EmpezarEscritura();
        EmpezarFundido();
        Boton1();
        Boton2();
    }

    public void EmpezarEscritura()
    {
        if (escritura != null)
            StopCoroutine(escritura);

        escritura = StartCoroutine(EscribirTexto());
    }
    
    public void EmpezarFundido()
    {
        if (fundido != null)
            StopCoroutine(fundido);
        
        fundido = StartCoroutine(FundidoEntrante(tiempoFundido, imagenes));
    }

    public void Boton1()
    {
        if (boton1 != null)
            StartCoroutine(EntradaBoton(tiempoBotones, boton1));
        entradaBoton1 = StartCoroutine(EntradaBoton(tiempoBotones, boton1));
    }

    public void Boton2()
    {
        if (boton2 != null)
            StartCoroutine(EntradaBoton(1f, boton2));
        entradaBoton2 = StartCoroutine(EntradaBoton(1f, boton2));
    }

    IEnumerator EntradaBoton(float tiempo, CanvasGroup boton)
    {
        yield return new WaitForSeconds(tiempo);
        float alpha = 0f;
        while (alpha < 1f)
        {
            alpha += Time.deltaTime / tiempo;
            boton.alpha = alpha;
            yield return null;
        }
    }
    IEnumerator FundidoEntrante(float tiempo, CanvasGroup imagenes)
    {
        float alpha = 0f;
        while (alpha < 1f)
        {
            alpha += Time.deltaTime / tiempo;
            imagenes.alpha = alpha;
            yield return null;
        }
    }

    IEnumerator EscribirTexto()
    {
        textoUI.text = "";
        foreach (char letra in textoCompleto.ToCharArray())
        {
            textoUI.text += letra;
            yield return new WaitForSeconds(velocidadLetra);
        }
    }
}
