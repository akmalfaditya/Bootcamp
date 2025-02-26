class MyOwnCollection<T> {
	public T[] myCollection = new T[5];
	public int count = 0;
	public void Add(T input) {
		if(count == myCollection.Length) 
		{
			return;
		}
		myCollection[count] = input;
		count++;
	}
	public T Get(int index) {
		return myCollection[index];
	}
	public void Remove(int index) {
		myCollection[index] = default;
	}
}
class Program{
	static void Main() {
		MyOwnCollection<int> myOwnCollection = new MyOwnCollection<int>();
		MyOwnCollection<string> myOwnCollectionString = new MyOwnCollection<string>();

	}
}