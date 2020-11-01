using System;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Enclosing all classes in a CommunityLibrary namespace allows for easy
/// reuse in other projects
/// </summary>

namespace CommunityLibrary
{
    /// <summary>
    /// Date object is implimented here rather than using one of the built in
    /// types. The fidelity of the object is sufficient for our needs (movie release
    /// dates) and behaves externally as if it is a string in the format DD/MM/YYYY
    /// for fast data entry. Extends IComparable so that statements like the
    /// following can be written:
    /// (new Date("01/01/1970") < new Date("28/10/2020")) == true;
    /// </summary>

    public class Date : IComparable
    {
        protected int day;
        protected int month;
        protected int year;

        public Date(string date)
        {
            try
            {
                // A Date is a string in the format of DD/MM/YYYY
                if (date.Length != 10)
                    throw new ArgumentException("Invalid date");

                day = Int32.Parse(date.Substring(0, 2));
                month = Int32.Parse(date.Substring(3, 2));
                year = Int32.Parse(date.Substring(6, 4));

                if (day < 1 || day > 31)
                    throw new ArgumentException("Invalid date");
                if (month < 1 || month > 12)
                    throw new ArgumentException("Invalid date");
                if (year < 1850 || year > 2250) // Assumptions based on longevity of Library
                    throw new ArgumentException("Invalid date");

                if (month == 4 && day > 30) // April
                    throw new ArgumentException("Invalid date");
                if (month == 6 && day > 30) // June
                    throw new ArgumentException("Invalid date");
                if (month == 9 && day > 30) // September
                    throw new ArgumentException("Invalid date");
                if (month == 11 && day > 30) // November
                    throw new ArgumentException("Invalid date");

                if ((year % 4) > 0 && (year % 100) != 0 && month == 2 && day > 29) // Leap Years 
                    throw new ArgumentException("Invalid date");
                if (month == 2 && day > 28) //February
                    throw new ArgumentException("Invalid date");
            }
            catch (FormatException)
            {
                throw new ArgumentException("Invalid date");
            }
        }

        public override string ToString()
        {
            string[] months = {"January", "February", "March", "April",
                               "May", "June", "July", "August", "September",
                               "October", "November", "December" };
            string returnstring = "";

            returnstring += day.ToString();

            if (day == 1)
                returnstring += "st";
            if (day == 3)
                returnstring += "rd";
            if (day == 21)
                returnstring += "st";
            if (day == 23)
                returnstring += "rd";
            if (day == 31)
                returnstring += "st";

            if (returnstring.Length < 3)
                returnstring += "th";

            returnstring += " of ";

            returnstring += months[month - 1];

            returnstring += ", ";

            returnstring += year.ToString();

            return returnstring;
        }

        public override bool Equals(object other)
        {
            if (other.GetType() == this.GetType())
            {
                var otherDate = other as Date;
                return otherDate.day == this.day &&
                       otherDate.month == this.month &&
                       otherDate.year == this.year;
            }
            return false;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public int CompareTo(object other)
        {
            if (other == null)
                return 1;

            Date otherDate = other as Date;

            // Check Year, then Month, then Day
            if (otherDate.year == this.year)
            {
                if (otherDate.month == this.month)
                    return this.day.CompareTo(otherDate.day);
                else
                    return this.month.CompareTo(otherDate.month);
            }
            else
                return this.year.CompareTo(otherDate.year);
        }

        public static bool operator ==(Date self, Date other)
        {
            return self.Equals(other);
        }

        public static bool operator !=(Date self, Date other)
        {
            return !(self.Equals(other));
        }

        public static bool operator >(Date self, Date other)
        {
            return self.CompareTo(other) > 0;
        }

        public static bool operator <(Date self, Date other)
        {
            return self.CompareTo(other) < 0;
        }

        public static bool operator >=(Date self, Date other)
        {
            return self > other || self == other;
        }

        public static bool operator <=(Date self, Date other)
        {
            return self < other || self == other;
        }
    }


    /// <summary>
    /// Generic Media object extends IComparable. This implimentation allows
    /// media to be sorted by the title, and could be extended to Media beyond DVDs
    /// </summary>

    public class Media : IComparable
    {
        public string title;

        public Media(string title)
        {
            this.title = title;
        }

        public override string ToString()
        {
            return this.title;
        }

        public override bool Equals(object other)
        {
            if (other.GetType() == this.GetType())
            {
                var otherMedia = other as Media;
                return otherMedia.title.ToUpper() == this.title.ToUpper();
            }

            return false;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public int CompareTo(object other)
        {
            if (other == null)
                return 1;

            return this.ToString().CompareTo(other.ToString());
        }

        public static bool operator ==(Media self, Media other)
        {
            if (self is null && other is null)
                return true;
            if (other is null)
                return false;
            return self.Equals(other);
        }

        public static bool operator !=(Media self, Media other)
        {
            if (self is null && other is null)
                return false;
            if (other is null)
                return true;
            return !(self.Equals(other));
        }

        public static bool operator >(Media self, Media other)
        {
            // A Media item is considered greater than another one
            // if - when sorted alphabetically - the title is
            // later ("follows") in the sort order. Note that later
            // alpha characters have a higher ASCII value than sooner!
            return self.CompareTo(other) > 0;
        }

        public static bool operator <(Media self, Media other)
        {
            // A Media item is considered less than another one
            // if - when sorted alphabetically - the title is
            // sooner ("preceeds") in the sort order. Note that later
            // alpha characters have a higher ASCII value than sooner!
            return self.CompareTo(other) < 0; ;
        }
    }


    /// <summary>
    /// Generic Person class, extends IComparable. Allows items to be stored in
    /// a SortedLinkedList. Each Person has a unique GUID to ensure there is no
    /// collision for people sharing the same name
    /// </summary>

    public class Person : IComparable
    {
        public string firstName;
        public string lastName;
        Guid personID;

        public Person(string firstName, string lastName)
        {
            this.firstName = firstName;
            this.lastName = lastName;
            this.personID = Guid.NewGuid();
        }

        public override string ToString()
        {
            return lastName + ", " + firstName;
        }

        public override bool Equals(object other)
        {
            if (other.GetType() == this.GetType())
            {
                var otherPerson = other as Person;
                return otherPerson.personID == this.personID;
            }

            return false;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public int CompareTo(object other)
        {
            if (other == null)
                return 1;

            var otherPerson = other as Person;

            // A Person is ordered based on last name first, then first name
            if (this.Equals(otherPerson))
                return 0;
            if (this.lastName.CompareTo(otherPerson.lastName) < 0)
                return -1;
            if (this.lastName.CompareTo(otherPerson.lastName) > 0)
                return 1;
            // If program flow falls through to here, the last name is the same
            // Order based on first name. If people have the same first
            // name but different personIDs, this Person preceeds the other Person
            if (this.firstName.CompareTo(otherPerson.firstName) == 0)
                return -1;
            return this.firstName.CompareTo(otherPerson.firstName);
        }
    }


