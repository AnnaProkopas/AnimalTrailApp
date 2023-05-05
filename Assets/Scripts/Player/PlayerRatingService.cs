using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerRatingService
{
    private const string ScoreListField = "scoreList";

    /* deprecated */
    private const string RatingRecordField = "ratingRecord";
    private const string RatingListCountField = "ratingListCount";
    private const string RatingDatePrefixField = "statsRecordDate";
    private const string RatingValuePrefixField = "statsRecordValue";
    
    private static PlayerRatingService _instance;
    public static PlayerRatingService instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new PlayerRatingService();
            }

            return _instance;
        }
    }

    private List<ScoreRecord> scoreRecordList = new List<ScoreRecord>();
    private List<ScoreRecord> orderedScoreRecordList = new List<ScoreRecord>();

    
    public static int GetRecordFoodCounter() 
    {
        return PlayerPrefs.GetInt(RatingRecordField, 0);
    }

    public static void SetRecordFoodCounter(int value) 
    {
        PlayerPrefs.SetInt(RatingRecordField, value);
    }

    public static void AddRecord(int value, int humans = 0, int years = 0)
    {
        ScoreRecord score = new ScoreRecord(value, humans, years);
        instance.scoreRecordList.Add(score);
        
        UpdateScoreList();
    }

    private static void UpdateScoreList()
    {
        Wrapper<ScoreRecord> wrapper = new Wrapper<ScoreRecord>();
        wrapper.Items = instance.scoreRecordList.ToArray();
        string json = JsonUtility.ToJson(wrapper);
        PlayerPrefs.SetString(ScoreListField, json);
    }
    
    public static List<ScoreRecord> GetScoreRecords()
    {
        if (instance.scoreRecordList.Count == 0)
        {
            var json = PlayerPrefs.GetString(ScoreListField);
            Wrapper<ScoreRecord> wrapper = JsonUtility.FromJson<Wrapper<ScoreRecord>>(json);
            if (wrapper.Items != null)
            {
                instance.scoreRecordList = new List<ScoreRecord>(wrapper.Items);
            }
        }

        return instance.scoreRecordList;
    }

    public static ScoreRecord GetLastScoreRecord()
    {
        if (GetScoreRecords().Count == 0)
        {
            return new ScoreRecord();
        }
        return GetScoreRecords().Last();
    }

    public static IEnumerable<ScoreRecord> GetTopTenHighestScores()
    {
        if (instance.orderedScoreRecordList.Count != instance.scoreRecordList.Count)
        {
            instance.orderedScoreRecordList = instance.scoreRecordList.ToList();
            instance.orderedScoreRecordList.Sort(( record1, record2) => record2.value - record1.value);
        }

        return instance.orderedScoreRecordList.Take(10);
    }
    
    
    [Serializable]
    private class Wrapper<T>
    {
        public T[] Items;
    }
}

[Serializable]
public class ScoreRecord
{
    public int value;
    public int humans;
    public int level;
    public int years;
    public long date;

    public ScoreRecord()
    {
        value = 0;
        date = 0;
        level = 0;
        humans = 0;
        years = 0;
    }
    
    public ScoreRecord(int value, string date)
    {
        this.value = value;
        this.date = ((DateTimeOffset)DateTime.Parse(date)).ToUnixTimeSeconds();
        level = 0;
        humans = 0;
        years = 1;
    }

    public ScoreRecord(int value, int humans, int years)
    {
        this.value = value;
        this.humans = humans;
        this.years = years;
        level = 0;
        date = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
    }

    public DateTimeOffset GetDateTime()
    {
        return DateTimeOffset.FromUnixTimeSeconds(date);
    }
}