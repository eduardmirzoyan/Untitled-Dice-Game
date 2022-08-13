using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Party")]
public class Party : ScriptableObject
{
        public Unit[] partyMembers;

        public Unit this[int index] {
            get {
                return partyMembers[index];
            }
            set {
                partyMembers[index] = value;
            }
        }

    public Unit[] GetMembers() {
        return partyMembers;
    }
}
