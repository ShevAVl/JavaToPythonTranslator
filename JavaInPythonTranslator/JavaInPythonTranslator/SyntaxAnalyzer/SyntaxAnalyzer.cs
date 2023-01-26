using static JavaInPythonTranslator.BlockOfCodeRules;
using static JavaInPythonTranslator.Globals;
using static JavaInPythonTranslator.SyntaxGlobals;


namespace JavaInPythonTranslator {
    static class SyntaxAnalyzer {
        #region <программа> → <подключение пакетов> | <объявление класса> 
        static public string startRule(List<LexList> lexems, List<TreeNode> treeNodes) {
            if (String.Equals(lexems[pos].type, importClass))
                return importCheck(lexems, treeNodes);
            else
                return classCheck(lexems, treeNodes);
        }
        #endregion

        #region <подключение пакетов> → K1 <идентификатор> <подключение пакетов> | <объявление класса> 
        static string importCheck(List<LexList> lexems, List<TreeNode> treeNodes) {
            string check;

            check = compare(lexems[pos].type, importClass);
            if (String.Equals(check, successMessage))
                treeNodes.Add(new TreeNode(lexems[pos - 1], null));
            if (!String.Equals(check, successMessage))
                return check;

            check = compare(lexems[pos].type, identificator);
            if (String.Equals(check, successMessage))
                treeNodes.Add(new TreeNode(lexems[pos - 1], null));
            if (!String.Equals(check, successMessage))
                return check;

            check = compare(lexems[pos].type, D3);
            if (String.Equals(check, successMessage))
                treeNodes.Add(new TreeNode(lexems[pos - 1], null));
            if (!String.Equals(check, successMessage))
                return check;

            List<TreeNode> treeNode1 = new();
            treeNodes.Add(new TreeNode(new LexList(NewTree, NewTree), treeNode1));
            if (String.Equals(lexems[pos].type, importClass))
            {
                return importCheck(lexems, treeNode1);
            }
            else
            {
                return classCheck(lexems, treeNode1);
            }
        }
        #endregion

        #region Правило <объявление класса> → class Main {<главная функция> <тело класса>} | class Main {<главная функция>}
        static string classCheck(List<LexList> lexems, List<TreeNode> treeNodes) {
            string check;

            check = compare(lexems[pos].type, classClass);
            if (String.Equals(check, successMessage))
                treeNodes.Add(new TreeNode(lexems[pos - 1], null));
            if (!String.Equals(check, successMessage))
                return check;

            check = compare(lexems[pos].type, classMainClass);
            if (String.Equals(check, successMessage))
                treeNodes.Add(new TreeNode(lexems[pos - 1], null));
            if (!String.Equals(check, successMessage))
                return check;

            check = compare(lexems[pos].type, D4);
            if (String.Equals(check, successMessage))
                treeNodes.Add(new TreeNode(lexems[pos - 1], null));
            if (!String.Equals(check, successMessage))
                return check;

            List<TreeNode> treeNode1 = new();
            treeNodes.Add(new TreeNode(new LexList(NewTree, NewTree), treeNode1));
            check = voidmainCheck(lexems, treeNode1);
            if (!String.Equals(check, successMessage))
                return check;

            List<TreeNode> treeNode2 = new();
            treeNodes.Add(new TreeNode(new LexList(NewTree, NewTree), treeNode2));
            check = bodyclassCheck(lexems, treeNode2);
            if (!String.Equals(check, "NULL") && !String.Equals(check, successMessage))
                return check;

            check = compare(lexems[pos].type, D5);
            if (String.Equals(check, successMessage))
                treeNodes.Add(new TreeNode(lexems[pos - 1], null));
            if (!String.Equals(check, successMessage))
                return check;

            return successMessage;
        }
        #endregion

        #region <главная функция> → public static void main (String[] args) { <блок кода> }
        static string voidmainCheck(List<LexList> lexems, List<TreeNode> treeNodes) {
            string check;

            //public
            check = compare(lexems[pos].type, publicClass);
            if (String.Equals(check, successMessage))
                treeNodes.Add(new TreeNode(lexems[pos - 1], null));
            if (!String.Equals(check, successMessage))
                return check;

            //static
            check = compare(lexems[pos].type, staticClass);
            if (String.Equals(check, successMessage))
                treeNodes.Add(new TreeNode(lexems[pos - 1], null));
            if (!String.Equals(check, successMessage))
                return check;

            //void
            check = compare(lexems[pos].type, voidClass);
            if (String.Equals(check, successMessage))
                treeNodes.Add(new TreeNode(lexems[pos - 1], null));
            if (!String.Equals(check, successMessage))
                return check;

            //main
            check = compare(lexems[pos].type, funcMainClass);
            if (String.Equals(check, successMessage))
                treeNodes.Add(new TreeNode(lexems[pos - 1], null));
            if (!String.Equals(check, successMessage))
                return check;

            //(
            check = compare(lexems[pos].type, D6);
            if (String.Equals(check, successMessage))
                treeNodes.Add(new TreeNode(lexems[pos - 1], null));
            if (!String.Equals(check, successMessage))
                return check;

            //String
            check = compare(lexems[pos].type, stringClass);
            if (String.Equals(check, successMessage))
                treeNodes.Add(new TreeNode(lexems[pos - 1], null));
            if (!String.Equals(check, successMessage))
                return check;

            //[
            check = compare(lexems[pos].type, D8);
            if (String.Equals(check, successMessage))
                treeNodes.Add(new TreeNode(lexems[pos - 1], null));
            if (!String.Equals(check, successMessage))
                return check;

            //]
            check = compare(lexems[pos].type, D9);
            if (String.Equals(check, successMessage))
                treeNodes.Add(new TreeNode(lexems[pos - 1], null));
            if (!String.Equals(check, successMessage))
                return check;

            //args
            check = lexems[pos].value;
            if (String.Equals(check, "args")) {
                pos++;
                treeNodes.Add(new TreeNode(lexems[pos - 1], null));
            }
            else
                return "Ошибка: \"Ожидалось \"args\"\"";

            //)
            check = compare(lexems[pos].type, D7);
            if (String.Equals(check, successMessage))
                treeNodes.Add(new TreeNode(lexems[pos - 1], null));
            if (!String.Equals(check, successMessage))
                return check;

            //{
            check = compare(lexems[pos].type, D4);
            if (String.Equals(check, successMessage))
                treeNodes.Add(new TreeNode(lexems[pos - 1], null));
            if (!String.Equals(check, successMessage))
                return check;

            //блок кода
            List<TreeNode> treeNode1 = new();
            treeNodes.Add(new TreeNode(new LexList(NewTree, NewTree), treeNode1));
            check = blockOfCodeCheck(lexems, treeNode1);
            if (!String.Equals(check, successMessage) && !String.Equals(check, "NULL"))
            {
                return check;
            }

            //}
            check = compare(lexems[pos].type, D5);
            if (String.Equals(check, successMessage))
                treeNodes.Add(new TreeNode(lexems[pos - 1], null));
            if (!String.Equals(check, successMessage))
                return check;

            return successMessage;
        }
        #endregion

        #region Правило <тело класса> → <объявление переменной> <тело класса> | <объявление переменной> | <объявление функции> <тело класса> | <объявление функции> | <объявление константы> <тело класса> | <объявление константы> 
        static string bodyclassCheck(List<LexList> lexems, List<TreeNode> treeNodes) {
            return successMessage;
        }
        #endregion

    }
}
