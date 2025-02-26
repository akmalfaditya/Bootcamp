class Engine { //internal
 int type; //private
 void Run() { //private
  Console.WriteLine("Running");
 }
}
class Program {
 public static void Main()  {
  Engine engine = new Engine();
  engine.type = 1;
  engine.Run();
 }
}