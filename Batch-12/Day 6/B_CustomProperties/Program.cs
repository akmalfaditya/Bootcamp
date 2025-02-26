//Custom Properties
public class Car {
	private int _price;
	public int Price { 
		get 
		{
			return _price;
		}
		set 
		{
			if (value < 0) 
			{
				_price = 0;
			}
			_price = value;
		} 
	}
	
}