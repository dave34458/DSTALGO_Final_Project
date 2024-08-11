using Microsoft.SqlServer.Server;
using System;
using System.Collections;
using System.Data;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Runtime.InteropServices;
using FinalProjectClassLibrary;

namespace FinalProject
{
    internal class Program
    {
        // Private static fields
        private static string _currentUser = "";
        private static string _courseBeingEdited = "";
        private static bool _isAdmin = false;

        private static CustomStack _historyforwards = new CustomStack();
        private static CustomStack _historybackwards = new CustomStack();
        private static CustomDictionary<string, string> _studentAccounts = new CustomDictionary<string, string>();
        private static CustomDictionary<string, string> _adminAccounts = new CustomDictionary<string, string>();
        private static CustomList<Courses> _database = new CustomList<Courses>();
        private static CustomQueue<Request> _studentRequests = new CustomQueue<Request>();

        // Public properties to access and modify the private static fields
        public static string CurrentUser
        {
            get { return _currentUser; }
            set { _currentUser = value; }
        }

        public static string CourseBeingEdited
        {
            get { return _courseBeingEdited; }
            set { _courseBeingEdited = value; }
        }

        public static bool IsAdmin
        {
            get { return _isAdmin; }
            set { _isAdmin = value; }
        }

        public static CustomStack HistoryForwards
        {
            get { return _historyforwards; }
            set { _historyforwards = value; }
        }

        public static CustomStack HistoryBackwards
        {
            get { return _historybackwards; }
            set { _historybackwards = value; }
        }

        public static CustomDictionary<string, string> StudentAccounts
        {
            get { return _studentAccounts; }
            set { _studentAccounts = value; }
        }

        public static CustomDictionary<string, string> AdminAccounts
        {
            get { return _adminAccounts; }
            set { _studentAccounts = value; }
        }
        public static CustomList<Courses> Database
        {
            get { return _database; }
            set { _database = value; }
        }
        public static CustomQueue<Request> StudentRequests
        {
            get { return _studentRequests; }
            set { _studentRequests = value; }
        }

        //Main Method
        static void Main(string[] args)
        {
            StudentAccounts.Add("s1", "p1"); StudentAccounts.Add("s2", "p2"); StudentAccounts.Add("s3", "p3"); StudentAccounts.Add("s4", "p4");
            AdminAccounts.Add("a1", "p1"); AdminAccounts.Add("a2", "p2"); AdminAccounts.Add("a3", "p3"); AdminAccounts.Add("a4", "p4");
            Courses course = new Courses("FUNDSYS", "Lorem Ipsum Lorem", 3, 50);
            Database.Add(course);
            course = new Courses("INCOMPU", "lololololololol", 2, 40);
            Database.Add(course);
            Section section = new Section("M", "1", "XTIS1", "R805", "Jino", 30);
            Database[0].Sections.Add(section);
            section = new Section("M", "1", "XTIS2", "R807", "Marites", 23);
            Database[1].Sections.Add(section);
            LoginPage();
        }



        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //Navigation
        static void NavigateBackwards()
        {
            HistoryForwards.Push(HistoryBackwards.Pop());
            if (HistoryBackwards.Peek() == "LoginPage")
            {
                //pop once more because the loginpage is only being pushed after successfully logging in
                HistoryBackwards.Pop();
                CurrentUser = "";
                IsAdmin = false;
                LoginPage();
            }
            if (HistoryBackwards.Peek() == "AdminMenuPage")
            {
                AdminMenuPage();
            }
            if (HistoryBackwards.Peek() == "StudentMenuPage")
            {
                StudentMenuPage();
            }
            if (HistoryBackwards.Peek() == "CoursesPage")
            {
                CoursesPage();
            }
        }
        static void NavigateForwards()
        {
            if (HistoryForwards.Count() == 0)
            {
                if (HistoryBackwards.Peek() == "AdminMenuPage")
                {
                    AdminMenuPage();
                }
                if (HistoryBackwards.Peek() == "StudentMenuPage")
                {
                    StudentMenuPage();
                }
                if (HistoryBackwards.Peek() == "CoursesPage")
                {
                    CoursesPage();
                }
                return;
            }
            HistoryBackwards.Push(HistoryForwards.Pop());
            if (HistoryBackwards.Peek() == "LoginPage")
            {
                //pop once more because the loginpage is only being pushed after successfully logging in
                HistoryBackwards.Pop();
                CurrentUser = "";
                IsAdmin = false;
                LoginPage();
            }
            if (HistoryBackwards.Peek() == "AdminMenuPage")
            {
                AdminMenuPage();
            }
            if (HistoryBackwards.Peek() == "StudentMenuPage")
            {
                StudentMenuPage();
            }
            if (HistoryBackwards.Peek() == "CoursesPage")
            {
                CoursesPage();
            }
            if (HistoryBackwards.Peek() == "StudentRequestsPage")
            {
                StudentRequestsPage();
            }
            if (HistoryBackwards.Peek() == "AdminRequestsPage")
            {
                AdminRequestsPage();
            }
            if (HistoryBackwards.Peek() == "StudentAccountsPage")
            {
                StudentAccountsPage();
            }
        }
        public static void ShowURL() //for testing purposes only, used to show navigation history, remove on final implementation
        {
            for (int i = 0; i < HistoryBackwards.Count(); i++)
            {
                if (HistoryBackwards.array[i] == HistoryBackwards.Peek())
                    Console.ForegroundColor = ConsoleColor.Green;
                Console.Write(HistoryBackwards.array[i] + "\t");
                Console.ForegroundColor = ConsoleColor.Gray;
            }
            for (int k = HistoryForwards.Count() - 1; k >= 0; k--)
                Console.Write(HistoryForwards.array[k] + "\t");
            Console.WriteLine();
            Console.WriteLine();
        }



        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        ///Pages
        static void LoginPage()
        {
            //UI
            Console.Clear();

            Console.WriteLine("Welcome to the Flexi Enlistment/Enrollment System!\n");
            Console.Write("Enter username: ");
            string Username = Console.ReadLine();
            Console.Write("Enter password: ");
            string Password = Console.ReadLine();
            //Login logic
            //Student Account is logged In
            if (StudentAccounts.ContainsKey(Username) && StudentAccounts[Username] == Password)
            {
                CurrentUser = Username;
                HistoryBackwards.Clear();
                HistoryForwards.Clear();
                HistoryBackwards.Push("LoginPage");
                HistoryBackwards.Push("StudentMenuPage");
                StudentMenuPage(); // Navigate to StudentMenu
            }
            //Admin Account is logged In
            else if (AdminAccounts.ContainsKey(Username) && AdminAccounts[Username] == Password)
            {
                CurrentUser = Username;
                IsAdmin = true;
                HistoryBackwards.Clear();
                HistoryForwards.Clear();
                HistoryBackwards.Push("LoginPage");
                HistoryBackwards.Push("AdminMenuPage");
                AdminMenuPage(); // Navigate to AdminMenu

            }
            //Login Failed
            else
            {
                Console.WriteLine("\nInvalid username or password.");
                Console.WriteLine("Press any key to try again...");
                Console.ReadKey();
                LoginPage(); // Retry login
            }
        }
        
