using System;
using System.Linq;

public class Program
{
    public static void Main(string[] args)
    {
        Banco banco = new Banco();
        bool sair = false;

        while (!sair)
        {
            Console.Clear();
            Console.WriteLine("--- MENU SAPIENS BANK ---");
            Console.WriteLine("1 - Inserir conta");
            Console.WriteLine("2 - Listar contas");
            Console.WriteLine("3 - Depositar");
            Console.WriteLine("4 - Sacar");
            Console.WriteLine("5 - Aumentar limite");
            Console.WriteLine("6 - Diminuir limite");
            Console.WriteLine("0 - Sair");
            Console.Write("Escolha uma opção: ");

            if (int.TryParse(Console.ReadLine(), out int opcao))
            {
                switch (opcao)
                {
                    case 1:
                        InserirConta(banco);
                        break;
                    case 2:
                        ListarContas(banco);
                        break;
                    case 3:
                        RealizarDeposito(banco);
                        break;
                    case 4:
                        RealizarSaque(banco);
                        break;
                    case 5:
                        AlterarLimite(banco, aumentar: true);
                        break;
                    case 6:
                        AlterarLimite(banco, aumentar: false);
                        break;
                    case 0:
                        sair = true;
                        Console.WriteLine("Saindo do Sapiens Bank. Obrigado!");
                        break;
                    default:
                        Console.WriteLine("Opção inválida.");
                        break;
                }
            }
            else
            {
                Console.WriteLine("Entrada inválida. Digite um número.");
            }

            if (!sair)
            {
                Console.WriteLine("\nPressione qualquer tecla para continuar...");
                Console.ReadKey();
            }
        }
    }

    static void InserirConta(Banco banco)
    {
        Console.WriteLine("\n--- Inserir Nova Conta ---");
        Console.Write("Número da Conta: ");
        if (!int.TryParse(Console.ReadLine(), out int numero) || banco.Contas.Any(c => c.Numero == numero))
        {
            Console.WriteLine("Número de conta inválido ou já existente.");
            return;
        }

        Console.Write("Nome do Cliente: ");
        string cliente = Console.ReadLine() ?? string.Empty;

        Console.Write("CPF: ");
        string cpf = Console.ReadLine() ?? string.Empty;

        Console.Write("Senha: ");
        string senha = Console.ReadLine() ?? string.Empty;

        Console.Write("Limite Inicial (R$): ");
        decimal limite = decimal.TryParse(Console.ReadLine(), out decimal l) ? l : 0m;
        
        Conta novaConta = new Conta(numero, cliente, cpf, senha, limite);
        banco.Contas.Add(novaConta);
        banco.SaveContas();
        Console.WriteLine($"\n✅ Conta {numero} de {cliente} criada com sucesso!");
    }

    static void ListarContas(Banco banco)
    {
        Console.WriteLine("\n--- Lista de Contas ---");
        if (banco.Contas.Count == 0)
        {
            Console.WriteLine("Nenhuma conta cadastrada.");
            return;
        }

        foreach (var conta in banco.Contas)
        {
            Console.WriteLine("------------------------------");
            Console.WriteLine($"Conta: {conta.Numero}");
            Console.WriteLine($"Cliente: {conta.Cliente}");
            Console.WriteLine($"CPF: {conta.Cpf}");
            Console.WriteLine($"Saldo: R$ {conta.Saldo:N2}");
            Console.WriteLine($"Limite: R$ {conta.Limite:N2}");
            Console.WriteLine($"Saldo Disponível: R$ {conta.SaldoDisponível:N2}");
        }
        Console.WriteLine("------------------------------");
    }

