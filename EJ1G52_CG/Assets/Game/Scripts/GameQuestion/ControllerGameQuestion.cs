using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;

public class ControllerGameQuestion : MonoBehaviour
{
    [Header("Configuración de UI")]
    public GameObject panelPreguntaCompleto;
    public TMP_Text textoPreguntas;
    public TMP_Text[] textoOpciones;
    public TMP_Text[] textoVF;
    public TMP_Text textoDificultad;
    public TMP_Text textoRespuesta;
    public TMP_InputField inputRespuestaAbierta;

    [Header("Navegación de Paneles")]
    public GameObject panelBienvenida; // El que tiene el botón Start
    public GameObject panelAnuncioNivel; // Un panel que diga "Nivel 1 - Fácil"
    public TMP_Text textoAnuncioNivel; // El texto dentro de ese panel

    [Header("Progreso del Juego")]
    public int puntosActuales = 0;
    public int puntosParaNivelDificil = 20;

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
    private string tipoPregunta;
    private int indiceActual = 0;

    void Start()
    {
        ReadCreateListQuestion();
        ReadCreateListFVQuestion();
        ReadCreateListAbiertaQuestion();

        // Inicialización de UI
        if (panelPreguntaCompleto != null) panelPreguntaCompleto.SetActive(false);
        panelCorrecto.SetActive(false);
        panelIncorrecto.SetActive(false);
    }

    // Función principal para los botones de "Siguiente" o "Jugar"
    public void SeleccionarPreguntaPorNivel()
    {
        // 1. Escondemos el menú de inicio
        if (panelBienvenida != null) panelBienvenida.SetActive(false);

        CerrarPaneles();
        if (inputRespuestaAbierta != null) inputRespuestaAbierta.text = "";

        // 2. Decidimos el nivel según los puntos
        bool esFacil = puntosActuales < puntosParaNivelDificil;

        // 3. Mostramos el anuncio de nivel antes de la pregunta (Opcional pero recomendado)
        if (panelAnuncioNivel != null)
        {
            panelAnuncioNivel.SetActive(true);
            textoAnuncioNivel.text = esFacil ? "Nivel 1: Fácil" : "Nivel 2: Difícil";

            // Usamos Invoke para que el anuncio se quite solo tras 2 segundos y salga la pregunta
            Invoke(esFacil ? "LanzarFacil" : "LanzarDificil", 2f);
        }
        else
        {
            // Si no tienes panel de anuncio, lanza la pregunta de una vez
            SiguientePreguntaAzar(esFacil);
        }
    }

    // Funciones auxiliares para el Invoke
    void LanzarFacil() { panelAnuncioNivel.SetActive(false); SiguientePreguntaAzar(true); }
    void LanzarDificil() { panelAnuncioNivel.SetActive(false); SiguientePreguntaAzar(false); }

    #region Lectura de Archivos
    public void ReadCreateListQuestion()
    {
        string rutaArchivo = Path.Combine(Application.streamingAssetsPath, "ArchivoPreguntasMV3.txt");
        if (File.Exists(rutaArchivo))
        {
            string[] lineas = File.ReadAllLines(rutaArchivo);
            foreach (string linea in lineas)
            {
                if (string.IsNullOrEmpty(linea)) continue;
                string[] datos = linea.Split('-');
                if (datos.Length == 8)
                {
                    MultipleQuestion nuevaP = new MultipleQuestion(datos[0], datos[1], datos[2], datos[3], datos[4], datos[5], datos[6], datos[7]);
                    if (datos[7].Trim().ToLower() == "facil") listaPreguntasF.Add(nuevaP);
                    else listaPreguntasD.Add(nuevaP);
                }
            }
        }
    }

    public void ReadCreateListFVQuestion()
    {
        string rutaArchivo = Path.Combine(Application.streamingAssetsPath, "preguntasFVV3.txt");
        if (File.Exists(rutaArchivo))
        {
            string[] lineas = File.ReadAllLines(rutaArchivo);
            foreach (string linea in lineas)
            {
                if (string.IsNullOrEmpty(linea)) continue;
                string[] datos = linea.Split('-');
                if (datos.Length == 4)
                {
                    bool respuestaBool = datos[1].Trim().ToLower() == "true";
                    FVQuestions nuevaP = new FVQuestions(datos[0], respuestaBool, datos[2], datos[3]);
                    if (datos[3].Trim().ToLower() == "facil") PreguntasFVF.Add(nuevaP);
                    else PreguntasFVD.Add(nuevaP);
                }
            }
        }
    }

    public void ReadCreateListAbiertaQuestion()
    {
        string rutaArchivo = Path.Combine(Application.streamingAssetsPath, "ArchivoPreguntasAbiertasV3.txt");
        if (File.Exists(rutaArchivo))
        {
            string[] lineas = File.ReadAllLines(rutaArchivo);
            foreach (string linea in lineas)
            {
                if (string.IsNullOrEmpty(linea)) continue;
                string[] datos = linea.Split('-');
                if (datos.Length == 4)
                {
                    QuestionAbierta nuevaP = new QuestionAbierta(datos[0], datos[1], datos[2], datos[3]);
                    if (datos[3].Trim().ToLower() == "facil") listasAbiertasF.Add(nuevaP);
                    else listasAbiertasD.Add(nuevaP);
                }
            }
        }
    }
    #endregion

