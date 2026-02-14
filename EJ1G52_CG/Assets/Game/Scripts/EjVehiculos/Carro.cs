using UnityEngine;

[System.Serializable]
public class Carro : Vehiculo
{
   
    public string placa;
    public int numeroPuertas;

    public Carro()
    {
    }

    public Carro(string idVehiculo, string marca, string modelo, string placa, int numeroPuertas) 
        : base(idVehiculo, marca, modelo)
    {
        this.placa = placa;
        this.numeroPuertas = numeroPuertas;
    }

    public string Placa { get => placa; set => placa = value; }
    public int NumeroPuertas { get => numeroPuertas; set => numeroPuertas = value; }

    public override string ToString()
    {
        return base.ToString() + ", Placa: " + Placa + ", Número de Puertas: " + NumeroPuertas; 
    }
}
