
namespace JavaInPythonTranslator {
    class Program {
        public static int Main(String[] args) {

            // Чтение из файла

            Console.WriteLine("Введите имя текстового файла с кодом для трансляции");

            string inputFile = Console.ReadLine();

            List<String> inputText = Utils.getInputText(inputFile);

            if (String.Equals("Некорректный путь до файла", inputText[0])) 
                return 1;

            if (Globals.logVerboseLevel >= 1)
                Utils.inpRun(inputText);


            // Лексический анализ

            List<LexList> lexList = new();

            if (!LexicalAnalyzer.initLexAnalyzer()) {
                Console.WriteLine("Ошибка при заполнении лексических классов");
                return 2;
            }
            if (!LexicalAnalyzer.runLexScan(lexList, inputText)) {
                Console.WriteLine("Ошибка при анализе входного текста");
                return 3;
            }
            if (Globals.logVerboseLevel >= 1)
                Utils.lexRun(lexList);


            // Синтаксический анализ

            List<TreeNode> treeNodes = new();
            string syntaxResult = SyntaxAnalyzer.startRule(lexList, treeNodes);
            if (syntaxResult != SyntaxGlobals.successMessage) {
                if (Globals.logVerboseLevel >= 1)
                    Console.WriteLine(syntaxResult);
                    return 4;
            }

            if (Globals.logVerboseLevel >= 1)
                Utils.treeRun(treeNodes);


            // Семантический анализ

            if (SemanticAnalyzer.runSemanticScan(treeNodes))
                return 5;


            // Генератор кода

            string outputPath = "../../../../Output/";
            System.IO.Directory.CreateDirectory(outputPath);
            using (StreamWriter file = new StreamWriter(outputPath + System.IO.Path.ChangeExtension(inputFile, ".py"))) {
                CodeGenerator.Generate(file, treeNodes);
                CodeGenerator.addMainFunctionCall(file);
                Console.WriteLine("Сгенерированный файл расположен в " + System.IO.Path.GetFullPath(outputPath));
            }
            
            Console.Read();
            return 0;
        }
    }
}