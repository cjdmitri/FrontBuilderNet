using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Encodings.Web;
using System.Text.Unicode;
using System.Diagnostics;

namespace FrontBuilderNet
{
    public static class Buildings
    {

        private static JsonSerializerOptions options = new JsonSerializerOptions
        {
            WriteIndented = false,
            Encoder = JavaScriptEncoder.Create(UnicodeRanges.BasicLatin, UnicodeRanges.Cyrillic)
        };
        /// <summary>
        /// Список файлов, для отслеживания изменений
        /// </summary>
        static List<KeyValuePair<string, string>> files = new List<KeyValuePair<string, string>>();



        /// <summary>
        /// Построить проект
        /// </summary>
        public static void Build()
        {
            string projectPath = Project.Path;
            Stopwatch sw = new Stopwatch();
            sw.Restart();
            if (string.IsNullOrEmpty(projectPath))
            {
                Console.WriteLine("Путь не может быть пустым. Необходимо открыть или создать проект");
                return;
            }
            Console.WriteLine($"{DateTime.Now.ToLongTimeString()} Сборка проекта...");
            string sourcePath = Path.Combine(projectPath, "src");
            string sourcePathPartial = Path.Combine(projectPath, Project.DIR_NAME_SOURCE, Project.DIR_NAME_PARTIAL);
            string destPath = Path.Combine(projectPath, Project.DIR_NAME_TARGET);
            string settingsFile = Path.Combine(projectPath, Project.FILE_NAME_CONFIG);

            //Get all partials
            string[] partialsFiles = Directory.GetFiles(sourcePathPartial, "*.html");
            //Console.WriteLine($"Найдено {partialsFiles.Length} частичных файлов");

            //Page Files
            string[] pageFiles = Directory.GetFiles(sourcePath, "*.html");
            //Console.WriteLine($"Найдено {pageFiles.Length} страниц");

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
                    if (outputFileContent.Contains($"<partial {partialName}/>") || outputFileContent.Contains($"<partial {partialName} />"))
                    {
                        outputFileContent = outputFileContent.Replace($"<partial {partialName}/>", File.ReadAllText(partialFile));
                    }
                }
                string outFilePath = Path.Combine(destPath, fName);
                outputFileContent = outputFileContent.ReplaceVariables(fName);
                File.WriteAllText(outFilePath, outputFileContent);
            }
            sw.Stop();
            Console.WriteLine($"{DateTime.Now.ToLongTimeString()} Сборка проекта завершена. {sw.ElapsedMilliseconds} мсек.");
        }

        /// <summary>
        /// Замена переменных в .html файлах на данные из настроек приложение frontbuilder.json
        /// </summary>
        /// <param name="inputText">Исходный текст</param>
        /// <param name="pageFileName">Страница, для которой происходит замена переменных</param>
        /// <returns></returns>
        static string ReplaceVariables(this string inputText, string pageFileName)
        {
            string pathsettings = Path.Combine(Project.Path, Project.FILE_NAME_CONFIG);
            JsonNode settings = JsonNode.Parse(File.ReadAllText(pathsettings));

            JsonNode variables = settings!["variables"]![pageFileName]!;
            if (variables != null)
            {
                foreach (JsonObject variable in variables.AsArray())
                {
                    Dictionary<string, JsonNode> jsonString = variable.ToDictionary(x => x.Key, x => x.Value);
                    string key = jsonString.Keys.FirstOrDefault();
                    string value = variable[key]!.GetValue<string>();
                    inputText = inputText.Replace($"<variable {key}/>", value).Replace($"<variable {key} />", value);
                }
            }
            return inputText;
        }


        /// <summary>
        /// Следить за изменениями в проекте
        /// Следит за временем сохранения файлов. При несовпадении происходит перестройка проекта
        /// </summary>
        public static async Task Watch()
        {
            if (string.IsNullOrEmpty(Project.Path))
            {
                Console.WriteLine("Необходимо открыть или создать проект");
            }
            Console.WriteLine($"{DateTime.Now.ToLongTimeString()} Отслеживаем изменения...");
            //string sourcePath = Path.Combine(Project.Path, "src");
            //string sourcePathPartial = Path.Combine(Project.Path, Project.DIR_NAME_SOURCE, Project.DIR_NAME_PARTIAL);

            //Бесконечный цикл проверки файлов
            while (true)
            {
                if (NeedUpdate())
                {
                    Build();
                    Bundle();
                    needUpdate = false;
                }
                await Task.Delay(1000);
            }
        }

        /// <summary>
        /// Сборка CSS, JS файлов 
        /// </summary>
        public static void Bundle()
        {
            Stopwatch sw = new Stopwatch();
            sw.Restart();
            Console.WriteLine($"{DateTime.Now.ToLongTimeString()} Сборка CSS, JS файлов...");
            string pathsettings = Path.Combine(Project.Path, Project.FILE_NAME_CONFIG);
            JsonNode settings = JsonNode.Parse(File.ReadAllText(pathsettings));
            //Файлы назначения
            if (settings!["bundle"]! != null)
            {
                foreach (JsonNode bundle in settings!["bundle"]!.AsArray())
                {
                    string outputPath = Path.Combine(Project.Path, bundle!["outputfile"]!.GetValue<string>());
                    string outputContent = "";
                    //Файлы для объединения
                    foreach (JsonNode inputfile in bundle!["inputfiles"]!.AsArray())
                    {
                        string inputPath = Path.Combine(Project.Path, inputfile!.GetValue<string>());
                        string inputContent = File.ReadAllText(inputPath);
                        outputContent += inputContent;
                    }
                    File.WriteAllText(outputPath, outputContent);
                }

            }
            sw.Stop();
            Console.WriteLine($"{DateTime.Now.ToLongTimeString()} Сборка CSS, JS файлов завершена. {sw.ElapsedMilliseconds} мсек.");
        }

        private static bool needUpdate = false;
        /// <summary>
        /// Отслеживает все файлы в каталоге src на наличие изменений
        /// </summary>
        /// <returns>true - если требуются изменения</returns>
        private static bool NeedUpdate(string path = "")
        {
            //bool needUpdate = false;
            string pathRootSrc;
            if (string.IsNullOrEmpty(path))
                pathRootSrc = Path.Combine(Project.Path, Project.DIR_NAME_SOURCE);
            else
                pathRootSrc = path;

            //Проход по всем файлам и папкам, для поиска изменений
            foreach (string file in Directory.GetFiles(pathRootSrc))
            {
                FileInfo fileInfo = new FileInfo(file);
                var f = files.FirstOrDefault(x => x.Key == fileInfo.FullName && x.Value == fileInfo.LastWriteTime.ToString());
                if (string.IsNullOrEmpty(f.Key))
                {
                    KeyValuePair<string, string> keyValuePair = new KeyValuePair<string, string>(fileInfo.FullName, fileInfo.LastWriteTime.ToString());
                    files.Add(keyValuePair);
                    needUpdate = true;
                    //break;
                }
            }
            foreach (string sub in Directory.GetDirectories(pathRootSrc))
            {
                NeedUpdate(sub);
            }
            return needUpdate;
        }
    }
}