        static void StudentMenuPage()
        {
            //Clear Page
            Console.Clear();
            ShowURL();

            string Choice = "";

            //UI
            Console.WriteLine($"Hello Student {CurrentUser}!");
            //Schedule Viewing
            CustomList<string> UserCourses = new CustomList<string>();
            CustomList<CustomList<string>> UserSections = new CustomList<CustomList<string>>();
            int UserUnits = 0;
            CustomList<string> temp = new CustomList<string>();
            for (int i = 0; i < Database.Count(); i++)
                if (Database[i].studentsEnrolled.Contains(CurrentUser))
                    UserCourses.Add(Database[i].Course);
            for (int i = 0; i < Database.Count(); i++)
            {
                for (int j = 0; j < Database[i].Sections.Count(); j++)
                {
                    if (Database[i].Sections[j].StudentsInSection.Contains(CurrentUser))
                    {
                        temp.Clear();
                        temp.Add(Database[i].Sections[j].Day);
                        temp.Add(Database[i].Sections[j].SectionCode);
                        if (Database[i].Sections[j].TimeSlot == "1")
                            temp.Add("8am to 11am");
                        if (Database[i].Sections[j].TimeSlot == "2")
                            temp.Add("11am to 2pm");
                        if (Database[i].Sections[j].TimeSlot == "3")
                            temp.Add("2pm to 5pm");
                        if (Database[i].Sections[j].TimeSlot == "4")
                            temp.Add("5pm to 8pm");
                        temp.Add(Database[i].Sections[j].Room);
                        temp.Add(Database[i].Sections[j].Professor);
                        UserSections.Add(temp);
                    }
                        
                }
            }
            Console.WriteLine("Your Courses: ");
            for (int i = 0; i < UserCourses.Count(); i++)
                Console.WriteLine(UserCourses[i]);
            Console.WriteLine();
            Console.WriteLine("Your Sections: ");
            for (int i = 0; i < UserSections.Count(); i++)
                for (int j = 0; j < UserSections[i].Count(); j++)
                    Console.Write(UserSections[i][j] + "\t");
            Console.WriteLine();
            for (int i = 0; i < Database.Count(); i++)
                for (int j = 0; j < Database[i].Sections.Count(); j++)
                    if (Database[i].Sections[j].StudentsInSection.Contains(CurrentUser))
                        UserUnits += Database[i].CourseUnits;
            Console.Write("Your Units: " + UserUnits + "\n");

            Console.WriteLine("\n[1] Enroll In A Course\n[2] View My Requests\n[4] Change Password\n[0] Back (Logout)\n[-] Forwards");
            Console.Write("Enter Action: ");
            Choice = Console.ReadLine();
            if (Choice == "1")
            {
                HistoryBackwards.Push("CoursesPage");
                CoursesPage();
            }
            if (Choice == "2")
            {
                HistoryBackwards.Push("StudentRequestsPage");
                StudentRequestsPage();
            }
            if (Choice == "4")
            {
                ChangePasswordPage();
            }
            if (Choice == "0")
            {
                NavigateBackwards();
            }
            if (Choice == "-")
            {
                NavigateForwards();
            }
            else
            {
                StudentMenuPage();
            }
        }

        static void AdminMenuPage()
        {
            //Clear Page
            Console.Clear();
            ShowURL();

            string Choice = "";
            //UI
            Console.WriteLine($"Hello Admin {CurrentUser}!");
            Console.WriteLine("[1] View courses\n[2] View Student Requests (" + StudentRequests.GetQueue().Count() + ")\n[3] View Student Accounts\n[4] Change Password\n[0] Back (Logout)\n[-] Forwards");
            Console.Write("Enter Action: ");
            Choice = Console.ReadLine();
            if (Choice == "1")
            {
                HistoryBackwards.Push("CoursesPage");
                CoursesPage();
            }
            if (Choice == "2")
            {
                HistoryBackwards.Push("AdminRequestsPage");
                AdminRequestsPage();
            }
            if (Choice == "3")
            {
                HistoryBackwards.Push("StudentAccountsPage");
                StudentAccountsPage();
            }
            if (Choice == "4")
            {
                ChangePasswordPage();

            }
            if (Choice == "0")
            {
                NavigateBackwards();
            }
            if (Choice == "-")
            {
                NavigateForwards();
            }
            else
            {
                AdminMenuPage();
            }
        }

        public static void StudentAccountsPage()
        {
            Console.Clear();
            //remove forward stack history if different page
            if (HistoryForwards.Count() > 0)
                if (HistoryForwards.Peek() != "StudentAccountsPage")
                    HistoryForwards.Clear();
                else
                    HistoryForwards.Pop();

            ShowURL();
            int temp = 0;
            string Choice = "";
            Console.WriteLine("Student Accounts: ");
            for (int i = 0; i < StudentAccounts.Count; i++) 
                Console.WriteLine("[" + (i + 1) + "] " + StudentAccounts.Keys()[i] + " : " + StudentAccounts.Values()[i]);
            Console.WriteLine();

            Console.WriteLine("[-] Add a student account\n[#] Edit a student's password\n[=] Delete an account\n[0] Back");
            Console.Write("Enter Action: ");
            Choice = Console.ReadLine();
            bool Result = Int32.TryParse(Choice, out temp);
            if (Result && temp - 1 >= 0 && temp - 1 <= StudentAccounts.Count)
            {
                Console.Write("\nEnter " + StudentAccounts.Keys()[temp - 1] + " new password: ");
                StudentAccounts[StudentAccounts.Keys()[temp - 1]] = Console.ReadLine();
                StudentAccountsPage();
            }
            else if (!Result && Choice == "-")
            {
                string StudentUserName =  "", StudentPassword = "";
                Console.Write("Enter Student Username: ");
                StudentUserName = Console.ReadLine();
                Console.Write("Enter Student Password: ");
                StudentPassword = Console.ReadLine();
                for (int i = 0; i < StudentAccounts.Keys().Count(); i++)
                {
                    if (StudentAccounts.Keys()[i] == StudentUserName)
                    {
                        Console.WriteLine("\nStudent with the same username already exists\nPress any key to refresh...");
                        Console.ReadKey();
                        StudentAccountsPage();
                    }
                }
                StudentAccounts.Add(StudentUserName, StudentPassword);
                StudentAccountsPage();

            }
            else if (!Result && Choice == "=")
            {
                Console.Write("Enter [#] of account to delete: ");
                bool Result2 = Int32.TryParse(Console.ReadLine(), out temp);
                if (Result2 && temp > 0 && temp - 1 <= StudentAccounts.Count)
                {
                    string UsernameOfDeletedAccount = StudentAccounts.Keys()[temp - 1];
                    for (int i = 0; i < Database.Count(); i ++)
                    {
                        Database[i].studentsEnrolled.Remove(UsernameOfDeletedAccount);
                        for (int j = 0; j < Database[i].Sections.Count(); j ++)
                        {
                            Database[i].Sections[j].StudentsInSection.Remove(UsernameOfDeletedAccount);
                        }
                    }
                    StudentAccounts.Remove(StudentAccounts.Keys()[temp - 1]);
                    StudentAccountsPage();
                }
                else
                {
                    Console.WriteLine("\nIncorrect entry\nPress any key to refresh page...");
                    StudentAccountsPage();
                }
            }
            else if (Choice == "0")
            {
                NavigateBackwards();
            }
            else
            {
                StudentAccountsPage();
            }
        }

