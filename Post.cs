using System;
using System.Collections.Generic;

class Mail {
    // Attributes common to all types of mail
    protected double weight;
    protected bool express;
    protected string destinationAddress;

    // Constructor to initialize mail attributes
    public Mail(double weight, bool express, string destinationAddress) {
        this.weight = weight;
        this.express = express;
        this.destinationAddress = destinationAddress;
    }

    // Calculating postage cost for each type of mail
    public virtual double CalculatePostage() {
        return 0.0; // Default implementation, to be overridden in subclasses
    }

    // Checking if mail is valid (has a destination address)
    public bool IsValid() {
        return !string.IsNullOrEmpty(destinationAddress);
    }
}

// Subclass representing a letter
class Letter : Mail {
    private string format;

    public Letter(double weight, bool express, string destinationAddress, string format) 
        : base(weight, express, destinationAddress) {
        this.format = format;
    }

    // Calculating postage cost for a letter
    public override double CalculatePostage() {
        double baseFare = format == "A3" ? 3.5 : 2.5;
        double postage = express ? baseFare * 2 : baseFare;
        postage += weight / 1000.0; // Converting grams to kilograms
        return postage;
    }
}

// Subclass representing a parcel
class Parcel : Mail {
    private double volume;

    public Parcel(double weight, bool express, string destinationAddress, double volume) 
        : base(weight, express, destinationAddress) {
        this.volume = volume;
    }

    // calculating postage cost for a parcel
    public override double CalculatePostage() {
        double postage = (0.25 * volume + weight / 1000.0); // Converting grams to kilograms
        if (express) {
            postage *= 2;
        }
        return postage;
    }

    // Override the IsValid method to check parcel volume
    public override bool IsValid() {
        return base.IsValid() && volume <= 50;
    }
}

// Subclass representing an advertisement
class Advertisement : Mail {

    public Advertisement(double weight, bool express, string destinationAddress) 
        : base(weight, express, destinationAddress) {
    }

    // Calculating postage cost for an advertisement
    public override double CalculatePostage() {
        double postage = 5 * weight / 1000.0; // Converting grams to kilograms
        if (express) {
            postage *= 2;
        }
        return postage;
    }
}

// Main class to demonstrate the mailbox functionality
class Program {
    static void Main(string[] args) {
        // Creating a mailbox and add some mails
        Mailbox mailbox = new Mailbox();
        mailbox.AddMail(new Letter(200.0, true, "Chemin des Acacias 28, 1009 Pully", "A3"));
        mailbox.AddMail(new Letter(800.0, false, ""));
        mailbox.AddMail(new Advertisement(1500.0, true, "Les Moilles 13A, 1913 Saillon"));
        mailbox.AddMail(new Advertisement(3000.0, false, ""));
        mailbox.AddMail(new Parcel(5000.0, true, "Grand rue 18, 1950 Sion", 30.0));
        mailbox.AddMail(new Parcel(3000.0, true, "Chemin des fleurs 48, 2800 Delemont", 70.0));

        // Displaying the contents of the mailbox
        mailbox.DisplayContents();

        // Displaying total postage cost
        Console.WriteLine("Total amount of postage: $" + mailbox.CalculateTotalPostage());

        // Displaying number of invalid mails
        Console.WriteLine("Number of invalid mails: " + mailbox.CountInvalidMails());
    }
}

// Class representing a mailbox containing multiple mails
class Mailbox {
    private List<Mail> mails;

    // Constructor to initialize the mailbox
    public Mailbox() {
        mails = new List<Mail>();
    }

    // Adding a mail to the mailbox
    public void AddMail(Mail mail) {
        mails.Add(mail);
    }

    // Calculating the total postage cost of all mails in the mailbox
    public double CalculateTotalPostage() {
        double totalPostage = 0.0;
        foreach (Mail mail in mails) {
            if (mail.IsValid()) {
                totalPostage += mail.CalculatePostage();
            }
        }
        return totalPostage;
    }

    // Counting the number of invalid mails in the mailbox
    public int CountInvalidMails() {
        int count = 0;
        foreach (Mail mail in mails) {
            if (!mail.IsValid()) {
                count++;
            }
        }
        return count;
    }

    // Displaying the contents of the mailbox
    public void DisplayContents() {
        foreach (Mail mail in mails) {
            Console.WriteLine(mail.GetType().Name);
            if (!mail.IsValid()) {
                Console.WriteLine("(Invalid courier)");
            }
            else {
                Console.WriteLine("Weight: " + mail.Weight + " grams");
                Console.WriteLine("Express: " + (mail.Express ? "yes" : "no"));
                Console.WriteLine("Destination: " + mail.DestinationAddress);
                Console.WriteLine("Price: $" + mail.CalculatePostage());
                if (mail is Letter) {
                    Console.WriteLine("Format: " + ((Letter)mail).Format);
                }
                else if (mail is Parcel) {
                    Console.WriteLine("Volume: " + ((Parcel)mail).Volume + " liters");
                }
            }
            Console.WriteLine();
        }
    }
}
