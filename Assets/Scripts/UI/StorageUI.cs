using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text.RegularExpressions;

public class StorageUI : MonoBehaviour
{
    // TODO
    public string testString;

    private void Update() {
        if (Input.GetKeyDown(KeyCode.N)) {
            Regex regex = new Regex("(([0-9])XB)");
            MatchCollection result = regex.Matches(testString);
            for (int i = 0; i < result.Count; i++)
            {
                print("Grp 2: " + result[i].Groups[2].Value);
            }
            //print(result[0].Groups.Count);
            //print("Grp 0: " + result[0].Groups[0].Value);
            //print("Grp 1: " + result[0].Groups[1].Value);
            //print("Grp 2: " + result[0].Groups[2].Value);
        }
    }
}
