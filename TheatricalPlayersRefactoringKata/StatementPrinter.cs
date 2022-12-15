using System;
using System.Collections.Generic;
using System.Globalization;

namespace TheatricalPlayersRefactoringKata
{
  public class StatementPrinter
  {
    public string Print(Invoice invoice, Dictionary<string, Play> plays)
    {
      var totalAmount = 0;
      var volumeCredits = 0;
      var printedResult = string.Format("Statement for {0}\n", invoice.Customer);
      CultureInfo cultureInfo = new CultureInfo("en-US");

      foreach (var performance in invoice.Performances)
      {
        var play = plays[performance.PlayID];
        int thisAmount = getTotalAmmountFrom(play, performance);
        totalAmount += thisAmount;

        volumeCredits += addVolumeCredits(performance.Audience);

        if (play.Type == "comedy")
        {
          volumeCredits += addExtraComedyCredits(performance.Audience);
        }
        printedResult += printOrder(cultureInfo, play.Name, thisAmount, performance.Audience);

      }
      printedResult += amountOwedToString(cultureInfo, totalAmount);
      printedResult += String.Format("You earned {0} credits\n", volumeCredits);
      return printedResult;
    }
    public int getTotalAmmountFrom(Play play, Performance perf)
    {
      int thisAmount = 0;
      switch (play.Type)
      {
        case "tragedy":
          thisAmount = 40000;
          if (perf.Audience > 30)
          {
            thisAmount += 1000 * (perf.Audience - 30);
          }
          break;
        case "comedy":
          thisAmount = 30000;
          if (perf.Audience > 20)
          {
            thisAmount += 10000 + 500 * (perf.Audience - 20);
          }
          thisAmount += 300 * perf.Audience;
          break;
        default:
          throw new Exception("unknown type: " + play.Type);
      }
      return thisAmount;
    }
    public int addVolumeCredits(int audience)
    {
      int volumeCredits = Math.Max(audience - 30, 0);
      return volumeCredits;
    }
    public int addExtraComedyCredits(int audience)
    {
      int extraCredits = (int)Math.Floor((decimal)audience / 5);
      return extraCredits;
    }
    public string printOrder(CultureInfo cultureInfo, string playName, int amount, int audience)
    {
      string order = String.Format(cultureInfo, "  {0}: {1:C} ({2} seats)\n", playName, Convert.ToDecimal(amount / 100), audience);
      return order;
    }
    public string amountOwedToString(CultureInfo cultureInfo, int amount)
    {
      string amountOwed = String.Format(cultureInfo, "Amount owed is {0:C}\n", Convert.ToDecimal(amount / 100));
      return amountOwed;
    }
  }
}
