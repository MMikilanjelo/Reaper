using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using Godot;
using GameLogick.Utilities;

namespace GameLogick
{
    public class LootTable<T>
    {
        private Godot.RandomNumberGenerator random;
        public Dictionary<T  , int > WeigthTable = new ();
        public LootTable()
        {
            random = MathUtil.RNG;
        }
        public void SetSeed(ulong seed) => random.Seed = seed;

        public void SetRandom(Godot.RandomNumberGenerator random) => this.random = random;
       
        int  TotalWeigthSum = 0;
        public void AddItemToTable(T item , int item_weigth)
        {
                WeigthTable.Add(item , item_weigth);
                TotalWeigthSum += item_weigth;
        }
        public T PickItem()
        {
            var chosen_random_weigth = random.RandiRange(1 , TotalWeigthSum);
            var iteration_sum_coeficient = 0;
            foreach(var item in WeigthTable)
            {
                iteration_sum_coeficient += item.Value;
                if(chosen_random_weigth <= iteration_sum_coeficient)
                {
                    return item.Key;
                }
            }
            return default;
        }
    }
}