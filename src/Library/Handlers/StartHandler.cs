using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.InputFiles;
using Nito.AsyncEx;
using System.Text;
using System.Collections.ObjectModel;

namespace Library
{
    /// <summary>
    /// Un "handler" del patrón Chain of Responsibility que implementa el comando "hola".
    /// </summary>
    public class StartHandler : BaseHandler
    {
        /// <summary>
        /// Inicializa una nueva instancia de la clase <see cref="HelloHandler"/>. Esta clase procesa el mensaje "hola".
        /// </summary>
        /// <param name="next">El próximo "handler".</param>
        public StartHandler(TelegramBotClient bot, BaseHandler next) : base(next)
        {

            this.Keywords = new string[] { "/start", "/MENU" };
            this.bot=bot;
        }
        private TelegramBotClient bot;

        /// <summary>
        /// Procesa el mensaje "hola" y retorna true; retorna false en caso contrario.
        /// </summary>
        /// <param name="message">El mensaje a procesar.</param>
        /// <param name="response">La respuesta al mensaje procesado.</param>
        /// <returns>true si el mensaje fue procesado; false en caso contrario.</returns>
        protected override bool InternalHandle(Message message, out string response)
        {
            if (this.CanHandle(message))
            {
                if (!HistorialUser.Instance.Historial.ContainsKey(message.From.ToString()))
                {
                    HistorialUser.Instance.Historial.Add(message.From.ToString(),new Collection<string>());
                    StringBuilder CompleteMessage = new StringBuilder();
                    CompleteMessage.Append("BIENVENIDO AL BOT DEL EQUIPO 10 \n");
                    CompleteMessage.Append("Usted no se encuentra registrado... \n");
                    CompleteMessage.Append("Ingrese /REGISTRARME para continuar");
                    
                    AsyncContext.Run(() => SendGameImage(message));

                    response = CompleteMessage.ToString();
                   return true;

                }
                else 
                {
                    StringBuilder CompleteMessage = new StringBuilder();
                    CompleteMessage.Append("BIENVENIDO AL BOT DEL EQUIPO 10\n");   
                    CompleteMessage.Append("    MENU DE JUEGO \n");
                    CompleteMessage.Append("    /CREAR_PARTIDA \n");
                    CompleteMessage.Append("    /BUSCAR_PARTIDA \n");
                    CompleteMessage.Append("    /OTRAS_OPCIONES \n");

                    response = CompleteMessage.ToString();
                   return true;
                }

            }
            response = string.Empty;
            return false;
        }
        private async Task SendGameImage(Message message)
        {
            // Can be null during testing
            if (bot != null)
            {
                await bot.SendChatActionAsync(message.Chat.Id, ChatAction.UploadPhoto);

                const string filePath = @"..\Library\Images\batallaV2.png";
                using var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read);
                var fileName = filePath.Split(Path.DirectorySeparatorChar).Last();

                await bot.SendPhotoAsync(
                    chatId: message.Chat.Id,
                    photo: new InputOnlineFile(fileStream, fileName),
                    caption: "-GRUPO 10-"
                );
            }
        }
    }
}