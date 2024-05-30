using ConsumeEbizCharge.API.Model;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Xml;


namespace ConsumeEbizCharge.API.Services
{
    public class SoapService
    {
        private readonly HttpClient _httpClient;

        public SoapService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<SoapResponse> GetMerchantIntegrationSettingsAsync(SoapRequest request)
        {
            var soapEnvelope = $@"
                <soapenv:Envelope xmlns:soapenv=""http://schemas.xmlsoap.org/soap/envelope/"" xmlns:ebiz=""http://eBizCharge.ServiceModel.SOAP"">
                    <soapenv:Header/>
                    <soapenv:Body>
                        <ebiz:GetMerchantIntegrationSettings>
                            <ebiz:securityToken>
                                <ebiz:SecurityId>{request.SecurityKey}</ebiz:SecurityId>
                                <ebiz:UserId>{request.Username}</ebiz:UserId>
                                <ebiz:Password>{request.Password}</ebiz:Password>
                            </ebiz:securityToken>
                        </ebiz:GetMerchantIntegrationSettings>
                    </soapenv:Body>
                </soapenv:Envelope>";

            var content = new StringContent(soapEnvelope, Encoding.UTF8, "text/xml");
            content.Headers.Add("SOAPAction", "http://eBizCharge.ServiceModel.SOAP/IeBizService/GetMerchantIntegrationSettings");

            var response = await _httpClient.PostAsync("https://soapapi1.ebizcharge.net/v2", content);
            response.EnsureSuccessStatusCode();

            var responseString = await response.Content.ReadAsStringAsync();
            return ParseSoapResponse(responseString);
        }

        private SoapResponse ParseSoapResponse(string responseString)
        {
            var soapResponse = new SoapResponse();
            var xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(responseString);

            var namespaceManager = new XmlNamespaceManager(xmlDoc.NameTable);
            namespaceManager.AddNamespace("soapenv", "http://schemas.xmlsoap.org/soap/envelope/");
            namespaceManager.AddNamespace("ebiz", "http://eBizCharge.ServiceModel.SOAP");

            var settingsNodes = xmlDoc.SelectNodes("//ebiz:MerchantIntegrationSettings", namespaceManager);
            if (settingsNodes != null)
            {
                foreach (XmlNode settingNode in settingsNodes)
                {
                    var settingName = settingNode["SettingName"]?.InnerText;
                    var settingValue = settingNode["SettingValue"]?.InnerText;

                    switch (settingName)
                    {
                        case "IsEMVEnabled":
                            soapResponse.IsEMVEnabled = bool.Parse(settingValue);
                            break;
                        case "IsEMVPreAuthEnabled":
                            soapResponse.IsEMVPreAuthEnabled = bool.Parse(settingValue);
                            break;
                        case "UseEConnectTransactionReceipts":
                            soapResponse.UseEConnectTransactionReceipts = bool.Parse(settingValue);
                            break;
                        case "BatchProcessingSurchargeTermsNote":
                            soapResponse.BatchProcessingSurchargeTermsNote = settingValue;
                            break;
                    }
                }
            }

            return soapResponse;
        }
    }
}
