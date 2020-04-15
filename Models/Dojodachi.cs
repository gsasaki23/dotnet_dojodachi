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

        public string feed()
        {
            // Don't feed if no meals left
            if (this.Meals == 0){return "noMeals";}
            // -1 meal and +5~10 fullness (25% fail)
            this.Meals--;
            if(new Random().Next(0,4) != 0){
                int diff = new Random().Next(5,11);
                this.Fullness += diff;
                return diff.ToString();
            }
            return "fail";
        }
        public string play()
        {
            // Don't play if no energy left
            if (this.Energy < 5){return "noEnergy";}
            // -5 energy and +5~10 happiness (25% fail)
            this.Energy -= 5;
            if(new Random().Next(0,4) != 0){
                int diff = new Random().Next(5,11);
                this.Happiness += diff;
                return diff.ToString();
            }
            return "fail";
        }
        public string work()
        {
            // Don't work if no energy left
            if (this.Energy < 5){return "noEnergy";}
            // -5 energy and +1~3 meals
            this.Energy -= 5;
            int diff = new Random().Next(1,4);
            this.Meals += diff;
            return diff.ToString();
        }
        public string sleep()
        {
            // +15 energy and -5 fullness and -5 happiness
            this.Energy += 15;
            this.Fullness -= 5;
            this.Happiness -= 5;
            return "";
        }

        public bool gameWon()
        {
            return Fullness >= 100 && Happiness >= 100;
        }

        public bool dead()
        {
            return Fullness <= 0 || Happiness <= 0 || Energy <= 0;
        }
    }
}