

class Car
{
	//sifat public di akses disemua kelas
	// public int price;
	//modifier private yang diakses hanya dalam kelas saja
	private int _price;

	public Car(int price)
	{
		// this.price = price;
		this._price = price;
	}


	//fuction untuk menampilkan data dengan variabel protected/ private
	public int CheckVariable(string validasi)
	{
		if(validasi.Equals("password"))
		{
			return _price;
			
		}
		return 0;

	}
	
	//fuction overide
	public virtual void Greating()
	{
		Console.WriteLine("Helloo Anak");
	}
	
	
	//fucn untuk mengubah nilai dengan valiadsi

	protected void SetPrice(int price)
	{
		if (!(price < 0))
		{
			this._price = price;

		}


	}
}

class CarAmphibi:Car
{
	public string skill;
	public CarAmphibi(int _price, string skill):base(_price)
	{
		this.skill = skill;
	}
	//modifier public acces protected acces private
	public void SetPricePublic(int price)
	{
		SetPrice(price);
		
	}
	
	//Overide
	public override void Greating()
	{
		base.Greating();
		Console.WriteLine("Helo Bapak"); 
	}
	
}

class Program 
{
	static void Main()
	{
		Car car = new Car(2000);
		// access public
		// car.price = 20;
		
		// access private
		//car.SetPrice(200);
		
		CarAmphibi carAmphibi = new CarAmphibi(599,"Menyelam");
		Console.WriteLine(carAmphibi.CheckVariable("password"));
		
		//acces protected
		carAmphibi.SetPricePublic(7);
		Console.WriteLine(carAmphibi.CheckVariable("password"));
		
		//ovveride
		carAmphibi.Greating();
	
		
	}
}
