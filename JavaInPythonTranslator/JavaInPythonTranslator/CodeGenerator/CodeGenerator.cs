namespace JavaInPythonTranslator {
    /*
         * ----------------------------------------------------------------------------------------
         * ----------------------------------Генератор кода----------------------------------------
         * 
         *      Генератор кода переводит синтаксически корректную программу на Java в программу на 
         * Python. Дерево, построенное синтаксическим анализатором используется, чтобы перевести
         * входную программу на Java в программу на Python при помощи таблицы соотвествия.
         * 
         * ----------------------------------------------------------------------------------------
         * 
         * Есть нода дерева TreeNode в Globals
         * И есть корень дерева List<TreeNode>
         * Каждый из этих листов либо имеет NULL потомок и содержит лексему, либо имеет потомок List<TreeNode> и не имеет лексему
         * Задача прохода - идти по каждому из списков слева направо, и если находишь потомка, то спускаться вниз, 
         * а потом возвращаться наверх, когда кончатся все элементы
         *
         * ----------------------------------------------------------------------------------------
         * 
         * Допустим,
         * Для public static void main() {
         * int a = 5 + 5 * 5;
         * int b = 6 + 6 * 6;
         * System.out.println(a + b);
         * b = 0;
         * }
         * Дерево будет выглядеть как (здесь :N - номер потомка, на самом деле они непосредственно связаны)
         * :0->public static void main() { :1 }
         * :1->int a = 5 + 5 * 5; :2
         * :2->int b = 6 + 6 * 6;
         * :3->System.out.println( :4 ); :5
         * :4->a + b
         * :5->b = 0 
         * 
         * 
         * */

    internal static class CodeGenerator {
        // стек для работы с "(" и ")"
        // если прямо сейчас обрабатываются параметры функции, то в стек спушится "("
        // если скобка ")" закрылась (закончили обрабатываться параметры), то из стека удаляется "("
        static Stack<string> inParametersStack = new Stack<string>(); 

        // функция проверяет, есть ли лексема с "(" и ")", то есть проверка на то, что
        // текущие лексемы принадлежат параметрам функции/команды.
        static void inParametersCheck(TreeNode treeNode) { 
            switch (treeNode.lexem.type) {

                // "("
                case "D6":
                    inParametersStack.Push("(");
                    break;

                // ")"
                case "D7":
                    inParametersStack.Pop();
                    break;
            }
        }

        // стек для работы с "{" и "}"
        static Stack<string> inBodyStack = new Stack<string>();

        // Отступ строки (для функций)
        static int intOffset = 0;

        static string stringOffset() {
            string offset = "";
            for (int i = 1; i <= intOffset; i++) offset += "\t";
            return offset;
        }

        // Множество операторов, для которых в правой части могут быть ссылки на поддеревья синт. дерева
        // Обычно для новых поддеревьев пишется новая строка в выходном файле, но здесь это не требуется,
        // поэтому выделили след. мн-во исключений для перевода строки
        static string[] operators = { "P1", "P2", "P3", "P4", "P5", "P6", "S1", "S2", "S3", "S4",
         "S5", "S6", "L1", "L2", "U1", "U2","B1", "B2", "B3", "B4", "B5" };

        // Главная функция генератора кода
        public static void Generate(StreamWriter file, List<TreeNode> treeNodes) {
            
            for (int i = 0; i < treeNodes.Count; i++) {
                inParametersCheck(treeNodes[i]);
                if (treeNodes[i].nextLevelNodes != null) {
                    // Новая строка
                    // Если сейчас в лексемах - параметры функции, то отмена
                    // Если лексемы принадлежат множеству исключений operators, то отмена
                    if (!inParametersStack.Contains("(") && !(operators.Contains(treeNodes[i-1].lexem.type))) {
                        file.WriteLine();
                        file.Write(stringOffset());
                    }

                    Generate(file, treeNodes[i].nextLevelNodes);
                }
                file.Write(String.Equals(treeNodes[i].lexem.value, "NewTree") ? "" : Translate(treeNodes, ref i, treeNodes.Count));
            }
        }

        public static void addMainFunctionCall(StreamWriter file) {
            file.WriteLine("\nif __name__ == '__main__':");
            file.WriteLine("\tmain()");
        }

        static List<TreeNode> loopBody = new List<TreeNode>();

        // Трансляция Java в Python
        // В параметрах - список узлов и индекс обрабатываемой ноды, т.к., возможно, потребуется доступ к содержимому других нод
        // поэтому работать с объектом treeNode не вариант
        static string Translate(List<TreeNode> treeNodes, ref int i, int size) {
           
            switch (treeNodes[i].lexem.type) {
                
                case "K1": // import
                    if ((i + 1 < size) && (treeNodes[i + 1].lexem.value == "java.lang.Math"))
                    {
                        //если import math, то отбросить
                        i++;
                        return "";
                    }
                    return treeNodes[i].lexem.value + " ";
                
                case "K2": // public
                    // проверка на главную функцию public static void main(String [] args)
                    if ((i + 3 < size) && (treeNodes[i + 3].lexem.type == "K7")) {
                        intOffset++;
                        inBodyStack.Push("{");
                        i += 9;
                        return "def main()";
                    }
                    return treeNodes[i].lexem.value + " ";
                
                case "K9": // while
                    // проверяем, что это цикл "while(){}"
                    if ((i + 4 < size) && (treeNodes[i + 4].lexem.type == "D4")) {
                        inBodyStack.Push("{");
                        inParametersStack.Push("(");
                        intOffset++;
                        return treeNodes[i].lexem.value;
                    }
                    // цикл "do{}while()"
                    // здесь тело цикла уже было записано 1 раз.
                    // теперь надо записать while(){}
                    i += 2;
                    List<TreeNode> whileCondition = treeNodes[i].nextLevelNodes;
                    String whileConditionString = "";
                    for (int j = 0; j < whileCondition.Count; j++)
                        whileConditionString += Translate(whileCondition, ref j, whileCondition.Count);
                    i++;
                    inBodyStack.Push("{");
                    intOffset++;

                    String newLoopBodyString = "";
                    translatePart(loopBody, ref newLoopBodyString);
                    String whileBodyString = stringOffset() + newLoopBodyString;
                    intOffset--;
                    return "while(" + whileConditionString + "):\n" + whileBodyString;
               
                case "K10": // do
                    // транслируем "do{}while() в {}while(){}"
                    loopBody = treeNodes[i + 2].nextLevelNodes;
                    String loopBodyString = "";

                    // читаем тело цикла
                    // функция представляет собой чутка измененный Generate()
                    translatePart(loopBody, ref loopBodyString);
                    i += 3;
                    // возвращаем тело цикла {}. Далее из основного Generate будет вызван Translate к while
                    return loopBodyString;

                case "K11": // for
                    // Да, хардкод, ибо синтаксический анализ позволяет :P

                    String offset = stringOffset();
                    intOffset++;
                    inBodyStack.Push("{");
                    inParametersStack.Push("(");
                    // Аргументы цикла for (first; second; third)
                    List<TreeNode> first = treeNodes[i + 2].nextLevelNodes;
                    List<TreeNode> second = treeNodes[i + 4].nextLevelNodes;
                    List<TreeNode> third = treeNodes[i + 6].nextLevelNodes;
                    
                    // Начальное условие, его надо отдельно записать, ибо Python...
                    String firstString = first[0].lexem.value + 
                        first[1].lexem.value +
                        first[2].nextLevelNodes[0].lexem.value;

                    // итератор, т.к. по грамматике может быть здесь выражение. надо обработать
                    String thirdString = "";
                    for (int j = 0; j < third.Count; j++)
                        thirdString += Translate(third, ref j, third.Count);
                    
                    i += 6;
                    return firstString + "\n" + offset + "for " + first[0].lexem.value + " in range(" +
                        first[2].nextLevelNodes[0].lexem.value + ", " +
                        second[2].lexem.value + ", " +
                        thirdString + ")";
                
                case "K12": // if
                    inBodyStack.Push("{");
                    inParametersStack.Push("(");
                    intOffset++;
                    return treeNodes[i].lexem.value;

                case "K13": // else
                    inBodyStack.Push("{");
                    intOffset++;
                    return treeNodes[i].lexem.value;
 
                case "K14": // class
                    if ((i + 1 < size) && (treeNodes[i + 1].lexem.type == "K6")) {
                        // если это class Main, то отбросить
                        // в стек "{" не добавляем
                        i++;
                        return "";
                    }
                    
                    // если класс транслируется, то запушить в стек "{"
                    inBodyStack.Push("{");
                    return treeNodes[i].lexem.value + " ";

                case "T1": // boolean
                    return typeTranslate(treeNodes, ref i, size, "bool");
       
                case "T2": // byte
                    return typeTranslate(treeNodes, ref i, size, "bytes");
      
                case "T3": // short
                    return typeTranslate(treeNodes, ref i, size, "int");

                case "T4": // int
                    return typeTranslate(treeNodes, ref i, size, "int");

                case "T5": // float
                    return typeTranslate(treeNodes, ref i, size, "float");

                case "T6": // double
                    return typeTranslate(treeNodes, ref i, size, "double");

                case "T7": // char
                    return typeTranslate(treeNodes, ref i, size, "str");

                
                case "T8": // string
                    return typeTranslate(treeNodes, ref i, size, "str");

                case "A1": // true
                    return "True ";

                case "A2": // false
                    return "False ";
   
                case "D3": // ;
                    // отбрасываем
                    return "";

                case "D4": // {
                    if (!inBodyStack.Contains("{"))
                        // если скобка "{" принадлежит функции, не нужной для трансляции, то отбрасывается
                        return "";

                    // если скобка "{" указывает на тело транслируемой функции, то ":"
                    return ":";

                case "D5": // }
                    if (!inBodyStack.Contains("{"))
                        // если скобка "}" принадлежит функции ,не нужной для трансляции, то отбрасывается
                        return "";

                    // если скобка "}" указывает на тело транслируемой функции, то убрать скобку из стека
                    intOffset--;
                    inBodyStack.Pop();
                    return "";

                case "D6": // (
                    if (!inParametersStack.Contains("("))
                        // если скобка "(" принадлежит функции ,не нужной для трансляции, то отбрасывается
                        return "";
                    return treeNodes[i].lexem.value;

                case "D7": // (
                    if (!inParametersStack.Contains("("))
                        // если скобка ")" принадлежит функции ,не нужной для трансляции, то отбрасывается
                        return "";
                    inParametersStack.Pop();
                    return treeNodes[i].lexem.value;

                case "U1": // ++
                    //В Python нет "++", поэтому заменим на скобку - ++a -> (a + 1)
                    i++;
                    return "(" + treeNodes[i].lexem.value + " + 1)";
                
                case "U2": // --
                    i++;
                    return "(" + treeNodes[i].lexem.value + " - 1)";

                case "U3": // !
                    i++;
                    String notString = "";
                    notString = Translate(treeNodes, ref i, treeNodes.Count);
                    return "(not " + notString + ") ";
       
                case "L1":  // ||
                    return "or ";

                case "L2": // !
                    return "and ";


                case "ID": // ID
                    // проверка на функцию вывода в консоль
                    if ((String.Equals(treeNodes[i].lexem.value, "System.out.println")) ||
                     (String.Equals(treeNodes[i].lexem.value, "System.out.print"))) {
                        inParametersStack.Push("(");
                        return "print";
                    }
                    // проверка на операторы "<ID> ++" и "<ID> --"
                    if (i + 1 < size) {
                        if (treeNodes[i + 1].lexem.type == "U1") {
                            i++;
                            return "(" + treeNodes[i-1].lexem.value + " + 1)";
                        }
                        if (treeNodes[i + 1].lexem.type == "U2") {
                            i++;
                            return "(" + treeNodes[i - 1].lexem.value + " - 1)";
                        }
                    }
                    return treeNodes[i].lexem.value + " ";

                default:
                    return treeNodes[i].lexem.value + " ";
            }
        }

        // Трансляция типов
        static string typeTranslate(List<TreeNode> treeNodes, ref int i, int size, String type) {
            // проверка на функцию
            // например, boolean func (...){...}
            if ((i + 2 < size) && (treeNodes[i + 2].lexem.type == "D6")) { // "("

                intOffset++;
                inBodyStack.Push("{");
                
                i += 2;
                return "def " + treeNodes[i + 1].lexem.value;
            }
            // проверка на параметры func(boolean a, ...)
            if (inParametersStack.Contains("("))
                return type + " ";
            // объявление переменной boolean a = 5
            return "";
        }

        static void translatePart(List<TreeNode> body, ref String result) {
            for (int j = 0; j < body.Count; j++) {
                inParametersCheck(body[j]);
                if (body[j].nextLevelNodes != null) {
                    if (!inParametersStack.Contains("(") && !(operators.Contains(body[j - 1].lexem.type)))
                    {
                        result += "\n" + stringOffset();
                    }

                    translatePart(body[j].nextLevelNodes, ref result);
                }
                result += (String.Equals(body[j].lexem.value, "NewTree") ? "" : Translate(body, ref j, body.Count));
            }
        }
    }
}
