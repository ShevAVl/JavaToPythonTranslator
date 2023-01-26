using static JavaInPythonTranslator.Globals;
using static JavaInPythonTranslator.SyntaxGlobals;

namespace JavaInPythonTranslator
{
    internal class EndPoints
    {
        #region <тип данных> → T1 | T2 | T3 | T4 | T5 | T6 | T7 | T8
        public static string dataTypeCheck(List<LexList> lexems)
        {
            bool trigger = false;

            if (lexems[pos].type[0] == 'T')
                foreach (LexicalClasses classes in letterClasses)
                {
                    if (classes.getLexClass() == lexems[pos].type)
                        trigger = true;
                }

            if (trigger)
                return successMessage;

            return "Ошибка: \"Ожидался тип данных\"";
        }
        #endregion

        #region <оператор присваивания> → P1 | P2 | P3 | P4 | P5 | P6
        public static string AssignmentOperatorCheck(List<LexList> lexems)
        {
            bool trigger = false;

            if (lexems[pos].type[0] == 'P')
                foreach (LexicalClasses classes in operatorClasses)
                {
                    if (classes.getLexClass() == lexems[pos].type)
                        trigger = true;
                }

            if (trigger)
                return successMessage;

            return "Ошибка: \"Ожидался оператор присваивания\"";
        }
        #endregion

        #region <арифметический оператор> → B1 | B2 | B3 | B4 | B5
        public static string ArithmeticalOperatorCheck(List<LexList> lexems)
        {
            bool trigger = false;

            if (lexems[pos].type[0] == 'B')
                foreach (LexicalClasses classes in operatorClasses)
                {
                    if (classes.getLexClass() == lexems[pos].type)
                        trigger = true;
                }

            if (trigger)
                return successMessage;

            return "Ошибка: \"Ожидался арифметический оператор\"";
        }
        #endregion

        #region <унарный арифметический оператор> → U1 | U2
        public static string UnaryOperatorCheck(List<LexList> lexems)
        {
            bool trigger = false;

            if (lexems[pos].type[0] == 'U')
                foreach (LexicalClasses classes in operatorClasses)
                {
                    if (classes.getLexClass() == lexems[pos].type)
                        trigger = true;
                }

            if (trigger)
                return successMessage;

            return "Ошибка: \"Ожидался унарный оператор\"";
        }
        #endregion

        #region <знак числа> → B1 | B2
        public static string SignOperatorCheck(List<LexList> lexems)
        {
            bool trigger = false;

            if (lexems[pos].type[0] == 'B')
                foreach (LexicalClasses classes in operatorClasses)
                {
                    if (classes.getLexClass() == lexems[pos].type)
                        trigger = true;
                }

            if (trigger)
                return successMessage;

            return "Ошибка: \"Ожидался знак числа\"";
        }
        #endregion

        #region <оператор сравнения> → S1 | S2 | S3 | S4 | S5 | S6
        public static string ComparisonOperatorCheck(List<LexList> lexems)
        {
            bool trigger = false;

            if (lexems[pos].type[0] == 'S')
                foreach (LexicalClasses classes in operatorClasses)
                {
                    if (classes.getLexClass() == lexems[pos].type)
                        trigger = true;
                }

            if (trigger)
                return successMessage;

            return "Ошибка: \"Ожидался оператор сравнения\"";
        }
        #endregion

        #region <логический бинарный оператор> → L1 | L2
        public static string LogicalBinaryOperatorCheck(List<LexList> lexems)
        {
            bool trigger = false;

            if (lexems[pos].type[0] == 'L')
                foreach (LexicalClasses classes in operatorClasses)
                {
                    if (classes.getLexClass() == lexems[pos].type)
                        trigger = true;
                }

            if (trigger)
                return successMessage;

            return "Ошибка: \"Ожидался бинарный логический оператор\"";
        }
        #endregion

        #region <логическое значение> → A1 | A2
        public static string LogicalValueCheck(List<LexList> lexems)
        {
            bool trigger = false;

            if (lexems[pos].type[0] == 'A')
                foreach (LexicalClasses classes in letterClasses)
                {
                    if (classes.getLexClass() == lexems[pos].type)
                        trigger = true;
                }

            if (trigger)
                return successMessage;

            return "Ошибка: \"Ожидалось логическое значение\"";
        }
        #endregion

        #region <число> → NN | NP
        public static string NumberValueCheck(List<LexList> lexems)
        {
            bool trigger = false;

            if (String.Equals(lexems[pos].type, wholeNumber) || String.Equals(lexems[pos].type, fractionaNumber))
            {
                trigger = true;
            }

            if (trigger)
                return successMessage;

            return "Ошибка: \"Ожидалось численное значение\"";
        }
        #endregion

        #region <идентификатор> → ID
        public static string IdentificatorCheck(List<LexList> lexems)
        {
            bool trigger = false;

            if (String.Equals(lexems[pos].type, identificator))
            {
                trigger = true;
            }

            if (trigger)
                return successMessage;

            return "Ошибка: \"Ожидался идентификатор\"";
        }
        #endregion

        #region <символьное значение> → NC
        public static string CharValueCheck(List<LexList> lexems)
        {
            bool trigger = false;

            if (String.Equals(lexems[pos].type, charNumber))
            {
                trigger = true;
            }

            if (trigger)
                return successMessage;

            return "Ошибка: \"Ожидалось символьное значение\"";
        }
        #endregion

        #region <строковое значение> → NS
        public static string StringValueCheck(List<LexList> lexems)
        {
            bool trigger = false;

            if (String.Equals(lexems[pos].type, stringNumber))
            {
                trigger = true;
            }

            if (trigger)
                return successMessage;

            return "Ошибка: \"Ожидалось строковое значение\"";
        }
        #endregion

        #region <значение> → <число> | <строковое значение> | <символьное значение> | <логическое значение>
        //Пока-что только числа
        public static string ValueCheck(List<LexList> lexems)
        {
            string check = NumberValueCheck(lexems);
            if (!String.Equals(check, successMessage))
                return successMessage;

            check = CharValueCheck(lexems);
            if (!String.Equals(check, successMessage))
                return successMessage;

            check = StringValueCheck(lexems);
            if (!String.Equals(check, successMessage))
                return successMessage;

            check = LogicalValueCheck(lexems);
            if (!String.Equals(check, successMessage))
                return successMessage;

            return check;
        }
        #endregion
    }
}
