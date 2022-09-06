using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class LootGenerator : ScriptableObject
{

    [Header("Weapon Generation Settings")]
    [SerializeField] private List<string> weaponNames;
    [SerializeField] private List<string> weaponFlavorTexts;
    [SerializeField] private List<Sprite> weaponSprites;
    [SerializeField] private Vector2Int rangeOfBaseDamage = new Vector2Int(4, 6);
    [SerializeField] private List<Action> basicActionsToChooseFrom;
    [SerializeField] private List<Action> specialActionsToChooseFrom;
    [SerializeField] private Vector2Int rangeOfSpecialsPerWeapon = new Vector2Int(1, 2);


    [Header("Armor Generation Settings")]
    [SerializeField] private List<string> armorNames;
    [SerializeField] private List<string> armorFlavorTexts;
    [SerializeField] private List<Sprite> armorSprites;
    [SerializeField] private List<Passive> passivesToChooseFrom;
    [SerializeField] private (int, int) rangeOfPassivesPerWeapon = (1, 1);

    public Weapon GenerateWeapon() {
        // Create empty weapon
        Weapon newWeapon = ScriptableObject.CreateInstance<Weapon>();

        // Generate random name
        newWeapon.name = weaponNames[Random.Range(0, weaponNames.Count)];

        // Generate random description
        newWeapon.description = weaponFlavorTexts[Random.Range(0, weaponFlavorTexts.Count)];

        // Generate random sprite
        newWeapon.sprite = weaponSprites[Random.Range(0, weaponSprites.Count)];

        // Generate a random base damage
        int baseDamage = Random.Range(rangeOfBaseDamage.x, rangeOfBaseDamage.y + 1);

        // Choose number of actions to add
        int numSpecials = Random.Range(rangeOfSpecialsPerWeapon.x, rangeOfSpecialsPerWeapon.y + 1);

        List<Action> actions = new List<Action>();

        // Add one basic action
        actions.Add(Instantiate(basicActionsToChooseFrom[Random.Range(0, basicActionsToChooseFrom.Count)]));

        // Choose that many actions
        for (int i = 0; i < numSpecials; i++) {
            // Choose a random special action and make a copy
            var action = Instantiate(specialActionsToChooseFrom[Random.Range(0, specialActionsToChooseFrom.Count)]);
            actions.Add(action);
        }
        
        // Now add the actions to the weapon
        newWeapon.Initialize(baseDamage, actions);

        return newWeapon;
    }

    public Armor GenerateArmor() {
        // Create empty weapon
        Armor newArmor = ScriptableObject.CreateInstance<Armor>();

        // Generate random name
        newArmor.name = armorNames[Random.Range(0, armorNames.Count)];

        // Generate random description
        newArmor.description = armorFlavorTexts[Random.Range(0, armorFlavorTexts.Count)];

        // Generate random sprite
        newArmor.sprite = armorSprites[Random.Range(0, armorSprites.Count)];

        // Choose number of actions to add
        int numPassives = Random.Range(rangeOfPassivesPerWeapon.Item1, rangeOfPassivesPerWeapon.Item2 + 1);

        List<Passive> passives = new List<Passive>();
        // Choose that many actions
        for (int i = 0; i < numPassives; i++) {
            // Choose a random action
            var passive = passivesToChooseFrom[Random.Range(0, passivesToChooseFrom.Count)];
            passives.Add(passive);
        }
        
        // Now add the actions to the weapon
        newArmor.Initialize(passives);

        return newArmor;
    }
}