    /// <summary>
    /// Generic Linked List node with the generic datatype. Extends IDisposable
    /// to allow for efficient garbage collection when the node is removed from
    /// the list, or the list is otherwise destroyed. Note that this node has fields
    /// for previous and next making it a doubly-linked node, and also a quantity
    /// field that allows a number of non-unique items in the list to be stored
    /// in a single node
    /// </summary>

    public class Node<T> : IDisposable
    {
        public T nodeObject;
        public Node<T> previous;
        public Node<T> next;
        public int quantity = 1;

        public Node(T nodeObject, Node<T> previous, Node<T> next)
        {
            this.nodeObject = nodeObject;
            this.previous = previous;
            this.next = next;
        }

        // Required for IDisposable objects
        public void Dispose()
        {
            this.nodeObject = default(T);
            this.previous = null;
            this.next = null;
        }
    }


    /// <summary>
    /// Basic Linked List extends the IEnumerable class with a generic object
    /// type. This allows for fast sequentil list access using an enumerator
    /// the the C# built-in foreach() method
    /// </summary>

    public class LinkedList<T> : IEnumerable<T>
    {
        // Impliments a doubly-linked list
        public Node<T> head = null;
        public Node<T> tail = null;
        public Node<T> current = null;
        public int Length = 0;

        public LinkedList() { }

        public Node<T> Previous()
        {
            return current.previous;
        }

        public Node<T> Next()
        {
            return current.next;
        }

        public virtual void Add(T obj)
        {
            // 'Add' operation adds to the end. Basic LinkedList acts like a stack
            Node<T> previous = null;
            if (this.tail != null)
                previous = this.tail;

            Node<T> newNode = new Node<T>(obj, previous, null);

            if (this.Length == 0) // Empty LinkedList
            {
                this.head = newNode;
                this.tail = newNode;
                this.current = newNode;
            }
            else
            {
                Node<T> testObject = this.Find(obj);
                if (testObject != null)
                {
                    testObject.quantity += 1;
                }
                else
                {
                    newNode.previous = this.tail;
                    this.tail.next = newNode;
                    this.tail = newNode;
                }
            }

            this.Length += 1;
        }

        public virtual void Remove(T obj)
        {
            // Generic class can only remove from the end. Basic LinkedList acts like a stack
            if (this.Length > 0) // Cannot remove objects when there are no objects
            {
                this.tail.previous.next = null; // Garbage collection will destroy the object
                this.Length -= 1;
            }
        }

        public virtual bool Contains(T obj)
        {
            foreach (T element in this)
            {
                if (element.Equals(obj))
                    return true;
            }
            return false;
        }

        public virtual Node<T> Find(T obj)
        {
            if (this.Contains(obj))
                return this.current;
            return null;
        }

        public virtual int GetQuantity(T obj)
        {
            Node<T> testObject = this.Find(obj);
            if (testObject != null)
                return testObject.quantity;
            return 0;
        }

        public IEnumerator<T> GetEnumerator()
        {
            this.current = this.head;

            while (this.current != null)
            {
                yield return this.current.nodeObject;
                this.current = this.current.next;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
        }
    }


    /// <summary>
    /// Generic implimentation of a sorted doubly-linked list. Takes any
    /// object that extends IComparable; that is, any object that can be
    /// compared to another with the CompareTo() method. Uses this method
    /// to conduct sorted insertions. On a worst case scenario (the object
    /// is second last in the linked list), the list is traversed twice: once
    /// to check whether the object already exists in the list, and secondly
    /// to check where in the list it should be inserted. This is considered
    /// acceptable for the target use-case (a community library). The list would
    /// have a (presumed) number of entries between 10**2 and 10**5. 
    /// </summary>

    public class SortedLinkedList<T> : LinkedList<T> where T : IComparable
    {
        public SortedLinkedList() : base() { }

        public void Insert(T obj)
        {
            if (this.Length == 0)
            {
                // List is empty. First object can be added as normal
                this.Add(obj);
                return;
            }

            if (this.Contains(obj))
            {
                // Object already in list. Call 'Add' to increase object count
                this.Add(obj);
                return;
            }

            if (obj.CompareTo(this.tail.nodeObject) > 0)
            {
                // Object belongs at the end of the list. Add there
                this.Add(obj);
                return;
            }

            if (obj.CompareTo(this.head.nodeObject) < 0)
            {
                // Object belongs at beginning of list
                Node<T> newNode = new Node<T>(obj, null, this.head);
                this.head.previous = newNode;
                this.head = newNode;
                return;
            }

            // If control falls through to here, we haven't been able to add the
            // object. Iterate through the list until we find where it belongs
            foreach (T testObject in this)
            {
                if (obj.CompareTo(testObject) < 0)
                {
                    // Next object follows new object. Insert object in this location
                    Node<T> previous = this.current.previous;
                    Node<T> next = this.current;

                    Node<T> newNode = new Node<T>(obj, previous, next);
                    next.previous = newNode;
                    previous.next = newNode;
                    return;
                }
            }
        }

        public override void Remove(T obj)
        {
            Node<T> testObject = this.Find(obj);

            if (testObject == null)
                throw new ArgumentException("Object not in List");

            if (testObject.quantity <= 0)
                throw new ArgumentException("No Objects left to remove");
            else
                testObject.quantity -= 1;
        }
    }


    /// <summary>
    /// The DVD Object extends the Media object and includes addition DVD
    /// specific fields. Includes error handling for invalid Genre or
    /// classification. Note Genre and Classification are case sensitive by design
    /// </summary>

    public class DVD : Media
    {
        protected string[] validGenres = {"Drama", "Adventure", "Family",
                                          "Action", "Sci-Fi", "Comedy",
                                          "Animated", "Thriller", "Other" };

        protected string[] validClassifications = {"General(G)",
                                                   "Parental Guidance(PG)",
                                                   "Mature(M15+)",
                                                   "Mature Accompanied(MA15+)" };
        public string[] starring;
        public string director;
        public int duration;
        public string[] genres;
        public string classification;
        public Date releaseDate;

        public DVD(string title,
                   string[] starring = null,
                   string director = null,
                   int duration = 0,
                   string[] genres = null,
                   string classification = null,
                   Date releaseDate = null) : base(title)
        {
            if (genres != null)
            {
                foreach (string genre in genres)
                {
                    if (!(Array.Exists(validGenres, element => element == genre)))
                        throw new ArgumentException("Invalid genre");
                }
            }

            if (classification != null)
            {
                if (!(Array.Exists(validClassifications, element => element == classification)))
                    throw new ArgumentException("Invalid classification");
            }

            this.starring = starring;
            this.director = director;
            this.duration = duration;
            this.genres = genres;
            this.classification = classification;
            this.releaseDate = releaseDate;
        }
    }


    /// <summary>
    /// Extends SortedLinkedList for a doubly-linked list of DVD objects
    /// sorted by title. The objects are sorted at insertion allowing for
    /// minimal additional sorting overhead
    /// </summary>

