//TODO:b 265. Add AssemblyInfo.cs to Tela project with InternalsVisibleTo attribute
//This will allow the ExamenTela project to access internal classes in the Tela project
//In particular, Tela.Program for use with GetHostBuilder<Program>() in ExamenTela.Startup 
using System.Runtime.CompilerServices;
[assembly: InternalsVisibleTo("ExamenTela")]