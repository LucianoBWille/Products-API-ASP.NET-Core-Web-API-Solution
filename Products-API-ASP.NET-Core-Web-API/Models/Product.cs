namespace Products_API_ASP.NET_Core_Web_API.Models
{
    public class Product
    {
        public int? ProductID { get; set; }

        public string? Nome { get; set; }

        public float? Preco { get; set; }

        public int? Quantidade { get; set; }
    }
}