    public class SortedDVDLibrary : SortedLinkedList<DVD>
    {
        public SortedDVDLibrary() : base() { }

        public override string ToString()
        {
            string returnString = "";
            int padding = 40;

            foreach (DVD dvd in this)
            {
                int quantity = this.GetQuantity(dvd);

                returnString += dvd.ToString().PadRight(padding);

                if (quantity < 10)
                    returnString += " ";

                returnString += quantity.ToString();

                if (quantity == 1)
                {
                    returnString += " item  available\n";
                }
                else
                {
                    returnString += " items available\n";
                }
            }
            return returnString;
        }
    }


    /// <summary>
    /// Class for a Customer extends the Person class. Additional customer
    /// specific fields are included, such as contact number and dvds
    /// that the customer has borrowed
    /// </summary>

    public class Customer : Person
    {
        public string contactNumber;
        public SortedDVDLibrary borrowedDVDs;

        public Customer(string firstName, string lastName, string contactNumber) :
            base (firstName, lastName)
        {
            this.contactNumber = contactNumber;
            this.borrowedDVDs = new SortedDVDLibrary();
        }

        public void Borrow(SortedDVDLibrary library, DVD dvd)
        {
            library.Remove(dvd);
            this.borrowedDVDs.Insert(dvd);
        }

        public void Return(SortedDVDLibrary library, DVD dvd)
        {
            this.borrowedDVDs.Remove(dvd);
            library.Insert(dvd);
        }
    }


    /// <summary>
    /// Customer Database extends SortedLinkedList with a doubly-linked list
    /// of Customer objects, sorted by customer surname
    /// </summary>


    public class CustomerDatabase : SortedLinkedList<Customer>
    {
        public CustomerDatabase() : base() { }

        public override string ToString()
        {
            string returnString = "";
            int padding = 40;

            foreach (Customer customer in this)
            {
                returnString += customer.ToString().PadRight(padding);
                returnString += customer.contactNumber + "\n";

            }
            return returnString;
        }
    }


    /// <summary>
    /// Impliments the UI of the library, including methods to List library
    /// items and customers, and manage the databases (represented by
    /// SortedLinkedList obejcts) of each. Command line UI elements for
    /// achieving the above with a menu-driven interface and appropriate
    /// error checking and handling
    /// </summary>

    public class LibraryUI
    {
        SortedDVDLibrary DVDLibrary = new SortedDVDLibrary();
        CustomerDatabase customerDatabase = new CustomerDatabase();

        string[] validGenres = { "Drama", "Adventure", "Family",
                                 "Action", "Sci-Fi", "Comedy",
                                 "Animated", "Thriller", "Other" };

        string[] validClassifications = {"General(G)",
                                         "Parental Guidance(PG)",
                                         "Mature(M15+)",
                                         "Mature Accompanied(MA15+)" };

        public LibraryUI() { }

        /// <summary>
        /// Below are the methods exposed to Find, List or Manage items. Note that
        /// it is possible to search by each of the criteria enherent to a DVD;
        /// that is:
        ///  - Title
        ///  - Starring Actors
        ///  - Director
        ///  - Duration
        ///  - Genre
        ///  - Classification
        ///  - Year Released
        /// </summary>

        public string ListItems(SortedDVDLibrary library)
        {
            if (library.Length > 0)
                return library.ToString();
            return "No Matches Found\n";
        }

        public DVD FindItem(SortedDVDLibrary library, string title)
        {
            DVD dvd = new DVD(title: title);

            if (library.Contains(dvd))
                return library.Find(dvd).nodeObject;
            return null;
        }

        public int GetQuantity(DVD dvd)
        {
            return this.DVDLibrary.Find(dvd).quantity;
        }

        public void AddItem(DVD dvd, int quantity = 0)
        {
            if (quantity < 0)
                throw new ArgumentOutOfRangeException("Quantity must be positive");

            this.DVDLibrary.Insert(dvd);

            if (quantity > 0)
            {
                this.DVDLibrary.Find(dvd).quantity = quantity;
            }
        }

        public string ListItemsByTitle(SortedDVDLibrary library, string title)
        {
            string returnString = "";
            SortedDVDLibrary selection = new SortedDVDLibrary();

            returnString += "Movies Titles matching \'" + title + "\':\n";

            foreach (DVD dvd in library)
            {
                if (dvd.title.ToUpper().Contains(title.ToUpper()))
                {
                    selection.Insert(dvd);
                    selection.Find(dvd).quantity = library.Find(dvd).quantity;
                }
            }

            if (selection.ToString() == "")
                returnString += "No Matches Found\n";
            else
                returnString += selection.ToString();

            return returnString;
        }

        public string ListItemsByStarring(SortedDVDLibrary library, string actor)
        {
            string returnString = "";
            SortedDVDLibrary selection = new SortedDVDLibrary();

            returnString += "Movies starring \'" + actor + "\':\n";

            actor = actor.ToUpper();

            foreach (DVD dvd in library)
            {
                if (dvd.starring != null)
                {
                    if (Array.Exists(dvd.starring, element => element.ToUpper() == actor))
                    {
                        selection.Insert(dvd);
                        selection.Find(dvd).quantity = library.Find(dvd).quantity;
                    }
                }
            }

            if (selection.ToString() == "")
                returnString += "No Matches Found\n";
            else
                returnString += selection.ToString();

            return returnString;
        }

        public string ListItemsByDirector(SortedDVDLibrary library, string director)
        {
            string returnString = "";
            SortedDVDLibrary selection = new SortedDVDLibrary();

            returnString += "Movies directed by \'" + director + "\':\n";

            foreach (DVD dvd in library)
            {
                if (dvd.director.ToUpper() == director.ToUpper())
                {
                    selection.Insert(dvd);
                    selection.Find(dvd).quantity = library.Find(dvd).quantity;
                }
            }

            if (selection.ToString() == "")
                returnString += "No Matches Found\n";
            else
                returnString += selection.ToString();

            return returnString;
        }

        public string ListItemsByMaxDuration(SortedDVDLibrary library, int duration)
        {
            string returnString = "";
            SortedDVDLibrary selection = new SortedDVDLibrary();

            returnString += "Movies with a maximum duration of ";
            returnString += duration.ToString();
            returnString += " minutes:\n";

            foreach (DVD dvd in library)
            {
                if (dvd.duration <= duration)
                {
                    selection.Insert(dvd);
                    selection.Find(dvd).quantity = library.Find(dvd).quantity;
                }
            }

            if (selection.ToString() == "")
                returnString += "No Matches Found\n";
            else
                returnString += selection.ToString();

            return returnString;
        }

