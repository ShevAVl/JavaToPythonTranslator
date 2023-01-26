using static JavaInPythonTranslator.Globals;

namespace JavaInPythonTranslator {
    internal class RepeatedInitializationsCheck {
        struct RInits {
            public string type;
            public string value;

            public RInits(string type, string value)
            {
                this.type = type;
                this.value = value;
            }
        }

        static bool interCheck(List<TreeNode> treeNodes, RInits checkedElement) {
            bool trigger = false;

            for (int i = 0; i < treeNodes.Count; i++) {

                if (String.Equals(treeNodes[i].lexem.value, checkedElement.type) && String.Equals(treeNodes[i + 1].lexem.value, checkedElement.value))
                    return true;
            }

            foreach (TreeNode treeNode in treeNodes) {
                if (treeNode.nextLevelNodes != null)
                {
                    trigger = interCheck(treeNode.nextLevelNodes, checkedElement);
                    if (trigger == true)
                        break;
                }
            }

            return trigger;
        }

        public static bool repeatedInitializations(List<TreeNode> treeNodes) {
            bool trigger = false;

            // Проход по строке
            for (int pos = 0; pos < treeNodes.Count; pos++) {
                // Если находим лексему-тип, то смотрим дальше
                if (treeNodes[pos].lexem.type[0] == 'T') {
                    // Проходимся по типам переменных в надежде найти существующий
                    foreach (LexicalClasses lexClass in letterClasses) {
                        if (String.Equals(treeNodes[pos].lexem.type, lexClass.getLexClass()) && (String.Equals(treeNodes[pos + 1].lexem.type, identificator))) {
                            RInits newElem = new RInits(treeNodes[pos].lexem.value, treeNodes[pos + 1].lexem.value);

                            foreach (TreeNode node in treeNodes) {
                                if (node.nextLevelNodes != null) {
                                    return interCheck(node.nextLevelNodes, newElem);
                                }
                            }
                        }
                    }
                }
            }

            foreach (TreeNode treeNode in treeNodes) {
                if (treeNode.nextLevelNodes != null) {
                    return repeatedInitializations(treeNode.nextLevelNodes);
                }
            }

            return trigger;
        }

    }
}
