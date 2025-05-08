using UnityEngine;
using TMPro;
using System.Collections;

public class TypeWriter : MonoBehaviour
{
    public TextMeshProUGUI textoUI;
    [TextArea]
    public string textoCompleto;
    public float velocidadLetra = 0.05f;

    private Coroutine escritura;

    void Start()
    {
        EmpezarEscritura();
    }

    public void EmpezarEscritura()
    {
        if (escritura != null)
            StopCoroutine(escritura);

        escritura = StartCoroutine(EscribirTexto());
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
