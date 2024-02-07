namespace DiscordBot
{
    /// <summary>
    /// General discord bot setting. DOES NOT INCLUDE TOKEN.
    /// </summary>
    public record Setting
    {
        private string? _imageUrl;
        /// <summary>
        /// Image URL to implement. If this is null or empty, the bot won't use image.
        /// </summary>
        /// <remarks>
        /// This url is expected to have specific file collections per bot.
        /// It's highly recommended that you have permission to access for file writing to the server.
        /// For detail, check example code.
        /// </remarks>
        /// <note>HTTP IS NOT SUPPORTED. ONLY HTTPS IS VALID.</note>
        public string? ImageUrl {
            get => _imageUrl;
            set
            {
                if (
                    Uri.TryCreate(value, UriKind.Absolute, out var uriResult)
                    && uriResult.Scheme == Uri.UriSchemeHttps
                    )
                {
                    _imageUrl = value;
                }
                else
                {
                    _imageUrl = null;
                }
            }
        }
    }
}
