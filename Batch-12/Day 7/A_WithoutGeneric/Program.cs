class MyOwnCollection {
	public int[] myCollection = new int[5];
	public int count = 0;
	public void Add(int input) {
		if(count == myCollection.Length) 
		{
			return;
		}
		myCollection[count] = input;
		count++;
	}
	public int Get(int index) {
		return myCollection[index];
	}
	public void Remove(int index) {
		myCollection[index] = default;
	}
}
class MyOwnCollectionString {
	public string[] myCollection = new string[5];
	public int count = 0;
	public void Add(string input) {
		if(count == myCollection.Length) 
		{
			return;
		}
		myCollection[count] = input;
		count++;
	}
	public string Get(int index) {
		return myCollection[index];
	}
	public void Remove(int index) {
		myCollection[index] = default;
	}
}
class MyOwnCollectionFloat {
	public float[] myCollection = new float[5];
	public int count = 0;
	public void Add(float input) {
		if(count == myCollection.Length) 
		{
			return;
		}
		myCollection[count] = input;
		count++;
	}
	public float Get(int index) {
		return myCollection[index];
	}
	public void Remove(int index) {
		myCollection[index] = default;
	}
}