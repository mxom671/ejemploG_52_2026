using UnityEngine;

[System.Serializable]

public class FVQuestions
{
    private string pregunta;
    private bool respuesta;
    private string versiculo;
    private string dificultad;

    public FVQuestions()
    {
    }

    public FVQuestions(string pregunta, bool respuesta, string versiculo, string diificultad)
    {
        this.pregunta = pregunta;
        this.respuesta = respuesta;
        this.versiculo = versiculo;
        this.dificultad = diificultad;
    }

    public string Pregunta { get => pregunta; set => pregunta = value; }
    public bool Respuesta { get => respuesta; set => respuesta = value; }
    public string Versiculo { get => versiculo; set => versiculo = value; }
    public string Dificultad { get => dificultad; set => dificultad = value; }
}
