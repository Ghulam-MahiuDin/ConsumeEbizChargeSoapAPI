namespace ConsumeEbizCharge.API.Model
{
    public class SoapResponse
    {
        public bool IsEMVEnabled { get; set; }
        public bool IsEMVPreAuthEnabled { get; set; }
        public bool UseEConnectTransactionReceipts { get; set; }
        public string BatchProcessingSurchargeTermsNote { get; set; }

        public bool EnableLendicaOnCloudVT {  get; set; }
    }
}
