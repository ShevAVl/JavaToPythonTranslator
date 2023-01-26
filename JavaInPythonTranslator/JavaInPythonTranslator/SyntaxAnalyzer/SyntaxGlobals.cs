namespace JavaInPythonTranslator {
    internal class SyntaxGlobals {
        public static string compare(string check, string lexClass) {
            if (String.Equals(check, lexClass)) {
                pos++;
            }
            else
                return "Ошибка: ожидалось " + lexClass;

            return successMessage;
        }

        // Текущая позиция в списке
        public static int pos = 0;

        // Сообщение об успешности проведения синт. анализа
        public static string successMessage = "success";

        // Коды классов лексем
        public static string importClass = "K1";
        public static string publicClass = "K2";
        public static string privateClass = "K3";
        public static string protectedClass = "K4";
        public static string staticClass = "K5";
        public static string classMainClass = "K6";
        public static string funcMainClass = "K7";
        public static string breakClass = "K8";
        public static string whileClass = "K9";
        public static string doClass = "K10";
        public static string forClass = "K11";
        public static string ifClass = "K12";
        public static string elseClass = "K13";
        public static string classClass = "K14";
        public static string voidClass = "K15";
        public static string returnClass = "K16";

        public static string U3 = "U3"; // !

        public static string D1 = "D1"; // .
        public static string D2 = "D2"; // ,
        public static string D3 = "D3"; // ;
        public static string D4 = "D4"; // {
        public static string D5 = "D5"; // }
        public static string D6 = "D6"; // (
        public static string D7 = "D7"; // )
        public static string D8 = "D8"; // [
        public static string D9 = "D9"; // ]

        public static string Q1 = "Q1"; // '
        public static string Q2 = "Q2"; // "

        public static string stringClass = "T8"; // Строковый тип, используемый в объявлении главной функции
        public static string P1 = "P1"; //=

        public static string NewTree = "exp"; // Текстовая заглушка для содержимого языковой конструкции
    }
}
