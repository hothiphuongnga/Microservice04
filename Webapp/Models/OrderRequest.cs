    public  class OrderRequest
    {
        public int BuyerId { get; set; }
        public decimal TotalAmount { get; set; }
        public List<OrderDetail> OrderDetails { get; set; }
    }

    public  class OrderDetail
    {
        public int ProductId { get; set; }
        public decimal UnitPrice { get; set; }
        public int Quantity { get; set; }
    }