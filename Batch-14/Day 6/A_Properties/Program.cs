class Car {
	private int _price; //variable
	public int GetPrice() {
		return _price;
	}
	public void SetPrice(int value) {
		_price = value;
	}
}
class CarWProperty { //property
	public int Price {get; private set;}
}