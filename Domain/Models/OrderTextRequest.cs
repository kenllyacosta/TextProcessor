using Mecalux.Domain.Enums;

namespace Mecalux.Domain.Models
{
    public class OrderTextRequest
    {
        public OrderOption OrderOption { get; set; }
        public required string TextToOrder { get; set; }
    }
}