using static JavaInPythonTranslator.Globals;

namespace JavaInPythonTranslator {
    internal class IsInitializedIdentificatorsUsedCheck {
        struct RInits {
            public string type;
            public string value;

            public RInits(string type, string value) {
                this.type = type;
                this.value = value;
            }
        }

        struct IDChecked {
            public string value;
            public List<TreeNode> whereIsChecked;

            public IDChecked(string value, List<TreeNode> whereIsChecked) {
                this.value=value;
                this.whereIsChecked=whereIsChecked;
            }
        }

        static List<IDChecked> idc = new();

        static void interCheck(List<TreeNode> treeNodes, RInits checkedElement) {

            for (int i = 0; i < treeNodes.Count; i++)
                if (String.Equals(treeNodes[i].lexem.value, checkedElement.value))
                    idc.Add(new IDChecked(checkedElement.value, treeNodes));

            foreach (TreeNode treeNode in treeNodes)
                if (treeNode.nextLevelNodes != null) 
                    interCheck(treeNode.nextLevelNodes, checkedElement);
        }


        public static void repeatedInitializations(List<TreeNode> treeNodes) {
            bool trigger = false;

            // Проход по строке
            for (int pos = 0; pos < treeNodes.Count; pos++) {
                // Если находим лексему-тип, то смотрим дальше
                if (treeNodes[pos].lexem.type[0] == 'T') {
                    // Проходимся по типам переменных в надежде найти существующий
                    foreach (LexicalClasses lexClass in letterClasses) {
                        if (String.Equals(treeNodes[pos].lexem.type, lexClass.getLexClass()) && 
                            (String.Equals(treeNodes[pos + 1].lexem.type, identificator))) {

                            RInits newElem = new RInits(treeNodes[pos].lexem.value, treeNodes[pos + 1].lexem.value);

                            foreach (TreeNode node in treeNodes)
                                if (node.nextLevelNodes != null)
                                    interCheck(node.nextLevelNodes, newElem);
                        }
                    }
                }
            }

            foreach (TreeNode treeNode in treeNodes) 
                if (treeNode.nextLevelNodes != null)
                    repeatedInitializations(treeNode.nextLevelNodes);
        }

        static bool check(List<TreeNode> treeNodes) {
            bool trigger = false;

            foreach (TreeNode treeNode in treeNodes) {
                foreach (IDChecked id in idc) {
                    if (treeNode.lexem.value == id.value) {
                        trigger = true;
                        if (treeNodes == id.whereIsChecked)
                            trigger = false;
                    }
                    if (trigger == true)
                        return true;
                }
            }

            foreach (TreeNode treeNode in treeNodes) {
                if (treeNode.nextLevelNodes != null) {
                    trigger = check(treeNode.nextLevelNodes);
                    if (trigger == true)
                        break;
                }
            }
            return trigger;
        }

        public static bool checkMain(List<TreeNode> treeNodes) {
            repeatedInitializations(treeNodes);
            check(treeNodes);
            return false;
        }
    }
}
