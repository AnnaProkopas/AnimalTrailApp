using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

namespace Awards
{
    public class AwardsStorage
    {
        private const string HasSavedAwardsField = "has_saved_awards";
        private const string FileName = "awardssave.save";

        private static List<StoredAward> staticStoredAwards = null;

        public static List<StoredAward> Load()
        {
            if (staticStoredAwards == null)
            {
                int hasSavedLevel = PlayerPrefs.GetInt(HasSavedAwardsField, 0);

                List<StoredAward> storedAwards = new List<StoredAward>();
                if (hasSavedLevel == 1)
                {
                    BinaryFormatter bf = new BinaryFormatter();
                    FileStream file = File.Open(Application.persistentDataPath + "/" + FileName, FileMode.Open);
                    storedAwards = ((StoredAwards)bf.Deserialize(file)).awards;
                    file.Close();
                }

                staticStoredAwards = storedAwards;
            }

            return staticStoredAwards;
        }

        public static void Save(List<AwardType> newAwards)
        {
            var storedAwardByType = Load().ToDictionary(x => x.type, x => x);
            foreach (var awardType in newAwards)
            {
                if (!storedAwardByType.ContainsKey(awardType))
                {
                    storedAwardByType[awardType] = new StoredAward(1, awardType);
                    storedAwardByType[awardType].isNew = true;
                }

                storedAwardByType[awardType].count += 1;
            }
            staticStoredAwards = storedAwardByType.Values.ToList();
            
            UpdateFile();
        }

        public static List<StoredAward> Open()
        {
            return staticStoredAwards;
        }
        
        public static void MarkOpened(AwardType type)
        {
            var ind = staticStoredAwards.FindIndex(award => award.type == type);
            if (staticStoredAwards[ind].isNew)
            {
                staticStoredAwards[ind].isNew = false;
                UpdateFile();
            }
        }

        private static void UpdateFile()
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Create(Application.persistentDataPath + "/" + FileName);
            StoredAwards storedAwards = new StoredAwards(staticStoredAwards);
            bf.Serialize(file, storedAwards);
            file.Close();
        }
    }
}