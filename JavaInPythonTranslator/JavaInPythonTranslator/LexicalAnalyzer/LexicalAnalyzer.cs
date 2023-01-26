using System.Text.RegularExpressions;
using static JavaInPythonTranslator.Globals;

namespace JavaInPythonTranslator {
    internal static class LexicalAnalyzer {
        private readonly static String defaultPath = "./LexicalClasses";


        ///<summary> Файлы из папки LexicalClasses содержат список регулярных выражений, определяющих класс объекта </summary>
        public static bool initLexAnalyzer() {
            try {
                // Заполнение лексических классов, начинающихся с буквы
                StreamReader lexClasses = new(defaultPath + "/letterClasses.txt");
                String[]? lexicalClassesLinear = lexClasses.ReadToEnd().Replace("\n", "~").Replace("\r", "").Split("~");

                for (int i = 0; i < lexicalClassesLinear.Length; i += 2) {
                    letterClasses.Add(new LexicalClasses(lexicalClassesLinear[i], lexicalClassesLinear[i + 1]));
                }

                // Заполнение лексических классов, являющихся операторами
                lexClasses = new(defaultPath + "/operatorClasses.txt");
                lexicalClassesLinear = lexClasses.ReadToEnd().Replace("\n", "~").Replace("\r", "").Split("~");

                for (int i = 0; i < lexicalClassesLinear.Length; i += 2) {
                    operatorClasses.Add(new LexicalClasses(lexicalClassesLinear[i], lexicalClassesLinear[i + 1]));
                }

                // Заполнение лексических классов, являющихся разделителями
                lexClasses = new(defaultPath + "/dividerClasses.txt");
                lexicalClassesLinear = lexClasses.ReadToEnd().Replace("\n", "~").Replace("\r", "").Split("~");

                for (int i = 0; i < lexicalClassesLinear.Length; i += 2) {
                    dividerClasses.Add(new LexicalClasses(lexicalClassesLinear[i], lexicalClassesLinear[i + 1]));
                }

                if (Globals.logVerboseLevel == 404)
                    for (int i = 0; i < operatorClasses.Count; i += 1) {
                        Console.WriteLine(operatorClasses[i].getLexClass() + " " + operatorClasses[i].getRegEx());
                    }

                return true;
            }
            catch {
                Console.WriteLine("Классы лексического анализатора не были найдены.");
            }
            return false;
        }

        static int row = 0;
        static int column = 0;

        ///<summary> Функция - точка входа в лексический анализ входного текста </summary>
        public static bool runLexScan(List<LexList> lexList, List<String> inputFile) {
            while (row < inputFile.Count) {
                while (column < inputFile[row].Length) {
                    if (letterStart(inputFile, lexList))
                    { }
                    else
                    if (operatorStart(inputFile, lexList))
                    { }
                    else
                    if (numberStart(inputFile, lexList))
                    { }
                    else
                    if (dividerStart(inputFile, lexList))
                    { }
                    else
                    if (spaceStart(inputFile, lexList))
                    { }
                    else
                        return false;

                    column++;
                }

                column = 0;
                row++;
            }
            return true;
        }

        ///<summary> Функция анализа лексических классов, значения которых начинаются с буквы </summary>
        public static bool letterStart(List<String> inputFile, List<LexList> lexList) {
            String word = "" + inputFile[row][column];

            if (Regex.IsMatch(word, @"[a-zA-Z]")) {
                column++;

                // Основной цикл прохода по каждой букве и цифре в "слове"
                while (column < inputFile[row].Length) {
                    if (Regex.IsMatch("" + inputFile[row][column], @"[A-Za-z0-9]")) {
                        word += inputFile[row][column];
                    }
                    else
                    if (column < inputFile[row].Length && String.Equals("" + inputFile[row][column], ".")) {
                        word += inputFile[row][column];
                    }
                    else
                        break;

                    column++;
                }
                column--;

                // Проверка на зарезервированность слова
                for (int i = 0; i < letterClasses.Count; i++) {
                    if (String.Equals(letterClasses[i].getRegEx(), word)) {
                        lexList.Add(new LexList(letterClasses[i].getLexClass(), word));
                        return true;
                    }
                }

                lexList.Add(new LexList(identificator, word));
                return true;
            }

            return false;
        }

