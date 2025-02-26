//Property
interface IParent 
{
	public int NIK {get; }
}
class Parent : IParent 
{
	public int NIK {get; private set;}
}