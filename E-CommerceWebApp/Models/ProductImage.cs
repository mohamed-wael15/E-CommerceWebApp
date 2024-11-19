namespace E_CommerceWebApp.Models
{
    public class ProductImage
    {
        public int ProductImageId { get; set; }
        public byte[] ImageData { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
