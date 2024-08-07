using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(menuName = "Game/GameSettings")]
public class GameSettings : ScriptableObject
{
    public float PlayerMoveSpeed;
    public float PlayerFireRate;
    public float PlayerBulletSpeed;
    public int PlayerStartAmmoAmount;
    public int PlayerDamage;

    public int EnemySpawnAmountMin;
    public int EnemySpawnAmountMax;
    public float EnemySpawnCooldown;
    public List<EnemyTypeSettings> EnemyTypeSettingsList;


    [Serializable]
    public class EnemyTypeSettings
    {
        public EnemyType EnemyType;
        public RuntimeAnimatorController AnimatorController;
        public int Health;
        public float SpeedMin;
        public float SpeedMax;
        public float MinSpawnDistance;
        public float MaxSpawnDistance; 
        public int AmmoDropAmount;
    }

    public EnemyTypeSettings GetEnemySettingsByType(EnemyType enemyType)
    {
        foreach (var enemyTypeSettings in EnemyTypeSettingsList)
        {
            if (enemyTypeSettings.EnemyType == enemyType)
                return enemyTypeSettings;
        }

        Debug.LogError($"Cant find enemy settings for type {enemyType}");
        return null;
    }
}

public enum EnemyType
{
    Zombie,
    Undead,
    Beast
}