        public string ListItemsByGenre(SortedDVDLibrary library, string genre)
        {
            string returnString = "";
            SortedDVDLibrary selection = new SortedDVDLibrary();

            if (!(Array.Exists(this.validGenres, element => element == genre)))
                throw new ArgumentException("Invalid genre");


            returnString += "Movies in the genre \'" + genre + "\':\n";

            genre = genre.ToUpper();

            foreach (DVD dvd in library)
            {
                if (dvd.genres != null)
                {
                    if (Array.Exists(dvd.genres, element => element.ToUpper() == genre))
                    {
                        selection.Insert(dvd);
                        selection.Find(dvd).quantity = library.Find(dvd).quantity;
                    }
                }
            }

            if (selection.ToString() == "")
                returnString += "No Matches Found\n";
            else
                returnString += selection.ToString();

            return returnString;
        }

        public string ListItemsByClassification(SortedDVDLibrary library, string classification)
        {
            string returnString = "";
            SortedDVDLibrary selection = new SortedDVDLibrary();

            if (!(Array.Exists(this.validClassifications, element => element == classification)))
                throw new ArgumentException("Invalid classification");

            returnString += "Movies classified as " + classification + ":\n";

            classification = classification.ToUpper();

            foreach (DVD dvd in library)
            {
                if (dvd.classification.ToUpper() == classification)
                {
                    selection.Insert(dvd);
                    selection.Find(dvd).quantity = library.Find(dvd).quantity;
                }
            }

            if (selection.ToString() == "")
                returnString += "No Matches Found\n";
            else
                returnString += selection.ToString();

            return returnString;
        }

        public string ListItemsByYear(SortedDVDLibrary library, string year)
        {
            string returnString = "";
            SortedDVDLibrary selection = new SortedDVDLibrary();

            Date startDate = new Date("01/01/" + year);
            Date endDate = new Date("31/12/" + year);

            returnString += "Movies released in " + year + ":\n";

            foreach (DVD dvd in library)
            {
                if (dvd.releaseDate >= startDate && dvd.releaseDate <= endDate)
                {
                    selection.Insert(dvd);
                    selection.Find(dvd).quantity = library.Find(dvd).quantity;
                }
            }

            if (selection.ToString() == "")
                returnString += "No Matches Found\n";
            else
                returnString += selection.ToString();

            return returnString;
        }

        /// <summary>
        /// Below are the methods exposed to Find, List or Manage customers
        /// </summary>
    
        public string ListCustomers()
        {
            return this.customerDatabase.ToString();
        }

        public Customer FindCustomerByName(string fullName)
        {
            if (!fullName.Contains(","))
                return null;

            fullName = fullName.ToUpper();
            string lastName = fullName.Split(',')[0].Trim();
            string firstName = fullName.Split(',')[1].Trim();

            foreach (Customer customer in this.customerDatabase)
            {
                // Note, will return FIRST instance of customer with this name
                if (customer.firstName.ToUpper() == firstName && customer.lastName.ToUpper() == lastName)
                    return customer;
            }
            return null;
        }

        public Customer FindCustomerByNumber(string number)
        {
            foreach (Customer customer in this.customerDatabase)
            {
                // Note, will return FIRST instance of customer with this number
                if (customer.contactNumber == number)
                    return customer;
            }
            return null;
        }

        public void AddCustomer(Customer customer)
        {
            this.customerDatabase.Insert(customer);
        }

        public string ListCustomersByName(string name)
        {
            string returnString = "";
            CustomerDatabase selection = new CustomerDatabase();

            returnString += "Customers matching Name " + name + ":\n";

            name = name.ToUpper();

            foreach (Customer customer in this.customerDatabase)
            {
                if (customer.firstName.ToUpper() == name || customer.lastName.ToUpper() == name)
                    selection.Insert(customer);
            }

            if (selection.ToString() == "")
                returnString += "No Matches Found\n";
            else
                returnString += selection.ToString();

            return returnString;
        }

        public string ListCustomersByNumber(string contactNumber)
        {
            string returnString = "";
            CustomerDatabase selection = new CustomerDatabase();

            returnString += "Customers matching Contact Number " + contactNumber + ":\n";

            foreach (Customer customer in this.customerDatabase)
            {
                if (customer.contactNumber == contactNumber)
                    selection.Insert(customer);
            }

            if (selection.ToString() == "")
                returnString += "No Matches Found\n";
            else
                returnString += selection.ToString();

            return returnString;
        }

        /// <summary>
        ///  The majority of the menu UI is outlined below. Note that individual methods
        ///  (eg MainMenu(), AddCustomerMenu() or LendItemMenu() are sparing commented
        ///  as these use self-explaniting identifies and = for the most part - are
        ///  very sparsely coded. Care is taken to detect and handle errors while still
        ///  trying to maintain compact code
        /// </summary>

        public void MainMenu()
        {
            bool exit = false;

            Console.WriteLine("================================================================================");
            Console.WriteLine("===================== COMMUNITY LIBRARY DVD LENDING SYSTEM =====================");
            Console.WriteLine("================================================================================");

            while (exit == false)
            {
                Console.WriteLine();
                Console.WriteLine("Please select from the following options:");
                Console.WriteLine("1. List Items");
                Console.WriteLine("2. Lend an Item");
                Console.WriteLine("3. Return an Item");
                Console.WriteLine("4. Manage Customers");
                Console.WriteLine("5. Manage Library");
                Console.WriteLine("0. Quit");
                Console.WriteLine();
                Console.Write("Selection > ");
                string input = Console.ReadLine();

                bool validInput = Int32.TryParse(input, out int selection);

                if (!validInput || selection < 0 || selection > 5)
                {
                    Console.WriteLine("Please enter a valid option");
                    continue;
                }

                switch (selection)
                {
                    case 0:
                        exit = true;
                        continue;
                    case 1:
                        ListItemsMenu(this.DVDLibrary);
                        continue;
                    case 2:
                        LendItemMenu();
                        continue;
                    case 3:
                        ReturnItemMenu();
                        continue;
                    case 4:
                        ManageCustomersMenu();
                        continue;
                    case 5:
                        ManageLibraryMenu();
                        continue;
                }
            }
        }

