using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;

public class ControllerGameQuestion : MonoBehaviour
{
    [Header("Configuración de UI")]
    public GameObject panelPreguntaCompleto; // El panel que contiene la pregunta y opciones
    public TMP_Text textoPreguntas;
    public TMP_Text[] textoOpciones;
    public TMP_Text[] textoVF;
    //public TextAsset archivoPreguntas;

    [Header("Paneles de Resultado")]
    public GameObject panelCorrecto;
    public GameObject panelIncorrecto;
    public TMP_Text textoVersiculo;

    private List<MultipleQuestion> listaPreguntas = new List<MultipleQuestion>();
    private int indiceActual = 0;

    void Start()
    {
        ReadCreateListQuestion();
        // 1. Escondemos todo al iniciar
        if (panelPreguntaCompleto != null) panelPreguntaCompleto.SetActive(false);
        panelCorrecto.SetActive(false);
        panelIncorrecto.SetActive(false);
    }

    public void ReadCreateListQuestion()
    {

        string carpeta = Application.streamingAssetsPath;
        string rutaArchivo = Path.Combine(carpeta, "ArchivoPreguntasMV3.txt");

        if (File.Exists(rutaArchivo))
        {
            string[] lineas = File.ReadAllLines(rutaArchivo);
            foreach (string linea in lineas)

            {
                if (!string.IsNullOrEmpty(linea))
                {
                    string[] datos = linea.Split('-');
                    // En lugar de datos.Length >= 8, usa esto para ver si hay errores en la consola
                    if (datos.Length != 8)
                    {
                        Debug.LogWarning("La línea '" + datos[0] + "' tiene un error de guiones. Tiene " + datos.Length + " elementos.");
                    }
                    {
                        MultipleQuestion nuevaP = new MultipleQuestion(
                            datos[0], datos[1], datos[2], datos[3],
                            datos[4], datos[5], datos[6], datos[7]
                        );
                        listaPreguntas.Add(nuevaP);
                    }
                }
            }
        }
    }

    public void MostrarPregunta(int indice)
    {
        indiceActual = indice;
        // 2. Activamos el panel cuando se selecciona una pregunta
        if (panelPreguntaCompleto != null) panelPreguntaCompleto.SetActive(true);

        textoPreguntas.text = listaPreguntas[indice].Question;
        textoOpciones[0].text = listaPreguntas[indice].Option1;
        textoOpciones[1].text = listaPreguntas[indice].Option2;
        textoOpciones[2].text = listaPreguntas[indice].Option3;
        textoOpciones[3].text = listaPreguntas[indice].Option4;

        CerrarPaneles();
    }

    public void SiguientePreguntaAzar()
    {
        int azar = Random.Range(0, listaPreguntas.Count);
        MostrarPregunta(azar);
    }

    public void ComprobarRespuesta(int botonIndice)
    {
        // Obtenemos los textos
        string resJugador = textoOpciones[botonIndice].text;
        string resCorrecta = listaPreguntas[indiceActual].Answer;

        // IMPRIMIMOS CON FLECHAS PARA VER ESPACIOS
        Debug.Log("JUGADOR: >" + resJugador + "<");
        Debug.Log("CORRECTA: >" + resCorrecta + "<");

        // Limpieza total para la comparación
        if (resJugador.Trim().ToLower() == resCorrecta.Trim().ToLower())
        {
            panelCorrecto.SetActive(true);
            textoVersiculo.text = "¡Correcto! " + listaPreguntas[indiceActual].Versiculo;
        }
        else
        {
            panelIncorrecto.SetActive(true);
        }
    }

    // 3. Nueva función para cerrar los avisos
    public void CerrarPaneles()
    {
        panelCorrecto.SetActive(false);
        panelIncorrecto.SetActive(false);
    }
}