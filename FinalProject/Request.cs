using System;


namespace FinalProject
{
    internal class Request
    {
        private string course;
        private string student;
        private string reason;


        public Request(string Course, string Student, string Reason)
        {
            course = Course;
            student = Student;
            reason = Reason;
        }

        public string Course
        {
            get { return course; }
            set { course = value; }
        }

        public string Student
        {
            get { return student; }
            set { student = value; }
        }

        public string Reason
        {
            get { return reason; }
            set { reason = value; }
        }
    }
}
