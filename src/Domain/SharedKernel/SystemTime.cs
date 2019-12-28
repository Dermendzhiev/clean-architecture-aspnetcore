namespace CleanArchitecture.Domain.SharedKernel
{
    using System;

    public static class SystemTime
    {
        private static DateTime? customDate;

        public static DateTime Now
        {
            get
            {
                if (customDate.HasValue)
                {
                    return customDate.Value;
                }

                return DateTime.UtcNow;
            }
        }

        public static void Set(DateTime customDate) => SystemTime.customDate = customDate;

        public static void Reset() => customDate = null;
    }
}