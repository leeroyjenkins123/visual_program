using System;

public class Account
{
    private decimal _balance;
    private List<INotifyer> _notifyers;

    public Account()
    {
        _balance = 0;
        _notifyers = new List<INotifyer>();
    }

    public Account(in decimal _balance)
    {
        this._balance = _balance;
        _notifyers = new List<INotifyer>();
    }

    public void AddNotifyer(in INotifyer notifyer)
    {
        _notifyers.Add(notifyer);
    }

    public void ChangeBalance(in decimal value)
    {
        this._balance = value;
        Notification();
    }

    public decimal Balance
    {
        get { return _balance; }
    }

    private void Notification()
    {
        foreach (var notifyer in _notifyers)
        {
            notifyer.Notify(_balance);
        }
    }
}

public interface INotifyer
{
    void Notify(in decimal balance);
}

public class SMSLowBalanceNotifyer : INotifyer
{
    private string _phone;
    private decimal _lowBalanceValue;

    public SMSLowBalanceNotifyer(string phone, decimal lowBalanceValue)
    {
        _phone = phone;
        _lowBalanceValue = lowBalanceValue;
    }

    public void Notify(in decimal balance)
    {
        if (balance < _lowBalanceValue)
        {
            Console.WriteLine($"Called from a class: {this.GetType().Name}. {_phone}: balance lower than {_lowBalanceValue}");
        }
    }
}

public class EMailBalanceChangedNotifyer : INotifyer
{
    private string _email;

    public EMailBalanceChangedNotifyer(in string email)
    {
        this._email = email;
    }

    public void Notify(in decimal balance)
    {
        Console.WriteLine($"Called from a class: {this.GetType().Name}. {_email}: new balance: {balance}");
    }
}

class Program
{
    static void Main(string[] args)
    {
        var email_1 = new EMailBalanceChangedNotifyer("email_1@mail.ru");
        var email_2 = new EMailBalanceChangedNotifyer("email_2@gmail.com");
        var phone_1 = new SMSLowBalanceNotifyer("+7-913-913-55-45", 40);
        var phone_2 = new SMSLowBalanceNotifyer("+7-905-605-23-54", 20);

        var account_1 = new Account();
        var account_2 = new Account(100);
        Console.WriteLine($"Account_1 balance: {account_1.Balance}");
        Console.WriteLine($"Account_2 balance: {account_2.Balance}");

        account_1.AddNotifyer(email_1);
        account_1.AddNotifyer(phone_1);
        account_1.AddNotifyer(phone_2);
        account_2.AddNotifyer(email_2);
        account_2.AddNotifyer(phone_2);

        Console.WriteLine("Account_1: ");
        account_1.ChangeBalance(10);
        Console.WriteLine();
        account_1.ChangeBalance(60);
        Console.WriteLine();


        Console.WriteLine("Account_2:");
        account_2.ChangeBalance(5);
        Console.WriteLine();
        account_2.ChangeBalance(30);

    }
}