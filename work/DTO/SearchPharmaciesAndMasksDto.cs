namespace work.DTO
{
    public class SearchPharmaciesAndMasksDto
    {
        public string Pharmac { get; set; } = "";

        public string Mask { get; set; } = "";

        public decimal Price { get; set; }

        public int DayOfWeek { get; set; }

        public TimeSpan OpenTime { get; set; }
        public TimeSpan CloseTime { get; set; }
    }
}
