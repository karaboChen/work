namespace work.DTO
{
    public class PharmacyOpeningDto
    {
        public string Name { get; set; } = string.Empty;   // 藥局名稱
        public int DayOfWeek { get; set; }                 // 星期幾 (1=星期一...)
        public TimeSpan OpenTime { get; set; }             // 開始時間
        public TimeSpan CloseTime { get; set; }            // 結束時間

    }
}
