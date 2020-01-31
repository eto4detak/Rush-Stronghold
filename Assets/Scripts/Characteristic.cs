using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Characteristic
{
    private float damage;
    private float bonusDamage;
    private float startDamage = 1f;

    private float speed;
    private float bonusSpeed;
    private float startSpeed = 1f;

    public List<Feature> features = new List<Feature>();


    public Characteristic()
    {
        speed = bonusSpeed + startSpeed;
    }

    public float Speed { get => speed; }

    public void AddFeature(Feature newFeature)
    {
        features.Add(newFeature);
    }

    public void RemoveFeature(Feature lost)
    {
        features.Remove(lost);
    }

    private void SetBonusSpeed(float bonus)
    {
        speed = startSpeed + bonus;
        if (speed < 0) speed = 0;
    }

    private void UpdateCharacteristic()
    {
        float newBonusSpeed = 0;
        for (int i = 0; i < features.Count; i++)
        {
            if (features[i].ch_type == CharacteristicType.Speed) newBonusSpeed += features[i].value;
        }
        SetBonusSpeed(newBonusSpeed);
    }

    private void Awake()
    {
        damage = startDamage;
        speed = startSpeed;
    }
}

public struct Feature
{
    public CharacteristicType ch_type;
    public float value;
}

public enum CharacteristicType
{
    Speed,
    Attack,
}