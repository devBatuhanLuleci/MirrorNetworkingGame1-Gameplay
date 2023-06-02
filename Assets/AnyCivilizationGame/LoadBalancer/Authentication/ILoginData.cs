namespace ACGAuthentication
{
    public interface ILoginData
    {
        public string Email { get; set; }
        public string MoralisId { get; set; }
        public string WalletId { get; set; }
        public string userName { get; set; }
    }

}
