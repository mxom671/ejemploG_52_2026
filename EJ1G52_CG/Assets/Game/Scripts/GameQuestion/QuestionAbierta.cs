using System;

[Serializable]
public class QuestionAbierta
{
    public string pregunta;
    public string respuestaCorrecta;
    public string versiculo;
    public string dificultad;

    public QuestionAbierta(string pregunta, string respuestaCorrecta, string versiculo, string dificultad)
    {
        this.pregunta = pregunta;
        this.respuestaCorrecta = respuestaCorrecta;
        this.versiculo = versiculo;
        this.dificultad = dificultad;
    }
}