using System;


namespace FinalProject
{
    internal class Courses
    {
        private string course;
        private string description;
        private int courseUnits;
        private int maxCourseStudents;
        public CustomList<string> studentsEnrolled;
        public CustomList<Section> Sections;

        public Courses(string Course, string Description, int CourseUnits, int MaxCourseStudents)
        {
            course = Course;
            description = Description;
            courseUnits = CourseUnits;
            maxCourseStudents = MaxCourseStudents;
            studentsEnrolled = new CustomList<string>();
            Sections = new CustomList<Section>();
        }

        public string Course
        {
            get { return course; }
            set { course = value; }
        }

        public string Description
        {
            get { return description; }
            set { description = value; }
        }

        public int CourseUnits
        {
            get { return courseUnits; }
            set { courseUnits = value; }
        }

        public int MaxCourseStudents
        {
            get { return maxCourseStudents; }
            set { maxCourseStudents = value; }
        }

        public bool IsOverlappingSection(Section newSection)
        {
            for (int i = 0; i < Sections.Count(); i++)
            {
                var existingSection = Sections[i];
                if (existingSection == null)
                    continue;
                else if (existingSection.Day == newSection.Day && existingSection.TimeSlot == newSection.TimeSlot)
                {
                    if (existingSection.Room == newSection.Room || existingSection.Professor == newSection.Professor || existingSection.SectionCode == newSection.SectionCode)
                    {
                        string time = "";
                        if (Sections[i].TimeSlot == "1")
                            time = "8am to 11am";
                        else if (Sections[i].TimeSlot == "2")
                            time = "11am to 2pm";
                        else if (Sections[i].TimeSlot == "3")
                            time = "2pm to 5pm";
                        else if (Sections[i].TimeSlot == "4")
                            time = "5pm to 8pm";
                        Console.WriteLine("\nOverlap with section:\n" +
                            Course + "\t" +
                            existingSection.Day + "\t" +
                            Sections[i].SectionCode + "\t" +
                            time + "\t" +
                            Sections[i].Room + "\t" +
                            Sections[i].Professor);
                        return true;
                    }
                }
            }
            return false;
        }
    }
}
