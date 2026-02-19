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
    public TMP_Text textoDificultad;
    public TMP_Text textoRespuesta;
    //public TextAsset archivoPreguntas;

    [Header("Paneles de Resultado")]
    public GameObject panelCorrecto;
    public GameObject panelIncorrecto;
    public TMP_Text textoVersiculo;

    private List<MultipleQuestion> listaPreguntasF = new List<MultipleQuestion>();
    private List<MultipleQuestion> listaPreguntasD = new List<MultipleQuestion>();
    private List<FVQuestions> PreguntasFVF = new List<FVQuestions>();
    private List<FVQuestions> PreguntasFVD = new List<FVQuestions>();
    private List<QuestionAbierta> listasAbiertasF = new List<QuestionAbierta>();
    private List<QuestionAbierta> listasAbiertasD = new List<QuestionAbierta>();

    private bool PreguntaFacil;
    private string tipoPregunta; // "Multiple", "VF", "Abierta"
    private int indiceActual = 0;

    void Start()
    {
        ReadCreateListQuestion();
        ReadCreateListFVQuestion();
        ReadCreateListAbiertaQuestion();
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

                        string dificultad = datos[7].Trim().ToLower();

                        if (dificultad == "facil")
                        {
                            listaPreguntasF.Add(nuevaP);
                        }
                        else if (dificultad == "dificil")
                        {
                            listaPreguntasD.Add(nuevaP);
                        }
                    }
                }
            }
        }
    }


    public  void ReadCreateListFVQuestion()
    {
        string carpeta = Application.streamingAssetsPath;
        string rutaArchivo = Path.Combine(carpeta, "preguntasFVV3.txt");
        if (File.Exists(rutaArchivo))
        {
            string[] lineas = File.ReadAllLines(rutaArchivo);
            foreach (string linea in lineas)
            {
                if (!string.IsNullOrEmpty(linea))
                {
                    string[] datos = linea.Split('-');
                  
                    if (datos.Length != 4)
                    {
                        Debug.LogWarning("La línea '" + datos[0] + "' tiene un error de guiones. Tiene " + datos.Length + " elementos.");
                    }
                    else
                    {
                       bool respuestaBool = datos[1].Trim().ToLower() == "true";
                        FVQuestions nuevaP = new FVQuestions(
                            datos[0], respuestaBool, datos[2], datos[3]
                        );

                        string dificultad = datos[3].Trim().ToLower();

                        if (dificultad == "facil")
                        {
                            PreguntasFVF.Add(nuevaP);
                        }
                        else if (dificultad == "dificil")
                        {
                            PreguntasFVD.Add(nuevaP);
                        }
                    }
                }
            }
        }
    }

    public void ReadCreateListAbiertaQuestion()
    {
        string carpeta = Application.streamingAssetsPath;
        string rutaArchivo = Path.Combine(carpeta, "preguntasAbiertasV3.txt");
        if (File.Exists(rutaArchivo))
        {
            string[] lineas = File.ReadAllLines(rutaArchivo);
            foreach (string linea in lineas)
            {
                if (!string.IsNullOrEmpty(linea))
                {
                    string[] datos = linea.Split('-');
                    if (datos.Length != 4)
                    {
                        Debug.LogWarning("La línea '" + datos[0] + "' tiene un error de guiones. Tiene " + datos.Length + " elementos.");
                    }
                    else
                    {
                        QuestionAbierta nuevaP = new QuestionAbierta(
                            datos[0], datos[1], datos[2], datos[3]
                        );

                        string dificultad = datos[3].Trim().ToLower();

                        if (dificultad == "facil")
                        {
                            listasAbiertasF.Add(nuevaP);
                        }
                        else if (dificultad == "dificil")
                        {
                            listasAbiertasD.Add(nuevaP);
                        }
                    }
                }
            }
        }
    }

    public void MostrarPreguntaMultiple(int indice, bool Facil)
    {
        tipoPregunta = "Multiple";
        PreguntaFacil = Facil;
        indiceActual = indice;
        // 2. Activamos el panel cuando se selecciona una pregunta
        if (panelPreguntaCompleto != null) panelPreguntaCompleto.SetActive(true);

        MultipleQuestion preguntaM = Facil
            ? listaPreguntasF[indice] 
            : listaPreguntasD[indice];

        textoPreguntas.text = preguntaM.Question;
        textoOpciones[0].text = preguntaM.Option1;
        textoOpciones[1].text = preguntaM.Option2;
        textoOpciones[2].text = preguntaM.Option3;
        textoOpciones[3].text = preguntaM.Option4;
        textoDificultad.text = "Dificultad: " + preguntaM.Dificulty;

        CerrarPaneles();
    }

    public void MostrarPreguntaVF(int indice, bool Facil)
    {
        tipoPregunta = "VF";
        PreguntaFacil = Facil;
        indiceActual = indice;
        // 2. Activamos el panel cuando se selecciona una pregunta
        if (panelPreguntaCompleto != null) panelPreguntaCompleto.SetActive(true);

        FVQuestions preguntaM = Facil
            ? PreguntasFVF[indice]
            : PreguntasFVD[indice];

        textoPreguntas.text = preguntaM.Pregunta;
        textoVF[0].text = "Verdadero";
        textoVF[1].text = "Falso";

        textoDificultad.text = "Dificultad: " + preguntaM.Dificultad;

        CerrarPaneles();
    }

    public void MostrarPreguntaAbierta(int indice, bool Facil)
    {
        tipoPregunta = "Abierta";
        PreguntaFacil = Facil;
        indiceActual = indice;
        // 2. Activamos el panel cuando se selecciona una pregunta
        if (panelPreguntaCompleto != null) panelPreguntaCompleto.SetActive(true);

        QuestionAbierta preguntaM = Facil
            ? listasAbiertasF[indice]
            : listasAbiertasD[indice];

        textoPreguntas.text = preguntaM.Pregunta;
        textoRespuesta.text = preguntaM.RespuestaCorrecta;
        textoDificultad.text = "Dificultad: " + preguntaM.Dificultad;

        CerrarPaneles();
    }

    public void SiguientePreguntaAzar(bool Facil)
    {
        int tipo = Random.Range(0, 3);
        switch (tipo)
        {
            case 0:
                if (Facil && listaPreguntasF.Count > 0)
                {
                    int azar = Random.Range(0, listaPreguntasF.Count);
                    MostrarPreguntaMultiple(azar,true);
                }
               else if (!Facil && listaPreguntasD.Count > 0)
                {
                    int azar = Random.Range(0, listaPreguntasD.Count);
                    MostrarPreguntaMultiple(azar,false);
                }
                break;

            case 1:
                if (Facil && PreguntasFVF.Count > 0)
                {
                    int azar = Random.Range(0, PreguntasFVF.Count);
                    MostrarPreguntaVF(azar, true);
                }
                else if (!Facil && PreguntasFVD.Count > 0)
                {
                    int azar = Random.Range(0, PreguntasFVD.Count);
                   MostrarPreguntaVF(azar, false);
                }
                break;
            case 2:
                if (Facil && listasAbiertasF.Count > 0)
                {
                    int azar = Random.Range(0, listasAbiertasF.Count);
                    MostrarPreguntaAbierta(azar, true);
                }
                else if (!Facil && listasAbiertasD.Count > 0)
                {
                    int azar = Random.Range(0, listasAbiertasD.Count);
                    MostrarPreguntaAbierta(azar, false);
                }
                break;
        }
    }

    public void ComprobarRespuesta(int botonIndice)
    {
        if (tipoPregunta == "Multiple")
        {
            MultipleQuestion preguntaM = PreguntaFacil
                ? listaPreguntasF[indiceActual]
                : listaPreguntasD[indiceActual];

            // Obtenemos los textos
            string resJugador = textoOpciones[botonIndice].text;
            string resCorrecta = preguntaM.Answer;

            // IMPRIMIMOS CON FLECHAS PARA VER ESPACIOS
            Debug.Log("JUGADOR: >" + resJugador + "<");
            Debug.Log("CORRECTA: >" + resCorrecta + "<");

            // Limpieza total para la comparación
            if (resJugador.Trim().ToLower() == resCorrecta.Trim().ToLower())
            {
                panelCorrecto.SetActive(true);
                textoVersiculo.text = "¡Correcto! " + preguntaM.Versiculo;
            }
            else
            {
                panelIncorrecto.SetActive(true);
            }

        }
        else if (tipoPregunta == "VF")
        {
            FVQuestions preguntaM = PreguntaFacil
                ? PreguntasFVF[indiceActual]
                : PreguntasFVD[indiceActual];

            bool respuestaJugador = botonIndice == 0; // Verdadero es el índice 0
            
            if (respuestaJugador == preguntaM.Respuesta)
            {
                panelCorrecto.SetActive(true);
                textoVersiculo.text = "¡Correcto! " + preguntaM.Versiculo;
            }
            else
            {
                panelIncorrecto.SetActive(true);
            }

        }
    }

    // 3. Nueva función para cerrar los avisos
    public void CerrarPaneles()
    {
        panelCorrecto.SetActive(false);
        panelIncorrecto.SetActive(false);
    }
}