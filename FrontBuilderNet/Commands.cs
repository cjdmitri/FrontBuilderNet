using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace FrontBuilderNet
{
    public static class Commands
    {
        /// <summary>
        /// Справка
        /// </summary>
        public static void Help()
        {
            Console.WriteLine("Доступные команды:"); 
            Console.WriteLine("==========================================:");
            Console.WriteLine("create - создание нового проекта");
            Console.WriteLine("build - построить ранее открытый проект");
            Console.WriteLine("open - открыть существующий проект");
        }

        /// <summary>
        /// Создание нового проекта
        /// </summary>
        public static void Create()
        {
            Console.Write("Введите название проекта:");
            string name = Console.ReadLine();
            Console.Write("Укажите путь где будет создана папка проекта:");
            string dirPath = Console.ReadLine();
            string destPath = Path.Combine(dirPath, name);

            if(Directory.Exists(destPath))
                Console.Write("Данный каталог уже существует. Не удалось создать проект.");
            else
            {
                //Текущая папка программы
                string pathTemplate = Path.Combine(Directory.GetCurrentDirectory(), "template");
                //Настройка проекта
                Project.Name = name;
                Project.Path = destPath;
                Directory.CreateDirectory(destPath);

                CopyDir(pathTemplate, destPath);
                Console.Write("Проект создан.");
            }
        }

        /// <summary>
        /// Открытие существующего проекта
        /// </summary>
        public static void Open()
        {
            Console.Write("Укажите путь к папке проекта:");
            string destPath = Console.ReadLine();
            Project.Path = destPath;
            Console.WriteLine("Проект открыт.");
        }

        static  void CopyDir(string FromDir, string ToDir)
        {
            Directory.CreateDirectory(ToDir);
            foreach (string s1 in Directory.GetFiles(FromDir))
            {
                string s2 = Path.Combine(ToDir, Path.GetFileName(s1));
                File.Copy(s1, s2);
            }
            foreach (string s in Directory.GetDirectories(FromDir))
            {
                CopyDir(s, Path.Combine(ToDir, Path.GetFileName(s)));
            }
        }
    }
}
