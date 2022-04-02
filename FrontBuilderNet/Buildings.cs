﻿using System;
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
        /// <summary>
        /// Построить проект
        /// </summary>
        public static void Build()
        {
            string projectPath = Project.Path;
            Stopwatch sw = new Stopwatch();
            sw.Start();
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
                    if (outputFileContent.Contains($"<partial {partialName}/>"))
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
            var options = new JsonSerializerOptions
            {
                WriteIndented = false,
                Encoder = JavaScriptEncoder.Create(UnicodeRanges.BasicLatin, UnicodeRanges.Cyrillic)
            };
            JsonNode variables = settings!["variables"]![pageFileName]!;
            if (variables != null)
            {
                foreach (JsonObject variable in variables.AsArray())
                {
                    Dictionary<string, JsonNode> jsonString = variable.ToDictionary(x => x.Key, x => x.Value);
                    string key = jsonString.Keys.FirstOrDefault();
                    string value = variable[key]!.GetValue<string>();
                    inputText = inputText.Replace($"<variable {key}/>", value);
                }
            }
            return inputText;
        }
    }
}
