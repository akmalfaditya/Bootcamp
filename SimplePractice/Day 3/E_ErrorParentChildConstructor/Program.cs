//Error ParentChild Constructor
class Component {
	public string brand;
	public string colour;
	public int price;
	public Component(string brand, string colour, int price)
	{
		this.brand = brand;
		this.colour = colour;
		this.price = price;
	}
}
class Bell : Component {
	public int size;
	public string model;
	public string material;
	//below error because requirement from Parent
	public Bell(int size, string model, string material) {
		this.size = size;
		this.model = model;
		this.material = material;
	}
}