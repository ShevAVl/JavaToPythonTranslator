using static JavaInPythonTranslator.SyntaxGlobals;

namespace JavaInPythonTranslator
{
    static internal class IfLoopRules
    {
        #region <цикл> → <while-цикл> | <do-while-цикл> | <for-цикл>
        public static string loopCheck(List<LexList> lexems, List<TreeNode> treeNodes)
        {
            string check;
            int startPos = pos;

            treeNodes.Clear();
            //Проверка начала арифметического выражения
            check = forCheck(lexems, treeNodes);
            if (String.Equals(check, successMessage))
                return check;

            treeNodes.Clear();
            pos = startPos;
            //Проверка начала логического выражения
            check = whileCheck(lexems, treeNodes);
            if (String.Equals(check, successMessage))
                return check;

            treeNodes.Clear();
            pos = startPos;
            //Проверка начала логического выражения
            check = doWhileCheck(lexems, treeNodes);
            if (String.Equals(check, successMessage))
                return check;

            return "Ожидался цикл";
        }
        #endregion

        #region <for-цикл> -> for (<инструкция>; <логическое выражение>; <инструкция>) {<тело цикла>}
        static string forCheck(List<LexList> lexems, List<TreeNode> treeNodes)
        {
            string check;

            //Проверяем, что начинаем с for
            check = compare(lexems[pos].type, forClass);
            if (String.Equals(check, successMessage))
                treeNodes.Add(new TreeNode(lexems[pos - 1], null));
            if (!String.Equals(check, successMessage))
                return check;

            //Проверяем открывающую скобку (
            check = compare(lexems[pos].type, D6);
            if (String.Equals(check, successMessage))
                treeNodes.Add(new TreeNode(lexems[pos - 1], null));
            if (!String.Equals(check, successMessage))
                return check;

            int startPos = pos;
            //Проверка на "объявление переменной"
            List<TreeNode> treeNode1 = new List<TreeNode>();
            treeNodes.Add(new TreeNode(new LexList(NewTree, NewTree), treeNode1));
            check = NewVariableRules.variableDeclarationCheck(lexems, treeNode1);
            if (!String.Equals(check, successMessage))
            {
                treeNode1.Clear();
                pos = startPos;

                //Проверка на операцию присваивания
                check = AssignmentRules.assignmentCheck(lexems, treeNode1);
                if (!String.Equals(check, successMessage))
                {
                    return check;
                }
            }
            //Проверяем на наличие ;
            check = compare(lexems[pos].type, D3);
            if (String.Equals(check, successMessage))
                treeNodes.Add(new TreeNode(lexems[pos - 1], null));
            if (!String.Equals(check, successMessage))
                return check;

            //Проверка на наличие логического условия
            List<TreeNode> treeNode2 = new List<TreeNode>();
            treeNodes.Add(new TreeNode(new LexList(NewTree, NewTree), treeNode2));
            check = ExpressionRules.logicalCheck(lexems, treeNode2);
            if (!String.Equals(check, successMessage))
                return check;


            //Проверяем на наличие ;
            check = compare(lexems[pos].type, D3);
            if (String.Equals(check, successMessage))
                treeNodes.Add(new TreeNode(lexems[pos - 1], null));
            if (!String.Equals(check, successMessage))
                return check;

            //Проверка на наличие логического условия
            List<TreeNode> treeNode4 = new List<TreeNode>();
            treeNodes.Add(new TreeNode(new LexList(NewTree, NewTree), treeNode4));
            check = ExpressionRules.arithmeticalCheck(lexems, treeNode4);
            if (!String.Equals(check, successMessage))
                return check;

            //Проверяем закрывающую скобку )
            check = compare(lexems[pos].type, D7);
            if (String.Equals(check, successMessage))
                treeNodes.Add(new TreeNode(lexems[pos - 1], null));
            if (!String.Equals(check, successMessage))
                return check;

            //Проверяем открывающую скобку {
            check = compare(lexems[pos].type, D4);
            if (String.Equals(check, successMessage))
                treeNodes.Add(new TreeNode(lexems[pos - 1], null));
            if (!String.Equals(check, successMessage))
                return check;

            //Проверка на наличие блока кода
            List<TreeNode> treeNode3 = new List<TreeNode>();
            treeNodes.Add(new TreeNode(new LexList(NewTree, NewTree), treeNode3));
            check = BlockOfCodeRules.blockOfCodeCheck(lexems, treeNode3);
            if (!String.Equals(check, successMessage))
                return check;

            //Проверяем закрывающую скобку }
            check = compare(lexems[pos].type, D5);
            if (String.Equals(check, successMessage))
                treeNodes.Add(new TreeNode(lexems[pos - 1], null));
            if (!String.Equals(check, successMessage))
                return check;

            return successMessage;
        }
        #endregion