    static void RealizarDeposito(Banco banco)
    {
        Console.WriteLine("\n--- Realizar Depósito ---");
        Console.Write("Digite o número da conta: ");

        if (int.TryParse(Console.ReadLine(), out int numeroConta))
        {
            Conta? conta = banco.BuscarConta(numeroConta);
            if (conta != null)
            {
                Console.Write("Digite o valor do depósito: R$ ");
                if (decimal.TryParse(Console.ReadLine(), out decimal valor) && valor > 0)
                {
                    conta.Depositar(valor);
                    banco.SaveContas();
                    Console.WriteLine($"\n✅ Depósito de R$ {valor:N2} realizado na conta {conta.Numero}. Saldo atual: R$ {conta.Saldo:N2}");
                }
                else
                {
                    Console.WriteLine("\n❌ Valor inválido ou negativo.");
                }
            }
            else
            {
                Console.WriteLine($"\n❌ Conta número {numeroConta} não encontrada.");
            }
        }
        else
        {
            Console.WriteLine("\n❌ Número de conta inválido.");
        }
    }

    static void RealizarSaque(Banco banco)
    {
        Console.WriteLine("\n--- Realizar Saque ---");
        Console.Write("Digite o número da conta: ");

        if (int.TryParse(Console.ReadLine(), out int numeroConta))
        {
            Conta? conta = banco.BuscarConta(numeroConta);
            if (conta != null)
            {
                Console.Write("Digite o valor do saque: R$ ");
                if (decimal.TryParse(Console.ReadLine(), out decimal valor) && valor > 0)
                {
                    if (conta.Sacar(valor))
                    {
                        banco.SaveContas();
                        Console.WriteLine($"\n✅ Saque de R$ {valor:N2} realizado na conta {conta.Numero}. Saldo disponível: R$ {conta.SaldoDisponível:N2}");
                    }
                    else
                    {
                        Console.WriteLine($"\n❌ Saque negado. Saldo disponível (R$ {conta.SaldoDisponível:N2}) é insuficiente para R$ {valor:N2}.");
                    }
                }
                else
                {
                    Console.WriteLine("\n❌ Valor inválido ou negativo.");
                }
            }
            else
            {
                Console.WriteLine($"\n❌ Conta número {numeroConta} não encontrada.");
            }
        }
        else
        {
            Console.WriteLine("\n❌ Número de conta inválido.");
        }
    }

    static void AlterarLimite(Banco banco, bool aumentar)
    {
        string operacao = aumentar ? "Aumentar" : "Diminuir";
        Console.WriteLine($"\n--- {operacao} Limite ---");
        Console.Write("Digite o número da conta: ");

        if (int.TryParse(Console.ReadLine(), out int numeroConta))
        {
            Conta? conta = banco.BuscarConta(numeroConta);
            if (conta != null)
            {
                Console.WriteLine($"Limite atual da conta {conta.Numero}: R$ {conta.Limite:N2}");
                Console.Write($"Digite o novo valor do limite: R$ ");

                if (decimal.TryParse(Console.ReadLine(), out decimal novoLimite) && novoLimite >= 0)
                {
                    bool sucesso;
                    if (aumentar)
                    {
                        sucesso = conta.AumentarLimite(novoLimite);
                    }
                    else
                    {
                        sucesso = conta.DiminuirLimite(novoLimite);
                    }

                    if (sucesso)
                    {
                        banco.SaveContas();
                        Console.WriteLine($"\n✅ Limite da conta {conta.Numero} foi {operacao.ToLower()} para R$ {conta.Limite:N2}.");
                    }
                    else
                    {
                        Console.WriteLine($"\n❌ Não foi possível {operacao.ToLower()} o limite para R$ {novoLimite:N2}.");
                        if (aumentar)
                        {
                            Console.WriteLine("  * O novo limite deve ser maior que o limite atual.");
                        }
                        else
                        {
                            Console.WriteLine("  * O novo limite não pode ser maior que o atual, negativo ou insuficiente para cobrir o saldo devedor.");
                        }
                    }
                }
                else
                {
                    Console.WriteLine("\n❌ Valor de limite inválido ou negativo.");
                }
            }
            else
            {
                Console.WriteLine($"\n❌ Conta número {numeroConta} não encontrada.");
            }
        }
        else
        {
            Console.WriteLine("\n❌ Número de conta inválido.");
        }
    }
}