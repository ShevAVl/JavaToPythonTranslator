using static JavaInPythonTranslator.SyntaxGlobals;

namespace JavaInPythonTranslator
{
    static internal class AssignmentRules
    {
        #region <присваивание> → <начало идентификатора> <оператор присваивания> <выражение>
        public static string assignmentCheck(List<LexList> lexems, List<TreeNode> treeNodes)
        {
            string check;

            //Проверка на наличие идентификатора
            check = EndPoints.IdentificatorCheck(lexems);
            if (String.Equals(check, successMessage))
                treeNodes.Add(new TreeNode(lexems[pos], null));
            if (!String.Equals(check, successMessage))
                return check;
            pos++;

            //Проверка на наличие оператора присваивания
            check = EndPoints.AssignmentOperatorCheck(lexems);
            if (String.Equals(check, successMessage))
                treeNodes.Add(new TreeNode(lexems[pos], null));
            if (!String.Equals(check, successMessage))
            {
                return check;
            }
            pos++;

            //Проверка на наличие выражения после оператора присваивания
            List<TreeNode> treeNode1 = new();
            treeNodes.Add(new TreeNode(new LexList(NewTree, NewTree), treeNode1));
            check = ExpressionRules.expressionCheck(lexems, treeNode1);
            if (!String.Equals(check, successMessage))
            {
                return check;
            }
            return successMessage;
        }
        #endregion
    }
}