        #region <while-цикл> -> while (<логическое выражение>) {<тело цикла>}
        static string whileCheck(List<LexList> lexems, List<TreeNode> treeNodes)
        {
            string check;

            //Проверяем, что начинаем с while
            check = compare(lexems[pos].type, whileClass);
            if (String.Equals(check, successMessage))
                treeNodes.Add(new TreeNode(lexems[pos - 1], null));
            if (!String.Equals(check, successMessage))
                return check;

            //Проверяем открывающую скобку (
            check = compare(lexems[pos].type, D6);
            if (String.Equals(check, successMessage))
                treeNodes.Add(new TreeNode(lexems[pos - 1], null));
            if (!String.Equals(check, successMessage))
                return check;

            //Проверка на наличие логического условия
            List<TreeNode> treeNode1 = new List<TreeNode>();
            treeNodes.Add(new TreeNode(new LexList(NewTree, NewTree), treeNode1));
            check = ExpressionRules.logicalCheck(lexems, treeNode1);
            if (!String.Equals(check, successMessage))
                return check;

            //Проверяем закрывающую скобку )
            check = compare(lexems[pos].type, D7);
            if (String.Equals(check, successMessage))
                treeNodes.Add(new TreeNode(lexems[pos - 1], null));
            if (!String.Equals(check, successMessage))
                return check;

            //Проверяем открывающую скобку {
            check = compare(lexems[pos].type, D4);
            if (String.Equals(check, successMessage))
                treeNodes.Add(new TreeNode(lexems[pos - 1], null));
            if (!String.Equals(check, successMessage))
                return check;

            //Проверка на наличие блока кода
            List<TreeNode> treeNode2 = new List<TreeNode>();
            treeNodes.Add(new TreeNode(new LexList(NewTree, NewTree), treeNode2));
            check = BlockOfCodeRules.blockOfCodeCheck(lexems, treeNode2);
            if (!String.Equals(check, successMessage))
                return check;

            //Проверяем закрывающую скобку }
            check = compare(lexems[pos].type, D5);
            if (String.Equals(check, successMessage))
                treeNodes.Add(new TreeNode(lexems[pos - 1], null));
            if (!String.Equals(check, successMessage))
                return check;

            return successMessage;
        }
        #endregion

        #region <do-while-цикл> -> do {<тело цикла>} while (<логическое выражение>);
        static string doWhileCheck(List<LexList> lexems, List<TreeNode> treeNodes)
        {
            string check;

            //Проверяем, что начинаем с do
            check = compare(lexems[pos].type, doClass);
            if (String.Equals(check, successMessage))
                treeNodes.Add(new TreeNode(lexems[pos - 1], null));
            if (!String.Equals(check, successMessage))
                return check;

            //Проверяем открывающую скобку {
            check = compare(lexems[pos].type, D4);
            if (String.Equals(check, successMessage))
                treeNodes.Add(new TreeNode(lexems[pos - 1], null));
            if (!String.Equals(check, successMessage))
                return check;

            //Проверка на наличие блока кода
            List<TreeNode> treeNode2 = new List<TreeNode>();
            treeNodes.Add(new TreeNode(new LexList(NewTree, NewTree), treeNode2));
            check = BlockOfCodeRules.blockOfCodeCheck(lexems, treeNode2);
            if (!String.Equals(check, successMessage))
                return check;

            //Проверяем закрывающую скобку }
            check = compare(lexems[pos].type, D5);
            if (String.Equals(check, successMessage))
                treeNodes.Add(new TreeNode(lexems[pos - 1], null));
            if (!String.Equals(check, successMessage))
                return check;

            //Проверяем, что продолжаем с while
            check = compare(lexems[pos].type, whileClass);
            if (String.Equals(check, successMessage))
                treeNodes.Add(new TreeNode(lexems[pos - 1], null));
            if (!String.Equals(check, successMessage))
                return check;

            //Проверяем открывающую скобку (
            check = compare(lexems[pos].type, D6);
            if (String.Equals(check, successMessage))
                treeNodes.Add(new TreeNode(lexems[pos - 1], null));
            if (!String.Equals(check, successMessage))
                return check;

            //Проверка на наличие логического условия
            List<TreeNode> treeNode1 = new List<TreeNode>();
            treeNodes.Add(new TreeNode(new LexList(NewTree, NewTree), treeNode1));
            check = ExpressionRules.logicalCheck(lexems, treeNode1);
            if (!String.Equals(check, successMessage))
                return check;

            //Проверяем закрывающую скобку )
            check = compare(lexems[pos].type, D7);
            if (String.Equals(check, successMessage))
                treeNodes.Add(new TreeNode(lexems[pos - 1], null));
            if (!String.Equals(check, successMessage))
                return check;

            //Проверяем на наличие ;
            check = compare(lexems[pos].type, D3);
            if (String.Equals(check, successMessage))
                treeNodes.Add(new TreeNode(lexems[pos - 1], null));
            if (!String.Equals(check, successMessage))
                return check;

            return successMessage;
        }
        #endregion