        public static void ChangePasswordPage()
        {
            Console.Clear();
            HistoryForwards.Clear();

            ShowURL();
            string temp = "";
            Console.Write("Enter current password: ");
            temp = Console.ReadLine();
            if (IsAdmin)
            {
                if (temp == AdminAccounts[CurrentUser])
                {
                    Console.Write("Enter new password: ");
                    temp = Console.ReadLine();
                    AdminAccounts[CurrentUser] = temp;
                    AdminMenuPage();
                }

            }
            else if (!IsAdmin)
            {
                if (temp == StudentAccounts[CurrentUser])
                {
                    Console.Write("Enter new password: ");
                    temp = Console.ReadLine();
                    StudentAccounts[CurrentUser] = temp;
                    StudentMenuPage();
                }
            }
            Console.WriteLine("\nIncorrect password\nPress any key to go back...");
            Console.ReadKey();
            if (IsAdmin)
                AdminMenuPage();
            else
                StudentMenuPage();
        }

        public static void CoursesPage()
        {
            Console.Clear();
            //remove forward stack history if different page
            if (HistoryForwards.Count() > 0)
                if (HistoryForwards.Peek() != "CoursesPage")
                    HistoryForwards.Clear();
                else
                    HistoryForwards.Pop();

            ShowURL();

            string Choice = "";
            //UI
            if (Database.Count() == 0)
            {
                Console.WriteLine("There are no courses available...");
            }
            else if (Database.Count() > 0) 
            { 
                for (int i = 0; i < Database.Count(); i++)
                {
                    Console.Write("[" + (i + 1) + "] " + Database[i].Course + "\t:\t" + Database[i].studentsEnrolled.Count() + "/" + Database[i].MaxCourseStudents);
                    if (Database[i].studentsEnrolled.Contains(CurrentUser))
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.Write("\tENROLLED");
                        Console.ResetColor();
                    }
                    Console.WriteLine();
                }
            }
            if (IsAdmin)
            {
                Console.WriteLine("\n[-] Add a Course\n[#] Edit Course\n[=] Delete a course\n[0] Back");
                Console.Write("Enter Action: ");
                Choice = Console.ReadLine();
                int choiceNumber;

                if (Choice == "0")
                {
                    NavigateBackwards();
                }
                else if (int.TryParse(Choice, out choiceNumber) && choiceNumber > 0 && choiceNumber <= Database.Count())
                {
                    int IndexOfCourseBeingEdited = choiceNumber - 1; // index of course to edit in database customlist
                    EditCoursePage(IndexOfCourseBeingEdited);
                }
                else if (Choice == "-")
                {
                    AddCoursePage();
                }
                else if (Choice == "=")
                {
                    if (Database.Count() <= 0)
                    {
                        Console.WriteLine("\nThere are no courses to delete\nPress any key to refresh the page...");
                        Console.ReadKey();
                        CoursesPage();
                    }
                    int temp = 0;
                    Console.Write("\nEnter # of course to be deleted: ");
                    bool Result2 = (Int32.TryParse(Console.ReadLine(), out temp));
                    if (Result2 && Database.Count() > 0 && temp >= 1 && temp - 1 <= Database.Count())
                    {
                        Database.RemoveAt(temp - 1);
                        CoursesPage();
                    }
                    else
                    {
                        Console.WriteLine("\nIncorrect entry\nPress any key to refresh the page...");
                        Console.ReadKey();
                        CoursesPage();
                    }
                }
                else
                {
                    CoursesPage(); // reset page if incorrect entry
                }
            }
            else if (!IsAdmin)
            {
                Console.WriteLine("\n[#] Course to enroll in\n[0] Back");
                Console.Write("Enter Action: ");
                Choice = Console.ReadLine();
                int choiceNumber;

                if (Choice == "0")
                {
                    NavigateBackwards();
                }
                else if (int.TryParse(Choice, out choiceNumber) && choiceNumber > 0 && choiceNumber <= Database.Count())
                {
                    int IndexOfCourseBeingEdited = choiceNumber - 1; // index of course to edit in database customlist
                    //Enroll Student in a course if he is not already enrolled yet before going to the enroll course page
                    if (!Database[IndexOfCourseBeingEdited].studentsEnrolled.Contains(CurrentUser) && Database[IndexOfCourseBeingEdited].studentsEnrolled.Count() + 1 <= Database[IndexOfCourseBeingEdited].MaxCourseStudents)
                        Database[IndexOfCourseBeingEdited].studentsEnrolled.Add(CurrentUser);
                    else if (Database[IndexOfCourseBeingEdited].studentsEnrolled.Count() + 1 > Database[IndexOfCourseBeingEdited].MaxCourseStudents)
                    {
                        Console.Clear();
                        Console.WriteLine(Database[IndexOfCourseBeingEdited].Course + " is full\n" + "Press any key to refresh the page...");
                        Console.ReadKey();
                        CoursesPage();
                    }
                    EnrollCoursePage(IndexOfCourseBeingEdited);
                }
                else
                {
                    CoursesPage(); // reset page if incorrect entry
                }
            }
        }
        public static void StudentRequestsPage()
        {
            //remove forward stack history if different page
            if (HistoryForwards.Count() > 0)
                HistoryForwards.Clear();


            Console.Clear();
            ShowURL();

            CustomList<Request> temp = new CustomList<Request>();
            string choice = "";
            for (int i = 0; i < StudentRequests.Count(); i++)
                if (StudentRequests.GetQueue()[i].Student == CurrentUser)
                    temp.Add(StudentRequests.GetQueue()[i]);
            if (temp.Count() > 0)
            {
                Console.WriteLine("Your requests for course removal are as follows: ");
                for (int i = 0; i < temp.Count(); i++)
                    Console.Write("[" + (i + 1) + "] " + temp[i].Course + "\t" + temp[i].Student + "\t" + temp[i].Reason);
                Console.WriteLine();
                Console.WriteLine("\n[0] Back");
                Console.Write("Enter Action: ");
                choice = Console.ReadLine();
                if (choice == "0")
                {
                    NavigateBackwards();
                }
                else
                {
                    StudentRequestsPage();
                }

            }
            else if (temp.Count() == 0)
            {
                Console.WriteLine("Your have made no requests so far\nPress any key to go back...");
                Console.ReadKey();
                NavigateBackwards();
            }
        }
        public static void AdminRequestsPage()
        {
            //remove forward stack history if different page
            if (HistoryForwards.Count() > 0)
                if (HistoryForwards.Peek() != "AdminRequestsPage")
                    HistoryForwards.Clear();
                else
                    HistoryForwards.Pop();

            Console.Clear();
            ShowURL();

            CustomList<Request> temp = new CustomList<Request>();
            string choice = "";
            for (int i = 0; i < StudentRequests.Count(); i++)
                temp.Add(StudentRequests.GetQueue()[i]);
            //Remove request if student is already not in the section or course cannot be found before rendering page
            bool HasFoundCourse = false;
            for (int i = 0; i < temp.Count(); i++)
            {
                for (int j = 0; j < Database.Count(); j++)
                {
                    if (temp[i].Course == Database[j].Course)
                    {
                        HasFoundCourse = true;
                        if (!Database[j].studentsEnrolled.Contains(temp[i].Student))
                        {
                            StudentRequests.Dequeue();
                            temp.RemoveAt(0);
                        }
                    }
                }
                if (!HasFoundCourse)
                {
                    StudentRequests.Dequeue();
                    temp.RemoveAt(0);
                }
            }

            if (temp.Count() > 0)
            {
                for (int i = 0; i < temp.Count(); i++)
                    Console.WriteLine("[" + (i + 1) + "] " + temp[i].Course + "\t" + temp[i].Student + "\t" + temp[i].Reason);
                Console.WriteLine();
                Console.WriteLine("\n[-] Approve Request\n[=] Deny Request\n[0] Back");
                Console.Write("Enter Action: ");
                choice = Console.ReadLine();
                if (choice == "-")
                {
                    for (int i = 0; i < Database.Count(); i++)
                    {
                        // finding and removing student from course
                        if (Database[i].Course == StudentRequests.Peek().Course && Database[i].studentsEnrolled.Contains(StudentRequests.Peek().Student))
                        {
                            Database[i].studentsEnrolled.Remove(StudentRequests.Peek().Student);
                            for (int j = 0; j < Database[i].Sections.Count(); j++)
                            {
                                Database[i].Sections[j].StudentsInSection.Remove(StudentRequests.Peek().Student);
                            }
                            StudentRequests.Dequeue();
                            AdminRequestsPage();
                        }
                    }
                }
                else if (choice == "=")
                {
                    StudentRequests.Dequeue();
                    AdminRequestsPage();
                }
                else if (choice == "0")
                {
                    NavigateBackwards();
                }
                else
                {
                    AdminRequestsPage();
                }
            }
            else if (temp.Count() == 0)
            {
                Console.WriteLine("There are no student requests so far\nPress any key to go back...");
                Console.ReadKey();
                NavigateBackwards();
            }
        }
        public static void EnrollCoursePage(int IndexOfCourseBeingEdited)
        {
            Console.Clear();
            string Choice = "", Time = "";
            ShowURL();
            CustomList<string> AvailableSections = new CustomList<string>();
            Console.WriteLine("Course: " + Database[IndexOfCourseBeingEdited].Course);
            Console.WriteLine("Description: " + Database[IndexOfCourseBeingEdited].Description);
            Console.WriteLine("Course Units: " + Database[IndexOfCourseBeingEdited].CourseUnits.ToString());
            Console.WriteLine("Number of Students Enrolled: " + Database[IndexOfCourseBeingEdited].studentsEnrolled.Count().ToString() + "/" + Database[IndexOfCourseBeingEdited].MaxCourseStudents.ToString() + "\n-----------------------------------------------------------------\n");
            if (Database[IndexOfCourseBeingEdited].Sections.Count() <= 0)
                Console.WriteLine("There are no sections for the course...");
            else
            {
                Console.WriteLine("Sections: ");
                for (int i = 0; i < Database[IndexOfCourseBeingEdited].Sections.Count(); i++)
                {
                    if (Database[IndexOfCourseBeingEdited].Sections[i].TimeSlot == "1")
                        Time = "8am to 11am";
                    if (Database[IndexOfCourseBeingEdited].Sections[i].TimeSlot == "2")
                        Time = "11am to 2pm";
                    if (Database[IndexOfCourseBeingEdited].Sections[i].TimeSlot == "3")
                        Time = "2pm to 5pm";
                    if (Database[IndexOfCourseBeingEdited].Sections[i].TimeSlot == "4")
                        Time = "5pm to 8pm";
                    Console.Write("[" + (i + 1) + "] " + 
                        Database[IndexOfCourseBeingEdited].Sections[i].Day + "\t" +
                        Database[IndexOfCourseBeingEdited].Sections[i].SectionCode + "\t" +
                        Time + "\t" +
                        Database[IndexOfCourseBeingEdited].Sections[i].Room + "\t" +
                        Database[IndexOfCourseBeingEdited].Sections[i].Professor + "\t" +
                        Database[IndexOfCourseBeingEdited].Sections[i].StudentsInSection.Count() + "/" + Database[IndexOfCourseBeingEdited].Sections[i].MaxStudentsInSection);
                    if (Database[IndexOfCourseBeingEdited].Sections[i].StudentsInSection.Contains(CurrentUser))
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.Write("\tENROLLED");
                        Console.ResetColor();
                    }
                }
            }

