
class Program {
	static void Main() {
		int result = (int) MonthOfYear.Feb;
		result.Dump();
		
		MonthOfYear result2 = (MonthOfYear)2;
		result2.Dump();
		
		string x = MonthOfYear.Dec.ToString();
		x.Dump();
	}
}
public enum MonthOfYear {
	Jan = 1,
	Feb,
	Mar,
	Apr,
	May,
	Jun,
	Jul,
	Aug,
	Sep,
	Oct,
	Nov,
	Dec
}