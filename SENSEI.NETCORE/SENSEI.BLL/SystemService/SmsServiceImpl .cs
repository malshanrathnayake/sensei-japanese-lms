using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Configuration;
using SENSEI.BLL.SystemService.Interfaces;

namespace SENSEI.BLL.SystemService
{
    public class SmsServiceImpl : ISmsService
    {
        private readonly string _apiToken;
        private readonly string _senderId;

        public SmsServiceImpl(IConfiguration configuration)
        {
            _apiToken = configuration["TextLk:ApiToken"];
            _senderId = configuration["TextLk:SenderId"];
        }

        /// <summary>
        /// Send SMS to a single number
        /// </summary>
        public async Task<bool> SendSingleAsync(string phoneNumber, string message)
        {
            try
            {
                var url = BuildUrl(phoneNumber, message);

                using var httpClient = new HttpClient();
                var response = await httpClient.GetAsync(url);

                return response.IsSuccessStatusCode;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Send SMS to multiple numbers (comma separated)
        /// </summary>
        public async Task<bool> SendBulkAsync(List<string> phoneNumbers, string message)
        {
            if (phoneNumbers == null || phoneNumbers.Count == 0)
                return false;

            try
            {
                var recipients = string.Join(",", phoneNumbers);
                var url = BuildUrl(recipients, message);

                using var httpClient = new HttpClient();
                var response = await httpClient.GetAsync(url);

                return response.IsSuccessStatusCode;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Build Text.lk HTTP API URL
        /// </summary>
        private string BuildUrl(string recipients, string message)
        {
            return
                "https://app.text.lk/api/http/sms/send" +
                $"?recipient={Uri.EscapeDataString(recipients)}" +
                $"&sender_id={Uri.EscapeDataString(_senderId)}" +
                $"&message={Uri.EscapeDataString(message)}" +
                $"&api_token={Uri.EscapeDataString(_apiToken)}";
        }
    }
}
