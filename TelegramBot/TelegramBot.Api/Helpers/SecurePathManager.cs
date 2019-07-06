using System;

namespace TelegramBot.Api.Helpers
{
    public static class SecurePathManager
    {
        private const char UrlSeparator = '/';

        static SecurePathManager()
        {
            SecureRoute = Guid.NewGuid().ToString();
        }

        public static string SecureRoute { get; }

        public static string GetFullSecureUrl(string baseUrl)
        {
            if (baseUrl == null)
            {
                throw new ArgumentNullException(nameof(baseUrl));
            }

            string trimmedBaseUrl = baseUrl.TrimEnd(UrlSeparator);
            return $"{trimmedBaseUrl}{UrlSeparator}{SecureRoute}";
        }
    }
}