        public void ListItemsMenu(SortedDVDLibrary library)
        {
            bool exit = false;

            while (exit == false)
            {
                Console.WriteLine();
                Console.WriteLine("Please select from the following options:");
                Console.WriteLine("1. List all Items");
                Console.WriteLine("2. List Items by Title");
                Console.WriteLine("3. List Items by Starring Actors");
                Console.WriteLine("4. List Items by Director");
                Console.WriteLine("5. List Items by Duration");
                Console.WriteLine("6. List Items by Genre");
                Console.WriteLine("7. List Items by Classification");
                Console.WriteLine("8. List Items by Year Released");
                Console.WriteLine("9. List Valid Genres and Classifications");
                Console.WriteLine("0. Go Back to Previous Menu");
                Console.WriteLine();
                Console.Write("Selection > ");
                string input = Console.ReadLine();

                bool validInput = Int32.TryParse(input, out int selection);

                if (!validInput || selection < 0 || selection > 9)
                {
                    Console.WriteLine("Please enter a valid option");
                    continue;
                }

                switch (selection)
                {
                    case 0:
                        exit = true;
                        continue;
                    case 1:
                        Console.Write("\n" + ListItems(library));
                        continue;
                    case 2:
                        Console.Write("Enter Title > ");
                        Console.Write("\n" + ListItemsByTitle(library, Console.ReadLine()));
                        continue;
                    case 3:
                        Console.Write("Enter Actor Name > ");
                        Console.Write("\n" + ListItemsByStarring(library, Console.ReadLine()));
                        continue;
                    case 4:
                        Console.Write("Enter Director Name > ");
                        Console.Write("\n" + ListItemsByDirector(library, Console.ReadLine()));
                        continue;
                    case 5:
                        Console.Write("Enter a Maximum Duration > ");
                        if (Int32.TryParse(Console.ReadLine(), out int duration))
                            Console.Write("\n" + ListItemsByMaxDuration(library, duration));
                        else
                            Console.Write("Please enter a valid duration");
                        continue;
                    case 6:
                        Console.Write("Enter a Genre > ");
                        try { Console.Write("\n" + ListItemsByGenre(library, Console.ReadLine())); }
                        catch (ArgumentException) { Console.WriteLine("Please enter a valid genre"); }
                        continue;
                    case 7:
                        Console.Write("Enter a Classification > ");
                        try { Console.Write("\n" + ListItemsByClassification(library, Console.ReadLine())); }
                        catch (ArgumentException) { Console.WriteLine("Please enter a valid classification"); }
                        continue;
                    case 8:
                        Console.Write("Enter Release Year > ");
                        Console.Write("\n" + ListItemsByYear(library, Console.ReadLine()));
                        continue;
                    case 9:
                        Console.WriteLine();
                        Console.WriteLine("Genres:");
                        foreach (string genre in validGenres)
                            Console.WriteLine(genre);
                        Console.WriteLine();
                        Console.WriteLine("Classifications:");
                        foreach (string classification in validClassifications)
                            Console.WriteLine(classification);
                        continue;
                }
            }
        }

        public void LendItemMenu()
        {
            bool exit = false;

            DVD item = null;
            Customer customer = null;
            string itemName;
            string customerName;

            while (exit == false)
            {
                if (item != null)
                    itemName = item.title;
                else
                    itemName = "(none selected)";

                if (customer != null)
                    customerName = customer.lastName + ", " + customer.firstName;
                else
                    customerName = "(none selected)";

                Console.WriteLine();
                Console.WriteLine("Please select from the following options:");
                Console.WriteLine("1. Select Customer");
                Console.WriteLine("2. Select Item");
                Console.WriteLine("3. Confirm Lend");
                Console.WriteLine("0. Go Back to Previous Menu");
                Console.WriteLine();
                Console.WriteLine("Item:        " + itemName);
                Console.WriteLine("Customer:    " + customerName);
                Console.WriteLine();
                Console.Write("Selection > ");
                string input = Console.ReadLine();

                bool validInput = Int32.TryParse(input, out int selection);

                if (!validInput || selection < 0 || selection > 3)
                {
                    Console.WriteLine("Please enter a valid option");
                    continue;
                }

                switch (selection)
                {
                    case 0:
                        exit = true;
                        continue;
                    case 1:
                        customer = SelectCustomerMenu();
                        continue;
                    case 2:
                        item = SelectItemMenu(this.DVDLibrary);
                        continue;
                    case 3:
                        if (item == null || customer == null)
                        {
                            Console.WriteLine(item == null);
                            Console.WriteLine(customer == null);
                            Console.WriteLine("Please select a customer and an item first");
                            continue;
                        }
                        else
                        {
                            try
                            {
                                customer.Borrow(this.DVDLibrary, item);
                                Console.WriteLine("\nSuccessfully lent \'" + item.title + "\'");
                                exit = true;
                            }
                            catch (ArgumentException) { Console.WriteLine("That item is not available to lend"); }
                        }
                        continue;
                }
            }
        }

        public void ReturnItemMenu()
        {
            bool exit = false;

            DVD item = null;
            Customer customer = null;
            string itemName;
            string customerName;

            while (exit == false)
            {
                if (item != null)
                    itemName = item.title;
                else
                    itemName = "(none selected)";

                if (customer != null)
                    customerName = customer.lastName + ", " + customer.firstName;
                else
                    customerName = "(none selected)";

                Console.WriteLine();
                Console.WriteLine("Please select from the following options:");
                Console.WriteLine("1. Select Customer");
                Console.WriteLine("2. Select Item");
                Console.WriteLine("3. Confirm Return");
                Console.WriteLine("0. Go Back to Previous Menu");
                Console.WriteLine();
                Console.WriteLine("Item:        " + itemName);
                Console.WriteLine("Customer:    " + customerName);
                Console.WriteLine();
                Console.Write("Selection > ");
                string input = Console.ReadLine();

                bool validInput = Int32.TryParse(input, out int selection);

                if (!validInput || selection < 0 || selection > 3)
                {
                    Console.WriteLine("Please enter a valid option");
                    continue;
                }

                switch (selection)
                {
                    case 0:
                        exit = true;
                        continue;
                    case 1:
                        customer = SelectCustomerMenu();
                        continue;
                    case 2:
                        if (customer == null)
                            Console.WriteLine("Please select a customer first");
                        else
                            item = SelectItemMenu(customer.borrowedDVDs);
                        continue;
                    case 3:
                        if (item == null || customer == null)
                        {
                            Console.WriteLine("Please select a customer and an item first");
                            continue;
                        }
                        else
                        {
                            try
                            {
                                customer.Return(this.DVDLibrary, item);
                                Console.WriteLine("\nSuccessfully returned \'" + item.title + "\'");
                                exit = true;
                            }
                            catch (ArgumentException) { Console.WriteLine("That item is not available to return"); }
                        }
                        continue;
                }
            }
        }

        public void ManageLibraryMenu()
        {
            bool exit = false;

            while (exit == false)
            {
                Console.WriteLine();
                Console.WriteLine("Please select from the following options:");
                Console.WriteLine("1. List all Items");
                Console.WriteLine("2. Add New Item");
                Console.WriteLine("0. Go Back to Previous Menu");
                Console.WriteLine();
                Console.Write("Selection > ");
                string input = Console.ReadLine();

                bool validInput = Int32.TryParse(input, out int selection);

                if (!validInput || selection < 0 || selection > 2)
                {
                    Console.WriteLine("Please enter a valid option");
                    continue;
                }

                switch (selection)
                {
                    case 0:
                        exit = true;
                        continue;
                    case 1:
                        ListItemsMenu(this.DVDLibrary);
                        continue;
                    case 2:
                        AddItemMenu(this.DVDLibrary);
                        continue;
                }
            }
        }

