using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Code_Analysis
{
    public enum TypeLeks 
    {
        ident,
        number,
        eqvivalent,
        and,
        r_skobka,
        l_skobka,
        end_string,
        error,
        badident
    }
    public struct Leksem 
    {
        public int start;
        public string value;
        public TypeLeks type;

        public Leksem(int start, string value, TypeLeks type)
        {
            this.start = start;
            this.value = value;
            this.type = type;
        }
    }
    public static class Scaner
    {
        public static Leksem scan(string s, int i) 
        {
            if (i >= s.Length) 
            {
                return new Leksem(i, "", TypeLeks.end_string);
            }
            while (s[i] == '\t' || s[i] == ' ' || s[i] == '\n') 
            {
                i++;
                if (i >= s.Length)
                    return new Leksem(i, "", TypeLeks.end_string);
            }

            if (Char.IsLetter(s[i])) 
            {
                int j = i;
                while (Char.IsLetter(s[j]) || Char.IsDigit(s[j])) 
                {
                    j++;
                    if (j >= s.Length)
                        break;
                }
                if (s.Substring(i, j-i) == "and") 
                {
                    return new Leksem(i, "and", TypeLeks.and);
                }
                return new Leksem(i, s.Substring(i, j - i), TypeLeks.ident);
            }

            if (Char.IsDigit(s[i]))
            {
                int j = i;
                while (Char.IsDigit(s[j]))
                {
                    j++;
                    if (j >= s.Length)
                        break;
                }
                if (j < s.Length && Char.IsLetter(s[j])) 
                {
                    while (Char.IsLetter(s[j]))
                    {
                        j++;
                        if (j >= s.Length)
                            break;
                    }

                    return new Leksem(i, s.Substring(i, j - i), TypeLeks.badident);

                }
                return new Leksem(i, s.Substring(i, j - i), TypeLeks.number);
            }
            if (s[i] == '(')
                return new Leksem(i, "(", TypeLeks.l_skobka);
            if (s[i] == ')')
                return new Leksem(i, ")", TypeLeks.r_skobka);
            if (s[i] == '=') 
            {
                if (i + 1 < s.Length)
                {
                    if (s[i+1] == '=') 
                    {
                        return new Leksem(i, "==", TypeLeks.eqvivalent);
                    }
                    else 
                    {
                        return new Leksem(i, s[i].ToString(), TypeLeks.error);
                    }
                }
                else 
                {
                    return new Leksem(i, s[i].ToString(), TypeLeks.error);
                }
            }
            return new Leksem(i, s[i].ToString(), TypeLeks.error);
        }
    }
}
