namespace JavaInPythonTranslator {
    internal class Utils {
        public static List<String> getInputText(String pathToFile) {
            List<String> inputText = new();

            try {
                string inputPath = "../../../../Input/";
                StreamReader lexClasses = new(inputPath + pathToFile);
                while (!lexClasses.EndOfStream)
                    inputText.Add(lexClasses.ReadLine());
            }
            catch {
                inputText.Add(" ");
                inputText[0] = ("Некорректный путь до файла");


                return inputText;
            }

            return inputText;
        }
        public static void treeRun(List<TreeNode> treeNodes) {
            foreach (TreeNode treeNode in treeNodes) {
                Console.Write(treeNode.lexem.value + " ");
                if (String.Equals(treeNode.lexem.value, ";"))
                    Console.Write(" ");
            }

            foreach (TreeNode treeNode in treeNodes) {
                if (treeNode.nextLevelNodes != null) {
                    Console.WriteLine();
                    treeRun(treeNode.nextLevelNodes);
                }
            }
        }

        public static void lexRun(List<LexList> lexems) {
            foreach (LexList lexem in lexems)
                Console.WriteLine(lexem.type + " " + lexem.value);

            Console.WriteLine();
        }

        public static void inpRun(List<String> lines) {
            foreach (String line in lines)
                Console.WriteLine(line);

            Console.WriteLine();
        }
    }
}
