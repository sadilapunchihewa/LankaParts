namespace LankaParts_Backend.DTOs.Admin
{
    public class AdminDashboardDto
    {
        public int TotalUsers { get; set; }
        public int ActiveCustomers { get; set; }
        public int ActiveSellers { get; set; }
        public int PendingSellerCompanies { get; set; }
        public int ApprovedSellerCompanies { get; set; }
        public int ActivePartListings { get; set; }
        public int LowStockListings { get; set; }
        public int TotalOrders { get; set; }
        public int PendingOrders { get; set; }
        public int DeliveredOrders { get; set; }
        public int CancelledOrders { get; set; }
        public decimal PaidRevenue { get; set; }
        public decimal PendingPaymentAmount { get; set; }
        public int TotalReviews { get; set; }
        public double AverageMarketplaceRating { get; set; }
    }
}
