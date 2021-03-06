<h1>FrontBuilderNet</h1>
Проект для сборки .html файлов из частей. Консольное приложение, написанное на .Net 6.
<h3>Доступные команды</h3>
<ul>
  <li><strong>create</strong> - создание нового проекта</li>
  <li><strong>build</strong> - построить ранее открытый проект</li>
  <li><strong>open</strong> - открыть существующий проект</li>
  <li><strong>bundle</strong> - Сборка CSS, JS файлов</li>
  <li><strong>watch</strong> - следить за изменениями в файлах. При обнаружении пересобирать проект</li>
</ul>
<h3>Как работает</h3>
<p>По команде <strong>create</strong> создается новый проект. В указанный каталог копируются файлы из каталога <strong>template</strong>, в котором присутствуют начальные файлы и каталоги</p>
<p>Страницы .html помещаются в корень папки <strong>src</strong></p>
<p>Частичные файлы .html находятся в <strong>src/partials</strong></p>
<p>Для того что бы добавить частичное представление в страницу, необходимо добавить <strong><code>&lt;partial footer.html/></code></strong></p>
<p>Далее необходимо выпонить команду <strong>build</strong> или <strong>watch</strong></p>
<p>Все собранные страницы помещаются в пупку <strong>dist</strong></p>
<h3>Настройки frontbuilder.json</h3>
<p>Для каждой страницы можно добавить определенные переменные. Для этого в файлах .html необходимо вставить следующую конструкцию: <strong><code>&lt;variable title/></code></strong> Далее необходимо указать значение переменных в файле <strong>frontbuilder.json</strong> в секции  <strong>variables</strong>. Пример:</p>
<pre>
<code>
{
    "variables": {
        "index.html": [
            { "title": "Главная страница" },
            { "description": "Описание главной страницы" }
        ],
        "about.html": [
            { "title": "О сайте" },
            { "description": "Страница описания проекта" }
        ]
    },
    "bundle": [
        {
            "outputfile": "dist/css/bundle.css",
            "inputfiles": [
                "src/css/core.css",
                "src/css/site.css"
            ]
        },
        {
            "outputfile": "dist/js/bundle.js",
            "inputfiles": [
                "src/js/site.js"
            ]
        }
    ]
}
</code>
</pre>
<p>Для каждой страницы есть свой массив переменных, значения которых будут заменены при построении проекта</p>
<h3>История изменений</h3>
<h4>Версия: 0.1.2022.2</h4>
<ul>
  <li>Добавлена команда: <strong>bundle</strong> - собирает стили и скрипты. Файлы для сборки указываются в <strong>frontbuilder.json</strong></li>
  <li>Добавлена команда: <strong>watch</strong> - следит за изменениями в файлах. При обнаружении пересобирает проект</li>
  <li>Изменён шаблон проекта</li>
  <li>Сохранение последнего открытого проекта, при завершении работы</li>
</ul>
<h4>Версия: 0.1.2022.1</h4>
<ul>
  <li>Добавлен стартовый шаблон в проект</li>
  <li>Сборка страниц из <strong>partials</strong></li>
  <li>Замена переменных для каждой страницы</li>
</ul>
