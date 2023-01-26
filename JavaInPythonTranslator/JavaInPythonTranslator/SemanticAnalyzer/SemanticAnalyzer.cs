using static JavaInPythonTranslator.Globals;

namespace JavaInPythonTranslator {
    /// <summary>
    /// Точка входа в семантический анализатор
    /// </summary>
    internal class SemanticAnalyzer {
        public static bool runSemanticScan(List<TreeNode> treeNodes) {
            if (RepeatedInitializationsCheck.repeatedInitializations(treeNodes)) {
                Console.WriteLine("Ошибка: повторное объявление переменной");
                return true;
            }

            if (IsInitializedIdentificatorsUsedCheck.checkMain(treeNodes)) {
                Console.WriteLine("Ошибка: не объявленная переменная");
                return true;
            }
            return false;
        }
    }
}
