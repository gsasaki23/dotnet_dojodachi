using System;

namespace dojodachi.Models
{
    public class DojodachiModel
    {
        public int Happiness { get; set; }
        public int Fullness { get; set; }
        public int Energy { get; set; }
        public int Meals { get; set; }

        public DojodachiModel()
        {
            Happiness = 20;
            Fullness = 20;
            Energy = 50;
            Meals = 3;
        }

        public void feed()
        {
            // cost 1 meal, dont feed if no meals
            // gain random fullness 5-10
                // 25% chance to fail (meal still charged)
        }
        public void play()
        {
            // costs 5 energy
            // gain random happiness 5-10
                // 25% chance to fail (energy still charged)
        }
        public void work()
        {
            // costs 5 energy
            // earn random 1~3 meals
        }
        public void sleep()
        {
            // earn 15 energy
            // decrease fullness and happiness by 5 each
        }

        public bool gameWon()
        {
            return Energy >= 100 && Fullness >= 100 && Happiness >= 100;
        }

        public bool dead()
        {
            return Fullness <= 0 || Happiness <= 0;
        }
    }
}