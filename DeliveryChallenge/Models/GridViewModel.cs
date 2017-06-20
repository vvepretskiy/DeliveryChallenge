namespace DeliveryChallenge.Models
{
	public class GridViewModel
	{
		public int TotalRowCount { get; set; }
		public int TotalPageCount { get; set; }
		public object Data { get; set; }

		public GridViewModel()
		{}

		public GridViewModel(int count, int rows)
		{
			TotalRowCount = count;

			int totalPages = 1;
			if (rows > 0)
			{
				totalPages = TotalRowCount / rows;
				if (TotalRowCount % rows != 0)
					totalPages += 1;
			}

			TotalPageCount = totalPages;
		}
	}
}
