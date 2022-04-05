
using FrontBuilderNet;
using System.Reflection;


Console.WriteLine("Version FrontBuilder: 0.1.2022.2");
Console.WriteLine("Для показа списка доступных команд введите 'help'");
//Проверка последнего проекта
string path = File.ReadAllText("LastProject.txt");
if (!string.IsNullOrEmpty(path))
{
    Project.Path = path;
    Console.WriteLine($"Загружен последний проект: {path}");
}
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
        case "watch":
            {
              await Task.Run(Buildings.Watch);
                break;
            }
        case "bundle":
            {
                Buildings.Bundle();
                break;
            }
        default:
            {
                Console.WriteLine("If need help, write 'help'");
                break;
            }
    }
}

