
using FrontBuilderNet;
using System.Reflection;



Console.WriteLine("Version FrontBuilder: 0.1.2022.1");
Console.WriteLine("If need help, write 'help'");
bool isExit = false;

while (!isExit)
{
    string command = Console.ReadLine();
    switch (command)
    {
        case "help":
            {
                Commands.Help();
                break;
            }
        case "create":
            {
                Commands.Create();
                break;
            }
        case "open":
            {
                Commands.Open();
                break;
            }
        case "build":
            {
                Buildings.Build();
                break;
            }
        default:
            {
                Console.WriteLine("If need help, write 'help'");
                break;
            }
    }
}