            Console.WriteLine("\n\n[#] Section to enroll in\n[-] Request removal from course\n[0] Back");
            Console.Write("Enter Action: ");
            int temp = -1;
            Choice = Console.ReadLine();
            bool Result = Int32.TryParse(Choice, out temp);
            if (Choice == "-")
            {
                //course removal request
                string Course = Database[IndexOfCourseBeingEdited].Course;
                string Student = CurrentUser;
                string Reason = "";
                for (int i = 0; i < StudentRequests.GetQueue().Length; i++)
                {
                    if (StudentRequests.GetQueue()[i].Course == Course && StudentRequests.GetQueue()[i].Student == Student)
                    {
                        Console.Write("\nYou have already made a request\nPress any key to refresh the page...");
                        Console.ReadKey();
                        EnrollCoursePage(IndexOfCourseBeingEdited);
                    }
                }
                Console.Write("\nEnter Reason (if any): ");
                Reason = Console.ReadLine();
                StudentRequests.Enqueue(new Request(Course, Student, Reason));
                EnrollCoursePage(IndexOfCourseBeingEdited);
            }
            else if (temp > 0 && temp <= Database[IndexOfCourseBeingEdited].Sections.Count())
            {
                //check if student has conflicting scheds
                Section tempSection = Database[IndexOfCourseBeingEdited].Sections[Int32.Parse(Choice) - 1];
                // Temporarily remove the section from the course
                Database[IndexOfCourseBeingEdited].Sections[Int32.Parse(Choice) - 1] = null;
                for (int i = 0; i < Database.Count(); i++)
                {
                    for (int j = 0; j < Database[i].Sections.Count(); j++)
                    {
                        if (Database[i].Sections[j] == null)
                            continue;
                        if (Database[i].Sections[j].Day == tempSection.Day && Database[i].Sections[j].TimeSlot == tempSection.TimeSlot && Database[i].Sections[j].StudentsInSection.Contains(CurrentUser))
                        {
                            Database[IndexOfCourseBeingEdited].Sections[Int32.Parse(Choice) - 1] = tempSection;
                            Console.WriteLine("\nConflict Schedule\nPress any key to refresh the page...");
                            Console.ReadKey();
                            EnrollCoursePage(IndexOfCourseBeingEdited);
                        }
                    }
                }
                Database[IndexOfCourseBeingEdited].Sections[Int32.Parse(Choice) - 1] = tempSection;
                if (!Database[IndexOfCourseBeingEdited].Sections[Int32.Parse(Choice) - 1].StudentsInSection.Contains(CurrentUser))
                {
                    //remove user from other sections in course
                    for (int i = 0; i < Database[IndexOfCourseBeingEdited].Sections.Count(); i++)
                        Database[IndexOfCourseBeingEdited].Sections[i].StudentsInSection.Remove(CurrentUser);
                    Database[IndexOfCourseBeingEdited].Sections[Int32.Parse(Choice) - 1].StudentsInSection.Add(CurrentUser);
                    EnrollCoursePage(IndexOfCourseBeingEdited);
                }
                else if (Database[IndexOfCourseBeingEdited].Sections[Int32.Parse(Choice) - 1].StudentsInSection.Count() + 1 > Database[IndexOfCourseBeingEdited].Sections[Int32.Parse(Choice) - 1].MaxStudentsInSection)
                {
                    //section is full
                    Console.Clear();
                    Console.WriteLine(Database[IndexOfCourseBeingEdited].Sections[Int32.Parse(Choice) - 1].SectionCode + " is full\nPress any key to refresh the page...");
                    Console.ReadKey();
                    EnrollCoursePage(IndexOfCourseBeingEdited);
                }
                else
                {
                    EnrollCoursePage(IndexOfCourseBeingEdited);
                }
            }
            else if (Choice == "0")
                CoursesPage();
            else
                EnrollCoursePage(IndexOfCourseBeingEdited);
        }
        
        public static void AddCoursePage()
        {
            Console.Clear();
            ShowURL();

            string Course = "", Description = "";
            int CourseUnits = 0, MaxCourseStudents = 0;
            Console.Write("Enter Name of Course: ");
            Course = Console.ReadLine();
            //Check if a course with the same name already exists
            for (int i = 0; i < Database.Count(); i++)
            {
                if (Database[i].Course.ToString() == Course) 
                {
                    Console.WriteLine("\nCourse Already Exists...\nPress Any Key to go back");
                    Console.ReadKey();
                    CoursesPage();
                }
            }
            Console.Write("Enter Description of Course: ");
            Description = Console.ReadLine();
            Console.Write("Enter amount of course units: ");
            bool result1 = int.TryParse(Console.ReadLine(), out CourseUnits);
            if (!result1)
            {
                Console.WriteLine("\nIncorrect Entry...\nPress Any Key to go back");
                Console.ReadKey();
                CoursesPage();
            }
            Console.Write("Enter maximum amount of students in the course: ");
            bool result2 = int.TryParse(Console.ReadLine(), out MaxCourseStudents);
            if (!result2)
            {
                Console.WriteLine("\nIncorrect Entry...\nPress Any Key to go back");
                Console.ReadKey();
                CoursesPage();
            }
            Courses course = new Courses(Course, Description, CourseUnits, MaxCourseStudents);
            Database.Add(course);
            Console.Clear();
            Console.WriteLine("Course has been added successfully");
            //Redirecting back to courses page
            CoursesPage();
        }

        public static void EditCoursePage(int IndexOfCourseBeingEdited)
        {
            Console.Clear();
            string Choice = "";
            ShowURL();
            CustomList<Section> AvailableSections = new CustomList<Section>();
            Console.WriteLine("Course: " + Database[IndexOfCourseBeingEdited].Course);
            Console.WriteLine("Description: " + Database[IndexOfCourseBeingEdited].Description);
            Console.WriteLine("Course Units: " + Database[IndexOfCourseBeingEdited].CourseUnits.ToString());
            Console.WriteLine("Number of Students Enrolled: " + Database[IndexOfCourseBeingEdited].studentsEnrolled.Count().ToString() + "/" + Database[IndexOfCourseBeingEdited].MaxCourseStudents.ToString() + "\n-----------------------------------------------------------------\n");
            if (Database[IndexOfCourseBeingEdited].Sections.Count() == 0)
                Console.WriteLine("There are no sections for the course...");
            else
            {

                for (int i = 0; i < Database[IndexOfCourseBeingEdited].Sections.Count(); i++)
                {
                    string TimeSlot = "";
                    if (Database[IndexOfCourseBeingEdited].Sections[i].TimeSlot == "1")
                        TimeSlot = "8am to 11am";
                    if (Database[IndexOfCourseBeingEdited].Sections[i].TimeSlot == "2")
                        TimeSlot = "11am to 2pm";
                    if (Database[IndexOfCourseBeingEdited].Sections[i].TimeSlot == "3")
                        TimeSlot = "2pm to 5pm";
                    if (Database[IndexOfCourseBeingEdited].Sections[i].TimeSlot == "4")
                        TimeSlot = "5pm to 8pm";
                    Console.WriteLine(
                        "[" + (i+1) + "] " + Database[IndexOfCourseBeingEdited].Sections[i].Day + "\t" +
                        TimeSlot + "\t" +
                        Database[IndexOfCourseBeingEdited].Sections[i].SectionCode + "\t" +
                        Database[IndexOfCourseBeingEdited].Sections[i].Room + "\t" +
                        Database[IndexOfCourseBeingEdited].Sections[i].Professor + "\t" +
                        Database[IndexOfCourseBeingEdited].Sections[i].StudentsInSection.Count() + "/" + Database[IndexOfCourseBeingEdited].Sections[i].MaxStudentsInSection
                        );
                    AvailableSections.Add(Database[IndexOfCourseBeingEdited].Sections[i]);
                }
            }

            Console.WriteLine("\n-----------------------------------------------------------------\n");
            Console.WriteLine("[#] Edit a section\n[-] Add a section\n[=] Delete a section\n[5] Remove a student\n[;] Edit Course\n[0] Back");
            Console.Write("Enter Action: ");
            Choice = Console.ReadLine();
            int temp = 0;
            bool Result = Int32.TryParse(Choice, out temp);
            if (Choice == "-")
            {
                AddSectionPage(IndexOfCourseBeingEdited);
            }
            else if (Choice == "=")
            {
                if (Database[IndexOfCourseBeingEdited].Sections.Count() <= 0)
                {
                    Console.WriteLine("\nThere are no sections you can delete\nPress any key to refresh the page...");
                    Console.ReadKey();
                    EditCoursePage(IndexOfCourseBeingEdited);
                }
                else
                {
                    Console.Write("\nEnter # of section to delete: ");
                    bool Result2 = Int32.TryParse(Console.ReadLine(), out temp);
                    if (Result2)
                    {
                        if (temp > 0 && temp <= Database[IndexOfCourseBeingEdited].Sections.Count())
                            Database[IndexOfCourseBeingEdited].Sections.RemoveAt(temp - 1);
                        EditCoursePage(IndexOfCourseBeingEdited);
                    }
                    else if (!Result2)
                    {
                        Console.WriteLine("\nIncorrect entry\nPress any key to refresh...");
                        Console.ReadKey();
                        EditCoursePage(IndexOfCourseBeingEdited);
                    }
                }
            }
            else if (temp == 5)
            {
                RemoveStudentPage(IndexOfCourseBeingEdited);
            }
            else if (temp > 0 && temp <= 4)
            {
                if (temp <= Database[IndexOfCourseBeingEdited].Sections.Count())
                {
                    EditSectionPage(IndexOfCourseBeingEdited, temp - 1);
                }
                else
                {
                    EditCoursePage(IndexOfCourseBeingEdited);
                }
            }
            else if (Choice == "0")
            {
                CoursesPage();
            }
            else if (Choice == ";")
            {
                Console.Clear();
                ShowURL();
                Console.WriteLine("[1] Course Name: " + Database[IndexOfCourseBeingEdited].Course);
                Console.WriteLine("[2] Course Description: " + Database[IndexOfCourseBeingEdited].Description);
                Console.WriteLine("[3] Course Units: " + Database[IndexOfCourseBeingEdited].CourseUnits.ToString());
                Console.WriteLine("[4] Maximum Amount of Students: " + Database[IndexOfCourseBeingEdited].MaxCourseStudents.ToString() + "\n");
                Console.Write("[#] Field to edit\n[0] Back\nEnter Action: ");
                for (int i = 0; i < StudentRequests.GetQueue().Length; i++)
                {
                    Console.WriteLine(StudentRequests.GetQueue()[i].Course + StudentRequests.GetQueue()[i].Student + StudentRequests.GetQueue()[i].Reason);
                }
                bool Result2 = (Int32.TryParse(Console.ReadLine(), out temp));
                if (Result2)
                {
                    if (temp == 1)
                    {
                        string PreviousNameOfCourse = Database[IndexOfCourseBeingEdited].Course;
                        //change course name
                        Console.Write("\nEnter new course name: ");
                        Choice = Console.ReadLine();
                        //Check if there already is a course with the same name
                        for (int i = 0; i < Database.Count(); i++)
                        {
                            if (Database[i].Course == Choice)
                            {
                                Console.WriteLine("\nThere already exists a course with the same name\nPress any key to go back...");
                                Console.ReadKey();
                                EditCoursePage(IndexOfCourseBeingEdited);
                            }
                        }
                        Database[IndexOfCourseBeingEdited].Course = Choice;
                        //sync student requests
                        for (int i = 0; i < StudentRequests.Count(); i++)
                            if (StudentRequests[i].Course == PreviousNameOfCourse)
                                StudentRequests[i].Course = Choice;
                        EditCoursePage(IndexOfCourseBeingEdited);
                    }
                    else if (temp == 2)
                    {
                        Console.Write("\nEnter new course description: ");
                        Database[IndexOfCourseBeingEdited].Description = Console.ReadLine();
                        EditCoursePage(IndexOfCourseBeingEdited);
                    }
                    else if (temp == 3)
                    {
                        Console.Write("\nEnter new amount of course units: ");
                        bool Result3 = Int32.TryParse(Console.ReadLine(), out temp);
                        if (Result3)
                        {
                            Database[IndexOfCourseBeingEdited].CourseUnits = temp;
                        }
                        EditCoursePage(IndexOfCourseBeingEdited);
                    }
                    else if (temp == 4)
                    {
                        Console.Write("\nEnter new amount of course's max student amount: ");
                        bool Result3 = Int32.TryParse(Console.ReadLine(), out temp);
                        if (Result3)
                        {
                            //get maximum students right now as per section capacity and check if it is larger than what you set
                            int CurrentSectionsMaxStudentsSummed = 0;
                            for (int i = 0; i < Database[IndexOfCourseBeingEdited].Sections.Count(); i++)
                                CurrentSectionsMaxStudentsSummed += Database[IndexOfCourseBeingEdited].Sections[i].MaxStudentsInSection;
                            if (temp > -1 && temp >= CurrentSectionsMaxStudentsSummed)
                                Database[IndexOfCourseBeingEdited].MaxCourseStudents = temp;
                            else
                            {
                                Console.WriteLine("\nThe sum of maximum students per section is greater than the course's new maximum student amount\nPress any key to go back...");
                                Console.ReadKey();
                                EditCoursePage(IndexOfCourseBeingEdited);
                            }
                        }
                        else
                        {
                            Console.WriteLine("\nIncorrect Entry\nPress any key to go back...");
                            Console.ReadKey();
                            EditCoursePage(IndexOfCourseBeingEdited);
                        }
                        EditCoursePage(IndexOfCourseBeingEdited);
                    }
                    else
                    {
                        EditCoursePage(IndexOfCourseBeingEdited);
                    }
                }
                else
                {
                    EditCoursePage(IndexOfCourseBeingEdited);
                }
            }
            else
            {
                EditCoursePage(IndexOfCourseBeingEdited);
            }
        }
        public static void AddSectionPage(int IndexOfCourseBeingEdited)
        {
            Console.Clear();
            CustomList<string> AvailableTimeSlots = new CustomList<string>();
            string Day = "", TimeSlot = "", SectionCode, Room, Professor, temp;
            int MaxAmountOfStudents, SectionMaxStudentsSum = 0;
            Console.Write("Select Day (MTWHF): ");
            temp = Console.ReadLine();
            if (temp != "M" && temp != "T" && temp != "W" && temp != "H" && temp != "F")
            {
                Console.WriteLine("Incorrect Entry\nPress any key to go back...");
                Console.ReadKey();
                EditCoursePage(IndexOfCourseBeingEdited);
            }
            else
                Day = temp;
            Console.Write("Enter Section Code: ");
            SectionCode = Console.ReadLine();
            Console.Write("[1] 8am to 11am\n[2] 11am to 2pm\n[3] 2pm to 5pm\n[4] 5pm to 8pm\nSelect Time Slot: ");
            temp = Console.ReadLine();
            if (temp != "1" && temp != "2" && temp != "3" && temp != "4")
            {
                Console.WriteLine("Incorrect Entry\nPress any key to go back...");
                Console.ReadKey();
                EditCoursePage(IndexOfCourseBeingEdited);
            }
            else
                TimeSlot = temp;
            Console.Write("Enter Room: ");
            Room = Console.ReadLine();
            Console.Write("Enter Professor: ");
            Professor = Console.ReadLine();
            Console.Write("Enter Maximum Amount of Students: ");
            bool Result = int.TryParse(Console.ReadLine(), out MaxAmountOfStudents);
            if (!Result || MaxAmountOfStudents < 0)
            {
                Console.WriteLine("\nIncorrect Entry\nPress any key to go back...");
                Console.ReadKey();
                EditCoursePage(IndexOfCourseBeingEdited);
            }
            else
            {
                if (MaxAmountOfStudents > Database[IndexOfCourseBeingEdited].MaxCourseStudents)
                {
                    Console.WriteLine("\nMaximum amount of students in a section cannot be larger than the maximum amount of students in the course\nPress any key to go back...");
                    Console.ReadKey();
                    EditCoursePage(IndexOfCourseBeingEdited);
                }
                for (int i = 0; i < Database[IndexOfCourseBeingEdited].Sections.Count(); i++)
                    SectionMaxStudentsSum += Database[IndexOfCourseBeingEdited].Sections[i].MaxStudentsInSection;
                if (SectionMaxStudentsSum + MaxAmountOfStudents > Database[IndexOfCourseBeingEdited].MaxCourseStudents)
                {
                    Console.WriteLine("\nChosen amount of students will exceed maximum amount of students allowed in course\nPress any key to go back...");
                    Console.ReadKey();
                    EditCoursePage(IndexOfCourseBeingEdited);
                }
                else
                {
                    //Checking Overlap
                    Section tempSection = new Section(Day, TimeSlot, SectionCode, Room, Professor, MaxAmountOfStudents);
                    for (int i = 0; i < Database.Count(); i++)
                    {
                        if (Database[i].IsOverlappingSection(tempSection))
                        {
                            Console.WriteLine("Press any key to go back...");
                            Console.ReadKey();
                            EditCoursePage(IndexOfCourseBeingEdited);
                        };
                    }
                    Database[IndexOfCourseBeingEdited].Sections.Add(tempSection);
                    EditCoursePage(IndexOfCourseBeingEdited);
                }
            }
        }
        public static void EditSectionPage(int IndexOfCourseBeingEdited, int SectionId)
        {
            Console.Clear();

            ShowURL();

            int Choice = 0;
            string temp = "", Time = "";
            Section tempSection = new Section(Database[IndexOfCourseBeingEdited].Sections[SectionId].Day, Database[IndexOfCourseBeingEdited].Sections[SectionId].TimeSlot, Database[IndexOfCourseBeingEdited].Sections[SectionId].SectionCode, Database[IndexOfCourseBeingEdited].Sections[SectionId].Room, Database[IndexOfCourseBeingEdited].Sections[SectionId].Professor, Database[IndexOfCourseBeingEdited].Sections[SectionId].MaxStudentsInSection);
            if (tempSection.TimeSlot == "1")
                Time = "8am to 11am";
            if (tempSection.TimeSlot == "2")
                Time = "11am to 2pm";
            if (tempSection.TimeSlot == "3")
                Time = "2pm to 5pm";
            if (tempSection.TimeSlot == "4")
                Time = "5pm to 8pm";
            Console.WriteLine("[1] Day: " + Database[IndexOfCourseBeingEdited].Sections[SectionId].Day
                + "\n[2] Section: " + Database[IndexOfCourseBeingEdited].Sections[SectionId].SectionCode
                + "\n[3] Time Slot: " + Time
                + "\n[4] Room: " + Database[IndexOfCourseBeingEdited].Sections[SectionId].Room
                + "\n[5] Professor: " + Database[IndexOfCourseBeingEdited].Sections[SectionId].Professor
                + "\n[6] Maximum Students Allowed: " + Database[IndexOfCourseBeingEdited].Sections[SectionId].MaxStudentsInSection.ToString());

            Console.WriteLine("\n[#] Field to edit\n[0] Back");
            Console.Write("Enter Action: ");
            bool Result = Int32.TryParse(Console.ReadLine(), out Choice);
            if (!Result)
                EditSectionPage(IndexOfCourseBeingEdited, SectionId);
            else
            {
                if (Choice == 1)
                {
                    Console.Write("\nEnter New Day: ");
                    temp = Console.ReadLine();
                    // Temporarily store the section to be edited
                    Section tempSection2 = Database[IndexOfCourseBeingEdited].Sections[SectionId];
                    // Temporarily remove the section from the course
                    Database[IndexOfCourseBeingEdited].Sections[SectionId] = null;
                    // Update the day of the tempSection
                    tempSection.Day = temp;
                    bool hasOverlap = false;
                    // Check for overlaps in all sections of all courses
                    for (int i = 0; i < Database.Count(); i++)
                        if (Database[i].IsOverlappingSection(tempSection))
                        {
                            hasOverlap = true;
                            break;
                        }
                    // Restore the original section
                    Database[IndexOfCourseBeingEdited].Sections[SectionId] = tempSection2;
                    if (hasOverlap)
                    {
                        Console.WriteLine("\nOverlap Detected\nPress any key to go back...");
                        Console.ReadKey();
                        EditCoursePage(IndexOfCourseBeingEdited);
                    }
                    else
                    {
                        Database[IndexOfCourseBeingEdited].Sections[SectionId] = tempSection;
                        EditCoursePage(IndexOfCourseBeingEdited);
                    }
                }
                else if (Choice == 2)
                {
                    Console.Write("\nEnter New Section: ");
                    temp = Console.ReadLine();
                    // Temporarily store the section to be edited
                    Section tempSection2 = Database[IndexOfCourseBeingEdited].Sections[SectionId];
                    // Temporarily remove the section from the course
                    Database[IndexOfCourseBeingEdited].Sections[SectionId] = null;
                    // Update the day of the tempSection
                    tempSection.SectionCode = temp;
                    bool hasOverlap = false;
                    // Check for overlaps in all sections of all courses
                    for (int i = 0; i < Database.Count(); i++)
                        if (Database[i].IsOverlappingSection(tempSection))
                        {
                            hasOverlap = true;
                            break;
                        }
                    // Restore the original section
                    Database[IndexOfCourseBeingEdited].Sections[SectionId] = tempSection2;
                    if (hasOverlap)
                    {
                        Console.WriteLine("\nOverlap Detected\nPress any key to go back...");
                        Console.ReadKey();
                        EditCoursePage(IndexOfCourseBeingEdited);
                    }
                    else
                    {
                        Database[IndexOfCourseBeingEdited].Sections[SectionId] = tempSection;
                        EditCoursePage(IndexOfCourseBeingEdited);
                    }
                }
                else if (Choice == 3)
                {
                    Console.Write("\nEnter New Time Slot (WARNING: SYSTEM WILL REMOVE ALL STUDENTS FROM SECTIONS TO PREVENT SCHEDULE CONFLICT)\n([1] 8am - 11am, [2] 11am to 2pm, [3] 2pm to 5pm, [4] 5pm to 8pm: ");
                    temp = Console.ReadLine();
                    // Temporarily store the section to be edited
                    Section tempSection2 = Database[IndexOfCourseBeingEdited].Sections[SectionId];
                    // Temporarily remove the section from the course
                    Database[IndexOfCourseBeingEdited].Sections[SectionId] = null;
                    // Update the day of the tempSection
                    tempSection.TimeSlot = temp;
                    bool hasOverlap = false;
                    // Check for overlaps in all sections of all courses
                    for (int i = 0; i < Database.Count(); i++)
                        if (Database[i].IsOverlappingSection(tempSection))
                        {
                            hasOverlap = true;
                            break;
                        }
                    // Restore the original section
                    Database[IndexOfCourseBeingEdited].Sections[SectionId] = tempSection2;
                    if (hasOverlap)
                    {
                        Console.WriteLine("\nOverlap Detected\nPress any key to go back...");
                        Console.ReadKey();
                        EditCoursePage(IndexOfCourseBeingEdited);
                    }
                    else
                    {
                        Database[IndexOfCourseBeingEdited].Sections[SectionId].StudentsInSection.Clear();
                        Database[IndexOfCourseBeingEdited].Sections[SectionId] = tempSection;
                        EditCoursePage(IndexOfCourseBeingEdited);
                    }
                }
                else if (Choice == 4)
                {
                    Console.Write("\nEnter New Room: ");
                    temp = Console.ReadLine();
                    // Temporarily store the section to be edited
                    Section tempSection2 = Database[IndexOfCourseBeingEdited].Sections[SectionId];
                    // Temporarily remove the section from the course
                    Database[IndexOfCourseBeingEdited].Sections[SectionId] = null;
                    // Update the day of the tempSection
                    tempSection.Room = temp;
                    bool hasOverlap = false;
                    // Check for overlaps in all sections of all courses
                    for (int i = 0; i < Database.Count(); i++)
                        if (Database[i].IsOverlappingSection(tempSection))
                        {
                            hasOverlap = true;
                            break;
                        }
                    // Restore the original section
                    Database[IndexOfCourseBeingEdited].Sections[SectionId] = tempSection2;
                    if (hasOverlap)
                    {
                        Console.WriteLine("\nOverlap Detected\nPress any key to go back...");
                        Console.ReadKey();
                        EditCoursePage(IndexOfCourseBeingEdited);
                    }
                    else
                    {
                        Database[IndexOfCourseBeingEdited].Sections[SectionId] = tempSection;
                        EditCoursePage(IndexOfCourseBeingEdited);
                    }
                }
                else if (Choice == 5)
                {
                    Console.Write("\nEnter New Professor: ");
                    temp = Console.ReadLine();
                    // Temporarily store the section to be edited
                    Section tempSection2 = Database[IndexOfCourseBeingEdited].Sections[SectionId];
                    // Temporarily remove the section from the course
                    Database[IndexOfCourseBeingEdited].Sections[SectionId] = null;
                    // Update the day of the tempSection
                    tempSection.Professor = temp;
                    bool hasOverlap = false;
                    // Check for overlaps in all sections of all courses
                    for (int i = 0; i < Database.Count(); i++)
                        if (Database[i].IsOverlappingSection(tempSection))
                        {
                            hasOverlap = true;
                            break;
                        }
                    // Restore the original section
                    Database[IndexOfCourseBeingEdited].Sections[SectionId] = tempSection2;
                    if (hasOverlap)
                    {
                        Console.WriteLine("\nOverlap Detected\nPress any key to go back...");
                        Console.ReadKey();
                        EditCoursePage(IndexOfCourseBeingEdited);
                    }
                    else
                    {
                        Database[IndexOfCourseBeingEdited].Sections[SectionId] = tempSection;
                        EditCoursePage(IndexOfCourseBeingEdited);
                    }
                }
                else if (Choice == 6)
                {
                    Console.Write("\nEnter New Maximum amount of students in the section: ");
                    int temp2 = 0, CurrentSectionMax = 0;
                    bool Result2 = Int32.TryParse(Console.ReadLine(), out temp2);
                    for (int i = 0; i < Database[IndexOfCourseBeingEdited].Sections.Count(); i++)
                    {
                        CurrentSectionMax += Database[IndexOfCourseBeingEdited].Sections[i].MaxStudentsInSection;
                    }
                    if (Result2)
                    {
                        if (temp2 < Database[IndexOfCourseBeingEdited].Sections[SectionId].StudentsInSection.Count() || temp2 + CurrentSectionMax > Database[IndexOfCourseBeingEdited].MaxCourseStudents)
                        {
                            Console.Write("\nThere are more students already in the section than your desired amount\nPress any key to go back...");
                            Console.ReadKey();
                            EditCoursePage(IndexOfCourseBeingEdited);
                        }
                        else
                        {
                            Database[IndexOfCourseBeingEdited].Sections[SectionId].MaxStudentsInSection = temp2;
                            EditCoursePage(IndexOfCourseBeingEdited);
                        }
                    }
                }
                else if (Choice == 0)
                {
                    EditCoursePage(IndexOfCourseBeingEdited);
                }
            }
        }
        public static void RemoveStudentPage(int IndexOfCourseBeingEdited)
        {
            Console.Clear();
            ShowURL();

            int temp = 0;
            string StudentToRemove = "";
            if (Database[IndexOfCourseBeingEdited].studentsEnrolled.Count() == 0)
            {
                Console.WriteLine("There are no students Enrolled in the course\nPress any key to go back...");
                Console.ReadKey();
                EditCoursePage(IndexOfCourseBeingEdited);
            }
            for (int i = 0; i < Database[IndexOfCourseBeingEdited].studentsEnrolled.Count(); i++)
                Console.WriteLine("[" + (i + 1) + "] " + Database[IndexOfCourseBeingEdited].studentsEnrolled[i]);
            Console.WriteLine("\n[#] Remove Student\n[0] Back");
            Console.Write("Enter Action: ");
            bool Result = int.TryParse(Console.ReadLine(), out temp);
            if (temp == 0)
                EditCoursePage(IndexOfCourseBeingEdited);
            else if (temp >= 1 && temp <= Database[IndexOfCourseBeingEdited].studentsEnrolled.Count())
            {
                StudentToRemove = Database[IndexOfCourseBeingEdited].studentsEnrolled[temp - 1];
                Database[IndexOfCourseBeingEdited].studentsEnrolled.Remove(StudentToRemove);
                for (int i = 0; i < Database[IndexOfCourseBeingEdited].Sections.Count(); i++)
                    Database[IndexOfCourseBeingEdited].Sections[i].StudentsInSection.Remove(StudentToRemove);
            }
            else
            {
                Console.WriteLine("\nIncorrect Entry. Press any key to go back...");
                Console.ReadKey();
                EditCoursePage(IndexOfCourseBeingEdited);
            }
        }

    }
}
