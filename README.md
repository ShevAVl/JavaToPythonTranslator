# JavaToPythonTranslator
A C# console app featuring a java->python translator with lexical, syntax and semantic analyzers.  

## Features:
<ul>
  <li>
    Lexical analyzer
  </li>
  <li>
    Syntax analyzer
  </li>
  <li>
    Semantic analyzer
  </li>
  <li>
    Code generator
  </li>
</ul>

**Analysis method:** Recursive descent (нисходящий с возвратами)

## How to use:
The repo contains a full set of files for a VS 2022 project (using net 6.0). Additionally, it contains a compiled build with an .exe file in bin/Debug/net6.0. The program uses 2 directories for input/output purposes: *Input* for input text files (java code) and *Output* for output .py files.  
<ul>
  <li>
    For the VS version, the program uses Input/Output folders located in the project root directory (the same where bin folder is)
  </li>
  <li>
    .exe executable searches for Input/Output folders in the same directory where it is located
  </li>
</ul>

**Примечание:** проект был разработан около двух лет назад в составе другой студенческой группы в рамках схожей дисциплины. Мой вклад в проект включал:
<ul>
  <li>
    участие в составлении проекта программной системы (требования, таблицы соответствия лексем, диаграмма автомата)
  </li>
  <li>
    кодирование правил грамматики
  </li>
  <li>
    блока функций Utils блока функций Utils
  </li>
  <li>
    тестирование
  </li>
</ul>
