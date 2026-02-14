using UnityEngine;

[System.Serializable]
public class Vehiculo 
{
   
    public string idVehiculo;
    public string marca;
    public string modelo;

    public Vehiculo()
    {
    }

    public Vehiculo(string idVehiculo, string marca, string modelo)
    {
        this.idVehiculo = idVehiculo;
        this.marca = marca;
        this.modelo = modelo;
    }

    public string IdVehiculo { get => idVehiculo; set => idVehiculo = value; }
    public string Marca { get => marca; set => marca = value; }
    public string Modelo { get => modelo; set => modelo = value; }

    public override string ToString()
        {
            return "ID: " + IdVehiculo + ", Marca: " + Marca + ", Modelo: " + Modelo;
    }
}
