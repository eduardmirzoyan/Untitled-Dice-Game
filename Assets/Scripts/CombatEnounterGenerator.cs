using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class CombatEnounterGenerator : ScriptableObject
{
    public List<Unit> commonEnemies;
    public List<Unit> bigEnemies;
    public float changeForBigEnemy = 0.25f;

    public Party GenerateEnemyEncounter() {
        Party party = ScriptableObject.CreateInstance<Party>();

        bool includeBig = Random.Range(0, 1) <= changeForBigEnemy;

        if (includeBig && bigEnemies.Count > 0) {
            // Get random large enemy
            var randomBig = bigEnemies[Random.Range(0, bigEnemies.Count)];
            // Add a copy
            party.Set(Instantiate(randomBig), 0);
        }

        // Fill the rest of the party with random enemies
        for (int i = 0; i < party.maxSize; i++) {
            if (party[i] == null) {
                // Get a copy of a random common enemy
                var randomCommon = Instantiate(commonEnemies[Random.Range(0, commonEnemies.Count)]);
                // Make copy of its own die
                randomCommon.dice = Instantiate(randomCommon.dice);
                party.Set(randomCommon, i);
            }
        }

        // Return the party as the encounter
        return party;
    }
}