        public void AddItemMenu(SortedDVDLibrary library)
        {
            bool exit = false;

            while (exit == false)
            {
                string title = "";
                string[] starring;
                string director;
                int duration = 0;
                string[] genres;
                string classification;
                Date releaseDate = new Date("01/01/1850");

                Console.WriteLine();
                Console.WriteLine("Please select from the following options:");
                Console.WriteLine("1. List Valid Genres and Classifications");
                Console.WriteLine("2. Add New Item");
                Console.WriteLine("0. Go Back to Previous Menu");
                Console.WriteLine();
                Console.Write("Selection > ");
                string input = Console.ReadLine();

                bool validInput = Int32.TryParse(input, out int selection);

                if (!validInput || selection < 0 || selection > 2)
                {
                    Console.WriteLine("Please enter a valid option");
                    continue;
                }

                switch (selection)
                {
                    case 0:
                        exit = true;
                        continue;
                    case 1:
                        Console.WriteLine();
                        Console.WriteLine("Genres:");
                        foreach (string validGenre in validGenres)
                            Console.WriteLine(validGenre);
                        Console.WriteLine();
                        Console.WriteLine("Classifications:");
                        foreach (string validClassification in validClassifications)
                            Console.WriteLine(validClassification);
                        continue;
                    case 2:
                        while (title == "")
                        {
                            Console.Write("Enter Item Title > ");
                            title = Console.ReadLine();
                        }
                        DVD newDVD = new DVD(title);
                        if (library.Contains(newDVD)) // Check if item exists already
                        {
                            library.Insert(newDVD);
                            Console.WriteLine("\nAdded New Item:");
                            Console.WriteLine(ListItemsByTitle(library, newDVD.title));
                            exit = true;
                            continue;
                        }
                        Console.Write("Enter Starring Actors (comma seperated) > ");
                        starring = Console.ReadLine().Split(",");
                        for (int i = 0; i < starring.Length; i++)
                            starring[i] = starring[i].Trim();
                        Console.Write("Enter the Name of the Director > ");
                        director = Console.ReadLine();
                        Console.Write("Enter the Movie Duration (Minutes) > ");
                        while (!Int32.TryParse(Console.ReadLine(), out duration) || duration <= 0)
                            Console.Write("Enter the Movie Duration (Minutes) > ");
                        Console.Write("Enter the Movie Genre(s) (comma seperated) > ");
                        genres = Console.ReadLine().Split(",");
                        for (int i = 0; i < genres.Length; i++)
                            genres[i] = genres[i].Trim();
                        Console.Write("Enter the Classification > ");
                        classification = Console.ReadLine();
                        while (releaseDate == new Date("01/01/1850"))
                        {
                            Console.Write("Enter the Release Date (DD/MM/YYYY) > ");
                            try { releaseDate = new Date(Console.ReadLine()); }
                            catch (ArgumentException) { continue; }
                        }
                        try
                        {
                            newDVD = new DVD(title: title,
                                          starring: starring,
                                          director: director,
                                          duration: duration,
                                          genres: genres,
                                          classification: classification,
                                          releaseDate: releaseDate);
                        }
                        catch (ArgumentException) { Console.WriteLine("\nUnable to add item. Please check for valid genre and classification"); continue; }
                        this.AddItem(newDVD, quantity: 1);
                        Console.WriteLine("\nAdded New Item:");
                        Console.Write(ListItemsByTitle(library, newDVD.title));
                        exit = true;
                        continue;
                }
            }
        }


        public void ManageCustomersMenu()
        {
            bool exit = false;

            while (exit == false)
            {
                Console.WriteLine();
                Console.WriteLine("Please select from the following options:");
                Console.WriteLine("1. List all Customers");
                Console.WriteLine("2. List Customers by Name");
                Console.WriteLine("3. List by Contact Number");
                Console.WriteLine("4. Add New Customer");
                Console.WriteLine("0. Go Back to Previous Menu");
                Console.WriteLine();
                Console.Write("Selection > ");
                string input = Console.ReadLine();

                bool validInput = Int32.TryParse(input, out int selection);

                if (!validInput || selection < 0 || selection > 4)
                {
                    Console.WriteLine("Please enter a valid option");
                    continue;
                }

                switch (selection)
                {
                    case 0:
                        exit = true;
                        continue;
                    case 1:
                        Console.Write("\n" + this.ListCustomers());
                        continue;
                    case 2:
                        Console.Write("Enter Customer Name > ");
                        Console.Write("\n" + ListCustomersByName(Console.ReadLine()));
                        continue;
                    case 3:
                        Console.Write("Enter Contact Number > ");
                        Console.Write("\n" + ListCustomersByNumber(Console.ReadLine()));
                        continue;
                    case 4:
                        AddCustomerMenu();
                        continue;
                }
            }
        }

        public void AddCustomerMenu()
        {
            bool exit = false;

            while (exit == false)
            {
                string firstName = "";
                string lastName = "";
                string number = "";

                Console.WriteLine();
                Console.WriteLine("Please select from the following options:");
                Console.WriteLine("1. Add New Customer");
                Console.WriteLine("0. Go Back to Previous Menu");
                Console.WriteLine();
                Console.Write("Selection > ");
                string input = Console.ReadLine();

                bool validInput = Int32.TryParse(input, out int selection);

                if (!validInput || selection < 0 || selection > 1)
                {
                    Console.WriteLine("Please enter a valid option");
                    continue;
                }

                switch (selection)
                {
                    case 0:
                        exit = true;
                        continue;
                    case 1:
                        while (firstName == "")
                        {
                            Console.Write("Enter Customer First Name > ");
                            firstName = Console.ReadLine();
                        }
                        while (lastName == "")
                        {
                            Console.Write("Enter Customer Last Name > ");
                            lastName = Console.ReadLine();
                        }
                        while (number == "")
                        {
                            Console.Write("Enter Customer Contact Number > ");
                            number = Console.ReadLine();
                        }
                        Customer customer = new Customer(firstName, lastName, number);
                        this.customerDatabase.Insert(customer);
                        Console.Write("\nAdded customer:\n" + ListCustomersByNumber(number));
                        exit = true;
                        continue;
                }
            }
        }

        public Customer SelectCustomerMenu()
        {
            bool exit = false;

            Customer customer = null;

            while (exit == false)
            {
                Console.WriteLine();
                Console.WriteLine("Please select from the following options:");
                Console.WriteLine("1. List or Manage Customers");
                Console.WriteLine("2. Select Customer by Name (Last, First)");
                Console.WriteLine("3. Select Customer by Contact Number");
                Console.WriteLine("0. Go Back to Previous Menu");
                Console.WriteLine();
                Console.Write("Selection > ");
                string input = Console.ReadLine();

                bool validInput = Int32.TryParse(input, out int selection);

                if (!validInput || selection < 0 || selection > 3)
                {
                    Console.WriteLine("Please enter a valid option");
                    continue;
                }

                switch (selection)
                {
                    case 0:
                        exit = true;
                        continue;
                    case 1:
                        ManageCustomersMenu();
                        continue;
                    case 2:
                        Console.Write("Enter Customer Name > ");
                        customer = FindCustomerByName(Console.ReadLine());
                        if (customer != null)
                            Console.WriteLine("\nSelected:\n" + customer);
                        else
                            Console.WriteLine("\nNo Customer Selected");
                        exit = true;
                        continue;
                    case 3:
                        Console.Write("Enter Contact Number > ");
                        customer = FindCustomerByNumber(Console.ReadLine());
                        if (customer != null)
                        {
                            Console.WriteLine("\nSelected:\n" + customer);
                            exit = true;
                        }
                        else
                            Console.WriteLine("\nNo Customer Selected");
                        continue;
                }
            }

            return customer;
        }


