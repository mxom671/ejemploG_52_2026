using UnityEngine;
using System.Collections.Generic;
using TMPro;
using System.IO;

public class ControllerSceneG52 : MonoBehaviour
{

    List<Carro> lista_Carros=new List<Carro>();
    public TMP_InputField idCarro;
    public TMP_InputField marcaCarro;
    public TMP_InputField modeloCarro;
    public TMP_InputField placaCarro;
    public TMP_InputField CantidadPuertasCarro;

    public TextMeshProUGUI mostrarCarros;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AgregarCarro()
    {
        string id = idCarro.text;
        string marca = marcaCarro.text;
        string modelo = modeloCarro.text;
        string placa = placaCarro.text;
        int cantidadPuertas = int.Parse(CantidadPuertasCarro.text);

        Carro carro = new Carro(id, marca, modelo, placa, cantidadPuertas);
        lista_Carros.Add(carro);
        Debug.Log("Carro agregado: " + carro.IdVehiculo + ", " + carro.Marca + ", " + carro.Modelo +
            ", " + carro.Placa + ", " + carro.NumeroPuertas);
    }

    public void showListCarro()
    {
        string mostrar = "";

    foreach (Carro carro in lista_Carros)
    {
            mostrar += carro.ToString() + "\n";
        }

        mostrarCarros.text = mostrar;

    }

    public void createJSONFile()
        {

        ListaCarros objList = new ListaCarros();
        objList.carros = lista_Carros;

        string json = JsonUtility.ToJson(objList, true);

        string carpeta = Application.streamingAssetsPath;
        string rutaArchivo = Path.Combine(carpeta, "carros.json");

        if (!Directory.Exists(carpeta))
        {
            Directory.CreateDirectory(carpeta);
        }   

        File.WriteAllText(rutaArchivo, json);
        Debug.Log("Archivo JSON creado en: " + rutaArchivo);
    }

}

[System.Serializable]
public class ListaCarros 
{
    public List<Carro> carros;
}