    #region Visualización de Preguntas
    public void MostrarPreguntaMultiple(int indice, bool Facil)
    {
        tipoPregunta = "Multiple";
        PreguntaFacil = Facil;
        indiceActual = indice;
        panelPreguntaCompleto.SetActive(true);

        MultipleQuestion p = Facil ? listaPreguntasF[indice] : listaPreguntasD[indice];
        textoPreguntas.text = p.Question;
        textoOpciones[0].text = p.Option1;
        textoOpciones[1].text = p.Option2;
        textoOpciones[2].text = p.Option3;
        textoOpciones[3].text = p.Option4;
        textoDificultad.text = "Dificultad: " + p.Dificulty;

        // Desactivamos el input de abiertas por si acaso
        inputRespuestaAbierta.gameObject.SetActive(false);
    }

    public void MostrarPreguntaVF(int indice, bool Facil)
    {
        tipoPregunta = "VF";
        PreguntaFacil = Facil;
        indiceActual = indice;
        panelPreguntaCompleto.SetActive(true);

        FVQuestions p = Facil ? PreguntasFVF[indice] : PreguntasFVD[indice];
        textoPreguntas.text = p.Pregunta;
        textoVF[0].text = "Verdadero";
        textoVF[1].text = "Falso";
        textoDificultad.text = "Dificultad: " + p.Dificultad;
        inputRespuestaAbierta.gameObject.SetActive(false);
    }

    public void MostrarPreguntaAbierta(int indice, bool Facil)
    {
        tipoPregunta = "Abierta";
        PreguntaFacil = Facil;
        indiceActual = indice;
        panelPreguntaCompleto.SetActive(true);

        QuestionAbierta p = Facil ? listasAbiertasF[indice] : listasAbiertasD[indice];
        textoPreguntas.text = p.Pregunta;
        textoDificultad.text = "Dificultad: " + p.Dificultad;

        if (textoRespuesta != null)
        {
            textoRespuesta.text = p.RespuestaCorrecta;
        }

        inputRespuestaAbierta.gameObject.SetActive(true);
        CerrarPaneles();
    

    // Activamos el input para que el usuario escriba
    inputRespuestaAbierta.gameObject.SetActive(true);
    }

    public void SiguientePreguntaAzar(bool Facil)
    {
        int tipo = Random.Range(0, 3);
        switch (tipo)
        {
            case 0:
                var listaM = Facil ? listaPreguntasF : listaPreguntasD;
                if (listaM.Count > 0) MostrarPreguntaMultiple(Random.Range(0, listaM.Count), Facil);
                else SiguientePreguntaAzar(Facil); // Reintentar otro tipo si esta lista está vacía
                break;
            case 1:
                var listaVF = Facil ? PreguntasFVF : PreguntasFVD;
                if (listaVF.Count > 0) MostrarPreguntaVF(Random.Range(0, listaVF.Count), Facil);
                else SiguientePreguntaAzar(Facil);
                break;
            case 2:
                var listaA = Facil ? listasAbiertasF : listasAbiertasD;
                if (listaA.Count > 0) MostrarPreguntaAbierta(Random.Range(0, listaA.Count), Facil);
                else SiguientePreguntaAzar(Facil);
                break;
        }
    }
    #endregion

    public void ComprobarRespuesta(int botonIndice)
    {
        bool esCorrecto = false;
        string versiculo = "";

        if (tipoPregunta == "Multiple")
        {
            MultipleQuestion p = PreguntaFacil ? listaPreguntasF[indiceActual] : listaPreguntasD[indiceActual];
            if (textoOpciones[botonIndice].text.Trim().ToLower() == p.Answer.Trim().ToLower())
            {
                esCorrecto = true;
                versiculo = p.Versiculo;
            }
        }
        else if (tipoPregunta == "VF")
        {
            FVQuestions p = PreguntaFacil ? PreguntasFVF[indiceActual] : PreguntasFVD[indiceActual];
            bool respuestaJugador = (botonIndice == 0);
            if (respuestaJugador == p.Respuesta)
            {
                esCorrecto = true;
                versiculo = p.Versiculo;
            }
        }
        else if (tipoPregunta == "Abierta")
        {
            QuestionAbierta p = PreguntaFacil ? listasAbiertasF[indiceActual] : listasAbiertasD[indiceActual];
            if (inputRespuestaAbierta.text.Trim().ToLower() == p.RespuestaCorrecta.Trim().ToLower())
            {
                esCorrecto = true;
                versiculo = p.Versiculo;
            }
        }

        if (esCorrecto)
        {
            puntosActuales += 5;
            panelCorrecto.SetActive(true);
            textoVersiculo.text = "¡Correcto! " + versiculo;
        }
        else
        {
            panelIncorrecto.SetActive(true);
        }
    }

    public void CerrarPaneles()
    {
        panelCorrecto.SetActive(false);
        panelIncorrecto.SetActive(false);
    }
}