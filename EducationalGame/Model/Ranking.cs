using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EducationalGame.Model
{
    public class Ranking
    {
        private int rank;
        private string studentNames;
        private string teacherNames;
        private uint totalPoints;
        private uint coins;

        public int Rank { get { return rank; } set { rank = value; } }
        public string StudentNames { get { return studentNames; } set { studentNames = value; } }
        public string TeacherNames { get { return teacherNames; }  set { teacherNames = value; } }
        public uint TotalPoints { get { return totalPoints; }  set { totalPoints = value; } }
        public uint Coins { get { return coins; } set { coins = value; } }

        public Ranking()
        {

        }

    }
}
