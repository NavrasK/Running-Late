using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Question {
    public static Question current;
    public string q;
    public string a0;
    public string a1;
    public string a2;
    public string a3;
    public int ans;

    public Question(string q, string a0, string a1, string a2, string a3, int ans) {
        this.q = q;
        this.a0 = a0;
        this.a1 = a1;
        this.a2 = a2;
        this.a3 = a3;
        this.ans = ans;
    }

    public override string ToString() {
        return "\nQ: "+q+"\n0) "+a0+"\n1) "+a1+"\n2) "+a2+"\n3) "+a3+"\nAns: "+ans;
    }
}