        public DVD SelectItemMenu(SortedDVDLibrary library)
        {
            bool exit = false;

            DVD item = null;

            while (exit == false)
            {
                Console.WriteLine();
                Console.WriteLine("Please select from the following options:");
                Console.WriteLine("1. List Items");
                Console.WriteLine("2. Select Item by Title");
                Console.WriteLine("0. Go Back to Previous Menu");
                Console.WriteLine();
                Console.Write("Selection > ");
                string input = Console.ReadLine();

                bool validInput = Int32.TryParse(input, out int selection);

                if (!validInput || selection < 0 || selection > 2)
                {
                    Console.WriteLine("Please enter a valid option");
                    continue;
                }

                switch (selection)
                {
                    case 0:
                        exit = true;
                        continue;
                    case 1:
                        ListItemsMenu(library);
                        continue;
                    case 2:
                        Console.Write("Enter Title > ");
                        item = FindItem(library, Console.ReadLine());
                        if (item != null)
                        {
                            Console.WriteLine("\nSelected:\n" + item);
                            exit = true;
                        }
                        else
                            Console.WriteLine("\nNo Item Selected");
                        continue;
                }
            }

            return item;
        }
    }


    /// <summary>
    /// Main function. This is only used for testing; the LibraryUI class is
    /// otherwise self-contained and could be called from another namespace
    /// </summary>

