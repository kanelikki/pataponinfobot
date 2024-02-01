using Discord;
using System.Security;

namespace DiscordBot
{
    internal class TokenLoader
    {
        const string _tokenPath = "./token.txt";

        internal async Task<string> GetTokenAsync(ILogger logger)
        {
            try
            {
                var token = (await File.ReadAllTextAsync(_tokenPath)).Trim();
                TokenUtils.ValidateToken(TokenType.Bot, token);
                return token;
            }
            catch (Exception ex)
            {
                await logger.Log("[!!! Token Failure !!!]\nFailed to get token! The bot cannot be launched.", LogSeverity.Critical);
                string message = "An unknown error is occurred. Our expert cat will try to fix the issue soon.";
                switch (ex)
                {
                    case ArgumentNullException:
                        message = "The TOKEN FILE is Empty! The token file MUST contain ONLY token as string.";
                        break;
                    case ArgumentException:
                        message = "INVALID TOKEN in the TOKEN FILE. The token file MUST contain ONLY token as string.";
                        break;
                    case FileNotFoundException:
                        message = $"Cannot find the TOKEN FILE. Make one in {_tokenPath} and fill it with ONLY token.";
                        break;
                    case SecurityException:
                        message = "You don't have permission to read the file. Check permission of the TOKEN FILE again.";
                        break;
                    case PathTooLongException:
                        message = "The path is too long. Maybe the app is located in too long path?";
                        break;
                    case NotSupportedException:
                        message = "The file seems invalid. TOKEN FILE must include ONLY token as text.";
                        break;
                    case IOException:
                        message = "An I/O error occurred while opening the file. Maybe TOKEN FILE is broken or in use?";
                        break;
                }
                await logger.Log($"[!!!] {message}", LogSeverity.Critical);
                throw;
            }
        }
    }
}
