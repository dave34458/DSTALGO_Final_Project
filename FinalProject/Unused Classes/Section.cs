using System;


namespace FinalProject
{
    internal class Section
    {
        private string day { get; set; }
        private string timeSlot { get; set; }
        private string sectionCode { get; set; }
        private string room { get; set; }
        private string professor { get; set; }
        private int maxStudentsInSection { get; set; }
        public CustomList<string> StudentsInSection { get; set; }


        public Section(string Day, string TimeSlot, string SectionCode, string Room, string Professor, int MaxStudentsInSection)
        {
            day = Day;
            timeSlot = TimeSlot;
            sectionCode = SectionCode;
            room = Room;
            professor = Professor;
            maxStudentsInSection = MaxStudentsInSection;
            StudentsInSection = new CustomList<string>();
        }

        public string Day
        {
            get { return day; }
            set { day = value; }
        }
        public string TimeSlot
        {
            get { return timeSlot; }
            set { timeSlot = value; }
        }
        public string SectionCode
        {
            get { return sectionCode; }
            set { sectionCode = value; }
        }
        public string Room
        {
            get { return room; }
            set { room = value; }
        }
        public string Professor
        {
            get { return professor; }
            set { professor = value; }
        }
        public int MaxStudentsInSection
        {
            get { return maxStudentsInSection; }
            set { maxStudentsInSection = value; }
        }
    }
}
