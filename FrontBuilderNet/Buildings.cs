using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrontBuilderNet
{
    public static class Buildings
    {
        /// <summary>
        /// Построить проект
        /// </summary>
        public static void Build()
        {
            //Console.Write("Укажите полный путь к каталогу проекта:");
            //string projectPath = Console.ReadLine();
            //D:\4\frontBuilderTest
            string projectPath = Project.Path;
            if (string.IsNullOrEmpty(projectPath))
            {
                Console.WriteLine("Путь не может быть пустым. Необходимо открыть или создать проект");
                return;
            }
            string sourcePath = Path.Combine(projectPath, "src");
            string sourcePathPartial = Path.Combine(projectPath, Project.DIR_NAME_SOURCE, Project.DIR_NAME_PARTIAL);
            string destPath = Path.Combine(projectPath, Project.DIR_NAME_TARGET);
            string settingsFile = Path.Combine(projectPath, Project.FILE_NAME_CONFIG);

            //Get all partials
            string[] partialsFiles = Directory.GetFiles(sourcePathPartial, "*.html");
            Console.WriteLine($"Найдено {partialsFiles.Length} частичных файлов");

            //Page Files
            string[] pageFiles = Directory.GetFiles(sourcePath, "*.html");
            Console.WriteLine($"Найдено {pageFiles.Length} страниц");

            //Проход повсем страницам файла, для вставки частичных представлений
            foreach (string filePage in pageFiles)
            {
                FileInfo fileInfo = new FileInfo(filePage);
                string fName = fileInfo.Name;
                string outputFileContent = File.ReadAllText(filePage);

                //Проход по частичным представлениям, для замены в выходном файле
                foreach (string partialFile in partialsFiles)
                {
                    FileInfo partialFileInfo = new FileInfo(partialFile);
                    string partialName = partialFileInfo.Name;
                    if (outputFileContent.Contains($"<partial {partialName}/>"))
                    {
                        outputFileContent = outputFileContent.Replace($"<partial {partialName}/>", File.ReadAllText(partialFile));
                    }
                }
                string outFilePath = Path.Combine(destPath, fName);
                File.WriteAllText(outFilePath, outputFileContent);
            }
        }
    }
}
