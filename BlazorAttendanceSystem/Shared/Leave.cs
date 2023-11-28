namespace BlazorAttendanceSystem.Shared
{
    public class Leave
    {
        public Guid Id { get; set; }
        public string LeaveType { get; set; } = string.Empty;
        public int MaxDaysAllowed { get; set; }
        public int RemainingLeaveDays { get; set; }
        public string Description { get; set; } = string.Empty;
        
    }
}
