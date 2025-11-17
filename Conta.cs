using System.Text.Json.Serialization;

public class Conta
{
    public int Numero { get; set; }
    public string Cliente { get; set; }
    public string Cpf { get; set; }
    public string Senha { get; set; }
    public decimal Saldo { get; set; }
    public decimal Limite { get; set; }

    [JsonIgnore]
    public decimal SaldoDisponível => Saldo + Limite;

    public Conta(int numero, string cliente, string cpf, string senha, decimal limite = 0)
    {
        Numero = numero;
        Cliente = cliente;
        Cpf = cpf;
        Senha = senha;
        Limite = limite;
    }

    public bool Sacar(decimal valor)
    {
        if (valor <= 0) return false;
        if (SaldoDisponível >= valor)
        {
            Saldo -= valor;
            return true;
        }
        return false;
    }

    public void Depositar(decimal valor)
    {
        if (valor > 0)
        {
            Saldo += valor;
        }
    }

    public bool AumentarLimite(decimal novoLimite)
    {
        if (novoLimite > Limite && novoLimite >= 0)
        {
            Limite = novoLimite;
            return true;
        }
        return false;
    }

    public bool DiminuirLimite(decimal novoLimite)
    {
        if (novoLimite < Limite && novoLimite >= 0 && (Saldo + novoLimite >= 0))
        {
            Limite = novoLimite;
            return true;
        }
        return false;
    }
}