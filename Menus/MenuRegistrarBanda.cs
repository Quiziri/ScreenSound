using OpenAI_API;
using ScreenSound.Modelos;

namespace ScreenSound.Menus;

internal class MenuRegistrarBanda : Menu
{
    public async override void Executar(Dictionary<string, Banda> bandasRegistradas)
    {
        base.Executar(bandasRegistradas);
        ExibirTituloDaOpcao("Registro das bandas");
        Console.Write("Digite o nome da banda que deseja registrar: ");
        string nomeDaBanda = Console.ReadLine()!;
        Banda banda = new Banda(nomeDaBanda);
        bandasRegistradas.Add(nomeDaBanda, banda);
        var client = Environment.GetEnvironmentVariable("OPENIA_APIKEY");

        var chat = client.Chat.CreateConversation();
        chat.AppendSystemMessage($"Resuma a banda {nomeDaBanda} em 1 parágrafo. Adote um estilo informal.");
        String resposta = chat.GetResponseFromChatbotAsync().GetAwaiter().GetResult();
        banda.Resumo = resposta;

        Console.WriteLine($"A banda {nomeDaBanda} foi registrada com sucesso!");
        Console.WriteLine("\nDigite uma tecla para voltar ao menu principal");
        Console.ReadKey();
        Console.Clear();
    }
}