        #region <ветвление> → if (<логическое выражение>) {<блок кода>} | if (<логическое выражение>) {<блок кода>} <иначе-ветвление>
        public static string ifCheck(List<LexList> lexems, List<TreeNode> treeNodes)
        {
            string check;

            //Проверяем, что начинаем с if
            check = compare(lexems[pos].type, ifClass);
            if (String.Equals(check, successMessage))
                treeNodes.Add(new TreeNode(lexems[pos - 1], null));
            if (!String.Equals(check, successMessage))
                return check;

            //Проверяем открывающую скобку (
            check = compare(lexems[pos].type, D6);
            if (String.Equals(check, successMessage))
                treeNodes.Add(new TreeNode(lexems[pos - 1], null));
            if (!String.Equals(check, successMessage))
                return check;

            //Проверка на наличие логического условия
            List<TreeNode> treeNode1 = new List<TreeNode>();
            treeNodes.Add(new TreeNode(new LexList(NewTree, NewTree), treeNode1));
            check = ExpressionRules.logicalCheck(lexems, treeNode1);
            if (!String.Equals(check, successMessage))
                return check;
            pos++;

            //Проверяем закрывающую скобку )
            check = compare(lexems[pos].type, D7);
            if (String.Equals(check, successMessage))
                treeNodes.Add(new TreeNode(lexems[pos - 1], null));
            if (!String.Equals(check, successMessage))
                return check;

            //Проверяем открывающую скобку {
            check = compare(lexems[pos].type, D4);
            if (String.Equals(check, successMessage))
                treeNodes.Add(new TreeNode(lexems[pos - 1], null));
            if (!String.Equals(check, successMessage))
                return check;

            //Проверка на наличие блока кода
            List<TreeNode> treeNode2 = new List<TreeNode>();
            treeNodes.Add(new TreeNode(new LexList(NewTree, NewTree), treeNode2));
            check = BlockOfCodeRules.blockOfCodeCheck(lexems, treeNode2);
            if (!String.Equals(check, successMessage))
                return check;

            //Проверяем закрывающую скобку }
            check = compare(lexems[pos].type, D5);
            if (String.Equals(check, successMessage))
                treeNodes.Add(new TreeNode(lexems[pos - 1], null));
            if (!String.Equals(check, successMessage))
                return check;

            //Проверка наличия else-блока
            int startPos = pos;
            List<TreeNode> treeNode3 = new List<TreeNode>();
            treeNodes.Add(new TreeNode(new LexList(NewTree, NewTree), treeNode3));
            check = elseCheck(lexems, treeNode3);
            if (!String.Equals(check, successMessage))
            {
                pos = startPos;
                treeNodes.RemoveAt(treeNodes.Count - 1);
                return successMessage;
            }

            return successMessage;

            //Проверяем else-часть ветвления

        }
        #endregion

        #region <иначе-ветвление> → else {<блок кода>}
        public static string elseCheck(List<LexList> lexems, List<TreeNode> treeNodes)
        {
            string check;

            //Проверяем наличие else
            check = compare(lexems[pos].type, elseClass);
            if (String.Equals(check, successMessage))
                treeNodes.Add(new TreeNode(lexems[pos - 1], null));
            if (!String.Equals(check, successMessage))
                return check;

            //Проверяем открывающую скобку {
            check = compare(lexems[pos].type, D4);
            if (String.Equals(check, successMessage))
                treeNodes.Add(new TreeNode(lexems[pos - 1], null));
            if (!String.Equals(check, successMessage))
                return check;

            //Проверка на наличие блока кода
            List<TreeNode> treeNode1 = new List<TreeNode>();
            treeNodes.Add(new TreeNode(new LexList(NewTree, NewTree), treeNode1));
            check = BlockOfCodeRules.blockOfCodeCheck(lexems, treeNode1);
            if (!String.Equals(check, successMessage))
                return check;

            //Проверяем закрывающую скобку }
            check = compare(lexems[pos].type, D5);
            if (String.Equals(check, successMessage))
                treeNodes.Add(new TreeNode(lexems[pos - 1], null));
            if (!String.Equals(check, successMessage))
                return check;

            return successMessage;
        }
        #endregion
    }
}
