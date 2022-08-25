using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Code_Analysis
{
    public struct SyntaxError
    {
        public int code;
        public string what;
        public int pos;

        public SyntaxError(int code, string what, int pos) 
        {
            this.code = code;
            this.what = what;
            this.pos = pos;
        }
    }

    public class SynAnaliz
    {
        List<Leksem> leksems;
        List<SyntaxError> syntaxErrors;
        int current;
        int exp_level;
        string sourceString;

        public SynAnaliz(List<Leksem> leksems, string sourceString)
        {
            this.leksems = leksems;
            this.sourceString = sourceString;
            current = 0;
            syntaxErrors = new List<SyntaxError>();
        }

        private bool isNotEnd()
        {
            return (current < leksems.Count) && (leksems[current].type != TypeLeks.end_string);
        }

        public List<SyntaxError> Exec()
        {
            current = 0;
            exp_level = 0;
            Expr();
            return syntaxErrors;
        }

        private void Expr() 
        {
            exp_level++;

            T();

            while (true)
            {
                List<Leksem> badLeksems = new List<Leksem>();

                // ищем терм или and, попутно ругаясь на все встречающиеся символы
                while (isNotEnd() && leksems[current].type != TypeLeks.and && leksems[current].type != TypeLeks.ident &&
                        leksems[current].type != TypeLeks.number && leksems[current].type != TypeLeks.l_skobka
                        && leksems[current].type != TypeLeks.r_skobka)
                {
                    //syntaxErrors.Add(new SyntaxError(1, $"Неверный символ {leksems[current].value}", leksems[current].start));
                    badLeksems.Add(leksems[current]);
                    current++;
                }

                int i = 0;

                if (isNotEnd())
                {
                    if (leksems[current].type == TypeLeks.r_skobka)
                    {
                        if (exp_level > 1)
                        { 
                            break;
                        }
                        else
                        {
                            syntaxErrors.Add(new SyntaxError(1, $"Лишняя )", leksems[current].start));
                            current++;
                            continue;
                        }
                    }

                    

                    if (leksems[current].type == TypeLeks.and)
                    {
                        current++;
                    }
                    else
                    {
                        if (badLeksems.Count != 0)
                        {
                            syntaxErrors.Add(new SyntaxError(1, "Вместо " + badLeksems[0].value + " ожидался and", badLeksems[0].start));
                            i++;
                        }
                        else
                        {
                            syntaxErrors.Add(new SyntaxError(1, "Не хватает ключевого слова and перед выражением", leksems[current].start));
                        }

                    }

                    for (; i < badLeksems.Count; i++)
                    {
                        syntaxErrors.Add(new SyntaxError(1, $"Неверный символ {badLeksems[i].value}", badLeksems[i].start));
                    }

                    T();

                }
                else
                {
                    for (; i < badLeksems.Count; i++)
                    {
                        syntaxErrors.Add(new SyntaxError(1, $"Неверный символ {badLeksems[i].value}", badLeksems[i].start));
                    }
                    break;
                }
            }

            exp_level--;
        }


        private bool findSecondOperand()
        {
            bool result = false;
            List<Leksem> badLeksems = new List<Leksem>();

            if (exp_level > 1)
            {
                while (isNotEnd() && leksems[current].type != TypeLeks.and && leksems[current].type != TypeLeks.ident 
                        && leksems[current].type != TypeLeks.number && leksems[current].type != TypeLeks.l_skobka 
                        && leksems[current].type != TypeLeks.r_skobka)
                {
                    badLeksems.Add(leksems[current]);
                    current++;
                }

            }
            else
            {
                while (isNotEnd() && leksems[current].type != TypeLeks.and && leksems[current].type != TypeLeks.ident
                        && leksems[current].type != TypeLeks.number && leksems[current].type != TypeLeks.l_skobka)
                {
                    badLeksems.Add(leksems[current]);
                    current++;
                }

            }


            int i = 0;

            if (leksems[current].type != TypeLeks.ident && leksems[current].type != TypeLeks.number)
            {
                if (badLeksems.Count != 0)
                {
                    syntaxErrors.Add(new SyntaxError(1, "Вместо " + badLeksems[0].value + " ожидался операнд", badLeksems[0].start));
                    i++;
                }
                else
                {
                    syntaxErrors.Add(new SyntaxError(1, "Отсутсвует операнд", leksems[current].start));
                }
            }
            else
            {
                current++;
                result = true;
            }

            for (; i < badLeksems.Count; i++)
            {
                syntaxErrors.Add(new SyntaxError(1, $"Неверный символ {badLeksems[i].value}", badLeksems[i].start));
            }

            return result;
        }


        private void T()
        {
            List<Leksem> badLeksems = new List<Leksem>();

            while (isNotEnd() && leksems[current].type != TypeLeks.and && leksems[current].type != TypeLeks.eqvivalent
                   && leksems[current].type != TypeLeks.ident && leksems[current].type != TypeLeks.number
                   && leksems[current].type != TypeLeks.l_skobka)
            {
                if (leksems[current].type == TypeLeks.r_skobka && exp_level > 1)
                {
                    break;
                }
                badLeksems.Add(leksems[current]);
                current++;
            }

            if (leksems[current].type == TypeLeks.l_skobka)
            {  
                for (int i = 0; i < badLeksems.Count; i++)
                    syntaxErrors.Add(new SyntaxError(1, $"Неверный символ {badLeksems[i].value}", badLeksems[i].start));
                current++;
                Expr();
                if (isNotEnd() && leksems[current].type == TypeLeks.r_skobka)
                {
                    current++;
                }
                else
                {
                    syntaxErrors.Add(new SyntaxError(1, $"Не хватает )", leksems[current].start));
                }

                return;
            }


            if (leksems[current].type == TypeLeks.ident || leksems[current].type == TypeLeks.number)
            {
                current++;
                for (int i = 0; i < badLeksems.Count; i++)
                    syntaxErrors.Add(new SyntaxError(1, $"Неверный символ {badLeksems[i].value}", badLeksems[i].start));
            }
            else
            {
                if (badLeksems.Count != 0)
                {
                    syntaxErrors.Add(new SyntaxError(1, "Вместо " + badLeksems[0].value + " ожидался операнд", badLeksems[0].start));
                    for (int i = 1; i < badLeksems.Count; i++)
                        syntaxErrors.Add(new SyntaxError(1, $"Неверный символ {badLeksems[i].value}", badLeksems[i].start));
                }
                else
                {
                    syntaxErrors.Add(new SyntaxError(1, "Отсутсвует операнд", leksems[current].start));
                    for (int i = 0; i < badLeksems.Count; i++)
                        syntaxErrors.Add(new SyntaxError(1, $"Неверный символ {badLeksems[i].value}", badLeksems[i].start));
                }
            }


            // ищем ==
            while (isNotEnd() && leksems[current].type != TypeLeks.and && leksems[current].type != TypeLeks.eqvivalent
                   && leksems[current].type != TypeLeks.r_skobka && leksems[current].type != TypeLeks.ident
                   && leksems[current].type != TypeLeks.number && leksems[current].type != TypeLeks.l_skobka)
            {
                syntaxErrors.Add(new SyntaxError(1, $"Неверный символ {leksems[current].value}", leksems[current].start));
                current++;
            }

            if (isNotEnd())
            {
                if (leksems[current].type == TypeLeks.eqvivalent)
                {
                    current++;
                    // продолжаем разбор после ==
                    findSecondOperand();
                }
                else
                {

                }
            }
            else
            {
               
            }
        }

        private void O()
        {
            
        }


    }
}
