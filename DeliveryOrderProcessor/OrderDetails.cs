namespace DeliveryOrderProcessor
{
	public class OrderDetails
	{
		public string Id { get; set; }
		public ShippingAddress ShippingAddress { get; set; }
		public decimal FinalPrice { get; set; }
		public List<Item> Items { get; set; }
	}

	public class Item
	{
		public int Count { get; set; }
		public int ItemId { get; set; }
		public string Name { get; set; }
	}

	public class ShippingAddress
	{
		public string Street { get; set; }
		public string City { get; set; }
		public string State { get; set; }
		public string Country { get; set; }
		public string ZipCode { get; set; }
	}
}
