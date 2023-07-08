using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System.Net;

namespace PracticeProject.Models
{
    public class GoogleCaptchaService
    {
        private readonly IOptionsMonitor<GoogleCaptchaConfig> _config;
        public GoogleCaptchaService(IOptionsMonitor<GoogleCaptchaConfig> config)
        {
            _config = config;
        }
        public async Task<bool> VerifyToken(string token)
        {
            try
            {
                var url = $"https://www.google.com/recaptcha/api/siteverify?secret={_config.CurrentValue.SecretKey}&response={token}";
                using (var clien = new HttpClient())
                {
                    var httpResult = await clien.GetAsync(url);
                    if (httpResult.StatusCode != HttpStatusCode.OK)
                    {
                        return false;
                    }
                    var responeString = await httpResult.Content.ReadAsStringAsync();
                    var result = JsonConvert.DeserializeObject<GoogleCaptchaResponse>(responeString);
                    return result.success && result.score >= 0.5;
                }
            }
            catch(Exception ex)
            {
                return false;
            }
        }
    }
}
