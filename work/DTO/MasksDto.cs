namespace work.DTO
{
    public class MasksDto
    {

        public string PharmacyName { get; set; } = string.Empty;   // 藥局名稱
        public string MaskName { get; set; } = string.Empty;   // 口罩名稱
        public decimal Price { get; set; }                 // 價格
        public string Color { get; set; } = string.Empty;         // 顏色
        public int Quantity { get; set; }          // 數量
    }
}