    public class CommunityLibrary
    {
        static void Main()
        {
            Random rand = new Random();

            // Sourced from IMDB.com, movie list from
            // https://www.ranker.com/crowdranked-list/the-best-movies-of-all-time
            // Back to the Future included for testing ordered sorting.
            // DVDs are stored in an array here; however, these are only used to
            // pre-populate the SortedLinkedList with valid DVDs for testing.
            // and generating screenshots for the project report. Note that all
            // sorting, storing and record keeping operations are conducted
            // against the SortedLinkedList class which is an implimentation
            // of a doubly-linked list sorted by the objects contained therein
            DVD[] dvds = new DVD[] {
                new DVD( title: "Back to the Future Part I",
                         starring: new string[] { "Michael J. Fox", "Christopher Lloyd", "Lea Thompson" },
                         director: "Robert Zemeckis",
                         duration: 116,
                         genres: new string[] { "Adventure", "Comedy", "Sci-Fi" },
                         classification: "Parental Guidance(PG)",
                         releaseDate: new Date("15/08/1985")),

                new DVD( title: "Back to the Future Part II",
                         starring: new string[] { "Michael J. Fox", "Christopher Lloyd", "Lea Thompson" },
                         director: "Robert Zemeckis",
                         duration: 108,
                         genres: new string[] { "Adventure", "Comedy", "Sci-Fi" },
                         classification: "Parental Guidance(PG)",
                         releaseDate: new Date("07/12/1989")),

                new DVD( title: "Back to the Future Part III",
                         starring: new string[] { "Michael J. Fox", "Christopher Lloyd", "Lea Thompson" },
                         director: "Robert Zemeckis",
                         duration: 116,
                         genres: new string[] { "Adventure", "Comedy", "Sci-Fi" },
                         classification: "Parental Guidance(PG)",
                         releaseDate: new Date("28/06/1990")),

                new DVD( title: "Jaws",
                         starring: new string[] { "Roy Scheider", "Robert Shaw", "Richard Dreyfuss" },
                         director: "Steven Spielberg",
                         duration: 124,
                         genres: new string[] { "Adventure", "Thriller" },
                         classification: "Mature(M15+)",
                         releaseDate: new Date("27/11/1975")),

                new DVD( title: "Forrest Gump",
                         starring: new string[] { "Tom Hanks", "Robin Wright", "Gary Sinise" },
                         director: "Robert Zemeckis",
                         duration: 142,
                         genres: new string[] { "Drama", "Family" },
                         classification: "Mature(M15+)",
                         releaseDate: new Date("17/11/1994")),

                new DVD( title: "Saving Private Ryan",
                         starring: new string[] { "Tom Hanks", "Matt Damon", "Tom Sizemore" },
                         director: "Steven Spielberg",
                         duration: 169,
                         genres: new string[] { "Drama" },
                         classification: "Mature Accompanied(MA15+)",
                         releaseDate: new Date("19/11/1998")),

                new DVD( title: "The Shawshank Redemption",
                         starring: new string[] { "Tim Robbins", "Morgan Freeman", "Bob Gunton" },
                         director: "Frank Darabont",
                         duration: 142,
                         genres: new string[] { "Drama" },
                         classification: "Mature Accompanied(MA15+)",
                         releaseDate: new Date("16/02/1995")),

                new DVD( title: "Toy Story",
                         starring: new string[] { "Tom Hanks", "Tim Allen", "Don Rickles" },
                         director: "John Lasseter",
                         duration: 81,
                         genres: new string[] { "Animated", "Adventure", "Comedy" },
                         classification: "General(G)",
                         releaseDate: new Date("07/12/1995")),

                new DVD( title: "The Green Mile",
                         starring: new string[] { "Tom Hanks", "Michael Clarke Duncan", "David Morse" },
                         director: "Frank Darabont",
                         duration: 189,
                         genres: new string[] { "Drama" },
                         classification: "Mature Accompanied(MA15+)",
                         releaseDate: new Date("10/02/1999")),

                new DVD( title: "Catch Me If You Can",
                         starring: new string[] { "Leonardo DiCaprio", "Tom Hanks", "Christopher Walken" },
                         director: "Steven Spielberg",
                         duration: 141,
                         genres: new string[] { "Drama" },
                         classification: "Mature(M15+)",
                         releaseDate: new Date("09/01/2003")),

                new DVD( title: "The Dark Knight",
                         starring: new string[] { "Christian Bale", "Heath Ledger", "Aaron Eckhart" },
                         director: "Christopher Nolan",
                         duration: 153,
                         genres: new string[] { "Action", "Drama" },
                         classification: "Mature(M15+)",
                         releaseDate: new Date("16/07/2008")),

                new DVD( title: "Cast Away",
                         starring: new string[] { "Tom Hanks", "Helen Hunt", "Paul Sanchez" },
                         director: "Robert Zemeckis",
                         duration: 143,
                         genres: new string[] { "Adventure", "Drama" },
                         classification: "Mature(M15+)",
                         releaseDate: new Date("18/01/2001")),

                new DVD( title: "The Lion King",
                         starring: new string[] { "Matthew Broderick", "Jeremy Irons", "James Earl Jones" },
                         director: "Roger Allers",
                         duration: 88,
                         genres: new string[] { "Animated", "Adventure", "Drama" },
                         classification: "General(G)",
                         releaseDate: new Date("25/08/1994")),

                new DVD( title: "Alien",
                         starring: new string[] { "Sigourney Weaver", "Tom Skerritt", "John Hurt" },
                         director: "Ridley Scott",
                         duration: 117,
                         genres: new string[] { "Thriller", "Sci-Fi" },
                         classification: "Mature(M15+)",
                         releaseDate: new Date("06/12/1979")),

                new DVD( title: "Scarface",
                         starring: new string[] { "Al Pacino", "Michelle Pfeiffer", "Steven Bauer" },
                         director: "Brian De Palma",
                         duration: 170,
                         genres: new string[] { "Drama", "Thriller" },
                         classification: "Mature Accompanied(MA15+)",
                         releaseDate: new Date("22/03/1983")),

                new DVD( title: "The Lord of the Rings: The Two Towers",
                         starring: new string[] { "Elijah Wood", "Ian McKellen", "Viggo Mortensen" },
                         director: "Peter Jackson",
                         duration: 179,
                         genres: new string[] { "Action", "Adventure", "Drama" },
                         classification: "Mature(M15+)",
                         releaseDate: new Date("26/12/2002")),

                new DVD( title: "The Silence of the Lambs",
                         starring: new string[] { "Jodie Foster", "Anthony Hopkins", "Lawrence A. Bonney" },
                         director: "Jonathan Demme",
                         duration: 118,
                         genres: new string[] { "Drama", "Thriller" },
                         classification: "Mature Accompanied(MA15+)",
                         releaseDate: new Date("09/05/1991")),

                new DVD( title: "The Sixth Sense",
                         starring: new string[] { "Bruce Willis", "Haley Joel Osment", "Toni Collette" },
                         director: "M. Night Shyamalan",
                         duration: 107,
                         genres: new string[] { "Drama", "Thriller" },
                         classification: "Mature(M15+)",
                         releaseDate: new Date("07/10/1999")),

                new DVD( title: "Full Metal Jacket",
                         starring: new string[] { "Matthew Modine", "R. Lee Ermey", "Vincent D'Onofrio" },
                         director: "Stanley Kubrick",
                         duration: 116,
                         genres: new string[] { "Drama", "Thriller" },
                         classification: "Mature Accompanied(MA15+)",
                         releaseDate: new Date("07/10/1999")),

                new DVD( title: "Stand by Me",
                         starring: new string[] { "Wil Wheaton", "River Phoenix", "Corey Feldman" },
                         director: "Rob Reiner",
                         duration: 89,
                         genres: new string[] { "Adventure", "Drama" },
                         classification: "Mature(M15+)",
                         releaseDate: new Date("19/03/1986")),

                new DVD( title: "Inception",
                         starring: new string[] { "Leonardo DiCaprio", "Joseph Gordon-Levitt", "Ellen Page" },
                         director: "Christopher Nolan",
                         duration: 149,
                         genres: new string[] { "Action", "Adventure", "Sci-Fi" },
                         classification: "Mature(M15+)",
                         releaseDate: new Date("22/07/2010")),

                new DVD( title: "Indiana Jones and the Last Crusade",
                         starring: new string[] { "Harrison Ford", "Sean Connery", "Alison Doody" },
                         director: "Steven Spielberg",
                         duration: 127,
                         genres: new string[] { "Action", "Adventure" },
                         classification: "Parental Guidance(PG)",
                         releaseDate: new Date("08/06/1989")),

                new DVD( title: "The Princess Bride",
                         starring: new string[] { "Cary Elwes", "Mandy Patinkin", "Robin Wright" },
                         director: "Rob Reiner",
                         duration: 98,
                         genres: new string[] { "Adventure", "Family" },
                         classification: "Parental Guidance(PG)",
                         releaseDate: new Date("03/12/1987")),

                new DVD( title: "The Incredibles",
                         starring: new string[] { "Craig T. Nelson", "Samuel L. Jackson", "Holly Hunter" },
                         director: "Brad Bird",
                         duration: 115,
                         genres: new string[] { "Animated", "Action", "Adventure" },
                         classification: "Parental Guidance(PG)",
                         releaseDate: new Date("26/12/2004")),
            };

            int numCustomers = 100;

            // Sourced from https://www.ssa.gov/oact/babynames/decades/century.html
            string[] firstNames = new string[] { "James", "John", "Robert", "Michael",
                                                 "William", "David", "Richard", "Joseph",
                                                 "Thomas", "Charles", "Christopher",
                                                 "Daniel", "Matthew", "Anthony", "Donald",
                                                 "Mary", "Patricia", "Jennifer", "Linda",
                                                 "Elizabeth", "Barbara", "Susan", "Jessica",
                                                 "Sarah", "Karen", "Nancy", "Lisa",
                                                 "Margaret", "Betty", "Sandra"};

            // Sourced from https://www.al.com/news/2019/10/50-most-common-last-names-in-america.html
            string[] lastNames = new string[] {  "Smith", "Johnson", "Williams", "Brown",
                                                 "Jones", "Miller", "Davis", "Wilson",
                                                 "Anderson", "Thomas", "Taylor", "Moore",
                                                 "Jackson", "Martin", "Lee", "Thompson",
                                                 "White", "Harris", "Clark", "Lewis",
                                                 "Robinson", "Walker", "Young", "Allen",
                                                 "King", "Wright", "Scott", "Hill",
                                                 "Green", "Adams"};

            // Instantiate the library here
            LibraryUI communityLibrary = new LibraryUI();

            // Pre-populate DVD Library for Screenshots
            foreach (DVD dvd in dvds)
                communityLibrary.AddItem(dvd, quantity: rand.Next(25));

            // Pre-populate Customer Database for Screenshots
            for (int i = 0; i < numCustomers; i++)
            {
                string firstName = firstNames[rand.Next(30)];
                string lastName = lastNames[rand.Next(30)];

                string phoneNumber = "+61 4 ";

                // Not all numbers immediately after "04" in a mobile number are valid
                // (https://en.wikipedia.org/wiki/Telephone_numbers_in_Australia)
                phoneNumber += rand.Next(4).ToString();

                for (int j = 0; j < 3; j++)
                    phoneNumber += rand.Next(10).ToString();

                phoneNumber += " ";

                for (int j = 0; j < 4; j++)
                    phoneNumber += rand.Next(10).ToString();

                Customer customer = new Customer(firstName: firstName,
                                                 lastName: lastName,
                                                 contactNumber: phoneNumber);

                communityLibrary.AddCustomer(customer);
            }

            // Now we have a fully populated library, we can proceed to fulfill the
            // requirements of the project:
            communityLibrary.MainMenu();
        }
    }
}