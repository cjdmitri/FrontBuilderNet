using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrontBuilderNet
{
    public static class Project
    {
        public const string DIR_NAME_SOURCE = "src";
        public const string DIR_NAME_TARGET = "dist";
        public const string DIR_NAME_PARTIAL = "partials";
        public const string FILE_NAME_CONFIG = "frontbuilder.json";

        /// <summary>
        /// Название проекта
        /// </summary>
        public static string Name { get; set; }
        /// <summary>
        /// Полный путь к проекту
        /// </summary>
        public static string Path { get; set; }
    }
}
