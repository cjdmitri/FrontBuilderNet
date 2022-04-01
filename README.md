<h1>FrontBuilderNet</h1>
Проект для сборки .html файлов из частей. Консольное приложение, написанное на .Net 6.
<h3>Доступные команды</h3>
<ul>
  <li><strong>create</strong> - создание нового проекта</li>
  <li><strong>build</strong> - построить ранее открытый проект</li>
  <li><strong>open</strong> - открыть существующий проект</li>
</ul>
<h3>Как работает</h3>
<p>По команде <strong>create</strong> создается новый проект. В указанный каталог копируются файлы из каталога <strong>template</strong>, в котором присутствуют начальные файлы и каталоги</p>
<p>Страницы .html помещаются в корень папки <strong>src</strong></p>
<p>Частичные файлы .html находятся в <strong>src/partials</strong></p>
<p>Для того что бы добавить частичное представление в страницу, необходимо добавить <strong><code>&lt;partial footer.html/></code></strong></p>
<p>Далее необходимо выпонить команду <strong>build</strong></p>
<p>Все собранные страницы помещаются в пупку <strong>dist</strong></p>
