namespace Products.WebApi.Models
{
    public class ViewProductDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public float Price { get; set; }
        public byte[] Image { get; set; }
    }
}