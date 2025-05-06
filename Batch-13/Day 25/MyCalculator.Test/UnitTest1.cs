namespace MyCalculator.Test;

public class MyCalculatorTest 
{
	private Calculator _calculator;
	[SetUp]
	public void SetUp() 
	{
		_calculator = new Calculator();
	}
	[Test]
	public void Add_ReturnCorrectResult() 
	{
		//Arrange
		int a = 3;
		int b = 2;
		int expected = 5;
		//Act
		int result = _calculator.Add(a, b);
		//Assert
		Assert.That(result, Is.EqualTo(expected));
	}
	[TestCase(3,4,7)]
	[TestCase(5,8,13)]
	[TestCase(13,-4,9)]
	[TestCase(1000,1,1001)]
	public void Add_ReturnCorrectResult(int a, int b, int expected) 
	{
		//Arrange
		//Act
		int result = _calculator.Add(a, b);
		//Assert
		Assert.That(result, Is.EqualTo(expected));
	}
	[Test] //Negative Case
	public void Add_ThrowException_AddingZero() 
	{
		//Arrange
		int a = 0;
		int b = 0;
		//Act

		//Assert
		Assert.Throws<Exception>(() => _calculator.Add(a, b));
	}
}