        ///<summary> Функция анализа лексических классов, значения которых начинаются с оператора </summary>
        public static bool operatorStart(List<String> inputFile, List<LexList> lexList) {
            String word = "" + inputFile[row][column];

            column++;
            // Ищем унарные операторы типа ++, +=
            if (column < inputFile[row].Length)
                for (int j = 0; j < operatorClasses.Count; j++) {
                    if (String.Equals(word + inputFile[row][column], operatorClasses[j].getRegEx())) {
                        word += inputFile[row][column];
                        lexList.Add(new LexList(operatorClasses[j].getLexClass(), word));
                        return true;
                    }
                }
            column--;

            // Ищем совпадения с операторами типа +
            for (int i = 0; i < operatorClasses.Count; i++) {
                if (String.Equals(word, operatorClasses[i].getRegEx())) {
                    lexList.Add(new LexList(operatorClasses[i].getLexClass(), word));
                    return true;
                }
            }

            return false;
        }

        ///<summary> Функция анализа лексических классов, значения которых начинаются с цифры </summary>
        public static bool numberStart(List<String> inputFile, List<LexList> lexList) {
            String word = "" + inputFile[row][column];
            bool isFractional = false;
            String numberType = wholeNumber;

            // Ищем целое число
            if (Regex.IsMatch(word, @"[0-9]")) {
                column++;

                // Проверка на то, что число не типа 0 (цифра)
                if (String.Equals(word, "0")) {
                    if (column < inputFile[row].Length && Regex.IsMatch("" + inputFile[row][column], @"[0-9]"))
                    {
                        lexList.Add(new LexList("E1", word));
                        return false;
                    }
                }

                // Основной цикл прохода по каждой букве и цифре в "слове"
                while (column < inputFile[row].Length) {
                    if (Regex.IsMatch("" + inputFile[row][column], @"[0-9]")) {
                        word += inputFile[row][column];
                    }
                    else
                    if (String.Equals("" + inputFile[row][column], ".")) {
                        if (!isFractional) {
                            numberType = fractionaNumber;
                            word += inputFile[row][column];
                            isFractional = true;
                        }
                        else {
                            lexList.Add(new LexList("E1", word));
                            return false;
                        }
                    }
                    else
                        break;

                    column++;
                }
                column--;

                lexList.Add(new LexList(numberType, word));
                return true;
            }

            return false;
        }

        ///<summary> Функция анализа лексических классов, значения которых начинаются с разделителя из языка Java </summary>
        public static bool dividerStart(List<String> inputFile, List<LexList> lexList) {
            String word = "" + inputFile[row][column];

            // Ищем совпадения с разделителями
            for (int i = 0; i < dividerClasses.Count; i++) {
                if (String.Equals(word, dividerClasses[i].getRegEx())) {
                    lexList.Add(new LexList(dividerClasses[i].getLexClass(), word));
                    return true;
                }
            }

            return false;
        }

        ///<summary> 
            /// Функция анализа пробелов, либо исключающая их, либо добавляющая в список в зависимости от глобальной переменной lexSpaces 
        ///</summary>
        public static bool spaceStart(List<String> inputFile, List<LexList> lexList) {
            String word = "" + inputFile[row][column];

            // Если пробел, то к следующему числу
            if (String.Equals(word, " ")) {
                if (column + 1 < inputFile[row].Length) {
                    while (String.Equals("" + inputFile[row][column], " ")) {
                        column++;
                    }
                    column--;
                }

                if (Globals.lexSpaces == true)
                    lexList.Add(new LexList("SP", word));

                return true;
            }
            return false;
        }
    }
}
