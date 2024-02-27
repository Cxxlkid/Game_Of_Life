using System.IO;
using Game_Of_Life.Models;
using Newtonsoft.Json;

namespace Game_Of_Life.Utils;

public class SaveUtil
{
    public static String FullPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
        "GameOfLife", "save.json");


    public static void Save(Save data)
    {
        try
        {
            string jsonData = JsonConvert.SerializeObject(data);

            //Create file if not exists
            if (!File.Exists(FullPath))
            {
                Directory.CreateDirectory(
                    Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "GameOfLife"));
                File.Create(FullPath).Close();
            }

            // Écrit les données JSON dans le fichier
            File.WriteAllText(FullPath, jsonData);

            Console.WriteLine("Les données ont été sauvegardées avec succès.");
        }
        catch (Exception ex)
        {
            Console.WriteLine("Une erreur s'est produite lors de la sauvegarde : " + ex.Message);
        }
    }

    public static Save Load()
    {
        try
        {
            if (File.Exists(FullPath))
            {
                string jsonData = File.ReadAllText(FullPath);
                return JsonConvert.DeserializeObject<Save>(jsonData) ?? new Save(10);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("Une erreur s'est produite lors du chargement : " + ex.Message);
        }

        return new Save(10);
    }
}