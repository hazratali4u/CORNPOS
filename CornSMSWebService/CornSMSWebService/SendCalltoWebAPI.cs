using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// Summary description for WebAPINotification
/// </summary>
public class SendCalltoWebAPI
{
    public SendCalltoWebAPI()
    {
        //
        // TODO: Add constructor logic here
        //
    }
    public async void CallWebAPI(string p_Text, string p_Receipants, string p_Masking, string p_Token,string p_URL,string p_URL2)
    {
        //CallWebAPIAsync(p_Text, p_Receipants,p_Masking,p_Token, p_URL, p_URL2).Wait();
       await CallWebAPIAsync();
    }
    static async Task CallWebAPIAsync()
    {
        try
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://thumb-crowd.com");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Add("Authorization", "TOKEN 6da996de24c87f6e3088dbd9a6d22e967d71059f");

                MessageBody SMSMessage = new MessageBody();
                SMSMessage.text = "Hi";
                SMSMessage.urns = "tel:+923110449206";
                SMSMessage.masking = "Mozarella27";

                var dataAsString = JsonConvert.SerializeObject(SMSMessage);
                var dataContent = new StringContent(dataAsString, Encoding.UTF8, "application/json");
                HttpResponseMessage response = await client.PostAsync("/api/v2/broadcasts.json", dataContent);

                if (response.IsSuccessStatusCode)
                {
                    Uri ncrUrl = response.Headers.Location;
                }
            }
        }
        catch (Exception ex)
        {
        }
    }

    static async Task CallWebAPIAsync(string p_Text, string p_Receipants,string p_Masking,string p_Token,string p_URL,string p_URL2)
    {
        try
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(p_URL);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Add("Authorization", "TOKEN " + p_Token);

                MessageBody SMSMessage = new MessageBody() { text = p_Text, urns = "tel:" + p_Receipants, masking = p_Masking };

                var dataAsString = JsonConvert.SerializeObject(SMSMessage);
                var dataContent = new StringContent(dataAsString, Encoding.UTF8, "application/json");
                HttpResponseMessage response = await client.PostAsync(p_URL2, dataContent);

                if (response.IsSuccessStatusCode)
                {
                    Uri ncrUrl = response.Headers.Location;
                }
            }
        }
        catch (Exception ex)
        {
            CORNCommon.Classes.ExceptionPublisher.PublishException(ex);
        }
    }
}
class MessageBody
{
    public string text { get; set; }
    public string urns { get; set; }
    public string masking { get; set; }
}