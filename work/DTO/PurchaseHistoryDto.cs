namespace work.DTO
{
    public class PurchaseHistoryDto
    {
        public string PurchaseId { get; set; } = "";
        public int UserId { get; set; } 
        public int PharmacyName { get; set; } 

        public int MaskName { get; set; } 
        public decimal TransactionAmount { get; set; }

        public DateTime TransactionDate { get; set; } 
    }

    public class PurchaseHistoryResponseDto
    {
        public string PurchaseId { get; set; } = "";

        public string PharmacyName { get; set; } = "";

        public string MaskName { get; set; }  = "";
        public decimal TransactionAmount { get; set; }

        public string TransactionDate { get; set; } = "";

        public string Color { get; set; } = "";
        public int Quantity { get; set; }
    }

}
