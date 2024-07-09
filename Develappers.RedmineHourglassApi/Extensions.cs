using System;
using System.IO;
using System.Net;
using Develappers.RedmineHourglassApi.Types;
using Newtonsoft.Json;

namespace Develappers.RedmineHourglassApi;

internal static class Extensions
{
    /// <summary>
    /// Extracts the error object in case of StatusCode >= 400
    /// </summary>
    /// <param name="wex">The web exception.</param>
    /// <returns>The error containing detailed error information.</returns>
    public static Error ExtractError(this WebException wex)
    {
        try
        {
            string result;
            using (var httpResponse = (HttpWebResponse) wex.Response)
            {
                var responseStream = httpResponse.GetResponseStream();
                if (responseStream == null)
                {
                    throw new IOException("response stream was null!");
                }

                using var streamReader = new StreamReader(responseStream);
                result = streamReader.ReadToEnd();
            }

            return JsonConvert.DeserializeObject<Error>(result);
        }
        catch (Exception ex)
        {
            // LogProvider.GetCurrentClassLogger().ErrorException($"unexpected exception {ex} occurred", ex);
            return null;
        }
    }
}