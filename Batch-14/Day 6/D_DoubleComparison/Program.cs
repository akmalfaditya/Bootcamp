void Main() {
	int a = 1;
	int b = 2;
	Console.WriteLine( (a+b) == 3 ); //True
	
	float c = 0.1f;
	float d = 0.2f;
	Console.WriteLine( (c+d) == 0.3f); //True False Ragu ragu
	
	double e = 0.1;
	double f = 0.2;
	Console.WriteLine( (e+f) );
	Console.WriteLine( (e+f) > 0.3 ); //True False
	
	decimal g = 0.01M;
	decimal h = 0.02M;
	Console.WriteLine( (g+h) == 0.03M); //True False Ragu ragu
}