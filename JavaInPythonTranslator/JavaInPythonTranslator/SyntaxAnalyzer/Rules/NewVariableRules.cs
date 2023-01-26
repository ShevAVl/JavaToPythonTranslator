using static JavaInPythonTranslator.SyntaxGlobals;

namespace JavaInPythonTranslator
{
    internal class NewVariableRules
    {
        #region <объявление переменной> → <тип данных переменной> <имя или инициализация>
        public static string variableDeclarationCheck(List<LexList> lexems, List<TreeNode> treeNodes)
        {
            string check;

            //Проверка на тип
            check = EndPoints.dataTypeCheck(lexems);
            if (String.Equals(check, successMessage))
                treeNodes.Add(new TreeNode(lexems[pos], null));
            if (!String.Equals(check, successMessage))
                return check;
            pos++;

            //Проверка содержимого после объявления типа
            check = nameAndRealizationCheck(lexems, treeNodes);
            if (!String.Equals(check, successMessage))
                return check;

            return successMessage;
        }
        #endregion

        #region <имя или инициализация> → <начало идентификатора>; | <начало идентификатора> = <значение>;
        static string nameAndRealizationCheck(List<LexList> lexems, List<TreeNode> treeNodes)
        {
            string check;

            //Проверяем, что это - идентификатор
            check = EndPoints.IdentificatorCheck(lexems);
            if (String.Equals(check, successMessage))
                treeNodes.Add(new TreeNode(lexems[pos], null));
            if (!String.Equals(check, successMessage))
                return check;
            pos++;

            //Если точка с запятой, то возвращаемся
            check = compare(lexems[pos].type, D3);
            if (String.Equals(check, successMessage))
                treeNodes.Add(new TreeNode(lexems[pos - 1], null));
            if (String.Equals(check, successMessage))
                return check;

            //Проверяем, что следует равенство
            check = compare(lexems[pos].type, P1);
            if (String.Equals(check, successMessage))
                treeNodes.Add(new TreeNode(lexems[pos - 1], null));
            if (!String.Equals(check, successMessage))
                return check;

            //Проверяем, что следует значение 
            check = EndPoints.ValueCheck(lexems);
            if (String.Equals(check, successMessage))
                treeNodes.Add(new TreeNode(lexems[pos], null));
            if (!String.Equals(check, successMessage))
                return check;
            pos++;

            //Проверяем, что следует точка с запятой
            check = compare(lexems[pos].type, D3);
            if (String.Equals(check, successMessage))
                treeNodes.Add(new TreeNode(lexems[pos - 1], null));
            if (!String.Equals(check, successMessage))
                return check;

            return successMessage;
        }
        #endregion
    }
}
