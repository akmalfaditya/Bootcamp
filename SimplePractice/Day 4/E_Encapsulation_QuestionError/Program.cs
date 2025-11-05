```csharp
class Engine {
 private void Run() {
  Console.WriteLine("Engine Run");
 }
}
class ElectricEngine : Engine {
 public void Off() {
  Console.WriteLine("Engine Off");
 }
}
class Program {
 static void Main() {
  Engine engine = new Engine();
  engine.Run(); //Bima,Fitria,Sanan,Maruf,Hanun,Fathan
  engine.Off(); //Akmal,Bima,Fitria,Sanan,Maruf,Hanun,Fathan
  
  ElectricEngine ee = new ElectricEngine();
  ee.Run(); //Bima,Fitria,Sanan,Maruf,Hanun,Fathan
  ee.Off(); 
 }
}
```