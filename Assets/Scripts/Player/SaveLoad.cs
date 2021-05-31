using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

[Serializable]
public class SaveData
{
    public int NumberScene;
    public float Health;
    public bool hasShotgun;
    public bool hasRifle;
    public EquipWeapon inHand;
    public int currentAmmoP;
    public int currentAmmoS;
    public int allAmmoR;
    public int allAmmoP;
    public int allAmmoS;
    public int currentAmmoR;
}

public class SaveLoad
{
    public void SaveData(Player player, int numberScene)
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath
          + "/2DSgame.json");
        SaveData data = new SaveData();
        data.NumberScene = numberScene;
        data.Health = player.Health;
        data.hasShotgun = player.hasShotgun;
        data.hasRifle = player.hasRifle;
        data.inHand = player.weapon.inHand;
        data.currentAmmoP = player.weapon.currentAmmoP;
        data.currentAmmoS = player.weapon.currentAmmoS;
        data.currentAmmoR = player.weapon.currentAmmoR;
        data.allAmmoP = player.weapon.allAmmoP;
        data.allAmmoS = player.weapon.allAmmoS;
        data.allAmmoR = player.weapon.allAmmoR;
        data.inHand = player.weapon.inHand;
        bf.Serialize(file, data);
        file.Close();
    }
    public bool LoadData(ref Player player, ref int numberScene)
    {
        if (File.Exists(Application.persistentDataPath
    + "/2DSgame.json"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file =
              File.Open(Application.persistentDataPath
              + "/2DSgame.json", FileMode.Open);
            SaveData data = (SaveData)bf.Deserialize(file);
            numberScene = data.NumberScene;
            player.Health = data.Health;
            player.hasShotgun = data.hasShotgun;
            player.hasRifle = data.hasRifle;
            player.weapon.inHand = data.inHand;
            player.weapon.currentAmmoP = data.currentAmmoP;
            player.weapon.currentAmmoS = data.currentAmmoS;
            player.weapon.currentAmmoR = data.currentAmmoR;
            player.weapon.allAmmoP = data.allAmmoP;
            player.weapon.allAmmoS = data.allAmmoS;
            player.weapon.allAmmoR = data.allAmmoR;
            player.weapon.inHand = data.inHand;
            file.Close();
            return true;
        }
        else
            return false;
    }
    public void ResetData()
    {
        if (File.Exists(Application.persistentDataPath
          + "/2DSgame.json"))
        {
            File.Delete(Application.persistentDataPath
              + "/2DSgame.json");
        }
    }
}